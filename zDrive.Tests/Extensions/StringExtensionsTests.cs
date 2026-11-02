using zDrive.Extensions;
using Xunit;

namespace zDrive.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, "a", false)]
        [InlineData("a", null, false)]
        [InlineData("a", "a", true)]
        [InlineData("a", "A", true)]
        [InlineData("Value", "value", true)]
        [InlineData("a", "b", false)]
        [InlineData("abc", "abd", false)]
        public void Compare_IsNullSafeAndCaseInsensitive(string first, string second, bool expected)
        {
            Assert.Equal(expected, first.Compare(second));
        }
    }
}
