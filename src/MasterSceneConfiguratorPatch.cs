using Bindito.Core;
using HarmonyLib;
using Timberborn.GameScene;
using ToolShortcuts.InputSystem;

namespace ToolShortcuts
{
	public static class MasterSceneConfiguratorPatch
	{
		[HarmonyPatch(typeof(GameSceneConfigurator), "Configure")]
		public static class ConfigurePatch
		{
			private static void Postfix(IContainerDefinition containerDefinition)
			{
				containerDefinition.Bind<KeyBindings>().AsSingleton();
				containerDefinition.Bind<ExtendedInputService>().AsSingleton();
			}
		}
	}
}