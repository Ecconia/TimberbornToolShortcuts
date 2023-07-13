using Bindito.Core;
using HarmonyLib;
using Timberborn.GameScene;

namespace ToolShortcuts
{
    class MasterSceneConfiguratorPatch
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