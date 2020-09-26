using System;

namespace MapParse.Util
{
    public static class ParseUtils
    {
        public static double RoundToSignificantDigits(double d, int digits)
        {
            if (Math.Abs(d) < Constants.Epsilon)
            {
                return 0;
            }

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }
    }
}
