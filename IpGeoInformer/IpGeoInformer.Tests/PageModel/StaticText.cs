using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class StaticText : ElementBase, ITextElement
    {
        public StaticText(IWebDriver webDriver, By @by) : base(webDriver, @by)
        {
        }

        public string GetText()
        {
            return Element.Text;
        }

        public void WaitText(string text)
        {
            Element.WaitText(text);
        }
    }
}