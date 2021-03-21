namespace zDrive.Interfaces
{
    /// <summary>
    /// Info widgets service manager.
    /// </summary>
    public interface IWidgetsService
    {
        /// <summary>
        /// Adds new widget.
        /// </summary>
        void Add(InfoWidget widget, params object[] param);
    }
}
