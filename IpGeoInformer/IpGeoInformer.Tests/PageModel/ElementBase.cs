using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public abstract class ElementBase
    {
        protected IWebDriver Driver { get; }

        private readonly By _by;

        protected IWebElement Element => Driver.FindElement(_by);

        protected ElementBase(IWebDriver webDriver, By by)
        {
            _by = @by;
            Driver = webDriver;
        }

        public ElementBase WaitPresent()
        {
            Driver.WaitElement(_by);
            return this;
        }
        
        public bool IsPresent()
        {
            return Driver.TryFindElement(_by, out _);
        }
    }

    public interface IValueElement
    {
        string GetValue();
        void SetValue(string value, string expectedValue = null);
    }
    
    public interface ITextElement
    {
        string GetText();

        void WaitText(string text);
    }
    
    public interface IClickElement
    {
        void Click();
    }
}