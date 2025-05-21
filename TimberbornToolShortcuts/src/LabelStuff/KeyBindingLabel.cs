using UnityEngine;
using UnityEngine.UIElements;

namespace ToolShortcuts.LabelStuff
{
	public class KeyBindingLabel : Label
	{
		public KeyBindingLabel(string keyBindingText, bool isTool) : base(keyBindingText)
		{
			style.position = Position.Absolute;
			style.top = isTool ? 0 : 1;
			style.left = style.right = isTool ? 3 : 4;
			
			style.unityTextAlign = TextAnchor.UpperLeft;
			style.overflow = Overflow.Hidden;
			style.fontSize = 10;
			style.color = new Color(0.85f, 0.75f, 0.64f);
			style.backgroundColor = new Color(0f, 0f, 0f, 0.0f);
		}
	}
}
