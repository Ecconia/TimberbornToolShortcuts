using System;
using System.Reflection;
using Bindito.Core;
using HarmonyLib;
using Timberborn.ModManagerScene;
using ToolShortcuts.LabelStuff;

namespace ToolShortcuts
{
	public class Plugin : IModStarter
	{
		public static bool directlyOpenFirstToolInGroup = true; //Default value, updated via settings UI.
		
		public void StartMod()
		{
			new Harmony("Mod:ToolShortcuts").PatchAll();
		}
		
		[Context("Game")]
		public class KeybindingRebindLabelConfigurator : IConfigurator
		{
			public void Configure(IContainerDefinition containerDefinition)
			{
				//Adds keybindings for tools:
				containerDefinition.Bind<KeybindingRebindLabelUpdater>().AsSingleton();
			}
		}
		
		[Context("Game")]
		[Context("MainMenu")]
		[Context("MapEditor")]
		public class DependencyExtractorConfigurator : IConfigurator
		{
			public void Configure(IContainerDefinition containerDefinition)
			{
				//A hacky service, to expose singletons. Must be loaded in all scenes, to ensure the up-to-date dependency reference.
				containerDefinition.Bind<DependencyExtractorSingleton>().AsSingleton();
			}
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
