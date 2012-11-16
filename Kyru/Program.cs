using System;
using System.Windows.Forms;
using Kyru.Utilities;
using System.Threading;

namespace Kyru
{
	internal static class Program
	{
		static ManualResetEvent _quitEvent = new ManualResetEvent(false);

		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Kyru debug console");
			Console.WriteLine();

			KyruTimer.Start();

			Core.App app = new Core.App();

			app.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			app.CreateSystemTray();

			// Todo, find a better solution
			_quitEvent.WaitOne();
		}
	}
}
