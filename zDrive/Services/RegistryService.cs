using System;
using System.Globalization;
using System.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using zDrive.Interfaces;

namespace zDrive.Services
{
    /// <summary>
    /// Class for writing data for current application
    /// Software\\(Application.ProductName)
    /// Represents a key-level node in the Windows registry. This class is a registry encapsulation.
    /// </summary>
    internal sealed class RegistryService : IRegistryService
    {
        private readonly ILogger logger;
        private const string AutoRunPath = @"Software\Microsoft\Windows\CurrentVersion\Run\";
        private readonly string keyName;
        private readonly string programName;

        public RegistryService(ILogger logger)
        {
            this.logger = logger;
            this.programName = Application.ResourceAssembly.GetName().Name;
            this.keyName = "Software\\" + this.programName;
        }

        public RegistryService(string programName, ILogger logger)
        {
            this.logger = logger;
            this.programName = programName;
            this.keyName = "Software\\" + programName;
        }

        /// <inheritdoc />
        public bool Write<TValue>(string path, string name, TValue value)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key?.SetValue(name, value);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Error on write registry {Name} on path {Path}", name, path);
                return false;
            }
        }

        /// <inheritdoc />
        public bool Write<TValue>(string name, TValue value) => this.Write(this.keyName, name, value);

        /// <inheritdoc />
        public bool Remove(string path, string name)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key?.DeleteValue(name, false);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Error on remove registry {Name} on path {Path}", name, path);
                return false;
            }
        }

        /// <inheritdoc />
        public bool Remove(string name) => this.Remove(this.keyName, name);

        /// <inheritdoc />
        public bool Remove() => this.Remove(this.keyName, this.programName);

        /// <inheritdoc />
        public TValue Read<TValue>(string path, string name, TValue defaultValue)
        {
            using var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadSubTree);
            var value = key?.GetValue(name, defaultValue);
            var type = typeof(TValue);

            if (type.BaseType == typeof(Enum))
            {
                return (TValue)Convert.ChangeType(value, Enum.GetUnderlyingType(type), CultureInfo.CurrentCulture);
            }

            return (TValue)Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
        }

        /// <inheritdoc />
        public TValue Read<TValue>(string name, TValue defaultValue) => this.Read(this.keyName, name, defaultValue);

        /// <inheritdoc />
        public string ReadAutoRun() => this.Read(AutoRunPath, this.programName, (string)null);

        /// <inheritdoc />
        public bool WriteAutoRun(string value) => this.Write(AutoRunPath, this.programName, value);

        /// <inheritdoc />
        public bool RemoveAutoRun() => this.Remove(AutoRunPath, this.programName);
    }
}
