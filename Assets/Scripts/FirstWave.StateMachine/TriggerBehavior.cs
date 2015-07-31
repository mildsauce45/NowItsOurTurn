using System;

namespace FirstWave.StateMachine
{
	internal abstract class TriggerBehavior<TState, TTrigger>
	{
		private readonly Func<bool> guard;

		public TTrigger Trigger { get; private set; }

		public bool IsGuardConditionMet
		{
			get { return guard(); }
		}

		protected TriggerBehavior(TTrigger trigger, Func<bool> guard)
		{
			Trigger = trigger;
			this.guard = guard;
		}

		public abstract bool ResultsInTransitionFrom(TState source, object[] args, out TState destination);
	}
}
