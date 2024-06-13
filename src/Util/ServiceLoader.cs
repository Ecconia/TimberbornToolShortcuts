using System;
using System.Collections.Generic;
using System.Reflection;
using Bindito.Core;
using HarmonyLib;
using Timberborn.GameScene;
using Timberborn.MainMenuScene;
using Timberborn.MapEditorScene;

namespace ToolShortcuts.Util
{
	public static class ServiceLoader
	{
		private static readonly List<Action<IContainerDefinition>> loadMainMenu = new List<Action<IContainerDefinition>>();
		private static readonly List<Action<IContainerDefinition>> loadGame = new List<Action<IContainerDefinition>>();
		private static readonly List<Action<IContainerDefinition>> loadMapEditor = new List<Action<IContainerDefinition>>();
		
		public static void loadInMainMenu<T>() where T : class
		{
			loadMainMenu.Add(wrap<T>());
		}
		
		public static void loadInGame<T>() where T : class
		{
			loadGame.Add(wrap<T>());
		}
		
		public static void loadInMapEditor<T>() where T : class
		{
			loadMapEditor.Add(wrap<T>());
		}

		public static void loadEverywhere<T>() where T : class
		{
			var wrap = wrap<T>();
			loadMainMenu.Add(wrap);
			loadGame.Add(wrap);
			loadMapEditor.Add(wrap);
		}
		
		private static Action<IContainerDefinition> wrap<T>() where T : class
		{
			return containerDefinition => containerDefinition.Bind<T>().AsSingleton();
		}
		
		public static void apply(Harmony harmony)
		{
			harmony.Patch(
				typeof(MainMenuSceneInstaller).GetMethod(nameof(MainMenuSceneInstaller.Configure), BindingFlags.Instance | BindingFlags.NonPublic),
				null, new HarmonyMethod(typeof(ServiceLoader).GetMethod(nameof(hookMainMenu), BindingFlags.NonPublic | BindingFlags.Static))
			);
			harmony.Patch(
				typeof(GameSceneInstaller).GetMethod(nameof(GameSceneInstaller.Configure), BindingFlags.Instance | BindingFlags.NonPublic),
				null, new HarmonyMethod(typeof(ServiceLoader).GetMethod(nameof(hookGame), BindingFlags.NonPublic | BindingFlags.Static))
			);
			harmony.Patch(
				typeof(MapEditorSceneInstaller).GetMethod(nameof(MapEditorSceneInstaller.Configure), BindingFlags.Instance | BindingFlags.NonPublic),
				null, new HarmonyMethod(typeof(ServiceLoader).GetMethod(nameof(hookMapEditor), BindingFlags.NonPublic | BindingFlags.Static))
			);
		}
		
		private static void hookMainMenu(IContainerDefinition containerDefinition)
		{
			loadMainMenu.ForEach(a => a(containerDefinition));
			loadMainMenu.Clear();
		}
		
		private static void hookGame(IContainerDefinition containerDefinition)
		{
			loadGame.ForEach(a => a(containerDefinition));
			loadGame.Clear();
		}
		
		private static void hookMapEditor(IContainerDefinition containerDefinition)
		{
			loadMapEditor.ForEach(a => a(containerDefinition));
			loadMapEditor.Clear();
		}
	}
}
