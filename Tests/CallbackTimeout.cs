using System.Threading;

namespace Tests
{
	internal sealed class CallbackTimeout
	{
		private ManualResetEventSlim ev = new ManualResetEventSlim();

		internal void Done(object o)
		{
			Done();
		}
		internal void Done()
		{
			ev.Set();
		}

		internal bool Block(int timeout = -1)
		{
			return ev.Wait(timeout);
		}
	}
}