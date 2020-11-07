using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace zDrive.Services
{
    /// <summary>
    ///     Simple implementation of Inversion of control.
    /// </summary>
    public static class SimpleIoc
    {
        private static readonly Dictionary<TypeKey, Func<object>> Initilizers = new Dictionary<TypeKey, Func<object>>();
        private static readonly Dictionary<TypeKey, object> Objects = new Dictionary<TypeKey, object>();

        private static object CtorResolver(Type type)
        {
            var tmp = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var ctor = tmp.FirstOrDefault();
            if (ctor == null)
                throw new NullReferenceException("There no constructotrs for " + type.Name);
            var types = ctor.GetParameters();
            var parameters = new object[types.Length];

            for (var i = 0; i < types.Length; i++) parameters[i] = Resolve(types[i].ParameterType);


            return ctor.Invoke(parameters);
        }

        public static void RegisterType<TBase, TDelivery>(Func<TBase> func = null, string key = null)
            where TDelivery : TBase
            where TBase : class
        {
            var baseType = typeof(TBase);
            var deliveryType = typeof(TDelivery);

            var typeKey = new TypeKey(baseType, key);

            if (Initilizers.ContainsKey(typeKey))
                throw new ArgumentException("Already exists!");

            if (func == null)
                func = () => (TBase) CtorResolver(deliveryType);

            Initilizers.Add(typeKey, func);
        }

        public static void RegisterType<TBase, TDelivery>(string key)
            where TDelivery : TBase
            where TBase : class
        {
            RegisterType<TBase, TDelivery>(null, key);
        }

        public static void RegisterType<TBase>(Func<TBase> func = null, string key = null) where TBase : class

        {
            var baseType = typeof(TBase);
            var typeKey = new TypeKey(baseType, key);

            if (Initilizers.ContainsKey(typeKey))
                throw new ArgumentException($"Already exists {typeKey}!");

            if (func == null)
                func = () => (TBase) CtorResolver(baseType);


            Initilizers.Add(typeKey, func);
        }

        public static void RegisterType<TBase>(string key) where TBase : class
        {
            RegisterType<TBase>(null, key);
        }

        public static TBase Resolve<TBase>(string key = null) where TBase : class
        {
            var baseType = typeof(TBase);
            return (TBase) Resolve(baseType, key);
        }

        public static object Resolve(Type type, string key = null)
        {
            var baseType = type;
            var typeKey = new TypeKey(baseType, key);

            if (Objects.ContainsKey(typeKey))
                return Objects[typeKey];

            if (!Initilizers.ContainsKey(typeKey))
                throw new ArgumentException($"Cannot resolve type {typeKey}");

            var instance = Initilizers[typeKey].Invoke();
            Objects.Add(typeKey, instance);

            return instance;
        }
    }
}