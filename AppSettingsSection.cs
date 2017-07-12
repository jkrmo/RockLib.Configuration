using Microsoft.Extensions.Configuration;

namespace System.Configuration
{
    /// <summary>
    /// Provides configuration system support for the AppSettings configuration section. This class cannot be inherited.
    /// </summary>
    internal sealed class AppSettingsSection : ConfigurationSection
    {
        protected override string ElementName => "AppSettings";
    }
}
