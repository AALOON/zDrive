using System;
using zDrive.Services.Ioc;
using Xunit;

namespace zDrive.Tests.Services.Ioc
{
    public class TypeKeyTests
    {
        [Fact]
        public void Equals_SameTypeAndNullKeys_ReturnsTrue()
        {
            var first = new TypeKey(typeof(string), null);
            var second = new TypeKey(typeof(string), null);

            Assert.True(first.Equals(second));
        }

        [Theory]
        [InlineData("key", "key", true)]
        [InlineData("key", "KEY", true)]
        [InlineData("key", "other", false)]
        public void Equals_KeysComparedCaseInsensitively(string firstKey, string secondKey, bool expected)
        {
            var first = new TypeKey(typeof(string), firstKey);
            var second = new TypeKey(typeof(string), secondKey);

            Assert.Equal(expected, first.Equals(second));
        }

        [Fact]
        public void Equals_DifferentTypes_ReturnsFalse()
        {
            var first = new TypeKey(typeof(string), "key");
            var second = new TypeKey(typeof(int), "key");

            Assert.False(first.Equals(second));
        }

        [Fact]
        public void Equals_NullObject_ReturnsFalse()
        {
            var key = new TypeKey(typeof(string), "key");

            Assert.False(key.Equals(null));
        }

        [Fact]
        public void GetHashCode_CaseInsensitiveEqualKeys_ReturnSameHash()
        {
            var first = new TypeKey(typeof(string), "key");
            var second = new TypeKey(typeof(string), "KEY");

            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void ToString_WithoutKey_ReturnsTypeName()
        {
            var key = new TypeKey(typeof(string), null);

            Assert.Equal("String", key.ToString());
        }

        [Fact]
        public void ToString_WithKey_ReturnsTypeNameAndKey()
        {
            var key = new TypeKey(typeof(string), "custom");

            Assert.Equal("String custom", key.ToString());
        }
    }
}
