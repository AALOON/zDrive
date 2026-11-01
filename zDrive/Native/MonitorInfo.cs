namespace zDrive.Native
{
    public sealed class MonitorInfo
    {
        public MonitorInfo(uint index, bool isPrimary, string deviceName, string friendlyName,
            bool isHdrSupported = false, bool isHdrEnabled = false)
        {
            this.Index = index;
            this.IsPrimary = isPrimary;
            this.DeviceName = deviceName;
            this.FriendlyName = friendlyName;
            this.IsHdrSupported = isHdrSupported;
            this.IsHdrEnabled = isHdrEnabled;
        }

        public uint Index { get; }

        public bool IsPrimary { get; }

        public string DeviceName { get; }

        public string FriendlyName { get; }

        public bool IsHdrSupported { get; }

        public bool IsHdrEnabled { get; }
    }
}
