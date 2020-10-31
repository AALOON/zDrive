namespace zDrive.Native
{
    public class MonitorInfo
    {
        public MonitorInfo(uint index, bool isPrimary, string deviceName, string friendlyName)
        {
            Index = index;
            IsPrimary = isPrimary;
            DeviceName = deviceName;
            FriendlyName = friendlyName;
        }

        public uint Index { get; }
        public bool IsPrimary { get; }
        public string DeviceName { get; }
        public string FriendlyName { get; }
    }
}