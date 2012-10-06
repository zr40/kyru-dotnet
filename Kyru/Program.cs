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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new KyruForm());
		}
	}
}