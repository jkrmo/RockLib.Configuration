using System.Collections.Generic;

namespace System.Configuration
{
    internal abstract class ConfigurationSection
    {
        /// <summary>
        /// Gets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <value>The configuration value.</value>
        /// <exception cref="KeyNotFoundException">If there is no configuration value for the given key.</exception>
        public string this[string key]
        {
            get
            {
                var value = ConfigurationManager.ConfigurationRoot.GetSection(ElementName)[key];

                if (value == null)
                {
                    throw new KeyNotFoundException($"The given key, '{key}', was not present in the configuration's '{ElementName}' section.");
                }

                return value;
            }
        }

        protected abstract string ElementName { get; }
    }
}
