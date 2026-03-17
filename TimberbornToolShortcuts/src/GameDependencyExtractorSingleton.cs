using Timberborn.ToolButtonSystem;
using Timberborn.ToolSystem;

namespace ToolShortcuts
{
	//Exposes Dependencies in a very bad way. This is done to access dependencies from static Harmony patches. Meh.
	public class GameDependencyExtractorSingleton
	{
		public static ToolButtonService toolButtonService { private set; get; }
		public static ToolService toolService { private set; get; }
		
		public GameDependencyExtractorSingleton(
			ToolButtonService toolButtonService,
			ToolService toolService
		) {
			GameDependencyExtractorSingleton.toolButtonService = toolButtonService;
			GameDependencyExtractorSingleton.toolService = toolService;
		}
	}
}
