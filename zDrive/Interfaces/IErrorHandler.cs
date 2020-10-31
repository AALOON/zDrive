using System;

namespace zDrive.Interfaces
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}