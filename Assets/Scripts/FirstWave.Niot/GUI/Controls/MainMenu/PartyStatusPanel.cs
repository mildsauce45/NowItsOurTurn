using FirstWave.Core.GUI;
using FirstWave.Niot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GUI.Controls.MainMenu
{
	public class PartyStatusPanel : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		public Thickness margin;
		public Thickness padding;

		public float itemPadding = 10f;

		private Player partyMember;
		public Player PartyMember
		{
			get { return partyMember; }
			set
			{
				partyMember = value;
				ResetColumns();
			}
		}

		private List<NameValuePair> column1;
		private List<NameValuePair> column2;

		#region Unity Engine

		void OnGUI()
		{
			if (PartyMember == null)
				return;

			if (column1 == null || column1.Count == 0)
				FillColumns();

			var style = GUIManager.GetMessageBoxStyle(fontProperties);			

			var xLoc = GetXLocation();
			var yLoc = GetYLocation();
			var width = GetWidth();
			var height = CalcHeight(style);

			var columnWidth = ((width - (2 * textures.BorderVertical.width) - padding.left - padding.right) / 2) - 30f;

			BorderBox.Draw(new Rect(xLoc, yLoc, width, height), textures);

			var column1xLoc = xLoc + textures.BorderVertical.width + padding.left;
			var column1yLoc = yLoc + textures.BorderHorizontal.height + padding.top;

			DrawColumn(column1, column1xLoc, column1yLoc, columnWidth, style);

			var column2xLoc = column1xLoc + columnWidth + 30f;
			var column2yLoc = column1yLoc;

			DrawColumn(column2, column2xLoc, column2yLoc, columnWidth, style);
		}

		private void DrawColumn(IEnumerable<NameValuePair> column, float x, float y, float width, GUIStyle style)
		{
			foreach (var nvp in column)
			{
				var labelContent = new GUIContent(nvp.Label);
				var valueContent = new GUIContent(nvp.Value);

				var labelSize = style.CalcSize(labelContent);
				var valueSize = style.CalcSize(valueContent);

				UnityEngine.GUI.Label(new Rect(x, y, labelSize.x, labelSize.y), labelContent, style);
				UnityEngine.GUI.Label(new Rect(x + width - valueSize.x, y, valueSize.x, valueSize.y), valueContent, style);

				y += labelSize.y + itemPadding;
			}
		}

		private float GetXLocation()
		{
			return margin.left;
		}

		private float GetYLocation()
		{
			return margin.top;
		}

		private float GetWidth()
		{
			return Screen.width - GetXLocation() - margin.right;
		}

		#endregion

		private void ResetColumns()
		{
			if (column1 == null)
				column1 = new List<NameValuePair>();

			if (column2 == null)
				column2 = new List<NameValuePair>();

			column1.Clear();
			column2.Clear();
		}

		private void FillColumns()
		{
			column1.Add(new NameValuePair("Name", PartyMember.Name));
			column1.Add(new NameValuePair("", ""));
			column1.Add(new NameValuePair("Level", PartyMember.Level.ToString()));
			column1.Add(new NameValuePair("Strength", PartyMember.Strength.ToString()));
			column1.Add(new NameValuePair("Endurance", PartyMember.Endurance.ToString()));
			column1.Add(new NameValuePair("Speed", PartyMember.Speed.ToString()));
			column1.Add(new NameValuePair("Will", PartyMember.Will.ToString()));

			column2.Add(new NameValuePair("Class", PartyMember.Class));
			column2.Add(new NameValuePair("", ""));
			column2.Add(new NameValuePair("Experience", PartyMember.Exp.ToString()));
			column2.Add(new NameValuePair("Weapon", PartyMember.Weapon != null ? PartyMember.Weapon.Name : "Not Equipped"));
			column2.Add(new NameValuePair("Armor", PartyMember.Chestpiece != null ? PartyMember.Chestpiece.Name : "Not Equipped"));
		}

		private float CalcHeight(GUIStyle style)
		{
			var column1Height = CalcColumnHeight(column1, style);
			var column2Height = CalcColumnHeight(column2, style);

			return Math.Max(column1Height, column2Height) + padding.top + padding.bottom + (textures.BorderHorizontal.height * 2);
        }

		private float CalcColumnHeight(IEnumerable<NameValuePair> column, GUIStyle style)
		{
			if (column == null || !column.Any())
				return 0;

			var totalItemPadding = itemPadding * (column.Count() - 1);
			var itemHeight = 0f;

			foreach (var nvp in column)
				itemHeight += style.CalcSize(new GUIContent(nvp.Label)).y;
			
			return totalItemPadding + itemHeight;
        }

		private class NameValuePair
		{
			public string Label { get; private set; }
			public string Value { get; private set; }

			public NameValuePair(string label, string value)
			{
				Label = label;
				Value = value;
			}
		}
	}
}
