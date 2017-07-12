using Newtonsoft.Json;
using System;
using System.Configuration;

namespace RockLib.Configuration.Example.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Configuration Manager Exmple Harness");

            GetAndDisplayAppSettingValue("ApplicationId");
            GetAndDisplayConnectionValue("Alternate");
            GetAndDisplaySection("Foo");


            Console.ReadLine();
        }

        private static void GetAndDisplayAppSettingValue(string key)
        {
            var foundValue = ConfigurationManager.AppSettings[key];

            Console.WriteLine($"AppSetting value for Key '{key}' is '{foundValue}'");
        }

        private static void GetAndDisplayConnectionValue(string key)
        {
            var foundValue = ConfigurationManager.ConnectionStrings[key];

            Console.WriteLine($"Connection String value for Key '{key}' is '{foundValue}'");
        }

        private static void GetAndDisplaySection(string key)
        {
            var fooSection = (FooSection)ConfigurationManager.GetSection(key);

            Console.WriteLine($"Section Value for Key '{key}' is '{JsonConvert.SerializeObject(fooSection)}'");
        }

        class FooSection
        {
            public int Bar { get; set; }
            public string Baz { get; set; }
            public bool Qux { get; set; }
        }
    }
}