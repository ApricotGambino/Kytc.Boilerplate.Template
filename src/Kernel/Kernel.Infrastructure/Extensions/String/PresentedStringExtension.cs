// PresentedStringExtension.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using System.Text.RegularExpressions;

//TODO: Document this.
namespace Kernel.Infrastructure.Extensions.String
{
    public static class PresentedStringExtension
    {
        public static string ToPresentedFormat(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return inputString.TrimAndReduce().FirstLetterToUpperCase().AppendPeriodToEndIfMissing();
        }
        public static string TrimAndReduce(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return ConvertWhitespacesToSingleSpaces(inputString).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return Regex.Replace(inputString, @"\s+", " ");
        }

        public static string FirstLetterToUpperCase(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            char[] firstChar = inputString.ToCharArray();
            firstChar[0] = char.ToUpper(firstChar[0]);
            return new string(firstChar);
        }

        public static string AppendPeriodToEndIfMissing(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            if (!inputString.EndsWith('.'))
                return inputString + ".";

            return inputString;
        }
    }
}
