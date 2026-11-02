using System.Globalization;
using zDrive.Converters;
using zDrive.Extensions;
using zDrive.Properties;
using Xunit;

namespace zDrive.Tests.Extensions
{
    public class InfoFormatExtensionsTests
    {
        private const double K = 1024d;

        [Theory]
        [InlineData(-1d)]
        [InlineData(-0.5d)]
        public void PrepareDouble_NegativeValue_ReturnsUnavailable(double input)
        {
            var value = input;

            var suffix = InfoFormatExtensions.PrepareDouble(ref value);

            Assert.Equal(Resources.Size_bytes_Unvailable, suffix);
            Assert.Equal(input, value);
        }

        [Fact]
        public void PrepareDouble_PicksSuffixAndScalesValue()
        {
            Check(0d, Resources.Size_bytes_Short, 0d);
            Check(1023.99d, Resources.Size_bytes_Short, 1023.99d);
            Check(K, Resources.Size_Kilobytes_Short, 1d);
            Check(K * K, Resources.Size_Megabytes_Short, 1d);
            Check(K * K * K, Resources.Size_Gigabytes_Short, 1d);
            Check(K * K * K * K, Resources.Size_Terabytes_Short, 1d);
            Check(K * K * K * K * K, Resources.Size_Petabytes_Short, 1d);
            Check(512d * K * K, Resources.Size_Megabytes_Short, 512d);
        }

        [Fact]
        public void ToFormatedString_FreeMax_FormatsFreeOverMax()
        {
            var max = 250e9;
            var free = 100e9;
            var used = max - free;

            var result = InfoFormat.FreeMax.ToFormatedString(max, free);

            var expected = string.Format(CultureInfo.CurrentCulture, Resources.InfoFormat_FreeMax,
                max / K / K / K, Resources.Size_Gigabytes_Short,
                free / K / K / K, Resources.Size_Gigabytes_Short,
                used / K / K / K, Resources.Size_Gigabytes_Short);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToFormatedString_UsedMax_FormatsUsedOverMax()
        {
            var max = 250e9;
            var free = 100e9;
            var used = max - free;

            var result = InfoFormat.UsedMax.ToFormatedString(max, free);

            var expected = string.Format(CultureInfo.CurrentCulture, Resources.InfoFormat_UsedMax,
                max / K / K / K, Resources.Size_Gigabytes_Short,
                free / K / K / K, Resources.Size_Gigabytes_Short,
                used / K / K / K, Resources.Size_Gigabytes_Short);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToFormatedString_Free_FormatsFreeOnly()
        {
            var max = 250e9;
            var free = 100e9;

            var result = InfoFormat.Free.ToFormatedString(max, free);

            var expected = string.Format(CultureInfo.CurrentCulture, Resources.InfoFormat_Free,
                max / K / K / K, Resources.Size_Gigabytes_Short,
                free / K / K / K, Resources.Size_Gigabytes_Short,
                (max - free) / K / K / K, Resources.Size_Gigabytes_Short);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToFormatedString_FreeGreaterThanMax_ReturnsUnavailable()
        {
            var result = InfoFormat.Free.ToFormatedString(max: 100d, free: 200d);

            Assert.Equal(Resources.Size_bytes_Unvailable, result);
        }

        [Theory]
        [InlineData(InfoFormat.Free)]
        [InlineData(InfoFormat.FreeMax)]
        [InlineData(InfoFormat.UsedMax)]
        public void PrepareInfoFormat_MapsEnumToResource(InfoFormat infoFormat)
        {
            var expected = infoFormat switch
            {
                InfoFormat.Free => Resources.InfoFormat_Free,
                InfoFormat.FreeMax => Resources.InfoFormat_FreeMax,
                InfoFormat.UsedMax => Resources.InfoFormat_UsedMax,
                _ => Resources.InfoFormat_Free
            };

            Assert.Equal(expected, infoFormat.PrepareInfoFormat());
        }

        [Fact]
        public void PrepareInfoFormat_UndefinedValue_FallsBackToFree()
        {
            var infoFormat = (InfoFormat)99;

            Assert.Equal(Resources.InfoFormat_Free, infoFormat.PrepareInfoFormat());
        }

        private static void Check(double input, string expectedSuffix, double expectedValue)
        {
            var value = input;

            var suffix = InfoFormatExtensions.PrepareDouble(ref value);

            Assert.Equal(expectedSuffix, suffix);
            Assert.Equal(expectedValue, value);
        }
    }
}
