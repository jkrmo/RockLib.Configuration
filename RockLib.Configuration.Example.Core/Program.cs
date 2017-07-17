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

            try
            {
                string applicationId = ConfigurationManager.AppSettings["ApplicationId"];
                var defaultConnectionString = ConfigurationManager.ConnectionStrings["Special"];
                //FooSection foo = (FooSection)ConfigurationManager.GetSection("Foo");
                //FooSection foo2 = (FooSection)ConfigurationManager.GetSection("Foo");

                AppSettingsSection appSettings = (AppSettingsSection)ConfigurationManager.GetSection("AppSettings");
                ConnectionStringsSection connectionStrings = (ConnectionStringsSection)ConfigurationManager.GetSection("ConnectionStrings");

                //appSettings.Settings["ApplicationId"] = "12345";
                ConfigurationManager.AppSettings["ApplicationId"] = "54321";

                var x = appSettings.Settings["ApplicationId"];
                //var x = ConfigurationManager.AppSettings["ApplicationId"];

                //Console.WriteLine($"applicationId: {applicationId}");
                //Console.WriteLine($"defaultConnectionString: {defaultConnectionString.ConnectionString}, provider: {defaultConnectionString.ProviderName}");
                //Console.WriteLine($"foo: {JsonConvert.SerializeObject(foo)}");
                //Console.WriteLine($"foo is same instance as foo2: {ReferenceEquals(foo, foo2)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        class FooSection
        {
            public int Bar { get; set; }
            public string Baz { get; set; }
            public bool Qux { get; set; }
        }
    }
}