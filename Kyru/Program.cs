using System;
using System.Drawing;
using System.Numerics;
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

			//var x = new BigInteger(int.MaxValue) * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue * int.MaxValue;

			var x = BigInteger.Parse("161521746670640296426473658428859984306663144318152681524054709078245736590366297248377298082656939330673286493230336261991466938596691073112968626710161521746670640296426473658428859984306663144318152681524054709078245736590366297248377298082656939330673286493230336261991466938596691073112968626710");

			Console.WriteLine("{0} bytes (at most {1} bits)", x.ToByteArray().Length, x.ToByteArray().Length * 8);

			var start = DateTime.Now;
			Console.WriteLine(x.FindPrime());
			Console.WriteLine(DateTime.Now - start);


			start = DateTime.Now;
			Console.WriteLine(Primes.Next(x));
			Console.WriteLine(DateTime.Now - start);
			Console.WriteLine();

			/*KyruTimer.Start();

			app = new KyruApplication();

			app.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			CreateSystemTray();

			Application.Run();
			 */
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