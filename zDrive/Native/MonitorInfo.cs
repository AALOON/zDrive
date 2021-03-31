namespace zDrive.Native
{
    public sealed class MonitorInfo
    {
        public MonitorInfo(uint index, bool isPrimary, string deviceName, string friendlyName)
        {
            this.Index = index;
            this.IsPrimary = isPrimary;
            this.DeviceName = deviceName;
            this.FriendlyName = friendlyName;
        }

        public uint Index { get; }

        public bool IsPrimary { get; }

        public string DeviceName { get; }

        public string FriendlyName { get; }
    }
}
