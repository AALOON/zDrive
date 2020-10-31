using zDrive.Converters;
using zDrive.Interfaces;

namespace zDrive.Services
{
    public class InfoFormatService : IInfoFormatService
    {
        public InfoFormat Format { get; set; }

        public string GetFormatedString(double max, double free)
        {
            return Format.ToFormatedString(max, free);
        }
    }
}