using zDrive.Converters;

namespace zDrive.Interfaces
{
    public interface IDriveInfoService
    {
        bool ShowUnavailable { get; set; }
        InfoFormat InfoFormat { get; set; }
        void Update();
        void UpdateAddition(string label);
        void UpdateRemoval(string label);
    }
}