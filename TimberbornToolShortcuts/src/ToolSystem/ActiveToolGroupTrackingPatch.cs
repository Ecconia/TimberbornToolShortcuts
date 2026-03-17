using System.Collections.Generic;
using HarmonyLib;
using Timberborn.ToolButtonSystem;
using Timberborn.ToolSystem;

namespace ToolShortcuts.ToolSystem
{
	public static class ActiveToolGroupTrackingPatch
	{
		public static List<ToolButton> activeToolGroupButtons;
		
		[HarmonyPatch(typeof(ToolGroupButton), nameof(ToolGroupButton.OnToolGroupEntered))]
		public static class PatchOnToolGroupEntered
		{
			public static void Postfix(ToolGroupEnteredEvent toolGroupOpenedEvent, ToolGroupSpec ____toolGroup, List<ToolButton> ____toolButtons)
			{
				if (toolGroupOpenedEvent.ToolGroup != ____toolGroup)
				{
					return;
				}
				activeToolGroupButtons = ____toolButtons;
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupButton), nameof(ToolGroupButton.OnToolGroupExited))]
		public static class PatchOnToolGroupExited
		{
			public static void Postfix(ToolGroupExitedEvent toolGroupExitedEvent, ToolGroupSpec ____toolGroup)
			{
				//TimberAPI has the habit of replacing references, means one has to default to comparing strings.
				if (!____toolGroup.DisplayNameLocKey.Equals(toolGroupExitedEvent.ToolGroup?.DisplayNameLocKey))
				{
					return;
				}
				activeToolGroupButtons = null;
			}
		}
	}
}
