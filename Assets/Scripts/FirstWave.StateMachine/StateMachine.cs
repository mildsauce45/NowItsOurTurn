using System;
using System.Collections.Generic;

namespace FirstWave.StateMachine
{
	public class StateMachine<TState, TTrigger>
	{
		private readonly IDictionary<TState, StateRepresentation<TState, TTrigger>> stateConfiguration = new Dictionary<TState, StateRepresentation<TState, TTrigger>>();
		
		private readonly Func<TState> stateAccessor;
		private readonly Action<TState> stateMutator;

		private Action<TState, TTrigger> unhandledTriggerAction = DefaultUnhandledTriggerAction;

		private static void DefaultUnhandledTriggerAction(TState state, TTrigger trigger)
		{
			throw new InvalidOperationException(string.Format("No valid leaving transitions are permitted from state '{1}' for trigger '{0}'. Consider ignoring the trigger.", trigger, state));
		}

		public TState State
		{
			get { return stateAccessor(); }
			set { stateMutator(value); }
		}

		public IEnumerable<TTrigger> PermittedTriggers
		{
			get { return CurrentRepresentation.PermittedTriggers; }
		}

		private StateRepresentation<TState, TTrigger> CurrentRepresentation
		{
			get { return GetRepresentation(State); }
		}

		public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
		{
			this.stateAccessor = stateAccessor;
			this.stateMutator = stateMutator;
		}

		public StateMachine(TState initialState)
		{
			var stateRef = new StateReference<TState> { State = initialState };

			this.stateAccessor = () => stateRef.State;
			this.stateMutator = s => stateRef.State = s;
		}

		public StateConfiguration<TState, TTrigger> Configure(TState state)
		{
			return new StateConfiguration<TState, TTrigger>(GetRepresentation(state));
		}

		public void Fire(TTrigger trigger)
		{
			InternalFire(trigger, new object[0]);
		}

		public bool IsInState(TState state)
		{
			return CurrentRepresentation.IsIncludedIn(state);
		}

		public bool CanFire(TTrigger trigger)
		{
			return CurrentRepresentation.CanHandle(trigger);
		}		

		private StateRepresentation<TState, TTrigger> GetRepresentation(TState state)
		{
			StateRepresentation<TState, TTrigger> result;

			if (!stateConfiguration.TryGetValue(state, out result))
			{
				result = new StateRepresentation<TState, TTrigger>(state);
				stateConfiguration.Add(state, result);
			}

			return result;
		}

		private void InternalFire(TTrigger trigger, params object[] args)
		{
			TriggerBehavior<TState, TTrigger> triggerBehavior;
			if (!CurrentRepresentation.TryFindHandler(trigger, out triggerBehavior))
			{
				unhandledTriggerAction(CurrentRepresentation.UnderlyingState, trigger);
				return;
			}

			var source = State;
			TState destination;

			if (triggerBehavior.ResultsInTransitionFrom(source, args, out destination))
			{
				var transition = new Transition<TState, TTrigger>(source, destination, trigger);

				CurrentRepresentation.Exit(transition);
				State = transition.Destination;
				CurrentRepresentation.Enter(transition, args);
			}
		}
	}
}
