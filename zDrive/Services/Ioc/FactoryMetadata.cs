using System;

namespace zDrive.Services.Ioc
{
    internal sealed class FactoryMetadata
    {
        public FactoryMetadata(TypeKey typeKey, Func<object> factory, Scope scope, Guid? instancesId = null)
        {
            this.TypeKey = typeKey;
            this.Factory = factory;
            this.Scope = scope;
            this.InstancesId = instancesId ?? GenerateInstanceId();
        }

        public Func<object> Factory { get; }

        public Scope Scope { get; }

        public TypeKey TypeKey { get; }

        public Guid InstancesId { get; }

        public static Guid GenerateInstanceId() => Guid.NewGuid();
    }
}
