using System;
using System.Windows;
using Microsoft.Win32;
using zDrive.Interfaces;

namespace zDrive.Services
{
    /// <summary>
    ///     Class for writing data for current application
    ///     Software\\(Application.ProductName)
    /// </summary>
    internal class RegistryService : IRegistryService
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

        public bool Write(string path, string name, object value)
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

        public bool Write(string name, object value)
        {
            return Write(_keyName, name, value);
        }

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

        public bool Remove(string name)
        {
            return Remove(_keyName, name);
        }

        public bool Remove()
        {
            return Remove(_keyName, _programName);
        }

        public object Read(string path, string name, object defaultValue)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadSubTree))
            {
                return key?.GetValue(name, defaultValue);
            }
        }

        public object Read(string name, object defaultValue)
        {
            return Read(_keyName, name, defaultValue);
        }

        public object ReadAutoRun()
        {
            return Read(AutoRunPath, null);
        }

        public object WriteAutoRun(object value)
        {
            return Write(AutoRunPath, value);
        }

        public object RemoveAutoRun()
        {
            return Remove(AutoRunPath);
        }

        public bool Write(object value)
        {
            return Write(_keyName, _programName, value);
        }

        public object Read(object defaultValue)
        {
            return Read(_keyName, _programName, defaultValue);
        }
    }
}