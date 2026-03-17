using HarmonyLib;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;
using ToolShortcuts.Keybindings;

namespace ToolShortcuts.ToolSystem
{
	public static class ToolGroupManagerPatch
	{
		[HarmonyPatch(typeof(ToolGroupService), nameof(ToolGroupService.Load))]
		public static class PatchLoad
		{
			public static void Postfix(ToolGroupService __instance, InputService ____inputService)
			{
				____inputService.AddInputProcessor(__instance);
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupService), nameof(ToolGroupService.EnterToolGroup))]
		public static class PatchSwitchToolGroup
		{
			public static void Postfix(ToolGroupSpec toolGroup, ToolGroupService __instance, InputService ____inputService)
			{
				if(toolGroup == null)
				{
					____inputService.AddInputProcessor(__instance);
				}
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupService), nameof(ToolGroupService.ProcessInput))]
		public static class PatchProcessInput
		{
			public static void Postfix(ref bool __result, ToolGroupService __instance)
			{
				var toolGroupName = isToolGroupKeybindingDown(__instance._inputService);
				if(toolGroupName != null)
				{
					if(SwitchToolGroup(toolGroupName, __instance))
					{
						__result = true;
					}
				}
			}
			
			private static string isToolGroupKeybindingDown(InputService inputService)
			{
				foreach(var (keybinding, tool) in KeybindingKeys.keybindingToTool)
				{
					if (inputService.IsKeyDown(keybinding))
					{
						return tool;
					}
				}
				return null;
			}
			
			private static bool SwitchToolGroup(string toolGroupName, ToolGroupService instance)
			{
				foreach(var toolGroupSpec in instance._toolGroups.Values)
				{
					if(toolGroupSpec.DisplayNameLocKey.Equals(toolGroupName))
					{
						if(instance.ActiveToolGroup == toolGroupSpec)
						{
							instance.ExitToolGroup();
						}
						else
						{
							instance.EnterToolGroup(toolGroupSpec);
							if(Plugin.directlyOpenFirstToolInGroup)
							{
								// Searching for the tool buttons like this is a bit uff, but for now the mod just got to work again.
								foreach(var toolGroupButton in GameDependencyExtractorSingleton.toolButtonService._toolGroupButtons)
								{
									if(toolGroupSpec == toolGroupButton._toolGroup)
									{
										var toolButtons = toolGroupButton._toolButtons;
										GameDependencyExtractorSingleton.toolService.SwitchTool(toolButtons[0].Tool);
										break;
									}
								}
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
