using System.Configuration;
using Finances.Core.Interfaces;

namespace Finances.Core
{
    public class AppSettings : IAppSettings
    {
        public string GetSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
