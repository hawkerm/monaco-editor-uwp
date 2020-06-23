using System;
using System.Collections.Generic;
using System.Text;
using Uno.Foundation.Interop;

namespace Monaco.Helpers
{
	partial class KeyboardListener : IJSObject
	{
		public KeyboardListener()
		{
			keyDown(0, false, false, false, false);

			Handle = JSObjectHandle.Create(this);
		}

		/// <inheritdoc />
		public JSObjectHandle Handle { get; }

		public bool keyDown(int keycode, bool ctrl, bool shift, bool alt, bool meta)
		{
			if (Handle == null) return false;
			return KeyDown(keycode, ctrl, shift, alt, meta);
		}
	}
}
