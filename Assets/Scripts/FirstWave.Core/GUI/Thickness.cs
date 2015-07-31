using System;

namespace FirstWave.Core.GUI
{
	[Serializable]
	public class Thickness
	{
		public float left;
		public float top;
		public float right;
		public float bottom;

		public Thickness()
		{
		}

		public Thickness(float leftRight, float topBottom)
		{
			left = leftRight;
			right = leftRight;

			top = topBottom;
			bottom = topBottom;
		}

		public Thickness(float left, float top, float right, float bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}
	}
}
