namespace zDrive.Interfaces
{
    /// <summary>
    ///     Windows Registry service.
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        ///     Write registry key/value pair to the path.
        /// </summary>
        bool Write<TValue>(string path, string name, TValue value);

        /// <summary>
        ///     Write key/value pair to app registry path.
        /// </summary>
        bool Write<TValue>(string name, TValue value);

        /// <summary>
        ///     Remove key from path.
        /// </summary>
        bool Remove(string path, string name);

        /// <summary>
        ///     Remove key from app registry path.
        /// </summary>
        bool Remove(string name);

        /// <summary>
        ///     Remove app registry path.
        /// </summary>
        bool Remove();

        /// <summary>
        ///     Read registry value from the path with specified default.
        /// </summary>
        TValue Read<TValue>(string path, string name, TValue defaultValue);

        /// <summary>
        ///     Read value from app registry path.
        /// </summary>
        TValue Read<TValue>(string name, TValue defaultValue);

        /// <summary>
        ///     Reads auto run path for app.
        /// </summary>
        string ReadAutoRun();

        /// <summary>
        ///     Writes auto run path for app.
        /// </summary>
        bool WriteAutoRun(string value);

        /// <summary>
        ///     Removes auto run path for app.
        /// </summary>
        bool RemoveAutoRun();
    }
}