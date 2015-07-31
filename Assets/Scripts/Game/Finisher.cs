using System;
using System.Collections.Generic;
using System.Linq;
using FirstWave.Niot.Abilities;

namespace FirstWave.Niot.Game
{
	public class Finisher : Ability
	{
		public IList<FinisherCostElement> ElementCost { get; private set; }

		public Finisher()
		{
			this.IsFinisher = true;
			ElementCost = new List<FinisherCostElement>();
		}

		public Finisher(Ability toClone)
			: base(toClone)
		{
			if (!(toClone is Finisher))
				throw new ApplicationException("Attempted to create a finisher from a non finisher ability");

			IsFinisher = true;
			ElementCost = new List<FinisherCostElement>();
			Cooldown = 0;

			var castClone = toClone as Finisher;

			foreach (var cost in castClone.ElementCost)
				this.ElementCost.Add(new FinisherCostElement(cost.ElementType, cost.Amount));
		}

		public override bool CanUse(ElementType[] field = null)
		{
			if (field == null)
				return false;

			foreach (var cost in ElementCost)
			{
				if (field.Count(fe => fe == cost.ElementType) < cost.Amount)
					return false;
			}

			return true;
		}

		public override Ability Clone()
		{
			return new Finisher(this);
		}
	}

	public class FinisherCostElement
	{
		public ElementType ElementType { get; set; }
		public int Amount { get; set; }

		public FinisherCostElement(ElementType type, int amount)
		{
			ElementType = type;
			Amount = amount;
		}
	}
}
