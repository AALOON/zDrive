using System.Globalization;
using zDrive.Converters;
using zDrive.Properties;

namespace zDrive.Extensions
{
    public static class InfoFormatExtensions
    {
        private static readonly string Un = Resources.Size_bytes_Unvailable;
        private static readonly string B = Resources.Size_bytes_Short;
        private static readonly string Kb = Resources.Size_Kilobytes_Short;
        private static readonly string Mb = Resources.Size_Megabytes_Short;
        private static readonly string Gb = Resources.Size_Gigabytes_Short;
        private static readonly string Tb = Resources.Size_Terabytes_Short;
        private static readonly string Pb = Resources.Size_Petabytes_Short;


        private static readonly string Free = Resources.InfoFormat_Free;
        private static readonly string FreeMax = Resources.InfoFormat_FreeMax;
        private static readonly string UsedMax = Resources.InfoFormat_UsedMax;

        public static string PrepareDouble(ref double d)
        {
            if (d < 0.0d)
            {
                return Un;
            }

            if (d < 1024d)
            {
                return B;
            }

            if ((d /= 1024d) < 1024d)
            {
                return Kb;
            }

            if ((d /= 1024d) < 1024d)
            {
                return Mb;
            }

            if ((d /= 1024d) < 1024d)
            {
                return Gb;
            }

            if ((d /= 1024d) < 1024d)
            {
                return Tb;
            }

            d /= 1024d;
            return Pb;
        }

        public static string PrepareInfoFormat(this InfoFormat infoFormat)
        {
            switch (infoFormat)
            {
                case InfoFormat.Free:
                    return Free;
                case InfoFormat.FreeMax:
                    return FreeMax;
                case InfoFormat.UsedMax:
                    return UsedMax;
                default:
                    return Free;
            }
        }

        public static string ToFormatedString(this InfoFormat infoFormat, double max, double free)
        {
            var used = max - free;
            var strSizeMax = PrepareDouble(ref max);
            var strSizeFree = PrepareDouble(ref free);
            var strSizeUsed = PrepareDouble(ref used);
            var formatString = infoFormat.PrepareInfoFormat();

            if (strSizeMax == Un || strSizeFree == Un || strSizeUsed == Un)
            {
                return Un;
            }

            return string.Format(CultureInfo.CurrentCulture, formatString,
                max, strSizeMax,
                free, strSizeFree,
                used, strSizeUsed);
        }
    }
}
