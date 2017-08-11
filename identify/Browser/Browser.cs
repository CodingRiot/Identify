namespace identify.Browser
{
    public class Browser
    {
        public BrowserHook.BrowserType BrowserType { get; set; }

        public string PreviousPageAccessed { get; set; }
        public string BrowserApplicationName { get; set; }

    }

    public class BrowserList
    {
        public Browser[] Browsers { get; set; }

        public BrowserList()
        {
            Browsers = new Browser[]
                {
                    new Browser(){ BrowserType = BrowserHook.BrowserType.InternetExplorer, PreviousPageAccessed = null, BrowserApplicationName = "iexplore" }, 
                    new Browser(){ BrowserType = BrowserHook.BrowserType.FireFox, PreviousPageAccessed = null, BrowserApplicationName = "firefox" }, 
                    new Browser(){ BrowserType = BrowserHook.BrowserType.Chrome, PreviousPageAccessed = null, BrowserApplicationName = "chrome" }, 
                };
        }
    }
}
