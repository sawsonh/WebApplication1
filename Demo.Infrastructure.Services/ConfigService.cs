using System;
using System.Collections.Generic;
using System.Text;
using Demo.Core.Services;
using Microsoft.Extensions.Configuration;

namespace Demo.Infrastructure.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfigurationRoot _cfgRoot;

        public ConfigService(IConfigurationRoot cfgRoot)
        {
            _cfgRoot = cfgRoot;
        }

        public T GetValue<T>(string keyName)
        {
            try
            {
                var value = _cfgRoot[keyName];
                if (value == null)
                    return default(T);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
