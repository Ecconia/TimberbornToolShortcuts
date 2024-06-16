using System;
using Timberborn.KeyBindingSystem;
using Timberborn.KeyBindingSystemUI;

namespace ToolShortcuts
{
	//Exposes Dependencies in a very bad way. This is done to access dependencies from static Harmony patches. Meh.
	public class DependencyExtractorSingleton
	{
		public static InputBindingNameService inputBindingNameService { private set; get; }
		public static InputBindingDescriber inputBindingDescriber { private set; get; }
		
		public DependencyExtractorSingleton(
			InputBindingNameService inputBindingNameService,
			InputBindingDescriber inputBindingDescriber
		) {
			DependencyExtractorSingleton.inputBindingNameService = inputBindingNameService;
			DependencyExtractorSingleton.inputBindingDescriber = inputBindingDescriber;
		}
	}
}
