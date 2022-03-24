using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class SearchLocationsByCityPage : Layout
    {
        public override bool IsPresent()
        {
            return City.IsPresent() || Button.IsPresent();
        }

        public SearchLocationsByCityPage(IWebDriver webDriver) : base(webDriver)
        {
            City = new Input(webDriver, By.ClassName("city-search-input"));
            Button = new Button(webDriver, By.ClassName("city-search-submit"));
            Result = new StaticText(webDriver, By.ClassName("city-search-result"));
        }

        public Input City { get; }
        public Button Button { get; }
        public StaticText Result { get; }
    }
}