namespace ToolShortcuts.ToolSystem
{
	public enum ToolGroupName
	{
		TreeCutting,
		FieldsPlanting,
		ForestryPlanting,
		Demolishing,
		Priority,
		Paths,
		Landscaping,
		Storage,
		Labor,
		Housing,
		Water,
		Food,
		Wood,
		Metal,
		Power,
		Science,
		Leisure,
		Decoration,
		Monuments,
		MapEditor,
		Ruins,
	}
	
	public static class ToolGroupNameHelper
	{
		public static ToolGroupName? FromNameLockey(string nameLockey)
		{
			return nameLockey switch
			{
				"ToolGroups.TreeCutting" => ToolGroupName.TreeCutting,
				"ToolGroups.FieldsPlanting" => ToolGroupName.FieldsPlanting,
				"ToolGroups.ForestryPlanting" => ToolGroupName.ForestryPlanting,
				"ToolGroups.Demolishing" => ToolGroupName.Demolishing,
				"ToolGroups.Priority" => ToolGroupName.Priority,
				"ToolGroups.Paths" => ToolGroupName.Paths,
				"ToolGroups.Landscaping" => ToolGroupName.Landscaping,
				"ToolGroups.Storage" => ToolGroupName.Storage,
				"ToolGroups.Labor" => ToolGroupName.Labor,
				"ToolGroups.Housing" => ToolGroupName.Housing,
				"ToolGroups.Water" => ToolGroupName.Water,
				"ToolGroups.Food" => ToolGroupName.Food,
				"ToolGroups.Wood" => ToolGroupName.Wood,
				"ToolGroups.Metal" => ToolGroupName.Metal,
				"ToolGroups.Power" => ToolGroupName.Power,
				"ToolGroups.Science" => ToolGroupName.Science,
				"ToolGroups.Leisure" => ToolGroupName.Leisure,
				"ToolGroups.Decoration" => ToolGroupName.Decoration,
				"ToolGroups.Monuments" => ToolGroupName.Monuments,
				"ToolGroups.MapEditor" => ToolGroupName.MapEditor,
				"ToolGroups.Ruins" => ToolGroupName.Ruins,
				_ => null,
			};
		}
	}
}
