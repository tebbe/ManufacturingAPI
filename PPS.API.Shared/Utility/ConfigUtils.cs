using System;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace PPS.API.Shared.Utility
{
	public class ConfigUtils
	{
		public static string GetSafeAppSettingValue(string key, string defaultValue = "")
		{
			try
			{
                if (ConfigurationManager.AppSettings.HasKeys() &&
				    ConfigurationManager.AppSettings.AllKeys.Any(k => k.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
				{
					return ConfigurationManager.AppSettings[key];
				}
				return defaultValue;
			}
			catch
			{
				return defaultValue;
			}			
		}

		public static int GetSafeAppSettingIntValue(string key, int defaultValue = 0)
		{
			int parsedValue;
			return int.TryParse(GetSafeAppSettingValue(key), out parsedValue)
				? parsedValue
				: defaultValue;
		}

		public static bool GetSafeAppSettingBoolValue(string key, bool defaultValue = false)
		{
			bool parsedValue;
			return bool.TryParse(GetSafeAppSettingValue(key), out parsedValue)
				? parsedValue
				: defaultValue;
		}

        public static bool GetSafeWebRequireSslValue(bool defaultValue = false)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                if (config != null)
                {
                    var webSection = config.GetSectionGroup("system.web") as SystemWebSectionGroup;

                    if (webSection != null)
                    {
                        return webSection.HttpCookies.RequireSSL;
                    }
                }
                return defaultValue;
            }
            catch(System.Exception e)
            {
                return defaultValue;
            }
        }
    }
}
