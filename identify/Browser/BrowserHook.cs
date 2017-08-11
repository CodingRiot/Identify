using System;
using System.CodeDom;
using System.Diagnostics;
using System.Windows.Automation;
using identify.Logs;
using NDde.Client;
using TreeScope = System.Windows.Automation.TreeScope;

namespace identify.Browser
{
    public class BrowserHook
    {

        public enum BrowserType
        {
            InternetExplorer = 0,
            FireFox = 1,
            Chrome = 2,
            NoneSpecified,
        }
        

        public string GetBrowserTabInfo(BrowserType browser, Process process)
        {
            try
            {
                switch (browser)
                {
                    case BrowserType.InternetExplorer:
                        return GetInternetExplorerUrl(process);

                    case BrowserType.FireFox:
                        return GetFirefoxUrl(process);

                    case BrowserType.Chrome:
                        return GetChromeUrl(process);

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Uses the 'UIAutomationClient.dll' and 'UIAutomationTypes' for AutomationElement objects
        // Reference from:  http://stackoverflow.com/questions/5317642/retrieve-current-url-from-c-sharp-windows-forms-application
        private string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                //return "Chrome process open, but unable to get details from window.";
                return null;
            }

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement edit = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            if (edit != null)
            {
                return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            }
            return null;
        }

        private string GetInternetExplorerUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                return null;
            }
                

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            AutomationElement edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            if (edit != null)
            {
                return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            }
            return null;
        }

        private string GetFirefoxUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                return null;
            }

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;


            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

    }
}
