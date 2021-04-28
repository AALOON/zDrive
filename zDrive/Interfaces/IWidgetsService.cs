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
        void Add(InfoWidget widget);

        /// <summary>
        /// Removes widget.
        /// </summary>
        void Remove(InfoWidget widget);
    }
}
