using System;
using System.Windows.Forms;

using Kyru.Core;

namespace Kyru
{
	internal partial class Login : Form
	{
		private App app;

		internal Login(App app)
		{
			this.app = app;
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (txtUsername.Text != "" && txtPassword.Text != "")
			{
				app.Login(txtUsername.Text, txtPassword.Text);
				Visible = false;
				var kform = new KyruForm(app);
				kform.FormClosed += new FormClosedEventHandler(logout);
				kform.ShowDialog();
			}
			else
			{
				MessageBox.Show("You seem to be missing your username or password", "Warning");
			}
		}

		private void btnRegister_Click(object sender, EventArgs e)
		{
			var registry = new Register(this);
			registry.Show();
			Visible = false;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			// how to make this work?
			Close();
		}

		private void logout(object sender, EventArgs e)
		{
			app.Session = null;
			Visible = true;
		}
	}
}