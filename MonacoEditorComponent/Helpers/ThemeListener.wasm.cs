using System;
using System.Collections.Generic;
using System.Text;
using Uno.Foundation.Interop;

namespace Monaco.Helpers
{
	partial class ThemeListener : IJSObject
	{
		partial void PartialCtor()
		{
			Handle = JSObjectHandle.Create(this);

			getCurrentThemeName();
			getIsHighContrast();
		}

		public JSObjectHandle Handle { get; private set; }

		public string getCurrentThemeName() => CurrentThemeName;

		public bool getIsHighContrast() => IsHighContrast;
	}
}
