
namespace FirstWave.StateMachine
{
	public class Transition<TState, TTrigger>
	{
		public TState Source { get; private set; }
		public TState Destination { get; private set; }
		public TTrigger Trigger { get; private set; }

		public bool IsReentry
		{
			get { return Source.Equals(Destination); }
		}

		public Transition(TState source, TState destination, TTrigger trigger)
		{
			Source = source;
			Destination = destination;
			Trigger = trigger;
		}
	}
}
