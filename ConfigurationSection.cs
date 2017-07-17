//using Microsoft.Extensions.Configuration;
//using RockLib.Configuration;
//using System.ComponentModel;

namespace System.Configuration
{
    internal abstract class ConfigurationSection : ConfigurationElement
    {
        ///// <summary>
        ///// Gets or sets a configuration value.
        ///// </summary>
        ///// <param name="key">The configuration key.</param>
        ///// <value>The configuration value.</value>
        //protected override dynamic this[string key]
        //{
        //    get
        //    {
        //        if (base[key] != null) return base[key];
        //        lock (this)
        //        {
        //            if (base[key] != null) return base[key];

        //            var section = ConfigurationManager.ConfigurationRoot.GetSection(ElementName);

        //            if (section.Value != null) base[key] = section.Value;
        //            else base[key] = new ConvertibleConfigurationSection(section);
        //        }

        //        return base[key];
        //    }
        //    set
        //    {
        //        var section = ConfigurationManager.ConfigurationRoot.GetSection(ElementName);

        //        var valueString = value as string;
        //        if (valueString != null)
        //            section[key] = valueString;
        //        else
        //        {
        //            var converter = TypeDescriptor.GetConverter(value);
        //            if (converter.CanConvertTo(typeof(string)))
        //                section[key] = (string)converter.ConvertTo(value, typeof(string));
        //            else
        //            {
        //                // TODO: figure out how to "serialize" to config
        //                throw new NotSupportedException($"The {nameof(ConfigurationSection)} indexer only supports setting simple values. Attempted to set a value of type {value.GetType()}.");
        //            }
        //        }

        //        base[key] = value;
        //    }
        //    //get => ConfigurationManager.ConfigurationRoot.GetSection(ElementName)[key];
        //    //set => ConfigurationManager.ConfigurationRoot.GetSection(ElementName)[key] = value;
        //}

        //protected abstract string ElementName { get; }
    }
}
