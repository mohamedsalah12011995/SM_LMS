using Microsoft.Extensions.Configuration;

namespace RM.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string ReadConfigurationFromSection(this IConfiguration configuration, string sectionName)
        {
            try
            {
                return configuration.GetSection("AppSettings").GetSection(sectionName).Value;
            }
            catch
            {
                return null;
            }
        }
          

    }
}
