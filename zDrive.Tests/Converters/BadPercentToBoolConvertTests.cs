using System;
using System.Globalization;
using zDrive.Converters;
using Xunit;

namespace zDrive.Tests.Converters
{
    public class BadPercentToBoolConvertTests
    {
        private readonly BadPercentToBoolConvert converter = new();

        [Theory]
        [InlineData(null, false)]
        [InlineData(0d, false)]
        [InlineData(50d, false)]
        [InlineData(90d, false)]
        [InlineData(90.1d, true)]
        [InlineData(100d, true)]
        public void Convert_ReturnsTrueOnlyAboveNinetyPercent(double? value, bool expected)
        {
            var result = this.converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertBack_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() =>
                this.converter.ConvertBack(true, typeof(double), null, CultureInfo.InvariantCulture));
        }
    }
}
