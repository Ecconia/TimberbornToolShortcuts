using Timberborn.KeyBindingSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using ToolShortcuts.Keybindings;

namespace ToolShortcuts.LabelStuff
{
	public class KeybindingRebindLabelUpdater
	{
		private readonly ToolButtonService toolButtonService;
		
		public KeybindingRebindLabelUpdater(ToolButtonService toolButtonService, EventBus eventBus)
		{
			this.toolButtonService = toolButtonService;
			eventBus._subscriptions.Add(typeof(KeyReboundEvent), this, eventObject => {
				handle((KeyReboundEvent) eventObject);
			});
		}
		
		private void handle(KeyReboundEvent keyReboundEvent)
		{
			var toolName = KeybindingKeys.getToolFromKeybinding(keyReboundEvent.KeyBindingId);
			if (toolName == null)
			{
				return; //Not related to this mod.
			}
			
			foreach(var toolGroupButton in toolButtonService._toolGroupButtons)
			{
				if (toolName.Equals(toolGroupButton._toolGroup.DisplayNameLocKey))
				{
					ToolButtonLabelHelper.removeCustomToolGroupKeybindingLabel(toolGroupButton);
					ToolButtonLabelHelper.addCustomToolGroupKeybindingLabel(toolGroupButton, keyReboundEvent.KeyBindingId);
					return;
				}
			}
		}
	}
}
