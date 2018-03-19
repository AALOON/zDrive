namespace zDrive.Interfaces
{
    public enum InfoWidget
    {
        RamDisk
    }
    public interface IInfosService
    {
        void Add(InfoWidget widget, params object[] param);
    }
}