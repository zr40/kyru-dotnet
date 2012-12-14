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
	internal class SystemTray : ITimerListener
	{
		private readonly KyruApplication app;

		private readonly NotifyIcon trayIcon;
		private readonly ContextMenu trayMenu;
		private bool connected;

		private readonly Icon iconNotConnected;
		private readonly Icon iconConnected;

		internal SystemTray(KyruApplication app)
		{
			this.app = app;

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Log in", OnLogin);
			trayMenu.MenuItems.Add("Connect", OnRegisterNode);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("System status", OnSystemStatus);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			iconConnected = new Icon("Icons/kyru.ico");
			iconNotConnected = SystemIcons.Exclamation;
			//TimerElapsed();
			trayIcon.MouseDoubleClick += OnLogin;

			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;

			TimerElapsed();
			KyruTimer.Register(this, 2);
		}

		private void OnExit(object sender, EventArgs e)
		{
			trayIcon.Dispose();
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

		public void TimerElapsed()
		{
			bool newConnected = app.Node.Kademlia.CurrentContactCount > 0;
			if (connected != newConnected)
				return;
			connected = newConnected;

			if (connected)
			{
				trayIcon.Text = "Kyru - Connected";
				trayIcon.Icon = iconConnected;
			}
			else
			{
				trayIcon.Text = "Kyru - Not Connected";
				trayIcon.Icon = iconNotConnected;
			}
		}
	}
}
