using System.Reflection;
using BepInEx;
using HarmonyLib;
using ToolShortcuts.Keybindings;
using ToolShortcuts.LabelStuff;
using ToolShortcuts.Util;

namespace ToolShortcuts
{
	[BepInPlugin("Mod.ToolShortcuts", "Tool Shortcuts", "2.0.3")]
	public class Plugin : BaseUnityPlugin
	{
		public static bool directlyOpenFirstToolInGroup = true; //Default value, updated via settings UI.
		
		private void Awake()
		{
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			var harmony = new Harmony("Mod.ToolShortcuts");
			
			//A hacky service, to expose singletons. Must be loaded in all scenes, to ensure the up-to-date dependency reference.
			ServiceLoader.loadEverywhere<DependencyExtractorSingleton>();
			//Adds keybindings for tools:
			ServiceLoader.loadEverywhere<ToolShortcutKeybindingInjector>();
			ServiceLoader.loadInGame<KeybindingRebindLabelUpdater>();
			
			ServiceLoader.apply(harmony);
		}
		
		public static void log(string text)
		{
			Console.WriteLine($"[ToolShortcuts / INFO] {text}");
		}
		
		public static void logWarning(string text)
		{
			Console.WriteLine($"[ToolShortcuts / WARNING] {text}");
		}
	}
}
