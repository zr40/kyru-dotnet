using System;
using System.Drawing;
using System.Windows.Forms;

using Kyru.Core;
using Kyru.Utilities;

namespace Kyru
{
	internal static class Program
	{
		private static KyruApplication app;

		private static NotifyIcon trayIcon;
		private static ContextMenu trayMenu;

		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Kyru debug console");
			Console.WriteLine();

			KyruTimer.Start();

			app = new KyruApplication();

			app.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CreateSystemTray();

			Application.Run();

			trayIcon.Visible = false;
		}

		private static void CreateSystemTray()
		{
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Log in", OnLogin);
			trayMenu.MenuItems.Add("Add node", OnRegisterNode);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("System status", OnSystemStatus);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Kyru";
			trayIcon.Icon = new Icon("kyru.ico");
			trayIcon.MouseDoubleClick += OnLogin;

			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		private static void OnExit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private static void OnLogin(object sender, EventArgs e)
		{
			new LoginForm(app.LocalObjectStorage).Show();
		}

		private static void OnRegisterNode(object sender, EventArgs e)
		{
			new AddNodeForm(app.Node.Kademlia).Show();
		}

		private static void OnSystemStatus(object sender, EventArgs e)
		{
			new SystemStatusForm(app).Show();
		}
	}
}
