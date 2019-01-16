using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Core.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static int GetIntConfig(
            this IConfiguration configuration, string key, int defaultValue = 0)
        {
            if (!int.TryParse(configuration[key], out int result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
