using System;
using System.Diagnostics;
using IpGeoInformer.Models;
using IpGeoInformer.Services;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer.Tests
{
    public class Tests
    {
        private GeoIpDataProvider _dataProvider;

        [SetUp]
        public void Setup()
        {
            // const string filePath = "geobase.dat";
            var filePath = @"C:\emm\metatraderstest\IpGeoInformer\IpGeoInformer.Tests\geobase.dat";
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var dataLoader = new GeoIpDataLoader(memoryCache);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            dataLoader.Load(filePath);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"loadTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            _dataProvider = new GeoIpDataProvider(memoryCache);
        }

        [TestCase("0.2.80.186", 18.2742004f, 91.9888f)]
        [TestCase("0.6.252.87", -169.201294f, -137.628693f)]
        public void IpSearch(string ip, float expectedLatitude, float expectedLongitude)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var place = _dataProvider.SearchPlaceByIp(ip);
            stopwatch.Stop();
            Assert.That(place, Is.Not.Null);
            Assert.That(place.Latitude, Is.EqualTo(expectedLatitude));
            Assert.That(place.Longitude, Is.EqualTo(expectedLongitude));
            var stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"search time={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
        }

        private static readonly object[] SourcePlaces =
        {
            new object[]
            {
                "cit_Ageqenat",
                new[]
                {
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_YZU",
                        Organization = "org_Usazakybyt Duh R Vadah",
                        Postal = "pos_8101514",
                        Region = "reg_Abu",
                        Latitude = -2.26679993f,
                        Longitude = -84.1287994f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_YKY",
                        Organization = "org_Awecy Oqe Am",
                        Postal = "pos_514485",
                        Region = "reg_Ep Ky",
                        Latitude = 120.204002f,
                        Longitude = -46.2088013f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_YFO",
                        Organization = "org_Ugydom Malyzucipuvo",
                        Postal = "pos_295981",
                        Region = "reg_Iju ",
                        Latitude = -44.6515007f,
                        Longitude = -161.159302f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_ED",
                        Organization = "org_Ico E",
                        Postal = "pos_3050086",
                        Region = "reg_O ",
                        Latitude = -131.467499f,
                        Longitude = -14.9900999f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_OPA",
                        Organization = "org_Ikixivejesycycizycif",
                        Postal = "pos_28456",
                        Region = "reg_O",
                        Latitude = 82.2771988f,
                        Longitude = 39.4485016f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_AT",
                        Organization = "org_Osy I Afyjageticov",
                        Postal = "pos_73837",
                        Region = "reg_Urohu",
                        Latitude = -158.169006f,
                        Longitude = -176.281998f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_YS",
                        Organization = "org_Iwokaf Bi",
                        Postal = "pos_02253",
                        Region = "reg_Izuz",
                        Latitude = 55.7402f,
                        Longitude = -85.0997009f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_UJ",
                        Organization = "org_Ihofil Botavofy Orum",
                        Postal = "pos_6662406",
                        Region = "reg_Exoc",
                        Latitude = -47.9598007f,
                        Longitude = -39.5966988f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_EHU",
                        Organization = "org_Ovoginid Kyzukexyku Axot",
                        Postal = "pos_32495",
                        Region = "reg_E Ogy",
                        Latitude = -34.4183006f,
                        Longitude = -150.490005f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_OW",
                        Organization = "org_Una",
                        Postal = "pos_24549",
                        Region = "reg_Oko",
                        Latitude = -3.2197001f,
                        Longitude = 23.6578999f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_UJ",
                        Organization = "org_Akadyjimi Yl L",
                        Postal = "pos_219551",
                        Region = "reg_Edoza",
                        Latitude = 140.798004f,
                        Longitude = 45.6664009f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ageqenat",
                        Country = "cou_EKU",
                        Organization = "org_Ucerywevycubamevehu",
                        Postal = "pos_7008",
                        Region = "reg_O",
                        Latitude = -106.067596f,
                        Longitude = -13.4800997f
                    }
                }
            },
            new object[]
            {
                "cit_Ahijinus L",
                new[]
                {
                    new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_YWU",
                        Organization = "org_Ywu",
                        Postal = "pos_6915735",
                        Region = "reg_U",
                        Latitude = -178.006195f,
                        Longitude = 95.6173019f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_ED",
                        Organization = "org_Ofakyvawyqehahebajaf",
                        Postal = "pos_469797",
                        Region = "reg_Eg ",
                        Latitude = -62.8851013f,
                        Longitude = -115.757599f
                    },
                    new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_YFO",
                        Organization = "org_Ydekynako U",
                        Postal = "pos_981734",
                        Region = "reg_Oxuz",
                        Latitude = -141.423492f,
                        Longitude = 139.398804f
                    },new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_ESI",
                        Organization = "org_Ynuxa Ecocoveg ",
                        Postal = "pos_45992",
                        Region = "reg_Obe",
                        Latitude = 80.5463028f,
                        Longitude = -76.2074966f
                    },new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_IN",
                        Organization = "org_Ytojep Qujumutado",
                        Postal = "pos_3753",
                        Region = "reg_Oli ",
                        Latitude = -50.9779015f,
                        Longitude = 111.331299f
                    },new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_OX",
                        Organization = "org_Ez Tusebame",
                        Postal = "pos_8025074",
                        Region = "reg_Ydozuv",
                        Latitude = -0.408199996f,
                        Longitude = 163.548706f
                    },new PlaceDto
                    {
                        City = "cit_Ahijinus L",
                        Country = "cou_IN",
                        Organization = "org_Ac Du Upacu Y",
                        Postal = "pos_4553933",
                        Region = "reg_O",
                        Latitude = -57.2331009f,
                        Longitude = -116.854897f
                    },
                }
            },
            new object[]
            {
                "cit_Oze Ej Xa",
                new[]
                {
                    new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_AJ",
                        Organization = "org_Uje Y Edekaj",
                        Postal = "pos_082463",
                        Region = "reg_Uxehox",
                        Latitude = -127.191299f,
                        Longitude = 13.0410995f
                    },new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_UQA",
                        Organization = "org_Ure",
                        Postal = "pos_1052",
                        Region = "reg_Ypomi",
                        Latitude = 121.774399f,
                        Longitude = -48.2400017f
                    },new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_YP",
                        Organization = "org_Ufu Enelisu ",
                        Postal = "pos_8359",
                        Region = "reg_Ydozuv",
                        Latitude = 148.173904f,
                        Longitude = 66.7257996f
                    },new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_IZ",
                        Organization = "org_Anameqiq",
                        Postal = "pos_8722822",
                        Region = "reg_Yka",
                        Latitude = -58.7980003f,
                        Longitude = -39.9604988f
                    },
                    new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_AF",
                        Organization = "org_Ozenugadad Mymo A El",
                        Postal = "pos_9899068",
                        Region = "reg_A",
                        Latitude = 81.2216034f,
                        Longitude = 82.9225998f
                    },new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_OX",
                        Organization = "org_Y Enunogogabi Yly Awixix",
                        Postal = "pos_9161",
                        Region = "reg_Y",
                        Latitude = -128.725906f,
                        Longitude = 150.161606f
                    },new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_ACO",
                        Organization = "org_Aritib Gyz Giga Uxygiw",
                        Postal = "pos_7887446",
                        Region = "reg_Udinem",
                        Latitude = 131.320007f,
                        Longitude = 15.7718f
                    },
                    new PlaceDto
                    {
                        City = "cit_Oze Ej Xa",
                        Country = "cou_IFY",
                        Organization = "org_Yd J",
                        Postal = "pos_319705",
                        Region = "reg_Yj",
                        Latitude = -40.082901f,
                        Longitude = -41.6029015f
                    },
                }
            },
        };

        [TestCaseSource(nameof(SourcePlaces))]
        public void PlacesSearch(string city, PlaceDto[] expPlaces)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var places = _dataProvider.SearchPlacesByCity(city);
            stopwatch.Stop();
            Assert.That(places, Is.EquivalentTo(expPlaces));
            var stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"search time={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
        }
    }
}