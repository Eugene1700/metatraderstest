using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class SearchCoordinatesPage : Layout
    {
        public SearchCoordinatesPage(IWebDriver webDriver) : base(webDriver)
        {
            var webDriver1 = webDriver;
            Ip = new Input(webDriver1, By.ClassName("ip-input"));
            Search = new Button(webDriver1, By.ClassName("ip-location-search"));
            Result = new StaticText(webDriver1, By.ClassName("ip-search-result"));
            Error = new StaticText(webDriver1, By.ClassName("content-error"));
        }

        public Input Ip { get; }
        public Button Search { get; }
        public StaticText Result { get; }
        public StaticText Error { get; }
        
        public override bool IsPresent()
        {
            return Ip.IsPresent() || Search.IsPresent();
        }
    }
}