using System;
using System.Windows.Forms;
using Kyru.Utilities;
using System.Threading;

namespace Kyru
{
	internal static class Program
	{
		static ManualResetEvent neverThrown = new ManualResetEvent(false);

		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Kyru debug console");
			Console.WriteLine();

			KyruTimer.Start();

			Core.KyruApplication app = new Core.KyruApplication();

			app.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			app.CreateSystemTray();

			// Make sure the application doesn't quit at the end of the main loop.
			// Todo, find a better solution
			neverThrown.WaitOne();
		}
	}
}
