using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identify.ActiveApp
{
    class ActiveApplication
    {
        public string ApplicationName { get; set; }
        public DateTime LastAccessTime { get; set; }

        public string PreviouslyAccessedAppName { get; set; }
    }
}
