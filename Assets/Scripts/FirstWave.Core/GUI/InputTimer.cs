
namespace FirstWave.Core.GUI
{
	public class InputTimer : Timer
	{
		public bool IsTimeUp { get; private set; }

		public InputTimer(float inputDelay)
			: base(inputDelay)
		{
			IsTimeUp = false;
			TickAction = FlipSwitch;
		}

		private void FlipSwitch()
		{
			IsTimeUp = true;
		}

		public override void Reset()
		{
			base.Reset();

			IsTimeUp = false;
		}
	}
}
