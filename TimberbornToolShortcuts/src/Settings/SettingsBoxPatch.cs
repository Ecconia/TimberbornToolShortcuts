using System;
using HarmonyLib;
using Timberborn.CoreUI;
using Timberborn.SettingsSystemUI;
using ToolShortcuts.Keybindings;
using UnityEngine.UIElements;

namespace ToolShortcuts.Settings
{
	public static class SettingsBoxPatch
	{
		private const string SettingsKey = "Mod.ToolShortcuts.Setting.ShouldDirectlyUseFirstTool";
		
		private static Toggle lastToggle;
		
		[HarmonyPatch(typeof(SettingsBox), nameof(SettingsBox.Load))]
		public static class PatchSettingsBoxLoad
		{
			public static void Postfix(SettingsBox __instance, VisualElement ____root, VisualElementLoader ____visualElementLoader)
			{
				var settingsParent = extractSettingsRoot(____root);
				
				var header = addHeader(settingsParent, "Mod.ToolShortcuts.Header", KeybindingKeys.LocalizationUIPrefix + "tool_shortcuts_settings_title");
				____visualElementLoader._visualElementInitializer.InitializeVisualElement(header);
				var toggle = addToggle(settingsParent, SettingsKey, KeybindingKeys.LocalizationUIPrefix + "should_directly_use_first_tool_description");
				____visualElementLoader._visualElementInitializer.InitializeVisualElement(toggle);
				
				//Make this settings access pretty and generic one day... Should be in a singleton.
				var settings = __instance._gameSavingSettingsController._gameSavingSetting._settings;
				var value = settings.GetBool(SettingsKey, Plugin.directlyOpenFirstToolInGroup);
				Plugin.directlyOpenFirstToolInGroup = value;
				toggle.SetValueWithoutNotify(value);
				toggle.RegisterValueChangedCallback(v => {
					settings.SetBool(SettingsKey, v.newValue);
					Plugin.directlyOpenFirstToolInGroup = v.newValue;
				});
				
				lastToggle = toggle;
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
			
			private static VisualElement addHeader(VisualElement parent, string name, string titleLocalizationKey)
			{
				var header = new LocalizableLabel
				{
					_textLocKey = titleLocalizationKey,
				};
				header.name = name;
				header.AddToClassList("settings-header");
				parent.Add(header);
				return header;
			}
			
			private static Toggle addToggle(VisualElement parent, string name, string descriptionLocalizationKey)
			{
				var toggle = new Toggle();
				toggle.name = name;
				toggle.AddToClassList("settings-element");
				toggle.AddToClassList("settings-toggle");
				toggle.AddToClassList("settings-text");
				toggle.AddToClassList("bottom-padding--medium");
				toggle[0].Add(new LocalizableLabel
				{
					_textLocKey = descriptionLocalizationKey,
				});
				parent.Add(toggle);
				return toggle;
			}
		}
		
		[HarmonyPatch(typeof(SettingsBox), nameof(SettingsBox.GetPanel))]
		public static class PatchSettingsBoxGetPanel
		{
			public static void Postfix(SettingsBox __instance)
			{
				if (lastToggle != null)
				{
					var settings = __instance._gameSavingSettingsController._gameSavingSetting._settings;
					var value = settings.GetBool(SettingsKey, Plugin.directlyOpenFirstToolInGroup);
					lastToggle.SetValueWithoutNotify(value);
				}
			}
		}
	}
}
