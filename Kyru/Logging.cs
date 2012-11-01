using System;
using System.Diagnostics;

namespace Kyru
{
	internal static class Logging
	{
		[Conditional("DEBUG")]
		internal static void Log(this object obj, string message, params object[] args)
		{
			message = string.Format(message, args);
			Console.WriteLine("{0}: {1}", obj.GetType().Name, message);
		}
	}
}