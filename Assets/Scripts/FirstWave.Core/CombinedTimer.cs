using System.Collections.Generic;
using System.Linq;

namespace FirstWave.Core
{
	public class CombinedTimer : ITimer
	{
		private Queue<ITimer> timers;
		private bool started;

		public bool IsComplete
		{
			get { return started && !timers.Any(); }
		}

		public bool Enabled
		{
			get { return started; }
		}

		public CombinedTimer(params ITimer[] initialTimers)
		{
			timers = new Queue<ITimer>();

			if (initialTimers != null && initialTimers.Any())
			{
				foreach (var t in initialTimers)
					timers.Enqueue(t);
			}
		}

		public void AddTimer(ITimer timer)
		{
			timers.Enqueue(timer);
		}

		public void Start()
		{
			if (!started)
				started = true;
		}

		public void Update()
		{
			if (started)
			{
				var currentTimer = timers.Peek();

				if (!currentTimer.Enabled)
					currentTimer.Start();

				bool beforeUpdate = currentTimer.IsComplete;

				currentTimer.Update();

				if (!beforeUpdate && currentTimer.IsComplete)
				{
					timers.Dequeue();
					currentTimer.Stop();
				}
			}
		}

		public void Stop()
		{
			started = false;
		}
	}
}
