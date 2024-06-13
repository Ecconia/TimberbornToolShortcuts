using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ToolShortcuts.Keybindings;
using ToolShortcuts.LabelStuff;
using ToolShortcuts.Util;

namespace ToolShortcuts
{
	[BepInPlugin("Mod.ToolShortcuts", "Tool Shortcuts", "2.0.0")]
	public class Plugin : BaseUnityPlugin
	{
		public static bool directlyOpenFirstToolInGroup = true; //Default value, updated via settings UI.
		public static ManualLogSource Log;
		
		private void Awake()
		{
			Log = Logger;
			
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			var harmony = new Harmony("Mod.ToolShortcuts");
			
			//A hacky service, to expose singletons. Must be loaded in all scenes, to ensure the up-to-date dependency reference.
			ServiceLoader.loadEverywhere<DependencyExtractorSingleton>();
			ServiceLoader.loadInGame<DependencyExtractorSingletonGameplay>();
			//Adds keybindings for tools:
			ServiceLoader.loadEverywhere<ToolShortcutKeybindingInjector>();
			ServiceLoader.loadInGame<KeybindingRebindLabelUpdater>();
			
			ServiceLoader.apply(harmony);
		}
	}
}
