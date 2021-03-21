using zDrive.Converters;
using zDrive.Extensions;
using zDrive.Interfaces;

namespace zDrive.Services
{
    /// <inheritdoc />
    public class InfoFormatService : IInfoFormatService
    {
        public InfoFormat Format { get; set; }

        /// <inheritdoc />
        public string GetFormatedString(double max, double free) => this.Format.ToFormatedString(max, free);
    }
}
