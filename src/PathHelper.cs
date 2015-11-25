﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Microsoft.Extensions.Configuration.UserSecrets
{
    public class PathHelper
    {
        private const string Secrets_File_Name = "secrets.json";

        public static string GetSecretsPath(string projectPath)
        {
            if (projectPath == null)
            {
                throw new ArgumentNullException(nameof(projectPath));
            }

            var projectFilePath = Path.Combine(projectPath, "project.json");

            if (!File.Exists(projectFilePath))
            {
                throw new InvalidOperationException(
                    $"Unable to locate a project.json at '{projectFilePath}'.");
            }

            var obj = JObject.Parse(File.ReadAllText(projectFilePath));
            var userSecretsId = obj.Value<string>("userSecretsId");

            if (string.IsNullOrEmpty(userSecretsId))
            {
                throw new InvalidOperationException(
                    $"Missing 'userSecretsId' in '{projectFilePath}'.");
            }

            return GetSecretsPathFromSecretsId(userSecretsId);
        }

        public static string GetSecretsPathFromSecretsId(string userSecretsId)
        {
            if (userSecretsId == null)
            {
                throw new ArgumentNullException(nameof(userSecretsId));
            }

            var badCharIndex = userSecretsId.IndexOfAny(Path.GetInvalidPathChars());
            if (badCharIndex != -1)
            {
                throw new InvalidOperationException(
                    $"Invalid character '{userSecretsId[badCharIndex]}' found in 'userSecretsId' value at index '{badCharIndex}'.");
            }

            var root = Environment.GetEnvironmentVariable("APPDATA") ??         // On Windows it goes to %APPDATA%\Microsoft\UserSecrets\
                        Environment.GetEnvironmentVariable("HOME");             // On Mac/Linux it goes to ~/.microsoft/usersecrets/

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPDATA")))
            {
                return Path.Combine(root, "Microsoft", "UserSecrets", userSecretsId, Secrets_File_Name);
            }
            else
            {
                return Path.Combine(root, ".microsoft", "usersecrets", userSecretsId, Secrets_File_Name);
            }
        }
    }
}