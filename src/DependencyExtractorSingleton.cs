using System;
using Timberborn.SettingsSystem;

namespace ToolShortcuts
{
	//Exposes Dependencies in a very bad way. This is done to access dependencies from static Harmony patches. Meh.
	public class DependencyExtractorSingleton
	{
		private static ISettings settings;
		
		public DependencyExtractorSingleton(ISettings settings)
		{
			DependencyExtractorSingleton.settings = settings;
		}
		
		public static ISettings getSettings()
		{
			if (settings == null)
			{
				throw new Exception("Attempted to get settings, but they had not been set. Developer make sure to first and always register singleton, before Harmony patches trigger.");
			}
			return settings;
		}
	}
}
