using System;
using System.Windows.Forms;

using Kyru.Core;
using Kyru;
using Kyru.Utilities;

namespace Kyru
{
	internal static class Program
	{
		private static KyruApplication app;
		private static SystemTray tray;
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

			tray = new SystemTray(app);

			Application.Run();
		}
	}
}
