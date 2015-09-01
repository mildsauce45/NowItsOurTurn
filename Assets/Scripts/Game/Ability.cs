using FirstWave.Core;
using FirstWave.Niot.Abilities;
using UnityEngine;

namespace FirstWave.Niot.Game
{
	public class Ability
	{
		public static readonly Ability ATTACK = new Ability
		{
			Id = -1,
			Name = "Attack",
			ElementType = ElementType.None,
			TargetType = TargetTypes.Single,
			AbilityType = AbilityType.Physical,
			IsFinisher = false,
			DamageRange = Tuple.Create(1, 1),
			Cooldown = 0
		};

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }		
		public ElementType ElementType { get; set; }
		public TargetTypes TargetType { get; set; }
		public AbilityType AbilityType { get; set; }
		public int Cooldown { get; set; }

		public Texture2D Icon { get; set; }
		public AudioClip AudioClip { get; set; }

		public int CooldownRemaining { get; set; }
		public bool IsFinisher { get; protected set; }

		public Tuple<int, int> DamageRange { get; set; }

		public Ability()
		{
		}

		public Ability(Ability toClone)
		{
			this.Id = toClone.Id;
			this.Name = toClone.Name;
			this.Description = toClone.Description;
			this.Icon = toClone.Icon;
			this.ElementType = toClone.ElementType;
			this.TargetType = toClone.TargetType;
			this.Cooldown = toClone.Cooldown;

			this.AudioClip = toClone.AudioClip;

			this.DamageRange = Tuple.Create(toClone.DamageRange.Item1, toClone.DamageRange.Item2);

			CooldownRemaining = 0;
		}

		public virtual bool CanUse(ElementType[] field = null)
		{
			return CooldownRemaining == 0;
		}

		public virtual Ability Clone()
		{
			return new Ability(this);
		}
	}
}
