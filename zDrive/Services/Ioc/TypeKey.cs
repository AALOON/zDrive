using System;
using zDrive.Extensions;

namespace zDrive.Services.Ioc
{
    /// <summary>
    /// Type key.
    /// </summary>
    internal readonly struct TypeKey
    {
        public TypeKey(Type type, string key)
        {
            this.Type = type;
            this.Key = key;
        }

        public Type Type { get; }

        public string Key { get; }

        public override bool Equals(object obj)
        {
            if (obj is TypeKey b)
            {
                return this.Equals(b);
            }

            return false;
        }

        public bool Equals(TypeKey b) => this.Type == b.Type && this.Key.Compare(b.Key);

        public override int GetHashCode() => this.Type.GetHashCode() ^ (this.Key?.GetHashCode() ?? 0);

        public override string ToString() =>
            string.IsNullOrEmpty(this.Key) ? this.Type.Name : this.Type.Name + " " + this.Key;
    }
}
