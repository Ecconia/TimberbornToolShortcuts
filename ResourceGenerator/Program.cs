using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Timberborn.KeyBindingSystem;
using ToolShortcuts.Keybindings;

namespace ResourceGenerator;

public static class Program
{
	private struct ToolBinding
	{
		public readonly string id;
		public readonly string localizationKey;
		public readonly string defaultTranslation;
		public readonly (string, InputModifiers)? keybinding;
		
		public ToolBinding(string id, string localizationKey, string defaultTranslation, (string, InputModifiers)? keybinding)
		{
			this.id = id;
			this.localizationKey = $"{KeybindingKeys.LocalizationKeybindingsPrefix}binding.{localizationKey}";
			this.defaultTranslation = defaultTranslation;
			this.keybinding = keybinding;
		}
	}
	
	private struct Group
	{
		public readonly string id;
		public readonly string localizationKey;
		public readonly string defaultTranslation;
		public readonly List<ToolBinding> bindings;
		
		public Group(string id, string localizationKey, string defaultTranslation, List<ToolBinding> bindings)
		{
			this.id = $"{KeybindingKeys.GroupPrefix}{id}";
			this.localizationKey = $"{KeybindingKeys.LocalizationKeybindingsPrefix}group.{localizationKey}";
			this.defaultTranslation = defaultTranslation;
			this.bindings = bindings;
		}
	}
	
	private const int firstGroupOrderIndex = 666333666; //"Random, but high"
	private const int firstKeybindingOrderIndex = 666; //Random
	private static readonly Group[] groups = {
		new ("action_tools", "action_tools", "Tool Shortcuts: Action tools", new List<ToolBinding>
		{
			new (KeybindingKeys.ActionTools.CutTrees, "tree_cutting", "Tree Cutting", ("/Keyboard/g", InputModifiers.Shift)),
			new (KeybindingKeys.ActionTools.PlantCrops, "crop_planting", "Crop Planting", ("/Keyboard/f", InputModifiers.Shift)),
			new (KeybindingKeys.ActionTools.PlantTrees, "tree_planting", "Tree Planting", ("/Keyboard/t", InputModifiers.Shift)),
			new (KeybindingKeys.ActionTools.Demolishing, "demolishing", "Demolishing", ("/Keyboard/x", InputModifiers.None)),
			new (KeybindingKeys.ActionTools.Priority, "priority", "Priority", null),
		}),
		new ("building_tools", "building_tools", "Tool Shortcuts: Building tools", new List<ToolBinding>
		{
			new (KeybindingKeys.BuildingTools.Paths, "paths_and_structures", "Paths and Structures", null),
			new (KeybindingKeys.BuildingTools.Landscaping, "landscaping", "Landscaping", null),
			new (KeybindingKeys.BuildingTools.Storage, "storage", "Storage", null),
			new (KeybindingKeys.BuildingTools.District, "district_managment", "District Management", null),
			new (KeybindingKeys.BuildingTools.Housing, "housing", "Housing", null),
			new (KeybindingKeys.BuildingTools.Water, "water", "Water", null),
			new (KeybindingKeys.BuildingTools.Food, "food", "Food", null),
			new (KeybindingKeys.BuildingTools.Wood, "wood", "Wood", null),
			new (KeybindingKeys.BuildingTools.Metal, "metal", "Metal", null),
			new (KeybindingKeys.BuildingTools.Power, "power", "Power", null),
			new (KeybindingKeys.BuildingTools.Science, "science", "Science", null),
			new (KeybindingKeys.BuildingTools.Automation, "automation", "Automation", null),
			new (KeybindingKeys.BuildingTools.Wellbeing, "well_being", "Well-being", null),
			new (KeybindingKeys.BuildingTools.Decoration, "decoration", "Decoration", null),
			new (KeybindingKeys.BuildingTools.Monuments, "monuments", "Monuments", null),
		}),
		new ("tool_indices", "tool_by_index", "Tool Shortcuts: Tool by index", new List<ToolBinding>
		{
			new (KeybindingKeys.ToolIndex.Index1, "index_1", "Index 1", ("/Keyboard/1", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index2, "index_2", "Index 2", ("/Keyboard/2", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index3, "index_3", "Index 3", ("/Keyboard/3", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index4, "index_4", "Index 4", ("/Keyboard/4", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index5, "index_5", "Index 5", ("/Keyboard/5", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index6, "index_6", "Index 6", ("/Keyboard/6", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index7, "index_7", "Index 7", ("/Keyboard/7", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index8, "index_8", "Index 8", ("/Keyboard/8", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index9, "index_9", "Index 9", ("/Keyboard/9", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index10, "index_10", "Index 10", ("/Keyboard/0", InputModifiers.Shift)),
			new (KeybindingKeys.ToolIndex.Index11, "index_11", "Index 11", null),
			new (KeybindingKeys.ToolIndex.Index12, "index_12", "Index 12", null),
			new (KeybindingKeys.ToolIndex.Index13, "index_13", "Index 13", null),
			new (KeybindingKeys.ToolIndex.Index14, "index_14", "Index 14", null),
			new (KeybindingKeys.ToolIndex.Index15, "index_15", "Index 15", null),
			new (KeybindingKeys.ToolIndex.Index16, "index_16", "Index 16", null),
			new (KeybindingKeys.ToolIndex.Index17, "index_17", "Index 17", null),
			new (KeybindingKeys.ToolIndex.Index18, "index_18", "Index 18", null),
			new (KeybindingKeys.ToolIndex.Index19, "index_19", "Index 19", null),
			new (KeybindingKeys.ToolIndex.Index20, "index_20", "Index 20", null),
			new (KeybindingKeys.ToolIndex.Index21, "index_21", "Index 21", null),
			new (KeybindingKeys.ToolIndex.Index22, "index_22", "Index 22", null),
			new (KeybindingKeys.ToolIndex.Index23, "index_23", "Index 23", null),
			new (KeybindingKeys.ToolIndex.Index24, "index_24", "Index 24", null),
		}),
	};
	
	public static void Main(string[] args)
	{
		var resourceFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "res");
		if (!Directory.Exists(resourceFolderPath))
		{
			Console.WriteLine("'res' folder does not exist. Make sure to set working directory to Git root folder.");
			return;
		}
		var blueprintsFolderPath = Path.Combine(resourceFolderPath, "Blueprints");
		if (Directory.Exists(blueprintsFolderPath))
		{
			// Delete the whole directory to make sure there are no definitions which are not meant to exist.
			Directory.Delete(blueprintsFolderPath, true);
		}
		Directory.CreateDirectory(blueprintsFolderPath);
		
		var localizationEntries = new List<(string id, string value, string comment)>();
		
		var currentGroupIndex = firstGroupOrderIndex;
		foreach (var group in groups)
		{
			localizationEntries.Add((group.localizationKey, group.defaultTranslation, "group"));
			var groupFilePath = Path.Combine(blueprintsFolderPath, $"{group.id}.blueprint.json");
			File.WriteAllText(groupFilePath, $$"""
				{
					"KeyBindingGroupSpec": {
						"Id": "{{group.id}}",
						"Order": {{currentGroupIndex++}},
						"LocKey": "{{group.localizationKey}}"
					}
				}
				
				"""
			);
			int bindingIndex = firstKeybindingOrderIndex;
			foreach (var binding in group.bindings)
			{
				localizationEntries.Add((binding.localizationKey, binding.defaultTranslation, "binding"));
				var bindingFilePath = Path.Combine(blueprintsFolderPath, $"{binding.id}.blueprint.json");
				var primary = !binding.keybinding.HasValue ? "" : $$"""
					,
						"PrimaryInputBindingSpec": {
							"Path": "{{binding.keybinding.Value.Item1}}",
							"InputModifiers": "{{binding.keybinding.Value.Item2}}",
							"Unchangeable": false
						}
					""";
				
				File.WriteAllText(bindingFilePath, $$"""
					{
						"KeyBindingSpec": {
							"Id": "{{binding.id}}",
							"GroupId": "{{group.id}}",
							"LocKey": "{{binding.localizationKey}}",
							"Order": {{bindingIndex++}},
							"AllowOtherModifiers": false,
							"DevModeOnly": false
						}{{primary}}
					}
					
					"""
				);
			}
		}
		
		var localizationBlob = string.Join("", localizationEntries.Select(entry => $"{entry.id},{entry.value},{entry.comment}\n"));
		File.WriteAllText(Path.Combine(resourceFolderPath, "Localizations", "enUS_keybindings.csv"), $"""
			ID,Text,Comment
			{localizationBlob}
			"""
		);
	}
}
