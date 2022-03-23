using System;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace IpGeoInformer.Tests
{
    public static class SeleniumExtensions
    {
        public static IWebElement WaitElement(this IWebDriver driver, By by, int timeoutInSeconds = 0)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            else
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                return wait.Until(drv => drv.FindElement(by));
            }
        }
        
        public static ReadOnlyCollection<IWebElement> WaitElements(this IWebDriver driver, By by, int timeoutInSeconds = 0)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElements(by));
            }
            else
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                return wait.Until(drv => drv.FindElements(by));
            }
        }

        public static IWebElement WaitOneElement(this IWebDriver driver, params By[] by)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            return wait.Until(drv =>
            {
                IWebElement current = null;
                for (int i = 0; i < by.Length; i++)
                {
                    try
                    {
                        current = drv.FindElement(by[i]);
                        if (current != null)
                            return current;
                    }
                    catch (NoSuchElementException e)
                    {
                        continue;
                    }
                    
                }
                throw new NoSuchElementException();
            });
        }

        public static bool TryFindElement(this IWebElement element, By by, out IWebElement foundElement)
        {
            try
            {
                foundElement = element.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
                foundElement = null;
                return false;
            }
        }
        
        public static bool TryFindElements(this IWebElement element, By by, out IWebElement[] foundElements)
        {
            try
            {
                foundElements = element.FindElements(by).ToArray();
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
                foundElements = null;
                return false;
            }
        }
        
        public static bool TryFindElement(this IWebDriver element, By by, out IWebElement foundElement)
        {
            try
            {
                foundElement = element.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
                foundElement = null;
                return false;
            }
        }
        
        public static bool TryFindElements(this IWebDriver element, By by, out IWebElement[] foundElements)
        {
            try
            {
                foundElements = element.FindElements(by).ToArray();
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
                foundElements = null;
                return false;
            }
        }

        public static void WaitText(this IWebDriver driver, By by, string text)
        {
            Waiter.Wait(() => driver.FindElement(by).Text == text,
                (i) => Assert.Fail($"Текст [{text}] не найден для элемента [{driver.FindElement(by).GetAttribute("class")}], был [{driver.FindElement(by).Text}]"));
        }
    }
}