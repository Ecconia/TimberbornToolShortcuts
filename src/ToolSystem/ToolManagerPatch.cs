using HarmonyLib;
using System.Linq;
using Timberborn.ToolSystem;

namespace ToolShortcuts.ToolSystem
{
	public static class ToolManagerPatch
	{
		[HarmonyPatch(typeof(ToolManager), "ProcessInput")]
		public static class PatchProcessInput
		{
			private static void Postfix(ref bool? __result, ToolManager __instance)
			{
				var toolIndex = Plugin.ExtendedInputService.SwitchTool;
				if(toolIndex.HasValue)
				{
					if(SwitchTool(toolIndex.Value, __instance))
					{
						__result = true;
					}
				}
			}
			
			private static bool SwitchTool(int toolIndex, ToolManager instance)
			{
				if(Plugin.ActiveToolGroupButtons != null && toolIndex < Plugin.ActiveToolGroupButtons.Count())
				{
					var tool = Plugin.ActiveToolGroupButtons.ElementAt(toolIndex).Tool;
					instance.SwitchTool(tool);
					return true;
				}
				return false;
			}
		}
	}
}
