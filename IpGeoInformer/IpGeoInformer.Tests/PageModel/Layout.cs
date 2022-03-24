using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public abstract class Layout : PageBase
    {
        public Layout(IWebDriver webDriver)
        {
            Menu = new Menu(webDriver, By.ClassName("nav"), By.ClassName("nav__link"));
        }
        public Menu Menu { get; set; }
    }

    public class Menu : UiList<MenuItem>
    {
        public Menu(IWebDriver webDriver, By @by, By itemBy) : base(webDriver, @by, itemBy)
        {
        }
    }

    public class MenuItem : IClickElement, ITextElement
    {
        private readonly IWebElement _webElement;

        public MenuItem(IWebElement webElement)
        {
            _webElement = webElement;
        }

        public void Click()
        {
            _webElement.Click();
        }

        public string GetText()
        {
            return _webElement.Text;
        }

        public void WaitText(string text)
        {
            _webElement.WaitText(text);
        }
    }
}