using System;
using Timberborn.InputSystem;
using Timberborn.KeyBindingSystem;
using Timberborn.KeyBindingSystemUI;
using Timberborn.SettingsSystem;

namespace ToolShortcuts
{
	//Exposes Dependencies in a very bad way. This is done to access dependencies from static Harmony patches. Meh.
	public class DependencyExtractorSingleton
	{
		private static ISettings settings;
		private static InputService inputService;
		private static KeyBindingRegistry keyBindingRegistry;
		private static InputBindingNameService inputBindingNameService;
		private static InputBindingDescriber inputBindingDescriber;
		
		public DependencyExtractorSingleton(
			ISettings settings,
			InputService inputService,
			KeyBindingRegistry keyBindingRegistry,
			InputBindingNameService inputBindingNameService,
			InputBindingDescriber inputBindingDescriber
		) {
			DependencyExtractorSingleton.settings = settings;
			DependencyExtractorSingleton.inputService = inputService;
			DependencyExtractorSingleton.keyBindingRegistry = keyBindingRegistry;
			DependencyExtractorSingleton.inputBindingNameService = inputBindingNameService;
			DependencyExtractorSingleton.inputBindingDescriber = inputBindingDescriber;
		}
		
		public static ISettings getSettings()
		{
			if (settings == null)
			{
				throw new Exception("Attempted to get settings, but they had not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return settings;
		}
		
		public static InputService getInputService()
		{
			if (inputService == null)
			{
				throw new Exception("Attempted to get input service, but it has not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return inputService;
		}
		
		public static KeyBindingRegistry getKeyBindingRegistry()
		{
			if (keyBindingRegistry == null)
			{
				throw new Exception("Attempted to get keybinding registry, but it has not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return keyBindingRegistry;
		}
		
		public static InputBindingNameService getInputBindingNameService()
		{
			if (inputBindingNameService == null)
			{
				throw new Exception("Attempted to get input binding name service, but it has not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return inputBindingNameService;
		}
		
		public static InputBindingDescriber getInputBindingDescriber()
		{
			if (inputBindingDescriber == null)
			{
				throw new Exception("Attempted to get input binding describer, but it has not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return inputBindingDescriber;
		}
	}
}
