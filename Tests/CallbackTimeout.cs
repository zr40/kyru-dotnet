using System.Threading;

namespace Tests
{
	internal class CallbackTimeout
	{
		private ManualResetEventSlim ev = new ManualResetEventSlim();

		internal void Done()
		{
			ev.Set();
		}

		internal bool Block(int timeout = -1)
		{
			return ev.Wait(timeout);
		}
	}

	internal sealed class CallbackTimeout<T> : CallbackTimeout
	{
		internal T Result;

		internal void Done(T t)
		{
			Result = t;
			Done();
		}
	}
}