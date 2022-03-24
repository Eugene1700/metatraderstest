using System.Linq;
using IpGeoInformer.Tests.PageModel;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace IpGeoInformer.Tests
{
    public class IntegrationTests
    {
        private ChromeDriver _driver;


        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver {Url = "http://localhost:3000/"};
        }
        
        [TearDown]
        public void TearDown()
        {
            _driver.Dispose();
        }

        [Test]
        public void IpSearch()
        {
            var page = (SearchCoordinatesPage) new SearchCoordinatesPage(_driver).WaitPresent();
            page.Ip.SetValue("045006006007", "045.006.006.007");
            page.Search.Click();
            var result = (StaticText) page.Result.WaitPresent();
            result.WaitText("Широта: 29.6055\r\nДолгота: 101.6624");
        }
        
        [Test]
        public void IpSearch_NotFound()
        {
            var page = (SearchCoordinatesPage) new SearchCoordinatesPage(_driver).WaitPresent();
            page.Ip.SetValue("255255255255", "255.255.255.255");
            page.Search.Click();
            var result = (StaticText) page.Result.WaitPresent();
            result.WaitText("Локации не найдены");
        }
        
        [Test]
        public void IpSearch_IncorrectIp()
        {
            var page = (SearchCoordinatesPage) new SearchCoordinatesPage(_driver).WaitPresent();
            page.Ip.SetValue("333333333333", "333.333.333.333");
            page.Search.Click();
            var error = (StaticText) page.Error.WaitPresent();
            error.WaitText("Некорректный ip-адрес");
        }

        [Test]
        public void LocationSearch()
        {
            GoToSearchCity();

            var cityPage = (SearchLocationsByCityPage) new SearchLocationsByCityPage(_driver).WaitPresent();
            cityPage.City.SetValue("cit_Oze Ej Xa");
            cityPage.Button.Click();
            var result = (StaticText)cityPage.Result.WaitPresent();
            var table = result.GetText().Split("\r\n");
            Assert.That(table, Is.EquivalentTo(new []
            {
                "Страна Регион Город Организация Почтовый индекс Широта Долгота",
                "cou_AJ reg_Uxehox cit_Oze Ej Xa org_Uje Y Edekaj pos_082463 -127.1913 13.0411",
                "cou_UQA reg_Ypomi cit_Oze Ej Xa org_Ure pos_1052 121.7744 -48.24",
                "cou_YP reg_Ydozuv cit_Oze Ej Xa org_Ufu Enelisu pos_8359 148.1739 66.7258",
                "cou_IZ reg_Yka cit_Oze Ej Xa org_Anameqiq pos_8722822 -58.798 -39.9605",
                "cou_AF reg_A cit_Oze Ej Xa org_Ozenugadad Mymo A El pos_9899068 81.2216 82.9226",
                "cou_OX reg_Y cit_Oze Ej Xa org_Y Enunogogabi Yly Awixix pos_9161 -128.7259 150.1616",
                "cou_ACO reg_Udinem cit_Oze Ej Xa org_Aritib Gyz Giga Uxygiw pos_7887446 131.32 15.7718",
                "cou_IFY reg_Yj cit_Oze Ej Xa org_Yd J pos_319705 -40.0829 -41.6029",
            }));
        }

        [Test]
        public void LocationSearch_NotFound()
        {
            GoToSearchCity();
            
            var cityPage = (SearchLocationsByCityPage) new SearchLocationsByCityPage(_driver).WaitPresent();
            cityPage.City.SetValue("papap");
            cityPage.Button.Click();
            var result = (StaticText)cityPage.Result.WaitPresent();
            result.WaitText("Локации не найдены");
        }

        private void GoToSearchCity()
        {
            var page = (SearchCoordinatesPage) new SearchCoordinatesPage(_driver).WaitPresent();
            var menuItems = page.Menu.WaitItems(2);
            Assert.That(menuItems.Select(x => x.GetText()), Is.EqualTo(new[] {"Поиск по IP", "Поиск по городу"}));
            menuItems.Single(x => x.GetText() == "Поиск по городу").Click();
        }
    }
}