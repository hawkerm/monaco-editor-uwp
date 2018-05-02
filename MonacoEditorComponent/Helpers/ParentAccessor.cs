using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.Foundation.Metadata;

namespace Monaco.Helpers
{
    /// <summary>
    /// Class to aid in accessing WinRT values from JavaScript.
    /// Not Thread Safe.
    /// </summary>
    [AllowForWeb]
    public sealed class ParentAccessor
    {
        private WeakReference<IParentAccessorAcceptor> parent;
        private Type typeinfo;
        private Dictionary<string, WeakReference<Action>> actions;

        private List<Assembly> Assemblies { get; set; } = new List<Assembly>();

        /// <summary>
        /// Constructs a new reflective parent Accessor for the provided object.
        /// </summary>
        /// <param name="parent">Object to provide Property Access.</param>
        public ParentAccessor(IParentAccessorAcceptor parent)
        { 
            this.parent = new WeakReference<IParentAccessorAcceptor>(parent);
            this.typeinfo = parent.GetType();
            this.actions = new Dictionary<string, WeakReference<Action>>();
        }

        /// <summary>
        /// Registers an action from the .NET side which can be called from within the JavaScript code.
        /// </summary>
        /// <param name="name">String Key.</param>
        /// <param name="action">Action to perform.</param>
        internal void RegisterAction(string name, Action action)
        {
            actions[name] = new WeakReference<Action>(action);
        }

        /// <summary>
        /// Adds an Assembly to use for looking up types by name for <see cref="SetValue(string, string, string)"/>.
        /// </summary>
        /// <param name="assembly">Assembly to add.</param>
        internal void AddAssemblyForTypeLookup(Assembly assembly)
        {
            this.Assemblies.Add(assembly);
        }

        /// <summary>
        /// Calls an Action registered before with <see cref="RegisterAction(string, Action)"/>.
        /// </summary>
        /// <param name="name">String Key.</param>
        /// <returns>True if method was found in registration.</returns>
        public bool CallAction(string name)
        {
            if (actions.ContainsKey(name) && actions[name].TryGetTarget(out Action action))
            {
                action.Invoke();
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
            if (parent.TryGetTarget(out IParentAccessorAcceptor tobj))
            {
                var propinfo = typeinfo.GetProperty(name);
                return propinfo?.GetValue(tobj);
            }

            return null;
        }

        public string GetJsonValue(string name)
        {
            if (parent.TryGetTarget(out IParentAccessorAcceptor tobj))
            {
                var propinfo = typeinfo.GetProperty(name);
                var obj = propinfo?.GetValue(tobj);

                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            return "{}";
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
            if (parent.TryGetTarget(out IParentAccessorAcceptor tobj))
            {
                // TODO: Support params for multi-level digging?
                var propinfo = typeinfo.GetProperty(name);
                var prop = propinfo?.GetValue(tobj);
                if (prop != null)
                {
                    var childinfo = prop.GetType().GetProperty(child);
                    return childinfo?.GetValue(prop);
                }
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
            if (parent.TryGetTarget(out IParentAccessorAcceptor tobj))
            {
                var propinfo = typeinfo.GetProperty(name); // TODO: Cache these?
                tobj.IsSettingValue = true;
                propinfo?.SetValue(tobj, value);
                tobj.IsSettingValue = false;
            }
        }

        /// <summary>
        /// Sets the value for the specified Property after deserializing the value as the given type name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void SetValue(string name, string value, string type)
        {
            if (parent.TryGetTarget(out IParentAccessorAcceptor tobj))
            {
                var propinfo = typeinfo.GetProperty(name);
                var typeobj = LookForTypeByName(type);

                var obj = JsonConvert.DeserializeObject(value, typeobj);

                tobj.IsSettingValue = true;
                propinfo?.SetValue(tobj, obj);
                tobj.IsSettingValue = false;
            }
        }

        private Type LookForTypeByName(string name)
        {
            // First search locally
            var result = Type.GetType(name);

            if (result != null)
            {
                return result;
            }

            // Search in Other Assemblies
            foreach (var assembly in Assemblies)
            {
                foreach (var typeInfo in assembly.ExportedTypes)
                {
                    if (typeInfo.Name == name)
                    {
                        return typeInfo;
                    }
                }
            }

            return null;
        }
    }

    //// TODO: Find better approach than this. Issue #21.
    /// <summary>
    /// Interface used on objects to be accessed.
    /// </summary>
    public interface IParentAccessorAcceptor
    {
        /// <summary>
        /// Property to tell object the value is being set by ParentAccessor.
        /// </summary>
        bool IsSettingValue { get; set; }
    }
}
