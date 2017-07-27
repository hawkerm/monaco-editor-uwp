using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Helpers
{
    public interface IJsonable
    {
        string ToJson();
    }

    public static class Json
    {
        /// <summary>
        /// Converts an array of strings to a JSON based string array.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string StringArray([ReadOnlyArray] string[] strings)
        {
            StringBuilder output = new StringBuilder("[", 100);
            foreach (string msg in strings)
            {
                output.Append(String.Format("\"{0}\",", msg));
            }
            if (strings.Length > 0)
            {
                output.Remove(output.Length - 1, 1); // Remove Trailing Comma
            }
            output.Append("]");

            return output.ToString();
        }

        /// <summary>
        /// Converts an array of Jsonable objects to JSON based array.
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static string ObjectArray(IEnumerable<IJsonable> objects)
        {
            StringBuilder output = new StringBuilder("[", 100);
            foreach (var obj in objects)
            {
                output.Append(String.Format("{0},", obj.ToJson()));
            }
            if (objects.Count() > 0)
            {
                output.Remove(output.Length - 1, 1); // Remove Trailing Comma
            }
            output.Append("]");

            return output.ToString();
        }

        /// <summary>
        /// Wrap a JSON string in a Parse statement for export.
        /// Escapes double-quotes.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string Parse(string json)
        {
            return String.Format("JSON.parse(\"{0}\")", json.Replace("\"", "\\\""));
        }
    }
}
