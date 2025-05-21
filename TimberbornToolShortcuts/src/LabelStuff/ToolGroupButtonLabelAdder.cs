using HarmonyLib;
using System.Collections.Generic;
using Timberborn.ToolSystem;

namespace ToolShortcuts.LabelStuff
{
	public static class ToolGroupButtonPatch
	{
		[HarmonyPatch(typeof(ToolGroupButton), nameof(ToolGroupButton.OnToolGroupEntered))]
		public static class PatchToolGroupButtonToolLabelsEntered
		{
			public static void Postfix(ToolGroupEnteredEvent toolGroupEnteredEvent, ToolGroupButton __instance, IReadOnlyList<ToolButton> ____toolButtons)
			{
				if (toolGroupEnteredEvent.ToolGroup != __instance._toolGroup)
				{
					return; //Event is received on all buttons, filter to the current one.
				}
				
				ToolButtonLabelHelper.addCustomToolIndexKeybindingLabel(__instance._toolGroupManager._inputService._keyBindingRegistry, ____toolButtons);
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupButton), nameof(ToolGroupButton.OnToolGroupExited))]
		public static class PatchToolGroupButtonToolLabelsExited
		{
			public static void Postfix(ToolGroupExitedEvent toolGroupExitedEvent, ToolGroupButton __instance, IReadOnlyList<ToolButton> ____toolButtons)
			{
				if (toolGroupExitedEvent.ToolGroup != __instance._toolGroup)
				{
					return; //Event is received on all buttons, filter to the current one.
				}
				
				foreach(var toolButton in ____toolButtons)
				{
					ToolButtonLabelHelper.removeCustomToolKeybindingLabel(toolButton);
				}
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupButton), nameof(ToolGroupButton.PostLoad))]
		public static class PatchPostLoad
		{
			public static void Postfix(ToolGroupButton __instance)
			{
				ToolButtonLabelHelper.addCustomToolGroupKeybindingLabel(__instance);
			}
		}
	}
}
