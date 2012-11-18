using System;
using System.Windows.Forms;
using Kyru.Utilities;
using System.Threading;
using System.Drawing;

namespace Kyru
{
	internal static class Program
	{
		static ManualResetEvent neverThrown = new ManualResetEvent(false);
		static Core.KyruApplication app;

		static NotifyIcon trayIcon;
		static ContextMenu trayMenu;

		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Kyru debug console");
			Console.WriteLine();

			KyruTimer.Start();

			app = new Core.KyruApplication();

			app.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var trayIcon = CreateSystemTray();

			Application.Run();
		}

		public static NotifyIcon CreateSystemTray()
		{
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);
			trayMenu.MenuItems.Add("Login", OnLogin);
			trayMenu.MenuItems.Add("Add Node", OnRegisterNode);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Kyru";
			trayIcon.Icon = new Icon("kyru.ico");
			trayIcon.Click += OnLogin;

			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
			return trayIcon;
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
