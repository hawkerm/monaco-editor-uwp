using System;
using System.Collections.Generic;
using System.Text;
using Uno.Foundation.Interop;

namespace Monaco.Helpers
{
	partial class DebugLogger : IJSObject
	{
		public DebugLogger()
		{
			Handle = JSObjectHandle.Create(this);

			log("created");
		}

		/// <inheritdoc />
		public JSObjectHandle Handle { get; }

		public void log(string message) => Log(message);
	}
}
