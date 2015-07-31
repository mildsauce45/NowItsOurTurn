using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstWave.Niot.Game
{
	public class Weapon
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public int Power { get; set; }
		public int MagicPower { get; set; }

		public int Value { get; set; }

		public Weapon Clone()
		{
			var nw = new Weapon();

			nw.Name = this.Name;
			nw.Power = this.Power;
			nw.MagicPower = this.MagicPower;
			nw.Value = this.Value;

			return nw;
		}
	}
}
