using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.Foundation.Metadata;

namespace Monaco.Helpers
{
    /// <summary>
    /// Class to aid in accessing WinRT values from JavaScript.
    /// </summary>
    [AllowForWeb]
    public sealed class ParentAccessor
    {
        private object parent;
        private TypeInfo typeinfo;
        private Dictionary<string, Action> actions;

        /// <summary>
        /// Constructs a new reflective parent Accessor for the provided object.
        /// </summary>
        /// <param name="parent">Object to provide Property Access.</param>
        public ParentAccessor(object parent)
        { 
            this.parent = parent;
            this.typeinfo = parent.GetType().GetTypeInfo();
            this.actions = new Dictionary<string, Action>();
        }

        /// <summary>
        /// Registers an action from the .NET side which can be called from within the JavaScript code.
        /// </summary>
        /// <param name="name">String Key.</param>
        /// <param name="action">Action to perform.</param>
        internal void RegisterAction(string name, Action action)
        {
            actions[name] = action;
        }

        /// <summary>
        /// Calls an Action registered before with <see cref="RegisterAction(string, Action)"/>.
        /// </summary>
        /// <param name="name">String Key.</param>
        /// <returns>True if method was found in registration.</returns>
        public bool CallAction(string name)
        {
            if (actions.ContainsKey(name))
            {
                actions[name].Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the winrt primative object value for the specified Property.
        /// </summary>
        /// <param name="name">Property name on Parent Object.</param>
        /// <returns>Property Value or null.</returns>
        public object GetValue(string name)
        {
            var propinfo = typeinfo.GetDeclaredProperty(name);
            return propinfo?.GetValue(this.parent);
        }

        /// <summary>
        /// Returns the winrt primative object value for a child property off of the specified Property.
        /// 
        /// Useful for providing complex types to users of Parent but still access primatives in JavaScript.
        /// </summary>
        /// <param name="name">Parent Property name.</param>
        /// <param name="child">Property's Property name to retrieve.</param>
        /// <returns>Value of Child Property or null.</returns>
        public object GetChildValue(string name, string child)
        {
            var propinfo = typeinfo.GetDeclaredProperty(name);
            var prop = propinfo?.GetValue(this.parent);
            if (prop != null)
            {
                var childinfo = prop.GetType().GetTypeInfo().GetDeclaredProperty(child);
                return childinfo?.GetValue(prop);
            }

            return null;
        }

        /// <summary>
        /// Sets the value for the specified Property.
        /// </summary>
        /// <param name="name">Parent Property name.</param>
        /// <param name="value">Value to set.</param>
        public void SetValue(string name, object value)
        {
            var propinfo = typeinfo.GetDeclaredProperty(name); // TODO: Cache these?
            propinfo?.SetValue(this.parent, value);
        }
    }
}
