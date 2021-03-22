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
    public sealed class SimpleIoc : IDisposable
    {
        private static readonly Lazy<SimpleIoc> DefaultLazyInstance = new(() => new SimpleIoc());

        private readonly OneTimeSetup disposed = new();

        private readonly ConcurrentDictionary<TypeKey, FactoryMetadata> factories = new();
        private readonly ConcurrentDictionary<Guid, object> instances = new();

        /// <summary>
        /// Returns default instance of IoC. (Not currently created)
        /// </summary>
        public static SimpleIoc Default => DefaultLazyInstance.Value;

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.disposed.TrySet())
            {
                return;
            }

            foreach (var disposable in this.instances.Values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            this.factories.Clear();
            this.instances.Clear();
        }

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TBase">Type by which instance will be resolved</typeparam>
        /// <typeparam name="TDelivery">Type of registered object.</typeparam>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TBase, TDelivery>(string key = null)
            where TDelivery : TBase
            where TBase : class =>
            this.RegisterSingleton<TBase, TDelivery>(null, key);

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TBase">Type by which instance will be resolved</typeparam>
        /// <typeparam name="TDelivery">Type of registered object.</typeparam>
        /// <param name="func">Custom factory.</param>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TBase, TDelivery>(Func<TBase> func, string key = null)
            where TDelivery : TBase
            where TBase : class
        {
            this.ThrowIfDisposed();
            var baseType = typeof(TBase);
            var deliveryType = typeof(TDelivery);

            var typeKey = new TypeKey(baseType, key);

            this.ThrowIfExists(typeKey);

            func ??= () => (TBase)this.CtorResolver(deliveryType);

            this.TryAddFactory(new FactoryMetadata(typeKey, func, Scope.Singleton));
        }

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TBase1">Type by which instance will be resolved</typeparam>
        /// <typeparam name="TBase2">Second type by which instance will be resolved</typeparam>
        /// <typeparam name="TDelivery">Type of registered object.</typeparam>
        /// <param name="func">Custom factory.</param>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TBase1, TBase2, TDelivery>(Func<TDelivery> func = null,
            string key = null)
            where TDelivery : TBase1, TBase2
            where TBase1 : class
            where TBase2 : class
        {
            this.ThrowIfDisposed();
            var baseType1 = typeof(TBase1);
            var baseType2 = typeof(TBase2);
            var deliveryType = typeof(TDelivery);

            var typeKey1 = new TypeKey(baseType1, key);
            var typeKey2 = new TypeKey(baseType2, key);

            this.ThrowIfExists(typeKey1);
            this.ThrowIfExists(typeKey2);

            func ??= () => (TDelivery)this.CtorResolver(deliveryType);
            TBase1 Func1() => func();
            TBase2 Func2() => func();

            var id = FactoryMetadata.GenerateInstanceId();
            this.TryAddFactory(new FactoryMetadata(typeKey1, Func1, Scope.Singleton, id));
            this.TryAddFactory(new FactoryMetadata(typeKey2, Func2, Scope.Singleton, id));
        }

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TBase1">Type by which instance will be resolved</typeparam>
        /// <typeparam name="TBase2">Second type by which instance will be resolved</typeparam>
        /// <param name="func">Custom factory.</param>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TBase1, TBase2>(Func<object> func,
            string key = null)
            where TBase1 : class
            where TBase2 : class
        {
            this.ThrowIfDisposed();
            func = func ?? throw new ArgumentNullException(nameof(func));

            var baseType1 = typeof(TBase1);
            var baseType2 = typeof(TBase2);

            var typeKey1 = new TypeKey(baseType1, key);
            var typeKey2 = new TypeKey(baseType2, key);

            this.ThrowIfExists(typeKey1);
            this.ThrowIfExists(typeKey2);
            TBase1 Func1() => (TBase1)func();
            TBase2 Func2() => (TBase2)func();

            var id = FactoryMetadata.GenerateInstanceId();
            this.TryAddFactory(new FactoryMetadata(typeKey1, Func1, Scope.Singleton, id));
            this.TryAddFactory(new FactoryMetadata(typeKey2, Func2, Scope.Singleton, id));
        }

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TDelivery">Type by which instance will be resolved</typeparam>
        /// <param name="func">Custom factory.</param>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TDelivery>(Func<TDelivery> func, string key = null) where TDelivery : class
        {
            this.ThrowIfDisposed();
            var baseType = typeof(TDelivery);
            var typeKey = new TypeKey(baseType, key);

            if (this.factories.ContainsKey(typeKey))
            {
                throw new ArgumentException($"Already exists {typeKey}!");
            }

            func ??= () => (TDelivery)this.CtorResolver(baseType);

            this.TryAddFactory(new FactoryMetadata(typeKey, func, Scope.Singleton));
        }

        /// <summary>
        /// Register new singleton type.
        /// </summary>
        /// <typeparam name="TDelivery">Type by which instance will be resolved</typeparam>
        /// <param name="key">The key how the service will be specified.</param>
        public void RegisterSingleton<TDelivery>(string key = null) where TDelivery : class =>
            this.RegisterSingleton<TDelivery>(null, key);

        /// <summary>
        /// Resolve service by type and key.
        /// </summary>
        /// <typeparam name="TBase">Base type.</typeparam>
        /// <param name="key">key of registered service..</param>
        /// <returns></returns>
        public TBase Resolve<TBase>(string key = null) where TBase : class
        {
            var baseType = typeof(TBase);
            return (TBase)this.Resolve(baseType, key);
        }

        /// <summary>
        /// Resolve service by type and key.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <param name="key">key of registered service..</param>
        /// <returns></returns>
        public object Resolve(Type type, string key = null)
        {
            this.ThrowIfDisposed();
            var baseType = type;
            var typeKey = new TypeKey(baseType, key);

            return this.ResolveByTypeKey(typeKey);
        }

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

        private void ThrowIfDisposed()
        {
            if (this.disposed.IsSet)
            {
                throw new ObjectDisposedException(nameof(SimpleIoc));
            }
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
