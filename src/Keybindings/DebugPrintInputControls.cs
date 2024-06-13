using System.Collections.Generic;
using System.Linq;
using Timberborn.SingletonSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace ToolShortcuts.Keybindings
{
	public class DebugPrintInputControls : ILoadableSingleton
	{
		public void Load()
		{
			var loaded_layouts = new List<InputControlLayout>();
			foreach(var layout_name in InputSystem.ListLayouts())
			{
				var loaded = InputSystem.LoadLayout(layout_name);
				if (!loaded.isGenericTypeOfDevice)
				{
					continue;
				}
				loaded_layouts.Add(loaded);
			}
			
			Plugin.Log.LogInfo($"Flag layouts:");
			foreach(var layout in loaded_layouts)
			{
				Plugin.Log.LogInfo($"- {layout.name} Generic: {layout.isGenericTypeOfDevice} Hidden: {layout.hideInUI} Override: {layout.isOverride} Background: {layout.canRunInBackground} Noisy: {layout.isNoisy}");
			}
			
			Plugin.Log.LogInfo($"Listing input-controls:");
			var layouts = loaded_layouts.Where(l => l.isGenericTypeOfDevice);
			foreach (var layout in layouts)
			{
				if (layout.controls.Count == 0)
				{
					//Skip this entry, else exception when adding device.
					continue;
				}
				var device = InputSystem.AddDevice(layout.name);
				foreach (var control in device.allControls)
				{
					var relativePath = control.path.Substring(device.path.Length);
					Plugin.Log.LogInfo($"- <{layout.name}>{relativePath}");
				}
				InputSystem.RemoveDevice(device);
			}
		}
	}
}
