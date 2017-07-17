using System.Collections.Generic;

namespace System.Configuration
{
    /// <summary>
    /// Provides configuration system support for the ConnectionStrings configuration section. This class cannot be inherited.
    /// </summary>
    internal sealed class ConnectionStringsSection : ConfigurationSection
    {
        public ConnectionStringSettingsCollection ConnectionStrings { get; } = new ConnectionStringSettingsCollection();

        protected override object this[string key]
        {
            get
            {
                SyncWithConnectionStrings();
                return base[key];
            }
            set
            {
                base[key] = value;
                ConnectionStrings[key] = (ConnectionStringSettings)base[key];
            }
        }

        private void SyncWithConnectionStrings()
        {
            var different = false;
            if (ConnectionStrings.Count == Count)
            {
                foreach (var item in ConnectionStrings)
                {
                    var baseValue = (ConnectionStringSettings)base[item.Key];
                    if (baseValue.ConnectionString != item.Value.ConnectionString
                        || baseValue.ProviderName != item.Value.ProviderName)
                    {
                        different = true;
                        break;
                    }
                }
            }
            else
            {
                different = true;
            }
            if (different)
            {
                Clear();
                foreach (var item in ConnectionStrings)
                {
                    base[item.Key] = item.Value;
                }
            }
        }

        protected override void CheckValue(string key, ref object value)
        {
            base.CheckValue(key, ref value);
            var stringValue = value as string;
            if (stringValue != null)
            {
                var settings = new ConnectionStringSettings { Name = key, ConnectionString = stringValue };
                value = settings;
            }
            var dictionary = value as Dictionary<string, object>;
            if (dictionary != null)
            {
                var settings = new ConnectionStringSettings { Name = key };
                object obj;
                if (dictionary.TryGetValue("connectionString", out obj)) settings.ConnectionString = (string)obj;
                if (dictionary.TryGetValue("providerName", out obj)) settings.ProviderName = (string)obj;
                value = settings;
            }
            if (value is ConnectionStringSettings) return;
            throw new ArgumentException("Invalid value for ConnectionStringsSection. Must be a string or an instance of ConnectionStringSettings.", nameof(value));
        }
    }
}
