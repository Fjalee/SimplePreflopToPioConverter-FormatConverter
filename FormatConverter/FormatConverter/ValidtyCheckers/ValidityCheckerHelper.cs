using System.Text.RegularExpressions;

namespace FormatConverter.ValidtyCheckers
{
    public static class ValidityCheckerHelper
    {
        public static bool IsValidFirstAndLast(string fstRegexPattern, string lstRegexPattern, string[] allVals)
        {
            var fstRegex = new Regex(fstRegexPattern);
            var lstRegex = new Regex(lstRegexPattern);
            var corrFst = IsValid(fstRegexPattern, allVals.First());
            var corrLst = IsValid(lstRegexPattern, allVals.Last());

            return corrFst && corrLst;
        }

        public static bool IsValid(string regexPattern, string val)
        {
            var regex = new Regex(regexPattern);
            return regex.IsMatch(val);
        }
    }
}
