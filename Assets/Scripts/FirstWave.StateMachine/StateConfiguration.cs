using System;

namespace FirstWave.StateMachine
{
	public class StateConfiguration<TState, TTrigger>
	{
		private readonly StateRepresentation<TState, TTrigger> representation;

		private static readonly Func<bool> NoGuard = () => true;

		internal StateConfiguration(StateRepresentation<TState, TTrigger> representation)
		{
			this.representation = representation;
		}

		public StateConfiguration<TState, TTrigger> Permit(TTrigger trigger, TState destination)
		{
			return InternalPermit(trigger, destination);
		}

		public StateConfiguration<TState, TTrigger> PermitIf(TTrigger trigger, TState destination, Func<bool> guard)
		{
			return InternalPermitIf(trigger, destination, guard);
		}

		public StateConfiguration<TState, TTrigger> PermitReentry(TTrigger trigger)
		{
			return InternalPermit(trigger, representation.UnderlyingState);
		}

		public StateConfiguration<TState, TTrigger> PermitReentryIf(TTrigger trigger, Func<bool> guard)
		{
			return InternalPermitIf(trigger, representation.UnderlyingState, guard);
		}

		public StateConfiguration<TState, TTrigger> Ignore(TTrigger trigger)
		{
			return IgnoreIf(trigger, NoGuard);
		}

		public StateConfiguration<TState, TTrigger> IgnoreIf(TTrigger trigger, Func<bool> guard)
		{
			representation.AddTriggerBehavior(new IgnoredTriggerBehavior<TState, TTrigger>(trigger, guard));

			return this;
		}

		public StateConfiguration<TState, TTrigger> OnEntry(Action<Transition<TState, TTrigger>> action)
		{
			if (action != null)
				representation.AddEntryAction((t, args) => action(t));

			return this;
		}

		public StateConfiguration<TState, TTrigger> OnEntryFrom(TTrigger trigger, Action entryAction)
		{
			return OnEntryFrom(trigger, t => entryAction());
		}

		public StateConfiguration<TState, TTrigger> OnEntryFrom(TTrigger trigger, Action<Transition<TState, TTrigger>> action)
		{
			representation.AddEntryAction(trigger, (t, args) => action(t));

			return this;
		}

		public StateConfiguration<TState, TTrigger> OnExit(Action<Transition<TState, TTrigger>> action)
		{
			representation.AddExitAction(action);

			return this;
		}

		public StateConfiguration<TState, TTrigger> PermitDynamic(TTrigger trigger, Func<TState> destinationStateSelector)
		{
			return PermitDynamicIf(trigger, destinationStateSelector, NoGuard);
		}

		public StateConfiguration<TState, TTrigger> PermitDynamicIf(TTrigger trigger, Func<TState> destinationStateSelector, Func<bool> guard)
		{
			return InternalPermitDynamicIf(trigger, args => destinationStateSelector(), guard);
		}

		private StateConfiguration<TState, TTrigger> InternalPermit(TTrigger trigger, TState destination)
		{
			return InternalPermitIf(trigger, destination, NoGuard);
		}

		private StateConfiguration<TState, TTrigger> InternalPermitIf(TTrigger trigger, TState destination, Func<bool> guard)
		{
			representation.AddTriggerBehavior(new TransitionTriggerBehavior<TState, TTrigger>(trigger, destination, guard));

			return this;
		}

		private StateConfiguration<TState, TTrigger> InternalPermitDynamicIf(TTrigger trigger, Func<object[], TState> destinationStateSelector, Func<bool> guard)
		{
			representation.AddTriggerBehavior(new DynamicTriggerBehavior<TState, TTrigger>(trigger, destinationStateSelector, guard));

			return this;
		}
	}
}
