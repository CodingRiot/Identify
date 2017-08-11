using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using identify.Notification;

namespace identify.Logs
{
    public class Logging
    {
        public static string logFilePath = Properties.Settings.Default.OutputFilePath;

        public static void LogEntry(Event.EventType type, string messageDetails, string additionalDetails = null)
        {
            try
            {
                var logPath = Path.Combine(logFilePath, type + "." + Properties.Settings.Default.OutputExtension);
                using (StreamWriter fileLog = new StreamWriter(logPath, true))
                {
                    fileLog.WriteLine(String.Format("[{0}] {4} {1} {2} {4} [{3}]", type, messageDetails,
                        additionalDetails, DateTime.Now.ToString(), LogConstants.DelimterChar));
                }
            }
            catch (Exception ex)
            {
                // Write error to Windows Event Log
                EventLog.WriteEntry(Application.ProductName, ex.Message, EventLogEntryType.Error, 0);
            }
        }

        public static void LogEntry(Exception ex)
        {
            LogEntry(Event.EventType.ERROR, ex.Message, ex.StackTrace);
        }

    }

    public static class LoggingExtensions
    {
        public static Event ParseLogDetails(this string inputString)
        {
            try
            {
                string[] eventDetails = inputString.Split(LogConstants.DelimterChar);
                
                return new Event()
                       {
                           //TypeOfEvent = (Event.EventType)Enum.Parse(typeof(Event.EventType), eventDetails[0].Replace("[", String.Empty).Replace("]", String.Empty).Trim(), true),
                           Details = eventDetails[1].Trim(),
                           TimeOccured = (DateTime)DateTime.Parse(eventDetails[2].Replace("[", String.Empty).Replace("]", String.Empty).Trim()),
                       };
            }
            catch (Exception ex)
            {
                Logging.LogEntry(ex);
                return null;
            }
        }
    }
}
