using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Interfaces
{
    public interface IAppSettings
    {
        string GetSetting(string name);
    }
}
