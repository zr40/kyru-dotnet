namespace Kyru
{
	internal interface ITimerListener
	{
		/// <summary>
		/// The specified interval has passed.
		/// </summary>
		void TimerElapsed();
	}
}