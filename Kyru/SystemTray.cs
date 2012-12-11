using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Kyru.Core;
using Kyru.Utilities;

namespace Kyru
{
	internal class SystemTray
	{
		private KyruApplication app;

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		internal SystemTray(KyruApplication app)
		{
			this.app = app;

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Log in", OnLogin);
			trayMenu.MenuItems.Add("Add node", OnRegisterNode);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("System status", OnSystemStatus);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Kyru";
			trayIcon.Icon = new Icon("Icons/kyru.ico");
			trayIcon.MouseDoubleClick += OnLogin;

			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		private void OnExit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void OnLogin(object sender, EventArgs e)
		{
			new LoginForm(app.LocalObjectStorage).Show();
		}

		private void OnRegisterNode(object sender, EventArgs e)
		{
			new AddNodeForm(app.Node.Kademlia).Show();
		}

		private void OnSystemStatus(object sender, EventArgs e)
		{
			new SystemStatusForm(app).Show();
		}
	}
}
