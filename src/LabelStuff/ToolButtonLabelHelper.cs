using System;
using System.Collections.Generic;
using Timberborn.KeyBindingSystem;
using Timberborn.ToolSystem;
using ToolShortcuts.Keybindings;
using UnityEngine.UIElements;

namespace ToolShortcuts.LabelStuff
{
	public static class ToolButtonLabelHelper
	{
		//Called by patch (on exit group)
		public static void removeCustomToolKeybindingLabel(ToolButton toolButton)
		{
			removeCustomLabel(toolButton.Root);
		}
		
		//Called when a keybinding got rebound
		public static void removeCustomToolGroupKeybindingLabel(ToolGroupButton toolGroupButton)
		{
			removeCustomLabel(toolGroupButton.Root);
			removeTooltipAddition(toolGroupButton.Root);
		}
		
		//Called by patch (on PostLoad)
		public static void addCustomToolGroupKeybindingLabel(ToolGroupButton toolGroupButton, string keyBindingId = null)
		{
			if (keyBindingId == null)
			{
				keyBindingId = KeybindingKeys.getKeybindingFromTool(toolGroupButton._toolGroup.DisplayNameLocKey);
				if (keyBindingId == null)
				{
					return; //Tool group unknown to this mod.
				}
			}
			
			var binding = getRelevantBinding(keyBindingId);
			if (binding != null)
			{
				toolGroupButton.Root.Add(new KeyBindingLabel(constructKeybindingShortText(binding), false));
				addTooltipKeybinding(toolGroupButton.Root, binding);
			}
		}
		
		//Called by patch (on enter group)
		public static void addCustomToolIndexKeybindingLabel(IReadOnlyList<ToolButton> toolButtons)
		{
			var indexToolShortcuts = KeybindingKeys.ToolIndex.allToolIndices;
			int toolShortcutIndex = 0;
			foreach(var toolButton in toolButtons)
			{
				if (!toolButton.ToolEnabled)
				{
					continue; //Some tools are dev mode, these are ignored.
				}
				if (toolShortcutIndex >= toolButtons.Count)
				{
					break; //We ran out of tool index shortcut keybindings.
				}
				
				var binding = getRelevantBinding(indexToolShortcuts[toolShortcutIndex++]);
				if (binding == null)
				{
					continue; //No keybinding defined - do not add label.
				}
				toolButton.Root.Add(new KeyBindingLabel(constructKeybindingShortText(binding), true));
			}
		}
		
		//Private helpers:
		
		private static void removeCustomLabel(VisualElement element)
		{
			for(int i = 0; i < element.childCount; i++)
			{
				if (element[i].GetType() == typeof(KeyBindingLabel))
				{
					element.RemoveAt(i);
					i -= 1;
				}
			}
		}
		
		private static InputBinding getRelevantBinding(string keyBindingId)
		{
			var bindingSpecifications = DependencyExtractorSingleton.getKeyBindingRegistry().Get(keyBindingId);
			var binding = bindingSpecifications.PrimaryInputBinding.IsDefined ? bindingSpecifications.PrimaryInputBinding : bindingSpecifications.SecondaryInputBinding;
			return !binding.IsDefined ? null : binding; //Null is, when neither keybinding is set.
		}
		
		private static void removeTooltipAddition(VisualElement element)
		{
			var tooltip = element.Q<Label>("Tooltip");
			var text = tooltip.text;
			if (text == null)
			{
				return;
			}
			
			if (!text.EndsWith(')'))
			{
				return;
			}
			
			var start = text.LastIndexOf(" (", StringComparison.Ordinal);
			if (start < -1)
			{
				return;
			}
			
			tooltip.text = text[..start];
		}
		
		private static void addTooltipKeybinding(VisualElement element, InputBinding binding)
		{
			var tooltip = element.Q<Label>("Tooltip");
			var text = tooltip.text;
			if (text == null)
			{
				return;
			}
			
			tooltip.text = $"{text} ({constructKeybindingLongText(binding)})";
		}
		
		private static string constructKeybindingShortText(InputBinding binding)
		{
			var text = DependencyExtractorSingleton.getInputBindingNameService().GetName(binding);
			if (binding.InputModifiers != InputModifiers.None)
			{
				text += '+';
			}
			return text;
		}
		
		private static string constructKeybindingLongText(InputBinding binding)
		{
			return DependencyExtractorSingleton.getInputBindingDescriber().GetInputBindingText(binding);
		}
	}
}
