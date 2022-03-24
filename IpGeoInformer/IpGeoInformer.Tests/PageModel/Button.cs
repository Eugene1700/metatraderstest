using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class Button : ElementBase, IClickElement
    {
        public Button(IWebDriver webDriver, By @by) : base(webDriver, @by)
        {
        }

        public void Click()
        {
            Element.Click();
        }
    }
}