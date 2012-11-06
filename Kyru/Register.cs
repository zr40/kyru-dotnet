﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kyru
{
	internal partial class Register : Form
    {
        Login logger;
		  internal Register(Login Logger)
        {
            logger = Logger;
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            bool un = false;
            bool conf = false;
            if (txtName.Text != "")
            {
                string username = txtName.Text;
                un = true;
            }
            if (txtPass.Text != "")
            {
                string password = txtPass.Text;
            }
            if (txtConfirm.Text != "")
            {
                string confirm = txtConfirm.Text;
                conf = true;
            }
            if (txtPass.Text != txtConfirm.Text)
            {
                MessageBox.Show("Your passwords don't match", "Warning");
                return;
            }
            if (!un || !conf)
            {
                MessageBox.Show("You seem to have forgotten your username, password, or confirmation", "Warning");
                return;
            }
            // logger.app.session.User = new Core.User(txtName.Text, ??????)
            logger.Visible = true;
            MessageBox.Show("You successfully registered!", "Message");
            this.Close();
        }
    }
}
