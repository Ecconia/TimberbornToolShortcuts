using System;
using HarmonyLib;
using Timberborn.SettingsSystemUI;
using UnityEngine.UIElements;

namespace ToolShortcuts.Settings
{
	public static class SettingsBoxPatch
	{
		[HarmonyPatch(typeof(SettingsBox), nameof(SettingsBox.GetPanel))]
		public static class PatchToolGroupButtonToolLabelsEntered
		{
			private static void Postfix(ref VisualElement __result, SettingsBox __instance)
			{
				var settingsParent = extractSettingsRoot(__result);
				
				addHeader(settingsParent, "Mod.ToolShortcuts.Header", "Tool Shortcuts");
				var toggle = addToggle(settingsParent, "Mod.ToolShortcuts.Setting.ShouldDirectlyUseFirstTool", "Use first tool of tool group, when opening it with keybindings?");
				
				//Make this settings access pretty and generic one day... Should be in a singleton.
				var settings = __instance._gameSavingSettingsController._gameSavingSetting._settings;
				var value = settings.GetBool("Mod.ToolShortcuts.Setting.ShouldDirectlyUseFirstTool", Plugin.directlyOpenFirstToolInGroup);
				Plugin.directlyOpenFirstToolInGroup = value;
				toggle.SetValueWithoutNotify(value);
				toggle.RegisterValueChangedCallback(v => {
					settings.SetBool("Mod.ToolShortcuts.Setting.ShouldDirectlyUseFirstTool", v.newValue);
					Plugin.directlyOpenFirstToolInGroup = v.newValue;
				});
			}
			
			private static VisualElement extractSettingsRoot(VisualElement settingsBox)
			{
				var savingsHeader = settingsBox.Q("SavingHeader");
				if (savingsHeader == null)
				{
					throw new Exception("Did not find the 'SavingHeader' element in 'SettingsBox'");
				}
				return savingsHeader.parent;
			}
			
			private static void addHeader(VisualElement parent, string name, string title)
			{
				var header = new Label(title);
				header.name = name;
				header.AddToClassList("settings-header");
				parent.Add(header);
			}
			
			private static Toggle addToggle(VisualElement parent, string name, string description)
			{
				var toggle = new Toggle();
				toggle.name = name;
				toggle.AddToClassList("settings-element");
				toggle.AddToClassList("settings-toggle");
				toggle.AddToClassList("settings-text");
				toggle.AddToClassList("bottom-padding--medium");
				toggle[0].Add(new Label(description));
				parent.Add(toggle);
				return toggle;
			}
		}
	}
}
