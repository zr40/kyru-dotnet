using System;
using System.IO;
using System.Windows.Forms;

using Kyru.Core;
using Kyru.Network.Objects;

namespace Kyru
{
	public partial class KyruForm : Form
	{
		private App app;

		internal KyruForm(App app)
		{
			this.app = app;
			InitializeComponent();

			virtualLocalFileTreeInit();
			Text = app.Session.Username + " - " + Text;
		}

		internal void virtualLocalFileTreeInit()
		{
			var session = app.Session;
			foreach (var fileToShow in session.User.Files)
			{
				showFile(session, fileToShow);
			}
		}

		internal void showFile(Session session, UserFile fileToShow)
		{
			string fileName = session.DecryptFileName(fileToShow);
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
			dialog.ShowDialog();
			foreach (string filename in dialog.FileNames)
			{
				var fs = new FileStream(filename, FileMode.Open);
				var file = app.Session.AddFile(fs, filename);
				fs.Close();
				showFile(app.Session, file);
			}
		}

		private void addANodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form form = new AddNodeForm(app.Node.Kademlia);
			form.ShowDialog();
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
				try
				{
					//app.Session.DecryptFile(userFile, fs);
				}
				catch (NullReferenceException)
				{
					MessageBox.Show("One or more of the chunks could not be found. Your file may be hopelessly lost.");
				}
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var userFile = virtualLocalFileTree.SelectedNode.Tag as UserFile;
			if (userFile == null)
				return;
			app.Session.DeleteFile(userFile);
			virtualLocalFileTree.Nodes.Remove(virtualLocalFileTree.SelectedNode);
		}

		private void infoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}