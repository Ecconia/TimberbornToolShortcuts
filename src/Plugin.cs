using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Timberborn.ToolSystem;
using ToolShortcuts.InputSystem;

namespace ToolShortcuts
{
	[BepInPlugin("Timberborn.ToolShortcuts", "Tool Shortcuts", "0.4.1")]
	public class Plugin : BaseUnityPlugin
	{
		public static ManualLogSource Log;
		public static KeyBindings KeyBindings;
		public static ExtendedInputService ExtendedInputService;
		public static List<ToolButton> ActiveToolGroupButtons;
		
		private void Awake()
		{
			Log = Logger;
			KeyBindingsConfig.Bind(Config);
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
