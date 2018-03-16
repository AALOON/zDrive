﻿using System;

namespace zDrive.Helpers
{
    public static class Extensions
    {
        public static bool Compare(this string s1, string s2)
        {
            if (ReferenceEquals(s1, s2))
                return true;
            return  s1!=null  && s2 != null && s1.Equals(s2, StringComparison.CurrentCulture);
        }
    }
}
