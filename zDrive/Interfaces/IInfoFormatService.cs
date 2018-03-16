using zDrive.Converters;

namespace zDrive.Interfaces
{
    internal interface IInfoFormatter
    {
        string GetFormatedString(double max, double free);
    }
    internal interface IInfoFormatService : IInfoFormatter
    {
        InfoFormat Format { get; set; }
    }
}
