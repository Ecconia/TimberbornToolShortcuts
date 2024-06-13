using System.Collections.Generic;
using HarmonyLib;
using Timberborn.ToolSystem;

namespace ToolShortcuts.ToolSystem
{
	public static class ActiveToolGroupTrackingPatch
	{
		public static List<ToolButton> activeToolGroupButtons;
		
		[HarmonyPatch(typeof(ToolGroupButton), "OnToolGroupEntered")]
		public static class PatchOnToolGroupEntered
		{
			private static void Postfix(ToolGroupEnteredEvent toolGroupEnteredEvent, ToolGroup ____toolGroup, List<ToolButton> ____toolButtons)
			{
				if (toolGroupEnteredEvent.ToolGroup != ____toolGroup)
				{
					return;
				}
				activeToolGroupButtons = ____toolButtons;
			}
		}
		
		[HarmonyPatch(typeof(ToolGroupButton), "OnToolGroupExited")]
		public static class PatchOnToolGroupExited
		{
			private static void Postfix(ToolGroupExitedEvent toolGroupExitedEvent, ToolGroup ____toolGroup)
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
