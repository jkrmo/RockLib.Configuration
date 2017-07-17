using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Configuration
{
    /// <summary>
    /// Provides configuration system support for the AppSettings configuration section. This class cannot be inherited.
    /// </summary>
    internal sealed class AppSettingsSection : ConfigurationSection
    {
        public NameValueCollection Settings { get; } = new NameValueCollection();

        protected override object this[string key]
        {
            get
            {
                SyncWithSettings();
                return base[key];
            }
            set
            {
                base[key] = value;
                Settings[key] = (string)value;
            }
        }

        private void SyncWithSettings()
        {
            var different = false;
            if (Settings.Count == Count)
            {
                foreach (string settingsKey in Settings.Keys)
                {
                    if (!Settings[settingsKey].Equals(base[settingsKey]))
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
                foreach (string settingsKey in Settings.Keys)
                {
                    base[settingsKey] = Settings[settingsKey];
                }
            }
        }

        protected override void CheckValue(string key, ref object value)
        {
            base.CheckValue(key, ref value);
            if (!(value is string))
            {
                var converter = TypeDescriptor.GetConverter(value);
                if (!converter.CanConvertTo(typeof(string)))
                    throw new ArgumentException("Invalid value for AppSettingsSection. Must be string or convertible to string via TypeDescriptor.GetConverter.", nameof(value));
                value = converter.ConvertTo(value, typeof(string));
            }
        }
    }
}
