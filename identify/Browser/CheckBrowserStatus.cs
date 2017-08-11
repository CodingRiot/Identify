using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using identify.Logs;
using identify.Notification;

namespace identify.Browser
{
    class CheckBrowserStatus
    {
        public BrowserList BrowserList = new BrowserList();
        public BrowserHook BrowserHook = new BrowserHook();

        public CheckBrowserStatus()
        {
            var browserTimer = new System.Timers.Timer(1000);
            browserTimer.Elapsed += new ElapsedEventHandler(OnBrowserCheck);
            browserTimer.Enabled = true;
        }

        private void OnBrowserCheck(object sender, ElapsedEventArgs e)
        {
            foreach (var browser in BrowserList.Browsers.Where(x=>x.BrowserApplicationName != null))
            {
                ProcessBrowserDetails(browser);
            }
        }

        private void ProcessBrowserDetails(Browser browser)
        {
            
            foreach (Process process in Process.GetProcessesByName(browser.BrowserApplicationName.ToLower()))
            {
                try
                {
                    string url = BrowserHook.GetBrowserTabInfo(browser.BrowserType, process);
                    if (url == null)
                        continue;

                    if (BrowserList.Browsers[Convert.ToInt32(browser.BrowserType)].PreviousPageAccessed != url)
                    {
                        // To prevent duplicate entries to the log for the same page, without a change in page.
                        Logging.LogEntry(Event.EventType.Browser, browser.BrowserType + " " + process.MainWindowTitle, url);
                    }

                    BrowserList.Browsers[Convert.ToInt32(browser.BrowserType)].PreviousPageAccessed = url;
                }
                catch (Exception ex)
                {
                    Logging.LogEntry(Event.EventType.ERROR, ex.Message);
                }
            }
        }

    }
}
