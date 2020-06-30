using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Uno;
using Uno.Extensions.Specialized;
using Uno.Foundation.Interop;
using Windows.Foundation;
using Uno.Foundation;
using Windows.Data.Text;
using Windows.Security.Cryptography.Certificates;

namespace Monaco.Helpers
{
    partial class ParentAccessor : IJSObject
    {
        partial void PartialCtor()
        {
            getValue(null);
            setValue(null, null);
            setValueWithType(null, null, null);
            getJsonValue(null);
            callAction(null);
            callActionWithParameters(null, null, null);
            callEvent(null, null, null, null);
            close();

            Handle = JSObjectHandle.Create(this);

            Console.Error.WriteLine($"Parent - {Handle.Metadata}");
        }

        /// <inheritdoc />
        public JSObjectHandle Handle { get; private set; }

        public object getValue(string name)
        {
            if (Handle == null) return null;


            var obj = GetValue(name);
            System.Diagnostics.Debug.WriteLine($"Get Value {name} - {obj?.ToString()}");
            return obj;
        }

        [Preserve]
        public void setValue(string name, string value)
        {
            if (Handle == null) return;
            var json = unsanitize(value);
            json = json.Replace(@"\\",@"\");
            json = json.Trim('"');
            json = json.Replace(@"\r\n", Environment.NewLine);
            json = json.Replace(@"\t", "\t");
            System.Diagnostics.Debug.WriteLine($"Trimmed: {json}");
            SetValue(name, json);
        }

        [Preserve]
        public void setValueWithType(string name, string value, string type)
        {
            if (Handle == null) return;
            var json = unsanitize(value);
            json = json.Replace(@"\\", @"\");
            json = json.Trim('"', ' ');
            json = json.Replace(@"\r\n", Environment.NewLine);
            json = json.Replace(@"\t", "\t");
            System.Diagnostics.Debug.WriteLine($"Trimmed: {json}");
            SetValue(name,json , type);
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
        public bool callActionWithParameters(string name, string parameter1, string parameter2)
        {
            if (Handle == null) return false;
            System.Diagnostics.Debug.WriteLine($"Calling action {name}");

            var parameters = new[] { unsanitize(parameter1), unsanitize(parameter2) }.Where(x => x != null).ToArray();
            var result = CallActionWithParameters(name, parameters);

            return result;
        }

        private string unsanitize(string parameter)
        {
            System.Diagnostics.Debug.WriteLine($"Encoded String: {parameter}");
            if (parameter == null) return parameter;
            var replacements = @"""'{}:,%";
            System.Diagnostics.Debug.WriteLine($"Replacements: >{replacements}<");
            for (int i = 0; i < replacements.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine($"Replacing: >%{(int)replacements[i]}< with >{(char)replacements[i] + "" }< ");
                parameter = parameter.Replace($"%{(int)replacements[i]}", (char)replacements[i] + "");
            }

            parameter = parameter.Replace(@"\\""", @"""");

            System.Diagnostics.Debug.WriteLine($"Decoded String: {parameter}");
            return parameter;
        }

        [Preserve]
        public async void callEvent(string name, string promiseId, string parameter1, string parameter2)
        {
            if (Handle == null) return;
            System.Diagnostics.Debug.WriteLine($"Calling event {name}");

            var parameters = new[] { unsanitize(parameter1), unsanitize(parameter2) }.Where(x => x != null).ToArray();
            var resultString = await CallEvent(name, parameters);
            try
            {
                Console.WriteLine("Event Callback - start");

                var callbackMethod = $"asyncCallback({promiseId},'{resultString}');";

                var result = WebAssemblyRuntime.InvokeJS(callbackMethod);

                Console.WriteLine("Event Callback - end");
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Event Callback - error {e.Message}");
            }

            return;



        }

        [Preserve]
        public void close()
        {
            if (Handle == null) return;
            Dispose();
        }

    }
}
