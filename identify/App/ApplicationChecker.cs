using System.Timers;
using identify.Logs;
using identify.Notification;

namespace identify.App
{
    class ApplicationChecker
    {
        public ApplicationHook ListenerApplication = new ApplicationHook();
        public ApplicationChecker()
        {
            var applicationTimer = new System.Timers.Timer(1000);
            applicationTimer.Elapsed += new ElapsedEventHandler(OnApplicationCheck);
            applicationTimer.Enabled = true;
        }

        private void OnApplicationCheck(object sender, ElapsedEventArgs e)
        {
            var newProcesses = ListenerApplication.CheckRunningProcesses();
            foreach (var process in newProcesses)
            {
                Logging.LogEntry(Event.EventType.Application, process.ProcessName, process.MainWindowTitle);
            }
        }
    }
}
