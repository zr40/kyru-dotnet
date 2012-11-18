using Kyru.Network;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading;

namespace Kyru.Core
{
	internal sealed class KyruApplication
	{
		internal readonly Config Config;
		internal readonly LocalObjectStorage LocalObjectStorage;
		internal Session Session;
		internal readonly Node Node;

		private NotifyIcon  trayIcon;
		private ContextMenu trayMenu;


		internal KyruApplication() : this(12045)
		{
		}

		internal KyruApplication(ushort port)
		{
			Config = new Config();
			LocalObjectStorage = new LocalObjectStorage(Config);
			Node = new Node(port, this);
		}

		internal void Start()
		{
			Node.Start();
		}

		public void CreateSystemTray()
		{
			// Create a simple tray menu with only one item.
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);
			trayMenu.MenuItems.Add("Login", OnLogin);
			trayMenu.MenuItems.Add("Add Node", OnRegisterNode);

			// Create a tray icon. In this example we use a
			// standard system icon for simplicity, but you
			// can of course use your own custom icon too.
			trayIcon      = new NotifyIcon();
			trayIcon.Text = "Kyru";
			trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

			// Add menu to tray icon and show it.
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible     = true;
		}

		private void OnExit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void OnLogin(object sender, EventArgs e)
		{
			new Thread(() => new Login(LocalObjectStorage).ShowDialog()).Start();
		}

		private void OnRegisterNode(object sender, EventArgs e)
		{
			new Thread(() => new AddNodeForm(Node.Kademlia).ShowDialog()).Start();
		}
	}
}