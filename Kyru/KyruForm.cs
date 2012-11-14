using System.Windows.Forms;

using Kyru.Core;
using System.IO;

using Kyru.Network.Objects;

namespace Kyru
{
	public partial class KyruForm : Form
	{
		App app;

		internal KyruForm(App app)
		{
			this.app = app;
			InitializeComponent();

			virtualLocalFileTreeInit();
			this.Text = app.Session.Username + " - " + this.Text;
		}

		internal void virtualLocalFileTreeInit()
		{
			var session = app.Session;
			foreach (var fileToShow in session.User.Files)
			{
				showFile(session, fileToShow);
			}
		}

		internal void showFile(Session session, UserFile fileToShow) {
			string fileName = session.DecryptFileName(fileToShow);
			virtualLocalFileTree.Nodes.Add(fileName);
		}

		private void addAFileToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect = true;
			dialog.ShowDialog();
			foreach (string filename in dialog.FileNames)
			{
				FileStream fs = new FileStream(filename, FileMode.Open);
				var file = app.Session.AddFile(fs);
				fs.Close();
				showFile(app.Session, file);
			}
		}

		private void addANodeToolStripMenuItem_Click(object sender, System.EventArgs e)
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

		private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void infoToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}