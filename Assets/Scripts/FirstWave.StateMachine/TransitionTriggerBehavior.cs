using System;

namespace FirstWave.StateMachine
{
	internal class TransitionTriggerBehavior<TState, TTrigger> : TriggerBehavior<TState, TTrigger>
	{
		private readonly TState destination;

		public TransitionTriggerBehavior(TTrigger trigger, TState destination, Func<bool> guard)
			: base(trigger, guard)
		{
			this.destination = destination;
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = this.destination;

			return true;
		}
	}
}
