using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace IpGeoInformer.Tests
{
    public class IntegrationTests
    {
        [Test]
        public void IpSearch()
        {
            using var driver = new ChromeDriver {Url = "http://localhost:3000/"};
            var input = driver.WaitElement(By.ClassName("ip-input"));
            input.SendKeys("045006006007");
            driver.WaitText(By.ClassName("city-search-input"), "");
            var submit = driver.WaitElement(By.ClassName("ip-location-search"));
            submit.Click();
            var result = driver.WaitElement(By.ClassName("ip-search-result"));
            driver.WaitText(By.ClassName("ip-search-result"), "Широта: 29.6055\r\nДолгота: 101.6624");
        }
        
        [Test]
        public void LocationSearch()
        {
            using var driver = new ChromeDriver {Url = "http://localhost:3000/"};
            var menuItems = driver.WaitElements(By.ClassName("nav__link"));
            Assert.That(menuItems.Select(x => x.Text), Is.EqualTo(new[] {"Поиск по IP", "Поиск по городу"}));
            menuItems.Single(x=>x.Text == "Поиск по городу").Click();
            var input = driver.WaitElement(By.ClassName("city-search-input"));
            input.SendKeys("cit_Oze Ej Xa");
            driver.WaitText(By.ClassName("city-search-input"), "cit_Oze Ej Xa");
            var submit = driver.WaitElement(By.ClassName("city-search-submit"));
            submit.Click();
            var result = driver.WaitElement(By.ClassName("city-search-result"));
        }
    }
}