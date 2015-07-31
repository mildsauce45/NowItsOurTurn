using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstWave.StateMachine
{
	internal class StateRepresentation<TState, TTrigger>
	{
		private readonly IDictionary<TTrigger, ICollection<TriggerBehavior<TState, TTrigger>>> triggerBehaviors = new Dictionary<TTrigger, ICollection<TriggerBehavior<TState, TTrigger>>>();

		private readonly ICollection<Action<Transition<TState, TTrigger>, object[]>> entryActions = new List<Action<Transition<TState, TTrigger>, object[]>>();
		private readonly ICollection<Action<Transition<TState, TTrigger>>> exitActions = new List<Action<Transition<TState, TTrigger>>>();	

		public TState UnderlyingState { get; private set; }

		public IEnumerable<TTrigger> PermittedTriggers
		{
			get { return triggerBehaviors.Where(t => t.Value.Any(a => a.IsGuardConditionMet)).Select(t => t.Key).ToArray(); }
		}
	
		public StateRepresentation(TState state)
		{
			UnderlyingState = state;
		}

		#region Public API

		public bool CanHandle(TTrigger trigger)
		{
			TriggerBehavior<TState, TTrigger> temp;

			return TryFindHandler(trigger, out temp);
		}

		public bool TryFindHandler(TTrigger trigger, out TriggerBehavior<TState, TTrigger> handler)
		{
			ICollection<TriggerBehavior<TState, TTrigger>> possible;

			if (!triggerBehaviors.TryGetValue(trigger, out possible))
			{
				handler = null;
				return false;
			}

			var actual = possible.Where(tb => tb.IsGuardConditionMet).ToArray();

			if (actual.Length > 1)
				throw new InvalidOperationException(string.Format("Multiple permitted exit transitions are configured from state '{1}' for trigger '{0}'. Guard clauses must be mutually exclusive.", trigger, UnderlyingState));

			handler = actual.FirstOrDefault();

			return handler != null;
		}

		public bool IsIncludedIn(TState state)
		{
			return UnderlyingState.Equals(state);
		}

		public void AddEntryAction(TTrigger trigger, Action<Transition<TState, TTrigger>, object[]> action)
		{
			entryActions.Add((t, args) =>
			{
				if (t.Trigger.Equals(trigger))
					action(t, args);
			});
		}

		public void AddEntryAction(Action<Transition<TState, TTrigger>, object[]> action)
		{
			entryActions.Add(action);
		}

		public void AddExitAction(Action<Transition<TState, TTrigger>> action)
		{
			exitActions.Add(action);
		}

		public void Enter(Transition<TState, TTrigger> transition, params object[] args)
		{
			if (transition.IsReentry || !Includes(transition.Source))
				ExecuteEntryActions(transition, args);
		}

		public void Exit(Transition<TState, TTrigger> transition)
		{
			if (transition.IsReentry || !Includes(transition.Destination))
				ExecuteExitActions(transition);
		}

		public void AddTriggerBehavior(TriggerBehavior<TState, TTrigger> triggerBehavior)
		{
			ICollection<TriggerBehavior<TState, TTrigger>> allowed;

			if (!triggerBehaviors.TryGetValue(triggerBehavior.Trigger, out allowed))
			{
				allowed = new List<TriggerBehavior<TState, TTrigger>>();
				triggerBehaviors.Add(triggerBehavior.Trigger, allowed);
			}

			allowed.Add(triggerBehavior);
		}

		#endregion

		#region Private Helpers

		private void ExecuteEntryActions(Transition<TState, TTrigger> transition, object[] args)
		{
			foreach (var action in entryActions)
				action(transition, args);
		}

		private void ExecuteExitActions(Transition<TState, TTrigger> transition)
		{
			foreach (var action in exitActions)
				action(transition);
		}

		private bool Includes(TState state)
		{
			return UnderlyingState.Equals(state);
		}

		#endregion
	}
}
