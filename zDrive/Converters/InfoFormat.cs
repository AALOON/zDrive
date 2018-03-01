using System;
using System.Globalization;

namespace zDrive.Converters
{
    public enum InfoFormat
    {
        Free = 1,
        FreeMax = 4,
    }

    public static class InfoFormatExtensions
    {
        private static readonly string Un = Properties.Resources.Size_bytes_Unvailable;
        private static readonly string B = Properties.Resources.Size_bytes_Short;
        private static readonly string Kb = Properties.Resources.Size_Kilobytes_Short;
        private static readonly string Mb = Properties.Resources.Size_Megabytes_Short;
        private static readonly string Gb = Properties.Resources.Size_Gigabytes_Short;
        private static readonly string Tb = Properties.Resources.Size_Terabytes_Short;
        private static readonly string Pb = Properties.Resources.Size_Petabytes_Short;


        private static readonly string Free = Properties.Resources.InfoFormat_Free;
        private static readonly string FreeMax = Properties.Resources.InfoFormat_FreeMax;

        public static string PrepareDouble(ref double d)
        {
            if (d < 0.0d)
                return Un;

            if (d < 1024d)
                return B;
            if ((d /= 1024d) < 1024d)
                return Kb;
            if ((d /= 1024d) < 1024d)
                return Mb;
            if ((d /= 1024d) < 1024d)
                return Gb;
            if ((d /= 1024d) < 1024d)
                return Tb;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(infoFormat), infoFormat, null);
            }
        }

        public static string ToFormatedString(this InfoFormat infoFormat, double max, double free)
        {
            var strSizeMax = PrepareDouble(ref max);
            var strSizeFree = PrepareDouble(ref free);
            var formatString = infoFormat.PrepareInfoFormat();

            if (strSizeMax == Un || strSizeFree == Un)
                return Un;

            return string.Format(CultureInfo.CurrentCulture, formatString, 
                                max, free, 
                                strSizeFree, strSizeMax);
        }
    }
}