﻿using System;
using System.Windows.Forms;
using Kyru.Utilities;

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

			app.Start();
			//app.Login("username", "password");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// TODO: Login

			Application.Run(new Login(app));
			//Application.Run(new KyruForm(app));
		}
	}
}
