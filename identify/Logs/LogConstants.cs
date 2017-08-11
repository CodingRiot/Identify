using System.Security.Principal;

namespace identify.Logs
{
    public static class LogConstants
    {
        public static char DelimterChar { get { return '^'; } }
        public static WindowsIdentity userAcct { get { return WindowsIdentity.GetCurrent(); } }
    }
}