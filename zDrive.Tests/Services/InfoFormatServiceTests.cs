using zDrive.Converters;
using zDrive.Extensions;
using zDrive.Services;
using Xunit;

namespace zDrive.Tests.Services
{
    public class InfoFormatServiceTests
    {
        [Fact]
        public void Format_DefaultsToFree()
        {
            var service = new InfoFormatService();

            Assert.Equal(InfoFormat.Free, service.Format);
        }

        [Fact]
        public void GetFormatedString_UsesCurrentFormat()
        {
            var service = new InfoFormatService { Format = InfoFormat.FreeMax };
            const double max = 250e9;
            const double free = 100e9;

            var result = service.GetFormatedString(max, free);

            Assert.Equal(InfoFormat.FreeMax.ToFormatedString(max, free), result);
        }

        [Fact]
        public void GetFormatedString_ReflectsFormatChange()
        {
            var service = new InfoFormatService { Format = InfoFormat.Free };
            const double max = 250e9;
            const double free = 100e9;
            var before = service.GetFormatedString(max, free);

            service.Format = InfoFormat.UsedMax;

            Assert.NotEqual(before, service.GetFormatedString(max, free));
        }
    }
}
