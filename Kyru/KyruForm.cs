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

		internal KyruForm(Session session)
		{
			this.session = session;
			InitializeComponent();

			ResetVirtualLocalFileTree();
			Text = session.Username + " - " + Text;

			session.OnUserMerged += () => BeginInvoke(new Action(ResetVirtualLocalFileTree));
		}

		internal void ResetVirtualLocalFileTree()
		{
			virtualLocalFileTree.Nodes.Clear();
			virtualLocalFileTree.Nodes.Add("", session.Username, 0,0);

			foreach (var fileToShow in session.User.Files)
			{
				ShowFile(fileToShow);
			}
		}

		internal void ShowFile(UserFile fileToShow)
		{
			string fileName = session.DecryptFileName(fileToShow);
			//Console.Write("showing file" + fileName);
			var dirs = fileName.Split('\\');
			TreeNode node = virtualLocalFileTree.Nodes[0];
			//TreeNodeCollection nodes = virtualLocalFileTree.Nodes;
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

		private void AddFiles(IEnumerable<string> filenames)
		{
			foreach (string path in filenames)
			{
				if (Directory.Exists(path)) // pathrefers to directory
				{
					AddFiles(Directory.EnumerateFileSystemEntries(path));
					continue;
				}
				try
				{
					using (var fs = new FileStream(path, FileMode.Open))
					{
						var split = path.Split('\\');
						var file = session.AddFile(fs, split.Last());
						ShowFile(file);
					}
				}
				catch (IOException ex)
				{
					Console.WriteLine(ex.Message);
					MessageBox.Show("Could not add file " + path);
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
			virtualLocalFileTree.Nodes.Clear();
			ResetVirtualLocalFileTree();
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
				session.LocalObjectStorage.DownloadObjects(userFile.ChunkList.Cast<KademliaId>().ToList(), (error) =>
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
			virtualLocalFileTree.Nodes.Remove(virtualLocalFileTree.SelectedNode);
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

			AddFiles((string[])e.Data.GetData(DataFormats.FileDrop));
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