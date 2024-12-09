using OpenQA.Selenium.Chrome;

namespace TOZTestApp.Selenium
{
    public class SeleniumManager
    {
        private ChromeDriver _driver;
        public SeleniumManager()
        {
            var chromeOptions = new ChromeOptions();
            _driver = new ChromeDriver(chromeOptions);
        }
        public string GetHtmlPage(string htmlLink)
        {
            _driver.Navigate().GoToUrl(htmlLink);
            return _driver.PageSource;
        }
        ~SeleniumManager()
        {
            _driver.Close();
        }
    }
}
