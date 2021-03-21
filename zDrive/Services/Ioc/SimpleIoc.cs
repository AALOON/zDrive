using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace zDrive.Services.Ioc
{
    /// <summary>
    /// Simple implementation of Inversion of control.
    /// </summary>
    public sealed class SimpleIoc
    {
        private static readonly Lazy<SimpleIoc> DefaultLazyInstance = new(() => new SimpleIoc());

        private readonly ConcurrentDictionary<TypeKey, FactoryMetadata> factories = new();
        private readonly ConcurrentDictionary<Guid, object> instances = new();

        public static SimpleIoc Default => DefaultLazyInstance.Value;

        private object CtorResolver(Type type)
        {
            var ctor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
            if (ctor == null)
            {
                throw new ArgumentNullException("There no constructors for " + type.Name);
            }

            var types = ctor.GetParameters();
            var parameters = new object[types.Length];

            for (var i = 0; i < types.Length; i++)
            {
                parameters[i] = this.Resolve(types[i].ParameterType);
            }

            return ctor.Invoke(parameters);
        }

        private static Type GetEnumerableType(Type type)
        {
            if (type.IsInterface && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments()[0];
            }

            foreach (var intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        public void RegisterSingleton<TBase, TDelivery>(string key = null)
            where TDelivery : TBase
            where TBase : class =>
            this.RegisterSingleton<TBase, TDelivery>(null, key);

        public void RegisterSingleton<TBase, TDelivery>(Func<TBase> func, string key = null)
            where TDelivery : TBase
            where TBase : class
        {
            var baseType = typeof(TBase);
            var deliveryType = typeof(TDelivery);

            var typeKey = new TypeKey(baseType, key);

            this.ThrowIfExists(typeKey);

            func ??= () => (TBase)this.CtorResolver(deliveryType);

            this.TryAddFactory(new FactoryMetadata(typeKey, func, Scope.Singleton));
        }

        public void RegisterSingleton<TBase1, TBase2, TDelivery>(Func<TBase1> func1 = null, Func<TBase2> func2 = null,
            string key = null)
            where TDelivery : TBase1, TBase2
            where TBase1 : class
            where TBase2 : class
        {
            var baseType1 = typeof(TBase1);
            var baseType2 = typeof(TBase2);
            var deliveryType = typeof(TDelivery);

            var typeKey1 = new TypeKey(baseType1, key);
            var typeKey2 = new TypeKey(baseType2, key);

            this.ThrowIfExists(typeKey1);
            this.ThrowIfExists(typeKey2);

            func1 ??= () => (TBase1)this.CtorResolver(deliveryType);
            func2 ??= () => (TBase2)this.CtorResolver(deliveryType);

            var id = FactoryMetadata.GenerateInstanceId();
            this.TryAddFactory(new FactoryMetadata(typeKey1, func1, Scope.Singleton, id));
            this.TryAddFactory(new FactoryMetadata(typeKey2, func2, Scope.Singleton, id));
        }

        public void RegisterSingleton<TBase>(Func<TBase> func, string key = null) where TBase : class
        {
            var baseType = typeof(TBase);
            var typeKey = new TypeKey(baseType, key);

            if (this.factories.ContainsKey(typeKey))
            {
                throw new ArgumentException($"Already exists {typeKey}!");
            }

            func ??= () => (TBase)this.CtorResolver(baseType);

            this.factories.TryAdd(typeKey, new FactoryMetadata(typeKey, func, Scope.Singleton));
        }

        public void RegisterSingleton<TBase>(string key = null) where TBase : class =>
            this.RegisterSingleton<TBase>(null, key);

        public TBase Resolve<TBase>(string key = null) where TBase : class
        {
            var baseType = typeof(TBase);
            return (TBase)this.Resolve(baseType, key);
        }

        public object Resolve(Type type, string key = null)
        {
            var baseType = type;
            var typeKey = new TypeKey(baseType, key);

            return this.ResolveByTypeKey(typeKey);
        }

        private object ResolveByTypeKey(TypeKey typeKey)
        {
            if (!this.factories.TryGetValue(typeKey, out var metadata))
            {
                var enumerableType = GetEnumerableType(typeKey.Type);
                if (enumerableType != null)
                {
                    return this.ResolveEnumerable(enumerableType);
                }

                throw new ArgumentException($"Cannot resolve type {typeKey}");
            }

            var instance = metadata.Scope switch
            {
                Scope.Singleton => this.instances.GetOrAdd(metadata.InstancesId, _ => metadata.Factory()),
                Scope.PerDependency => metadata.Factory(),
                _ => throw new InvalidOperationException(nameof(metadata.Scope))
            };

            return instance;
        }

        private object ResolveEnumerable(Type type)
        {
            var query = this.factories.Keys.Where(p => p.Type == type).Select(this.ResolveByTypeKey);
            var castMethod = typeof(Enumerable).GetMethod("Cast");
            Debug.Assert(castMethod != null, nameof(castMethod) + " != null");
            var castGenericMethod = castMethod.MakeGenericMethod(type);

            return castGenericMethod.Invoke(null, new object[] { query });
        }

        private void ThrowIfExists(TypeKey typeKey)
        {
            if (this.factories.ContainsKey(typeKey))
            {
                throw new ArgumentException($"Type {typeKey} already exists!");
            }
        }

        private void TryAddFactory(FactoryMetadata metadata)
        {
            if (!this.factories.TryAdd(metadata.TypeKey, metadata))
            {
                this.ThrowIfExists(metadata.TypeKey);
            }
        }
    }
}
