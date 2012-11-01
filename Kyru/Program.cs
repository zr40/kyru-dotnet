using System;
using System.Windows.Forms;

namespace Kyru
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Kyru debug console");
			Console.WriteLine();

			KyruTimer.Start();

			Core.App app = new Core.App();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// TODO: Login

			Application.Run(new KyruForm(app));
		}
	}
}
