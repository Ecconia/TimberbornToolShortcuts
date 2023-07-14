using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Timberborn.InputSystem;
using Timberborn.ToolSystem;

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
		
		[HarmonyPatch(typeof(ToolGroupManager), "ProcessInput")]
		public static class PatchProcessInput
		{
			private static readonly FieldInfo ToolButtonServiceField;
			private static readonly FieldInfo ToolGroupButtonsField;
			private static readonly FieldInfo ToolGroupField;
			private static readonly FieldInfo ToolButtonsField;
			
			static PatchProcessInput()
			{
				ToolButtonServiceField = typeof(ToolManager).GetField("_toolButtonService",
					BindingFlags.NonPublic | BindingFlags.Instance);
				ToolGroupButtonsField = typeof(ToolButtonService).GetField("_toolGroupButtons",
					BindingFlags.NonPublic | BindingFlags.Instance);
				ToolGroupField = typeof(ToolGroupButton).GetField("_toolGroup",
					BindingFlags.NonPublic | BindingFlags.Instance);
				ToolButtonsField = typeof(ToolGroupButton).GetField("_toolButtons",
					BindingFlags.NonPublic | BindingFlags.Instance);
			}
			
			private static void Postfix(ref bool __result, ToolGroupManager __instance, ToolManager ____toolManager)
			{
				var toolGroupName = Plugin.ExtendedInputService.SwitchToolGroup;
				if(toolGroupName.HasValue)
				{
					if(SwitchToolGroup(toolGroupName.Value, __instance, ____toolManager))
					{
						__result = true;
					}
				}
			}
			
			private static bool SwitchToolGroup(ToolGroupName toolGroupName, ToolGroupManager instance, ToolManager toolManager)
			{
				var toolButtonService = (ToolButtonService) ToolButtonServiceField.GetValue(toolManager);
				var toolGroupButtons = (List<ToolGroupButton>) ToolGroupButtonsField.GetValue(toolButtonService);
				
				foreach(ToolGroupButton groupBtn in toolGroupButtons)
				{
					var toolGroup = (ToolGroup) ToolGroupField.GetValue(groupBtn);
					if(ToolGroupNameHelper.FromNameLockey(toolGroup.DisplayNameLocKey) == toolGroupName)
					{
						if(instance.ActiveToolGroup == toolGroup)
						{
							instance.CloseToolGroup();
						}
						else
						{
							instance.SwitchToolGroup(toolGroup);
							if(KeyBindings.DirectlyOpenFirstToolInGroup)
							{
								var toolButtons = (List<ToolButton>) ToolButtonsField.GetValue(groupBtn);
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
