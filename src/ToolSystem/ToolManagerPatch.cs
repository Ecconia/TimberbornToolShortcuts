using HarmonyLib;
using System.Linq;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;
using ToolShortcuts.Keybindings;

namespace ToolShortcuts.ToolSystem
{
	public static class ToolManagerPatch
	{
		[HarmonyPatch(typeof(ToolManager), nameof(ToolManager.ProcessInput))]
		public static class PatchProcessInput
		{
			public static void Postfix(ref bool? __result, ToolManager __instance)
			{
				var toolIndex = IsToolIndexKeybindingDown(__instance._inputService);
				if(toolIndex != null)
				{
					if(SwitchTool(toolIndex.Value, __instance))
					{
						__result = true;
					}
				}
			}
			
			private static bool SwitchTool(int toolIndex, ToolManager instance)
			{
				var activeToolGroupButtons = ActiveToolGroupTrackingPatch.activeToolGroupButtons;
				if(activeToolGroupButtons != null && toolIndex < activeToolGroupButtons.Count())
				{
					var tool = activeToolGroupButtons.ElementAt(toolIndex).Tool;
					instance.SwitchTool(tool);
					return true;
				}
				return false;
			}
			
			private static int? IsToolIndexKeybindingDown(InputService inputService)
			{
				int index = 0;
				foreach(var toolIndexKeybinding in KeybindingKeys.ToolIndex.allToolIndices)
				{
					if (inputService.IsKeyDown(toolIndexKeybinding))
					{
						return index;
					}
					index++;
				}
				return null;
			}
		}
	}
}
