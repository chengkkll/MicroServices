﻿using AutoMapper;
using System;

namespace TianCheng.Common
{
    /// <summary>
    /// String -> DateTime?
    /// </summary>
    public class StringToDateTimeNullConverter : ITypeConverter<string, DateTime?>
    {
        /// <summary>
        ///  ObjectId -> String
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            if (DateTime.TryParse(source, out DateTime dt))
            {
                return dt;
            }
            return null;
        }
    }
    /// <summary>
    /// DateTime? -> String
    /// </summary>
    public class DateTimeNullToStringConverter : ITypeConverter<DateTime?, string>
    {
        /// <summary>
        ///  DateTime? -> String
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            if (source == null || source.Value == DateTime.MinValue || source.Value == DateTime.MaxValue)
            {
                return "";
            }

            return source.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
