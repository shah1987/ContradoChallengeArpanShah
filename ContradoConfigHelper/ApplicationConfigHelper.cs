using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace ContradoConfigHelper
{
    public class ApplicationConfigHelper
    {
        private static IConfiguration _configuration;
        public static string Get(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(Get(key)))
                return defaultValue;

            return Get(key);
        }
        public static string Get(string key)
        {
            return _configuration[key];
        }
        public static T Get<T>(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(Get(key)))
                    return default(T);

                return (T)Convert.ChangeType(Get(key), typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        public static T GetOrDefault<T>(string key, T defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(Get(key)))
                    return defaultValue;

                return (T)Convert.ChangeType(Get(key), typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        public static void GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
