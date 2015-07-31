using System;

namespace FirstWave.StateMachine
{
	internal class IgnoredTriggerBehavior<TState, TTrigger> : TriggerBehavior<TState, TTrigger>
	{
		public IgnoredTriggerBehavior(TTrigger trigger, Func<bool> guard)
			: base(trigger, guard)
		{
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = default(TState);
			return false;
		}
	}
}
