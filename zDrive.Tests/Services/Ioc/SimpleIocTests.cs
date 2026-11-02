using System;
using System.Collections.Generic;
using System.Linq;
using zDrive.Services.Ioc;
using Xunit;

namespace zDrive.Tests.Services.Ioc
{
    public class SimpleIocTests
    {
        private interface ITestService
        {
            Guid Id { get; }
        }

        private sealed class TestService : ITestService
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        private sealed class AnotherTestService : ITestService
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        private interface IAnotherInterface
        {
        }

        private sealed class DisposableService : ITestService, IAnotherInterface, IDisposable
        {
            public Guid Id { get; } = Guid.NewGuid();

            public bool IsDisposed { get; private set; }

            public void Dispose() => this.IsDisposed = true;
        }

        private sealed class Consumer
        {
            public Consumer(ITestService dependency) => this.Dependency = dependency;

            public ITestService Dependency { get; }
        }

        [Fact]
        public void RegisterSingleton_Resolve_ReturnsSameInstance()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>();

            var first = ioc.Resolve<ITestService>();
            var second = ioc.Resolve<ITestService>();

            Assert.Same(first, second);
        }

        [Fact]
        public void RegisterSingleton_Factory_IsInvokedOnce()
        {
            using var ioc = new SimpleIoc();
            var invocations = 0;
            ioc.RegisterSingleton<ITestService>(() =>
            {
                invocations++;
                return new TestService();
            });

            ioc.Resolve<ITestService>();
            ioc.Resolve<ITestService>();

            Assert.Equal(1, invocations);
        }

        [Fact]
        public void RegisterSingleton_WithKey_ResolvesIndependentInstances()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>("first");
            ioc.RegisterSingleton<ITestService, TestService>("second");

            var first = ioc.Resolve<ITestService>("first");
            var second = ioc.Resolve<ITestService>("second");

            Assert.NotSame(first, second);
            Assert.Same(first, ioc.Resolve<ITestService>("first"));
        }

        [Fact]
        public void RegisterSingleton_SameRegistrationTwice_Throws()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>();

            Assert.Throws<ArgumentException>(() => ioc.RegisterSingleton<ITestService, TestService>());
        }

        [Fact]
        public void RegisterSingleton_TwoInterfaces_ResolvesSameInstance()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, IAnotherInterface, DisposableService>();

            var service = ioc.Resolve<ITestService>();
            var other = ioc.Resolve<IAnotherInterface>();

            Assert.Same(service, other);
        }

        [Fact]
        public void RegisterPerDependency_Resolve_ReturnsNewInstances()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterPerDependency(() => new TestService());

            var first = ioc.Resolve<TestService>();
            var second = ioc.Resolve<TestService>();

            Assert.NotSame(first, second);
        }

        [Fact]
        public void Resolve_WithConstructorInjection_ResolvesDependencies()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>();
            ioc.RegisterSingleton<Consumer>();

            var consumer = ioc.Resolve<Consumer>();

            Assert.Same(ioc.Resolve<ITestService>(), consumer.Dependency);
        }

        [Fact]
        public void Resolve_UnregisteredType_Throws()
        {
            using var ioc = new SimpleIoc();

            Assert.Throws<ArgumentException>(() => ioc.Resolve<ITestService>());
        }

        [Fact]
        public void Resolve_IEnumerable_ReturnsAllRegisteredServices()
        {
            using var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>();
            ioc.RegisterSingleton<ITestService, AnotherTestService>("another");

            var services = ioc.Resolve<IEnumerable<ITestService>>();

            Assert.Equal(2, services.Count());
            Assert.Contains(services, s => s is TestService);
            Assert.Contains(services, s => s is AnotherTestService);
        }

        [Fact]
        public void Dispose_DisposesSingletons()
        {
            var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, DisposableService>();
            var service = (DisposableService)ioc.Resolve<ITestService>();

            ioc.Dispose();

            Assert.True(service.IsDisposed);
        }

        [Fact]
        public void Dispose_Twice_DoesNotThrow()
        {
            var ioc = new SimpleIoc();
            ioc.Dispose();

            ioc.Dispose();
        }

        [Fact]
        public void Resolve_AfterDispose_Throws()
        {
            var ioc = new SimpleIoc();
            ioc.RegisterSingleton<ITestService, TestService>();
            ioc.Resolve<ITestService>();
            ioc.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ioc.Resolve<ITestService>());
        }

        [Fact]
        public void Register_AfterDispose_Throws()
        {
            var ioc = new SimpleIoc();
            ioc.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ioc.RegisterSingleton<ITestService, TestService>());
        }
    }
}
