using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpaceOrEmpty(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;
            if (string.IsNullOrEmpty(value))
                return true;

            return false;
        }
        public static string ToFullName(string firstName, string lastName)
        {
            var fullName = firstName + " " + lastName;

            return fullName.Trim();
        }
    }
}