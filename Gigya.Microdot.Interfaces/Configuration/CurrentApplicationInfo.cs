﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Gigya.Microdot.LanguageExtensions;

namespace Gigya.Microdot.Interfaces.Configuration
{
    /// <summary>
    /// Provides info about the current application.
    /// </summary>
    /// <remarks>
    /// I think we should use this class only for init IEnvironment the get should be in IEnvironment it simply the code 
    /// </remarks>
    public class CurrentApplicationInfo
    {
        /// <summary>Application/system/micro-service name, as provided by developer.</summary>
        public string Name { get; }

        /// <summary>The name of the operating system user that runs this process.</summary>
        public string OsUser { get; }

        /// <summary>The application version.</summary>
        public Version Version { get; }

        /// <summary>The Infrastructure version.</summary>
        public Version InfraVersion { get; }

        /// <summary>
        /// Name of host, the current process is running on.
        /// </summary>
        public string HostName { get; }
        // TODO: Some components require this to be static. Resolve
        public static string s_HostName { get; private set; }

        /// <summary>
        /// Indicates if the current process is running as a Windows service.
        /// </summary>
        public bool IsRunningAsWindowsService { get; }

        /// <summary>
        /// Indicates whether current process has an interactive console window.
        /// </summary>
        public bool HasConsoleWindow { get; }

        /// <summary>
        /// Waring take it form IEnvironment
        /// </summary>
        internal string InstanceName { get; }

        public CurrentApplicationInfo(
            string name, 
            string osUser,
            string hostName,
            string instanceName = null, Version infraVersion = null)
        {
            Name     = name.NullWhenEmpty()     ?? throw new ArgumentNullException(nameof(name));
            OsUser   = osUser.NullWhenEmpty()   ?? throw new ArgumentNullException(nameof(osUser));
            HostName = hostName.NullWhenEmpty() ?? throw new ArgumentNullException(nameof(hostName));

            s_HostName = HostName;

            Version = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Version;

            // ReSharper disable once PossibleNullReferenceException
            // TODO: Test whether this code is still correct or remove outright
            IsRunningAsWindowsService = Environment.OSVersion.Platform == PlatformID.Win32NT &&
                OsUser == @"NT AUTHORITY\SYSTEM";

            // TODO: Consider using Environment.UserInteractive
            HasConsoleWindow = !IsRunningAsWindowsService && !Console.IsInputRedirected;

            // TODO: Possible error: Microdot version assigned to Infra version
            InfraVersion = infraVersion ?? typeof(CurrentApplicationInfo).Assembly.GetName().Version;

            InstanceName = instanceName;
        }
    }
}
