using System;
using zDrive.Extensions;

namespace zDrive.Services
{
    /// <summary>
    ///     Type key.
    /// </summary>
    internal readonly struct TypeKey
    {
        public TypeKey(Type type, string key)
        {
            Type = type;
            Key = key;
        }

        public Type Type { get; }
        public string Key { get; }

        public override bool Equals(object obj)
        {
            if (obj is TypeKey b)
                return Equals(b);
            return false;
        }

        public bool Equals(TypeKey b)
        {
            return Type == b.Type && Key.Compare(b.Key);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ (Key?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            return Type + " " + Key;
        }
    }
}