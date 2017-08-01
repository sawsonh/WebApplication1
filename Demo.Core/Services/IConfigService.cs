using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Services
{
    public interface IConfigService
    {
        T GetValue<T>(string keyName);
    }
}
