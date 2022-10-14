using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PPS.API.Shared.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the description of an enumerator
        /// </summary>
        /// <param name="enumerator">Enum value</param>
        /// <returns>Returns description if available. If description is not available, returns string value of enum</returns>
        public static string GetDescription(this Enum enumerator)
        {
            //get the field info
            FieldInfo fieldInfo = enumerator.GetType().GetField(enumerator.ToString());
            object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //return the description if it's found
            return (attributes.Length > 0) ? ((DescriptionAttribute)attributes[0]).Description
                : //if there's no description, return the string value of the enum
                enumerator.ToString();
        }
    
        /// <summary>
        /// Converts DateTime to respective time zone
        /// </summary>
        /// <param name="valueToConvert">DateTime value to convert</param>
        /// <param name="timeZone">TimeZone for conversion</param>
        /// <returns>Converted Time</returns>
        public static DateTime ConvertTimeZone(this DateTime valueToConvert, string timeZone)
        {
            return string.IsNullOrEmpty(timeZone) ?
                valueToConvert.ToUniversalTime() :
                TimeZoneInfo.ConvertTimeFromUtc(valueToConvert, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
        }

        /// <summary>
        /// Converts DateTime from Eastern time to UTC and back
        /// </summary>
        /// <param name="valueToConvert"></param>
        /// <param name="timeZone">Pass empty string to convert from Eastern time to UTC</param>
        /// <param name="bssTimeZone"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeZone(this DateTime valueToConvert, string timeZone, string bssTimeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(bssTimeZone);
                return TimeZoneInfo.ConvertTimeToUtc(valueToConvert, timeZoneInfo);
            }
            else
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return TimeZoneInfo.ConvertTimeFromUtc(valueToConvert, timeZoneInfo);
            }
        }

        /// <summary>
        /// Converts null DateTime to respective time zone
        /// </summary>
        /// <param name="valueToConvert">DateTime value to convert</param>
        /// <param name="timeZone">TimeZone for conversion</param>
        /// <returns>Converted Time</returns>
        public static DateTime? ConvertTimeZone(this DateTime? valueToConvert, string timeZone)
        {
            return valueToConvert?.ConvertTimeZone(timeZone);
        }

        /// <summary>
        /// Converts nullable DateTime from Eastern time to UTC and back
        /// </summary>
        /// <param name="valueToConvert"></param>
        /// <param name="timeZone">Pass empty string to convert from Eastern time to UTC</param>
        /// <param name="bssTimeZone"></param>
        /// <returns></returns>
        public static DateTime? ConvertTimeZone(this DateTime? valueToConvert, string timeZone, string bssTimeZone)
        {
                return valueToConvert?.ConvertTimeZone(timeZone, bssTimeZone);
        }

        public static string ConvertTimeSpanTohhmmtt(this TimeSpan? valueToConvert)
        {
            if (valueToConvert == null)
            {
                return null;
            }

            DateTime dt = new DateTime(2018, 01, 01); //any date
            dt = dt + (TimeSpan)valueToConvert;
            return dt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static string ConvertTimeSpanTohhmmtt(this TimeSpan valueToConvert)
        {
            DateTime dt = new DateTime(2018, 01, 01); //any date
            dt = dt + (TimeSpan)valueToConvert;
            return dt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static int GetBusinessDaysTo(this DateTime fromValue, DateTime toValue, bool toInclusive = false, List<DateTime> holidays = null)
        {
            if (fromValue.Date == toValue.Date)
            {
                return 0;
            }

            if (fromValue.Date > toValue.Date)
            {
                return -1;
            }

            int days = 0;
            bool holidaysSpecified = holidays != null && holidays.Any();

            for (DateTime dt = fromValue.Date; dt.Date <= toValue.Date; dt = dt.AddDays(1))
            {
                if (
                    holidaysSpecified && holidays.Any(x => x.Date == dt.Date) || 
                    dt.DayOfWeek == DayOfWeek.Saturday || 
                    dt.DayOfWeek == DayOfWeek.Sunday
                    )
                {
                    continue;
                }
                days++;
            }

            if (!toInclusive && days > 0)
            {
                days--;
            }
            return days;
        }

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the invariant culture, and ignoring the case of the strings being compared.
        /// </summary>
        /// <param name="source">Source value</param>
        /// <param name="stringToCompare"></param>
        /// <returns>Returns boolean if string and a specified String object have the same value</returns>
        public static bool EqualsIgnoreCase(this string source, string stringToCompare)
        {
            return source.Equals(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }
             
        /// <summary>
        /// Method for check difference between self and compareObject and excluding the ignoreProperties
        /// </summary>
        /// <typeparam name="LT"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="self"></param>
        /// <param name="compareObject"></param>
        /// <param name="ignorePropteries"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Equals<LT, RT>(this LT self, RT compareObject, params string[] ignorePropteries) 
            where LT : class
            where RT : class
        {
            var unequalProperties = new List<dynamic>();

            if (self != null && compareObject != null)
            {
                var lhsType = typeof(LT);
                var rhsType = typeof(RT);

                foreach (var prop in lhsType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var leftValue = lhsType.GetProperty(prop.Name).GetValue(self, null) != null ? lhsType.GetProperty(prop.Name).GetValue(self, null) : string.Empty;
                    var rightValue = rhsType.GetProperty(prop.Name) != null ? rhsType.GetProperty(prop.Name).GetValue(compareObject, null) : null;
                    if (leftValue != null && leftValue.ToString().Contains("System."))
                    {
                        continue;
                    }                    
                    if (leftValue != null && rightValue != null && !leftValue.Equals(rightValue) 
                        && (ignorePropteries == null || Array.IndexOf(ignorePropteries, prop.Name) < 0))                        
                    {
                        dynamic expandoObject = new ExpandoObject();
                        expandoObject.OldValue = leftValue.ToString();
                        expandoObject.NewValue = rightValue.ToString();
                        expandoObject.ColumnName = prop.Name;                      
                        unequalProperties.Add(expandoObject);
                    }
                }             
            }        

            return unequalProperties;
        }

        /// <summary>
        /// Method for generating log message from self and compareObject object
        /// </summary>
        /// <typeparam name="LT"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="self"></param>
        /// <param name="compareObject"></param>
        /// <param name="messageFormat"></param>
        /// <param name="ignorePropteries"></param>
        /// <returns></returns>
        public static string EqualsGetMessage<LT, RT>(this LT self, RT compareObject, string messageFormat, params string[] ignorePropteries)
            where LT : class
            where RT : class
        {
            var message = new StringBuilder(); 
                   
            var unequalProperties = Equals< LT, RT>(self, compareObject, ignorePropteries);

            foreach (var item in unequalProperties)
            {               
               message.Append(string.Format(messageFormat, item.OldValue, item.NewValue));                   
            }
            return message.ToString();
        }

        /// <summary>
        /// Method for log message from self and compareObject object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="compareObject"></param>
        /// <param name="messageFormat"></param>
        /// <param name="ignorePropteries"></param>
        /// <returns></returns>
        public static string EqualsGetMessage<T>(this T self, T compareObject, string messageFormat, params string[] ignorePropteries)
            where T : class
        {
            return EqualsGetMessage<T, T>(self, compareObject, messageFormat, ignorePropteries);
        }

        public static string ToAccountingString(this decimal amount, bool showZero = true)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var sAmount = "";
            if (amount < 0.00M)
            {
                sAmount = "(" + (-amount).ToString("N", culture) + ")";
            }
            else if (amount > 0.00M || showZero)
            {
                sAmount = amount.ToString("N", culture);
            }

            return sAmount;
        }

        public static string ToRoundTripLocalString(this DateTime date)
        {
            var localDate = new DateTime(date.Year, date.Month, date.Day, date.Hour,
                date.Minute, date.Second, DateTimeKind.Local);
            return localDate.ToString("o").Replace(".0000000", "");
        }

        public static string ToBemDateFormat(this DateTime date)
        {
            //2017-07-26T19:44:16-03:00
            return date.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public static string ToRoundTripLocalString(this DateTime? date)
        {
            return date.HasValue ? ToRoundTripLocalString(date.Value) : null;
        }

        public static string ToAccountingString(this decimal? amount, bool showZero = true)
        {
            return (amount ?? 0.00M).ToAccountingString(showZero);
        }

        /// <summary>
        /// Method for generating Expand Object 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static ExpandoObject GetDifferedObject(string oldValue, string newValue, string propertyName)
        {
            dynamic expandoObject = new ExpandoObject();
            expandoObject.OldValue = oldValue;
            expandoObject.NewValue = newValue;
            expandoObject.ColumnName = propertyName;
            return expandoObject;
        }
    }
}
