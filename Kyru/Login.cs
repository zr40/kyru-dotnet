using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kyru
{
	internal partial class Login : Form
    {
		public Kyru.Core.App app;
        internal Login(Kyru.Core.App app)
        {
			  this.app = app;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                app.Login(username, password);
            }
            else
            {
                MessageBox.Show("You seem to be missing your username or password", "Warning");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register registry = new Register(this);
            registry.Show();
            this.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // how to make this work?
            this.Close();
        }
    }
}
