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

		private int[] nodesPerLine = { 0, 1, 4, 20 };
		private readonly List<Icon> nodesIcon;

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

			nodesIcon = new List<Icon>();

			for (int i = 0; i < 4; i++){
				nodesIcon.Add(new Icon("Icons/kyru"+ i.ToString() + ".ico"));
			}
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
			int connectedCount = app.Node.Kademlia.CurrentContactCount;
			int lineCount = 0;
			for (int i = 0; i < nodesPerLine.Count(); i++)
			{
				if (nodesPerLine[i] <= connectedCount)
					lineCount = i;
			}

			if (lineCount == 0)
				trayIcon.Text = "Kyru - Not Connected";
			else
				trayIcon.Text = "Kyru - Connected";

			trayIcon.Icon = nodesIcon[lineCount];
		}
	}
}
