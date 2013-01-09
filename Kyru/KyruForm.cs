using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Kyru.Core;
using Kyru.Network;
using Kyru.Network.Objects;
using Kyru.Network.TcpMessages;

namespace Kyru
{
	public partial class KyruForm : Form
	{
		private Session session;

		private List<ulong> shownFileIDs = new List<ulong>();
		private List<ulong> deletedFileIDs = new List<ulong>();

		internal KyruForm(Session session)
		{
			this.session = session;
			InitializeComponent();

			InitLocalFileTree();
			Text = session.Username + " - " + Text;

			session.OnUserMerged += () => BeginInvoke(new Action(UpdateLocalFileTree));
		}

		internal void InitLocalFileTree()
		{
			virtualLocalFileTree.Nodes.Clear();
			virtualLocalFileTree.Nodes.Add("", session.Username, 0, 0);
			virtualLocalFileTree.Nodes[0].Expand();
			shownFileIDs.Clear();
			deletedFileIDs.Clear();
			UpdateLocalFileTree();
		}

		internal void UpdateLocalFileTree()
		{
			foreach (var fileToShow in session.User.Files)
			{
				if (shownFileIDs.Contains(fileToShow.FileId))
					continue;
				shownFileIDs.Add(fileToShow.FileId);
				ShowFile(fileToShow);
			}

			foreach (var deletedFile in session.User.DeletedFiles)
			{
				if (deletedFileIDs.Contains(deletedFile.Item2))
					continue;
				deletedFileIDs.Add(deletedFile.Item2);
				removeShownFile(deletedFile.Item2);
			}
		}

		internal void ShowFile(UserFile fileToShow)
		{
			string fileName = session.DecryptFileName(fileToShow);
			var dirs = fileName.Split('/');
			TreeNode node = virtualLocalFileTree.Nodes[0];
			for (int i = 0; i < dirs.Count(); i++)
			{
				TreeNodeCollection nodes = node.Nodes;

				if (nodes.ContainsKey(dirs[i]))
				{
					node = nodes[dirs[i]];
				}
				else
				{
					node = nodes.Add(dirs[i], dirs[i]);
					if (i == dirs.Count() - 1)
						node.ImageIndex = 3;
					else
						node.ImageIndex = 1;

					node.SelectedImageIndex = node.ImageIndex;
				}
			}
			node.Tag = fileToShow;
		}

		internal bool removeShownFile(ulong fileID, TreeNode parentNode = null)
		{
			if (parentNode == null)
				parentNode = virtualLocalFileTree.Nodes[0];

			foreach (TreeNode childNode in parentNode.Nodes)
			{
				var checkedFile = childNode.Tag as UserFile;

				if (checkedFile == null) // File is a directory.
				{
					if (removeShownFile(fileID, childNode))
						return true;
					continue;
				}

				if (checkedFile.FileId == fileID)
				{
					childNode.Remove();
					return true;
				}
			}
			return false;
		}

		private void AddFiles(IEnumerable<string> fileNames, string rootPath = null)
		{
			rootPath = rootPath == null ? "" : rootPath + "/";
			foreach (string fullPath in fileNames)
			{
				var path = fullPath.Split(Path.DirectorySeparatorChar).Last();
				if (Directory.Exists(fullPath)) // pathrefers to directory
				{
					AddFiles(Directory.EnumerateFileSystemEntries(fullPath), rootPath + path);
					continue;
				}
				try
				{
					using (var fs = File.OpenRead(fullPath))
					{
						var file = session.AddFile(fs, rootPath + path);
						ShowFile(file);
					}
				}
				catch (IOException ex)
				{
					Console.WriteLine(ex.Message);
					MessageBox.Show("Could not add file " + fullPath);
				}
			}
		}

		private void addAFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = true;
			Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
				AddFiles(dialog.FileNames);
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InitLocalFileTree();
		}

		private void virtualLocalFileTree_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				// Select the clicked node
				virtualLocalFileTree.SelectedNode = virtualLocalFileTree.GetNodeAt(e.X, e.Y);

				if (virtualLocalFileTree.SelectedNode != null)
				{
					rightClickMenu.Show(virtualLocalFileTree, e.Location);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var userFile = virtualLocalFileTree.SelectedNode.Tag as UserFile;
			if (userFile == null)
				return;

			var dialog = new SaveFileDialog();
			dialog.FileName = virtualLocalFileTree.SelectedNode.Text;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var ids = userFile.ChunkList.Select(a => (KademliaId) a).ToList();
				session.LocalObjectStorage.RetrieveObjects(ids, error =>
				                                                {
					                                                if (error != Error.Success)
					                                                {
						                                                MessageBox.Show("Saving failed: Could not find all parts of the file. Try to connect to the network or save the file later");
						                                                return;
					                                                }

					                                                using (var fs = new FileStream(dialog.FileName, FileMode.Create))
					                                                {
						                                                session.DecryptFile(userFile, fs);
					                                                }
				                                                });
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var userFile = virtualLocalFileTree.SelectedNode.Tag as UserFile;
			if (userFile == null)
				return;
			session.DeleteFile(userFile);
			UpdateLocalFileTree();
		}

		private void infoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var userFile = virtualLocalFileTree.SelectedNode.Tag as UserFile;
			if (userFile == null)
				return;

			var builder = new StringBuilder();

			builder.Append(session.DecryptFileName(userFile));
			builder.Append("\nID: ");
			builder.Append(userFile.FileId);

			builder.Append("\nChunks:");
			foreach (var c in userFile.ChunkList)
			{
				builder.Append("\n");
				builder.Append(BitConverter.ToString(c));
			}

			MessageBox.Show(builder.ToString());
		}

		private void KyruForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			session.Dispose();
		}

		private void virtualLocalFileTree_DragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

			AddFiles((string[]) e.Data.GetData(DataFormats.FileDrop));
		}

		private void virtualLocalFileTree_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}
	}
}