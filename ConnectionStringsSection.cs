using Microsoft.Extensions.Configuration;

namespace System.Configuration
{
    /// <summary>
    /// Provides configuration system support for the ConnectionStrings configuration section. This class cannot be inherited.
    /// </summary>
    internal sealed class ConnectionStringsSection : ConfigurationSection
    {
        protected override string ElementName => "ConnectionStrings";
    }
}
