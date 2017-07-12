using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace System.Configuration
{
    /// <summary>
    /// Provides configuration system support for the AppSettings configuration section. This class cannot be inherited.
    /// </summary>
    internal sealed class AppSettingsSection : ConfigurationSection
    {
        private readonly Lazy<NameValueCollection> _nameValueCollection;

        public AppSettingsSection()
        {
            _nameValueCollection = new Lazy<NameValueCollection>(() =>
            {
                var section = ConfigurationManager.ConfigurationRoot.GetSection(ElementName);
                var nameValueCollection = new NameValueCollection();
                foreach (var child in section.GetChildren()) 
                    if (child.Value != null)
                        nameValueCollection.Add(child.Key, child.Value);
                return nameValueCollection;
            });
        }

        protected override string ElementName => "AppSettings";

        public NameValueCollection NameValueCollection => _nameValueCollection.Value;
    }
}
