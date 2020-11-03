using System;
using System.Windows;
using Microsoft.Win32;
using zDrive.Interfaces;

namespace zDrive.Services
{
    /// <summary>
    ///     Class for writing data for current application
    ///     Software\\(Application.ProductName)
    ///     Represents a key-level node in the Windows registry. This class is a registry encapsulation.
    /// </summary>
    internal sealed class RegistryService : IRegistryService
    {
        private const string AutoRunPath = @"Software\Microsoft\Windows\CurrentVersion\Run\";
        private readonly string _keyName;
        private readonly string _programName;

        public RegistryService()
        {
            _programName = Application.ResourceAssembly.GetName().Name;
            _keyName = "Software\\" + _programName;
        }

        public RegistryService(string programName)
        {
            _programName = programName;
            _keyName = "Software\\" + programName;
        }

        /// <inheritdoc />
        public bool Write<TValue>(string path, string name, TValue value)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key?.SetValue(name, value);
                    return true;
                }
            }
            catch (Exception)
            {
                //TODO: check particular exceptions
                return false;
            }
        }

        /// <inheritdoc />
        public bool Write<TValue>(string name, TValue value)
        {
            return Write(_keyName, name, value);
        }

        /// <inheritdoc />
        public bool Remove(string path, string name)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key?.DeleteValue(name, false);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool Remove(string name)
        {
            return Remove(_keyName, name);
        }

        /// <inheritdoc />
        public bool Remove()
        {
            return Remove(_keyName, _programName);
        }

        /// <inheritdoc />
        public TValue Read<TValue>(string path, string name, TValue defaultValue)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadSubTree))
            {
                var value = key?.GetValue(name, defaultValue);
                var type = typeof(TValue);
                switch (true)
                {
                    case true when type.BaseType == typeof(Enum):
                        return (TValue)Convert.ChangeType(value, Enum.GetUnderlyingType(type));
                    default:
                        return (TValue)Convert.ChangeType(value, type);
                }
            }
        }

        /// <inheritdoc />
        public TValue Read<TValue>(string name, TValue defaultValue)
        {
            return Read(_keyName, name, defaultValue);
        }

        /// <inheritdoc />
        public string ReadAutoRun()
        {
            return Read(AutoRunPath, _programName, (string)null);
        }

        /// <inheritdoc />
        public bool WriteAutoRun(string value)
        {
            return Write(AutoRunPath, _programName, value);
        }

        /// <inheritdoc />
        public bool RemoveAutoRun()
        {
            return Remove(AutoRunPath, _programName);
        }
    }
}