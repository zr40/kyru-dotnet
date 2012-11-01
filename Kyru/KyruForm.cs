using System.Windows.Forms;

using Kyru.Core;

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
			if (session == null)
			{
				System.Console.WriteLine("Error in KyruForm: App has no session");
				return;
			}
			var user = session.User;
			if (session == null)
			{
				System.Console.WriteLine("Error in KyruForm: Session has no user");
				return;
			}

			foreach(var afile in user.Files){
				string fileName = session.DecryptFileName(afile);
				virtualLocalFileTree.Nodes.Add(fileName);
			}
		}
	}
}