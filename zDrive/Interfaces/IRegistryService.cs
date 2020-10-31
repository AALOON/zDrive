﻿namespace zDrive.Interfaces
{
    /// <summary>
    ///     Windows Registry service.
    /// </summary>
    public interface IRegistryService
    {
        bool Write(string path, string name, object value);
        bool Write(string name, object value);
        bool Remove(string path, string name);
        bool Remove(string name);
        bool Remove();
        object Read(string path, string name, object defaultValue);
        object Read(string name, object defaultValue);
        object ReadAutoRun();
        object WriteAutoRun(object value);
        object RemoveAutoRun();
    }
}