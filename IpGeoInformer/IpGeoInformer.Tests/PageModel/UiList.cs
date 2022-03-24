using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace IpGeoInformer.Tests.PageModel
{
    public class UiList<T> : ElementBase
    {
        public TP Create<TP>(object?[] paramsValue){
            return (TP)Activator.CreateInstance(typeof(TP), paramsValue);
        }
        private readonly By _itemBy;

        public T[] GetItems()
        {
            return Driver.FindElements(_itemBy).Select(x =>
                Create<T>(new object?[] {x})).ToArray();
        }
        
        public T[] WaitItems(int cnt)
        {
            ReadOnlyCollection<IWebElement> items = null;
            Waiter.Wait(() =>
            {
                items = Driver.FindElements(_itemBy);
                return items.Count == cnt;
            });
            return items?.Select(x =>
                Create<T>(new object?[] {x})).ToArray();
        }

        public UiList(IWebDriver webDriver, By @by, By itemBy) : base(webDriver, @by)
        {
            _itemBy = itemBy;
        }
    }
}