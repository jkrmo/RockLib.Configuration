namespace System.Configuration
{
    internal class ConnectionStringSettings : ConfigurationElement
    {
        public override void SetElementKey(string key)
        {
            Name = key;
        }

        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        public string ConnectionString
        {
            get => (string)this["connectionString"];
            set => this["connectionString"] = value;
        }

        public string ProviderName
        {
            get => (string)this["providerName"];
            set => this["providerName"] = value;
        }
    }
}
