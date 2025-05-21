using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Timberborn.KeyBindingSystem;

namespace ToolShortcuts.Keybindings
{
	public static class ToolShortcutKeybindingInjector
	{
		[HarmonyPatch(typeof(KeyBindingRegistry), nameof(KeyBindingRegistry.Load))]
		public static class PatchForKeybindings
		{
			public static void Postfix(KeyBindingRegistry __instance, KeyBindingSpecificationService ____keyBindingSpecificationService)
			{
				registerKeybindings(__instance, ____keyBindingSpecificationService);
			}
		}
		
		[HarmonyPatch(typeof(KeyBindingGroupSpecificationService), nameof(KeyBindingGroupSpecificationService.Load))]
		public static class PatchForKeybindingGroups
		{
			public static void Postfix(KeyBindingGroupSpecificationService __instance)
			{
				registerKeybindingGroups(__instance);
			}
		}
		
		private struct ToolBinding
		{
			public readonly string id;
			public readonly string name;
			public readonly (string, InputModifiers)? keybinding;
			
			public ToolBinding(string id, string name, (string, InputModifiers)? keybinding)
			{
				this.id = id;
				this.name = name;
				this.keybinding = keybinding;
			}
		}
		
		private struct Group
		{
			public readonly string id;
			public readonly string name;
			public readonly List<ToolBinding> bindings;
			
			public Group(string id, string name, List<ToolBinding> bindings)
			{
				this.id = id;
				this.name = name;
				this.bindings = bindings;
			}
		}
		
		private const int firstGroupOrderIndex = 666333666; //"Random, but high"
		private static readonly Group[] groups = {
			new (KeybindingKeys.GroupPrefix + "action_tools", "Tool Shortcuts: Action tools", new List<ToolBinding>
			{
				new (KeybindingKeys.ActionTools.CutTrees, "Tree Cutting", ("/Keyboard/g", InputModifiers.Shift)),
				new (KeybindingKeys.ActionTools.PlantCrops, "Crop Planting", ("/Keyboard/f", InputModifiers.Shift)),
				new (KeybindingKeys.ActionTools.PlantTrees, "Tree Planting", ("/Keyboard/t", InputModifiers.Shift)),
				new (KeybindingKeys.ActionTools.Demolishing, "Demolishing", ("/Keyboard/x", InputModifiers.None)),
				new (KeybindingKeys.ActionTools.Priority, "Priority", null),
			}),
			new ("mod.tool_shortcuts.keybindings_group.building_tools", "Tool Shortcuts: Building tools", new List<ToolBinding>
			{
				new (KeybindingKeys.BuildingTools.Paths, "Paths and Structures", null),
				new (KeybindingKeys.BuildingTools.Landscaping, "Landscaping", null),
				new (KeybindingKeys.BuildingTools.Storage, "Storage", null),
				new (KeybindingKeys.BuildingTools.District, "District Management", null),
				new (KeybindingKeys.BuildingTools.Housing, "Housing", null),
				new (KeybindingKeys.BuildingTools.Water, "Water", null),
				new (KeybindingKeys.BuildingTools.Food, "Food", null),
				new (KeybindingKeys.BuildingTools.Wood, "Wood", null),
				new (KeybindingKeys.BuildingTools.Metal, "Metal", null),
				new (KeybindingKeys.BuildingTools.Power, "Power", null),
				new (KeybindingKeys.BuildingTools.Science, "Science", null),
				new (KeybindingKeys.BuildingTools.Wellbeing, "Well-being", null),
				new (KeybindingKeys.BuildingTools.Decoration, "Decoration", null),
				new (KeybindingKeys.BuildingTools.Monuments, "Monuments", null),
			}),
			new ("mod.tool_shortcuts.keybindings_group.tool_indices", "Tool Shortcuts: Tool by index", new List<ToolBinding>
			{
				new (KeybindingKeys.ToolIndex.Index1, "Index 1", ("/Keyboard/1", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index2, "Index 2", ("/Keyboard/2", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index3, "Index 3", ("/Keyboard/3", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index4, "Index 4", ("/Keyboard/4", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index5, "Index 5", ("/Keyboard/5", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index6, "Index 6", ("/Keyboard/6", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index7, "Index 7", ("/Keyboard/7", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index8, "Index 8", ("/Keyboard/8", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index9, "Index 9", ("/Keyboard/9", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index10, "Index 10", ("/Keyboard/0", InputModifiers.Shift)),
				new (KeybindingKeys.ToolIndex.Index11, "Index 11", null),
				new (KeybindingKeys.ToolIndex.Index12, "Index 12", null),
				new (KeybindingKeys.ToolIndex.Index13, "Index 13", null),
				new (KeybindingKeys.ToolIndex.Index14, "Index 14", null),
				new (KeybindingKeys.ToolIndex.Index15, "Index 15", null),
				new (KeybindingKeys.ToolIndex.Index16, "Index 16", null),
			}),
		};
		
		private static void registerKeybindings(KeyBindingRegistry keyBindingRegistry, KeyBindingSpecificationService keyBindingSpecificationService)
		{
			var keybindingsToInject = groups.SelectMany(group => {
				int index = 666;
				return group.bindings.Select(binding => new KeyBindingSpecification(
					binding.id,
					group.id,
					binding.name,
					index++,
					binding.keybinding == null
						? InputBindingSpecification.Undefined
						: new InputBindingSpecification(
							binding.keybinding.Value.Item1,
							binding.keybinding.Value.Item2,
							null
						),
					InputBindingSpecification.Undefined,
					false,
					false,
					false
				));
			}).ToList();
			
			// Inject default values, allowing a player to reset its bindings:
			keyBindingSpecificationService._defaultSpecifications.AddRange(keybindingsToInject);
			// Inject keybindings into key registry, for them to be functional:
			keybindingsToInject.ForEach(defaultKeybinding => {
				//First customize them to load previous modifications from settings:
				keyBindingSpecificationService.TryGetCustomBinding(defaultKeybinding, out var customKeybinding);
				keyBindingRegistry.Add(customKeybinding);
			});
		}
		
		private static void registerKeybindingGroups(KeyBindingGroupSpecificationService keyBindingGroupSpecificationService)
		{
			var currentGroupIndex = firstGroupOrderIndex;
			var groupsToInject = groups.Select(groupSpec => new KeyBindingGroupSpecification(
				groupSpec.id, 
				groupSpec.name, 
				currentGroupIndex++, 
				false
			)).ToList();
			insertSorted(
				keyBindingGroupSpecificationService._keyBindingGroups,
				groupsToInject,
				firstGroupOrderIndex,
				e => e.Order,
				e => e.Id
			);
		}
		
		private static void insertSorted<T>(List<T> list, List<T> newEntries, int firstIndex, Func<T, int> orderGetter, Func<T, string> debugGetter)
		{
			// Make sure, that vanilla loaded its values, else it would overwrite the custom injected entries.
			if (list.Count == 0)
			{
				throw new Exception($"Attempted to insert new '{typeof(T).Name}' into the game, but the vanilla values have not been loaded into referenced list yet. This is not supported!");
			}
			
			// If our first index is higher than the last entry, we can append our entries without worry.
			if (orderGetter(list.Last()) <= firstIndex)
			{
				list.AddRange(newEntries);
				return;
			}
			
			// There are keybindings with a higher index than our first entry. Find the index and ensure the range is valid.
			int indexToInsertAt = list.FindIndex(e => orderGetter(e) > firstIndex); // Never -1, as there must be an element with higher index (see above).
			//With an index to insert entries found, make sure, that range of entries to insert won't collide with the next entry:
			if (orderGetter(list[indexToInsertAt]) <= (firstIndex + newEntries.Count - 1))
			{
				throw new Exception($"Attempted to insert {newEntries.Count}x new '{typeof(T).Name}' into the game with starting index '{firstIndex}', but this would collide with another entry order '{debugGetter(list[indexToInsertAt])}'");
			}
			
			//Proceed insertion:
			list.InsertRange(indexToInsertAt, newEntries);
		}
	}
}
