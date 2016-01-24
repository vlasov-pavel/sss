using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ETL
{
    public sealed class ComConnector : IDisposable
    {
        private readonly string version;
        private readonly string connection_string;
        private Type com_type;
        private object com_object;
        private object connector;

        private const string CONST_Connect = "Connect";
        private const string CONST_NewObject = "NewObject";
        private const string CONST_String = "String";

        public ComConnector(string version, string connection_string)
        {
            this.version = version;
            this.connection_string = connection_string;
            Initialize();
        }
        private void Initialize()
        {
            com_type = Type.GetTypeFromProgID(version, true);
            com_object = Activator.CreateInstance(com_type);
        }
        private object Call(string method_name, params object[] args)
        {
            return com_type.InvokeMember(method_name, BindingFlags.Public | BindingFlags.InvokeMethod, null, connector, args);
        }

        public void Connect()
        {
            connector = com_type.InvokeMember(CONST_Connect, BindingFlags.Public | BindingFlags.InvokeMethod, null, com_object, new object[] { connection_string });
        }
        public IComWrapper NewObject(string name)
        {
            return new ComWrapper(com_type, Call(CONST_NewObject, name));
        }
        public string ToString(object value)
        {
            return (string)Call(CONST_String, value);
        }
        public void Dispose()
        {
            Marshal.ReleaseComObject(connector);
            Marshal.ReleaseComObject(com_object);
        }
    }
}
