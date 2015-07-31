using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using FirstWave.Niot.Abilities;

namespace FirstWave.Niot.Game.Editors
{
	public class AbilityEditor : EditorWindow
	{
		private AbilitiesDatabase abilityDb;

		private Vector2 scrollPosition;
		private Texture2D selectedTexture;
		private int selectedIndex = -1;

		private const int SPRITE_BUTTON_SIZE = 32;

		[MenuItem("FirstWave/Database/Ability Editor %#i")]
		public static void Init()
		{
			var window = EditorWindow.GetWindow<AbilityEditor>();
			window.minSize = new Vector2(500, 300);
			window.title = "Ability Database Editor";
			window.Show();
		}

		void OnEnable()
		{
			if (abilityDb == null)
				abilityDb = AbilitiesDatabase.GetDatabase<AbilitiesDatabase>(Constants.Databases.DATABASE_PATH, Constants.Databases.ABILITY_DB_NAME);
		}

		void OnGUI()
		{
			if (abilityDb == null)
			{
				Debug.LogWarning("Ability DB not loaded");
				return;
			}

			ListView();

			GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

			BottomBar();

			GUILayout.EndHorizontal();
		}

		private void ListView()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

			DisplayAbilities();

			EditorGUILayout.EndScrollView();
		}

		private void DisplayAbilities()
		{
			for (int i = 0; i < abilityDb.Count; i++)
			{
				var currentAbility = abilityDb.Get(i);

				GUILayout.BeginVertical("Box");

				GUILayout.BeginHorizontal("Box");

				currentAbility.Id = EditorGUILayout.IntField("Id:", currentAbility.Id);

				currentAbility.Name = EditorGUILayout.TextField("Name:", currentAbility.Name);

				currentAbility.ElementType = (ElementType)EditorGUILayout.EnumPopup("Element Type:", (Enum)currentAbility.ElementType);
				currentAbility.TargetType = (TargetTypes)EditorGUILayout.EnumPopup("Target Type:", (Enum)currentAbility.TargetType);

				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal("Box");

				currentAbility.Description = EditorGUILayout.TextField("Description:", currentAbility.Description);

				currentAbility.Cooldown = EditorGUILayout.IntField("Cooldown:", currentAbility.Cooldown);

				selectedTexture = currentAbility.Icon ?? null;

				if (GUILayout.Button(selectedTexture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE)))
				{
					int controllerId = EditorGUIUtility.GetControlID(FocusType.Passive);
					EditorGUIUtility.ShowObjectPicker<Texture2D>(null, true, null, controllerId);
					selectedIndex = i;
				}

				var commandName = Event.current.commandName;

				if (commandName == "ObjectSelectorUpdated")
				{
					if (selectedIndex != -1)
					{
						abilityDb.Get(selectedIndex).Icon = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
						Repaint();						
					}
				}

				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("Remove Ability", GUILayout.Width(100), GUILayout.ExpandWidth(false)))
				{
					if (EditorUtility.DisplayDialog("Remove Ability",
													string.Format("Are you sure that you want to delete {0} from the database?", currentAbility.Name),
													"Delete",
													"Cancel"))
					{
						abilityDb.Remove(i);
					}
				}

				GUILayout.EndHorizontal();

				GUILayout.EndVertical();				
			}
		}

		private void BottomBar()
		{
			GUILayout.Label("Abilities: " + abilityDb.Count);

			if (GUILayout.Button("Add"))
				abilityDb.Add(new Ability());
		}
	}
}
#endif
