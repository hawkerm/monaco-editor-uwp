using System;
using System.Collections.Generic;
using System.Text;
using Uno;
using Uno.Foundation.Interop;

namespace Monaco.Helpers
{
	partial class ParentAccessor : IJSObject
	{
		public ParentAccessor()
		{
			getValue(null);
			setValue(null, null);
			setValue(null, null, null);
			getJsonValue(null);
			callAction(null);
			callEvent(null, null);

			Handle = JSObjectHandle.Create(this);

			Console.Error.WriteLine(Handle.Metadata);
		}

		/// <inheritdoc />
		public JSObjectHandle Handle { get; }

		public object getValue(string name)
		{
			if (Handle == null) return null;
			return GetValue(name);
		}

		[Preserve]
		public void setValue(string name, object value)
		{
			if (Handle == null) return;
			SetValue(name, value);
		}

		[Preserve]
		public void setValue(string name, string value, string type)
		{
			if (Handle == null) return;
			SetValue(name, value, type);
		}

		[Preserve]
		public string getJsonValue(string name)
		{
			if (Handle == null) return null;
			return GetJsonValue(name);
		}

		[Preserve]
		public bool callAction(string name)
		{
			if (Handle == null) return false;
			var result = CallAction(name);

			return result;
		}

		[Preserve]
		public string callEvent(string name, string[] parameters)
		{
			if (Handle == null) return null;
			return CallEvent(name, parameters).GetResults();
		}
	}
}
