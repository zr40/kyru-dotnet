using System;
using System.Collections.Generic;
using System.Timers;

namespace Kyru
{
	internal static class KyruTimer
	{
		private struct Client
		{
			internal ITimerListener Object;
			internal int Interval;
		}

		internal static void Start()
		{
			timer.Start();
		}

		internal static void Stop()
		{
			timer.Stop();
		}

		private static readonly List<Client> clients = new List<Client>();
		private static long tick;
		private static readonly Timer timer = new Timer(1000);

		static KyruTimer()
		{
			timer.Elapsed += TimerOnElapsed;
		}

		private static void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			tick++;

			foreach (var client in clients)
			{
				if (tick % client.Interval == 0)
				{
					client.Object.TimerElapsed();
				}
			}
		}

		internal static void Register(ITimerListener obj, int interval)
		{
			clients.Add(new Client {Object = obj, Interval = interval});
		}
	}
}