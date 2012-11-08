using System.Threading;

namespace Tests
{
	internal class CallbackTimeout
	{
		private readonly ManualResetEventSlim ev = new ManualResetEventSlim();

		internal void Done()
		{
			ev.Set();
		}

		internal bool Block(int timeout)
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

	internal sealed class CallbackTimeout<T1, T2> : CallbackTimeout
	{
		internal T1 Result1;
		internal T2 Result2;

		internal void Done(T1 t1, T2 t2)
		{
			Result1 = t1;
			Result2 = t2;
			Done();
		}
	}
}