using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IchigoJamKeyBoard
{
    class AppSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public string command
        {
            get
            {
                return (string)this["Command"];
            }
            set
            {
                this["Command"] = value;
            }
        }
    }
}

