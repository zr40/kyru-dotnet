using System;

namespace Kyru
{
	internal static class DateTimeExtension
	{
		internal static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		internal static uint UnixTimestamp(this DateTime dt)
		{
			return (uint)((dt - epoch).Ticks / TimeSpan.TicksPerSecond);
		}
	}
}