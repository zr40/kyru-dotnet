using System;
using System.Windows.Forms;

using Kyru.Core;

namespace Kyru
{
	internal partial class LoginForm : Form
	{
		private LocalObjectStorage localObjectStorage;

		internal LoginForm(LocalObjectStorage localObjectStorage)
		{
			this.localObjectStorage = localObjectStorage;
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (txtUsername.Text != "" && txtPassword.Text != "")
			{
				Session session = new Session(txtUsername.Text, txtPassword.Text, localObjectStorage);

				Visible = false;
				var kform = new KyruForm(session);
				kform.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("You seem to be missing your username or password", "Warning");
			}
		}
	}
}