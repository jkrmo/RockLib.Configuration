﻿using System;
using Microsoft.Extensions.Configuration;

namespace RockLib.Configuration
{
    /// <summary>
    /// Extension methods for adding RockLib's standard JSON config file to a <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class RockLibConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the RockLib configuration provider to the builder using the configuration file "rocklib.config.json",
        /// relative to the base path stored in <see cref="IConfigurationBuilder.Properties"/> of the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is null.</exception>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRockLib(this IConfigurationBuilder builder)
        {
            string supplementalJsonConfigPath = null;

            var environment = Environment.GetEnvironmentVariable("AppSettings:Environment");
            if (!string.IsNullOrEmpty(environment))
                supplementalJsonConfigPath = $"{environment.ToLower()}.rocklib.config.json";

            return builder.AddRockLib("rocklib.config.json", supplementalJsonConfigPath);
        }

        /// <summary>
        /// Adds the RockLib configuration provider to the builder using the specified configuration file,
        /// relative to the base path stored in <see cref="IConfigurationBuilder.Properties"/> of the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="jsonConfigPath">The path of the json config file.</param>
        /// <param name="supplementalJsonConfigPath">The path of the supplemental json config file.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="jsonConfigPath"/> is null or empty.</exception>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRockLib(
            this IConfigurationBuilder builder, string jsonConfigPath, string supplementalJsonConfigPath = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrEmpty(jsonConfigPath)) throw new ArgumentException($"'{nameof(jsonConfigPath)}' must be a non-empty string.", nameof(jsonConfigPath));

            // we want the optional value to be true so that it will not throw a runtime exception if the file is not found
            // the Rocklib.config.json is should not be required but just used.
            builder = builder.AddJsonFile(jsonConfigPath, optional: true);

            if (!string.IsNullOrEmpty(supplementalJsonConfigPath))
                builder = builder.AddJsonFile(supplementalJsonConfigPath, optional: true);

            return builder;
        }
    }
}
