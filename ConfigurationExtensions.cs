﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds the user secrets configuration source.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(configuration.GetBasePath()))
            {
                var msg =  $"Unable to resolve path '{configuration.GetBasePath()}'; construct this {typeof (IConfigurationBuilder).Name} with a non-null {"BasePath"}.";
                throw new InvalidOperationException(msg);
            }

            var secretPath = PathHelper.GetSecretsPath(configuration.GetBasePath());
            return configuration.AddJsonFile(secretPath, optional: true);
        }

        /// <summary>
        /// Adds the user secrets configuration source with specified secrets id.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddUserSecrets(this IConfigurationBuilder configuration, string userSecretsId)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (userSecretsId == null)
            {
                throw new ArgumentNullException(nameof(userSecretsId));
            }

            var secretPath = PathHelper.GetSecretsPathFromSecretsId(userSecretsId);
            return configuration.AddJsonFile(secretPath, optional: true);
        }
    }
}