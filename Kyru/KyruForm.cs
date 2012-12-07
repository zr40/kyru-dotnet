using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Kyru.Core;
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

			virtualLocalFileTreeInit();
			Text = session.Username + " - " + Text;

			session.User.OnFileAdded += f => BeginInvoke(new Action<UserFile>(showFile), f);
		}

		internal void virtualLocalFileTreeInit()
		{
			foreach (var fileToShow in session.User.Files)
			{
				showFile(fileToShow);
			}
		}

		internal void showFile(UserFile fileToShow)
		{
			string fileName = session.DecryptFileName(fileToShow);
			//Console.Write("showing file" + fileName);
			var dirs = fileName.Split('\\');
			TreeNode node = null;
			TreeNodeCollection nodes = virtualLocalFileTree.Nodes;
			foreach (var dir in dirs)
			{
				if (nodes.ContainsKey(dir))
				{
					node = nodes[dir];
				}
				else
				{
					node = nodes.Add(dir, dir);
					// TODO: Add images
				}
				nodes = node.Nodes;
			}
			node.Tag = fileToShow;
		}

		private void addAFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = true;
			Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
			dialog.ShowDialog();
			foreach (string filename in dialog.FileNames)
			{
				try
				{
					using (var fs = new FileStream(filename, FileMode.Open))
					{
						var split = filename.Split('\\');
						session.AddFile(fs, split.Last());
					}
				}
				catch (IOException ex)
				{
					Console.WriteLine(ex.Message);
					MessageBox.Show("Could not add file " + filename);
				}
			}
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			virtualLocalFileTree.Nodes.Clear();
			virtualLocalFileTreeInit();
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
				session.LocalObjectStorage.DownloadObjects(userFile.ChunkList, (error) =>
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
				builder.Append(c);
			}

			MessageBox.Show(builder.ToString());
		}

		private void KyruForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			session.Dispose();
		}
	}
}