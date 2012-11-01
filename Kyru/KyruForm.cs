using System.Windows.Forms;

using Kyru.Core;
using System.IO;

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
		}

		internal void virtualLocalFileTreeInit()
		{
			var session = app.session;
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
			dialog.ShowDialog();
			foreach (string filename in dialog.FileNames)
			{
				FileStream fs = new FileStream(filename, FileMode.Open);
				app.session.AddFile(fs);
			}
		}
	}
}