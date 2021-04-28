using System;

namespace zDrive.Native
{
    /// <summary>
    /// Pdh counter.
    /// </summary>
    public interface IPdhCounter : IDisposable
    {
        /// <summary>
        /// Collect data.
        /// </summary>
        double? Collect();

        /// <summary>
        /// Initialize counter.
        /// </summary>
        void InitializeCounters();
    }
}
