using System.Collections.Generic;

namespace ToolShortcuts.Keybindings
{
	public static class KeybindingKeys
	{
		private const string ModPrefix = "mod.tool_shortcuts.";
		public const string GroupPrefix = ModPrefix + "keybindings_group.";
		private const string ToolPrefix = ModPrefix + "keybindings_tool.";
		
		public static class ActionTools
		{
			private const string Prefix = ToolPrefix + "action.";
			public const string CutTrees = Prefix + "cut_tree";
			public const string PlantCrops = Prefix + "plant_crop";
			public const string PlantTrees = Prefix + "plant_tree";
			public const string Demolishing = Prefix + "demolish";
			public const string Priority = Prefix + "priority";
		}
		
		public static class BuildingTools
		{
			private const string Prefix = ToolPrefix + "building.";
			public const string Paths = Prefix + "paths";
			public const string Landscaping = Prefix + "landscaping";
			public const string Storage = Prefix + "storage";
			public const string District = Prefix + "district";
			public const string Housing = Prefix + "housing";
			public const string Water = Prefix + "water";
			public const string Food = Prefix + "food";
			public const string Wood = Prefix + "wood";
			public const string Metal = Prefix + "metal";
			public const string Power = Prefix + "power";
			public const string Science = Prefix + "science";
			public const string Wellbeing = Prefix + "wellbeing";
			public const string Decoration = Prefix + "decoration";
			public const string Monuments = Prefix + "monuments";
		}
		
		public static class ToolIndex
		{
			private const string Prefix = ToolPrefix + "index.";
			public const string Index1 = Prefix + "i1";
			public const string Index2 = Prefix + "i2";
			public const string Index3 = Prefix + "i3";
			public const string Index4 = Prefix + "i4";
			public const string Index5 = Prefix + "i5";
			public const string Index6 = Prefix + "i6";
			public const string Index7 = Prefix + "i7";
			public const string Index8 = Prefix + "i8";
			public const string Index9 = Prefix + "i9";
			public const string Index10 = Prefix + "i10";
			public const string Index11 = Prefix + "i11";
			public const string Index12 = Prefix + "i12";
			public const string Index13 = Prefix + "i13";
			public const string Index14 = Prefix + "i14";
			public const string Index15 = Prefix + "i15";
			public const string Index16 = Prefix + "i16";
			
			public static readonly List<string> allToolIndices = new List<string>
			{
				Index1, Index2, Index3, Index4,
				Index5, Index6, Index7, Index8,
				Index9, Index10, Index11, Index12,
				Index13, Index14, Index15, Index16,
			};
		}
		
		private static readonly Dictionary<string, string> toolToKeybinding = new Dictionary<string, string>();
		public static readonly Dictionary<string, string> keybindingToTool = new Dictionary<string, string>();
		
		static KeybindingKeys()
		{
			var pairs = new (string, string)[]
			{
				(ActionTools.CutTrees, "ToolGroups.TreeCutting"),
				(ActionTools.PlantCrops, "ToolGroups.FieldsPlanting"),
				(ActionTools.PlantTrees, "ToolGroups.ForestryPlanting"),
				(ActionTools.Demolishing, "ToolGroups.Demolishing"),
				(ActionTools.Priority, "ToolGroups.Priority"),
				(BuildingTools.Paths, "ToolGroups.Paths"),
				(BuildingTools.Landscaping, "ToolGroups.Landscaping"),
				(BuildingTools.Storage, "ToolGroups.Storage"),
				(BuildingTools.District, "ToolGroups.DistrictManagement"),
				(BuildingTools.Housing, "ToolGroups.Housing"),
				(BuildingTools.Water, "ToolGroups.Water"),
				(BuildingTools.Food, "ToolGroups.Food"),
				(BuildingTools.Wood, "ToolGroups.Wood"),
				(BuildingTools.Metal, "ToolGroups.Metal"),
				(BuildingTools.Power, "ToolGroups.Power"),
				(BuildingTools.Science, "ToolGroups.Science"),
				(BuildingTools.Wellbeing, "Wellbeing.DisplayName"),
				(BuildingTools.Decoration, "ToolGroups.Decoration"),
				(BuildingTools.Monuments, "ToolGroups.Monuments"),
			};
			foreach(var (keybinding, tool) in pairs)
			{
				keybindingToTool[keybinding] = tool;
				toolToKeybinding[tool] = keybinding;
			}
		}
		
		public static string getKeybindingFromTool(string tool)
		{
			return toolToKeybinding.GetValueOrDefault(tool);
		}
		
		public static string getToolFromKeybinding(string keybinding)
		{
			return keybindingToTool.GetValueOrDefault(keybinding);
		}
	}
}
