using System;
using System.Diagnostics;

namespace Kyru
{
	internal static class Logging
	{
		[Conditional("DEBUG")]
		private static void Output(this object obj, string message, params object[] args)
		{
			message = string.Format(message, args);
			Console.WriteLine("{2:00}{3:000} {0}: {1}", obj.GetType().Name, message, DateTime.Now.Second, DateTime.Now.Millisecond);
		}

		[Conditional("DEBUG")]
		internal static void Log(this object obj, string message, params object[] args)
		{
			Output(obj, message, args);
		}

		[Conditional("DEBUG")]
		internal static void Warn(this object obj, string message, params object[] args)
		{
			Output(obj, message, args);
		}
	}
}