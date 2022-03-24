using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class Input : ElementBase, IValueElement
    {
        public Input(IWebDriver webDriver, By by) : base(webDriver, by)
        {
            
        }
        
        public string GetValue()
        {
            return Element.GetAttribute("value");
        }

        public void SetValue(string value, string expectedValue = null)
        {
            Element.SendKeys(value);
            Element.WaitValue(expectedValue ?? value);
        }
    }
}