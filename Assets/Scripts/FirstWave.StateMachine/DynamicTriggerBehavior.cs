using System;

namespace FirstWave.StateMachine
{
	internal class DynamicTriggerBehavior<TState, TTrigger> : TriggerBehavior<TState, TTrigger>
	{
		private readonly Func<object[], TState> destination;

		public DynamicTriggerBehavior(TTrigger trigger, Func<object[], TState> destination, Func<bool> guard)
			: base(trigger, guard)
		{
			this.destination = destination;
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = this.destination(args);

			return true;
		}
	}
}
