using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstWave.TileMap
{
	[Flags]
	public enum Directions
	{
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8
	}
}
