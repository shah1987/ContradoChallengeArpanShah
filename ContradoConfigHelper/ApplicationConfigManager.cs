using System;
using System.Collections.Generic;
using System.Text;

namespace ContradoConfigHelper
{
    public static class ApplicationConfigManager
    {
        public static string SqlConnectionString
        {
            get
            {
                return ApplicationConfigHelper.Get<string>(ApplicationConfigKeys.SqlConnectionString);
            }
        }
        public static string CORSOrigin
        {
            get
            {
                return ApplicationConfigHelper.Get<string>(ApplicationConfigKeys.CORSOrigin);
            }
        }
    }
}
