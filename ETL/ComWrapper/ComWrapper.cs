﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ETL
{
    public class ComWrapper : IComWrapper
    {
        private readonly Type type;
        private readonly object obj;
        public ComWrapper(Type com_type, object com_object)
        {
            type = com_type;
            obj = com_object;
        }
        public object ComObject { get { return obj; } }
        public void Dispose()
        {
            Marshal.ReleaseComObject(obj);
        }
        public object Get(string property_name)
        {
            return type.InvokeMember(property_name, BindingFlags.Public | BindingFlags.GetProperty, null, obj, new object[] { });
        }
        public void Set(string property_name, object value)
        {
            type.InvokeMember(property_name, BindingFlags.Public | BindingFlags.SetProperty, null, obj, new object[] { value });
        }
        public object Call(string method_name)
        {
            return Call(method_name, new object[] { });
        }
        public object Call(string method_name, params object[] args)
        {
            return type.InvokeMember(method_name, BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, args);
        }
        public IComWrapper Wrap(object com_object)
        {
            return new ComWrapper(type, com_object);
        }
        public IComWrapper GetAndWrap(string property_name)
        {
            return Wrap(Get(property_name));
        }
        public IComWrapper CallAndWrap(string method_name)
        {
            return Wrap(Call(method_name));
        }
        public IComWrapper CallAndWrap(string method_name, params object[] args)
        {
            return Wrap(Call(method_name, args));
        }
    }
}
