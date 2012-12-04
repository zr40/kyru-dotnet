using System;
using System.Drawing;
using System.Text;
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
		}

		private static void CreateSystemTray()
		{
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);
			trayMenu.MenuItems.Add("Login", OnLogin);
			trayMenu.MenuItems.Add("Add Node", OnRegisterNode);

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
			Form f = new LoginForm(app.LocalObjectStorage);
			f.Show();
		}

		private static void OnRegisterNode(object sender, EventArgs e)
		{
			Form f = new AddNodeForm(app.Node.Kademlia);
			f.Show();
		}
	}
}