using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FirstWave.Core;
using FirstWave.Core.Extensions;
using FirstWave.Niot.Abilities;
using UnityEngine;

namespace FirstWave.Niot.Game.Managers
{
	public class AbilityManager : Singleton<AbilityManager>
	{
		private IDictionary<int, Ability> allAbilities;
		private int abilityId = 1;

		public Ability GetAbility(int abilityId)
		{
			if (allAbilities == null || allAbilities.Count == 0)
				InitializeAbilities();

			if (allAbilities.ContainsKey(abilityId))
				return allAbilities[abilityId].Clone();

			return null;
		}

		public Ability GetAbility(string name)
		{
			if (allAbilities == null || allAbilities.Count == 0)
				InitializeAbilities();

			var matchingAbility = allAbilities.Values.FirstOrDefault(a => a.Name == name);
			if (matchingAbility != null)
				return matchingAbility.Clone();

			return null;
		}

		private void InitializeAbilities()
		{
			allAbilities = new Dictionary<int, Ability>();

			var abilitiesAsset = Resources.Load("Abilities") as TextAsset;

			var doc = new XmlDocument();
			doc.LoadXml(abilitiesAsset.text);

			var abilityNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "ability");

			foreach (var abilityNode in abilityNodes)
			{
				Ability ability = null;
				if (abilityNode.Attributes.GetNamedItem("finisher") != null)
					ability = CreateFinisherAbility(abilityNode);
				else
					ability = CreateRegularAbility(abilityNode);

				if (ability != null)
					allAbilities.Add(ability.Id, ability);
			}

			// Initialize the Attack ability
			Ability.ATTACK.AudioClip = Resources.Load("SoundEffects/SwordSwing") as AudioClip;
		}

		private Ability CreateRegularAbility(XmlNode abilityNode)
		{
			return CreateRegularAbility(abilityNode, new Ability());
		}

		private Ability CreateRegularAbility(XmlNode abilityNode, Ability ability)
		{
			ability.Id = abilityId++;
			ability.Name = abilityNode.GetAttributeValue("name");
			ability.Description = abilityNode.GetAttributeValue("description");

			var cooldown = abilityNode.GetAttributeValue("cooldown");
			if (!string.IsNullOrEmpty(cooldown))
				ability.Cooldown = cooldown.ToInt();

			ability.ElementType = abilityNode.GetEnumAttributeValue<ElementType>("elementType", ElementType.None);
			ability.TargetType = abilityNode.GetEnumAttributeValue<TargetTypes>("targetType", TargetTypes.Single);
			ability.AbilityType = abilityNode.GetEnumAttributeValue<AbilityType>("abilityType", AbilityType.Physical);

			var iconString = abilityNode.GetAttributeValue("icon");
			if (!string.IsNullOrEmpty(iconString))
			{
				var icon = Resources.Load("Images/" + iconString) as Texture2D;
				if (icon != null)
					ability.Icon = icon;
			}

			ability.DamageRange = ParseDamageRange(abilityNode);

			var clip = Resources.Load("SoundEffects/SwordSwing") as AudioClip;
			ability.AudioClip = clip;

			return ability;
		}

		private Ability CreateFinisherAbility(XmlNode abilityNode)
		{
			var ability = new Finisher();

			var costNodes = abilityNode.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "fieldEffectCost");
			foreach (var cost in costNodes)
			{
				ElementType et = ElementType.None;

				var elementType = cost.GetAttributeValue("elementType");
				if (!string.IsNullOrEmpty(elementType))
					et = elementType.EnumFromString<ElementType>() ?? ElementType.None;

				var amount = cost.GetAttributeValue("number").ToInt();

				if (et != ElementType.None)
					ability.ElementCost.Add(new FinisherCostElement(et, amount));
			}

			return CreateRegularAbility(abilityNode, ability);
		}

		private Tuple<int, int> ParseDamageRange(XmlNode node)
		{
			var rangeNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "range");
			if (rangeNode == null)
				return Tuple.Create(0, 0);

			var low = rangeNode.FirstChild.InnerText.ToInt();
			var high = rangeNode.LastChild.InnerText.ToInt();

			return Tuple.Create(low, high);
		}
	}
}