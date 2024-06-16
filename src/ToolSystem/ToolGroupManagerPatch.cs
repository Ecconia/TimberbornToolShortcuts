using HarmonyLib;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;
using ToolShortcuts.Keybindings;

namespace ToolShortcuts.ToolSystem
{
	public static class ToolGroupManagerPatch
	{
		[HarmonyPatch(typeof(ToolGroupManager), "Load")]
		public static class PatchLoad
		{
			private static void Postfix(ToolGroupManager __instance, InputService ____inputService)
			{
				____inputService.AddInputProcessor(__instance);
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupManager), "SwitchToolGroup")]
		public static class PatchSwitchToolGroup
		{
			private static void Postfix(ToolGroup toolGroup, ToolGroupManager __instance, InputService ____inputService)
			{
				if(toolGroup == null)
				{
					____inputService.AddInputProcessor(__instance);
				}
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupManager), nameof(ToolGroupManager.ProcessInput))]
		public static class PatchProcessInput
		{
			private static void Postfix(ref bool __result, ToolGroupManager __instance, ToolManager ____toolManager)
			{
				var toolGroupName = isToolGroupKeybindingDown;
				if(toolGroupName != null)
				{
					if(SwitchToolGroup(toolGroupName, __instance, ____toolManager))
					{
						__result = true;
					}
				}
			}
			
			private static string isToolGroupKeybindingDown
			{
				get
				{
					var inputService = DependencyExtractorSingleton.getInputService();
					foreach(var (keybinding, tool) in KeybindingKeys.keybindingToTool)
					{
						if (inputService.IsKeyDown(keybinding))
						{
							return tool;
						}
					}
					return null;
				}
			}
			
			private static bool SwitchToolGroup(string toolGroupName, ToolGroupManager instance, ToolManager toolManager)
			{
				foreach(ToolGroupButton toolGroupButton in toolManager._toolButtonService._toolGroupButtons)
				{
					var toolGroup = toolGroupButton._toolGroup;
					if(toolGroup.DisplayNameLocKey.Equals(toolGroupName))
					{
						if(instance.ActiveToolGroup == toolGroup)
						{
							instance.CloseToolGroup();
						}
						else
						{
							instance.SwitchToolGroup(toolGroup);
							if(Plugin.directlyOpenFirstToolInGroup)
							{
								var toolButtons = toolGroupButton._toolButtons;
								toolManager.SwitchTool(toolButtons[0].Tool);
							}
						}
						return true;
					}
				}
				return false;
			}
		}
	}
}
