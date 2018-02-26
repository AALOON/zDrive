using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace zDrive.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        internal bool Set(ref sbyte field, sbyte value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref short field, short value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);

            return true;
        }

        internal bool Set(ref int field, int value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref long field, long value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref byte field, byte value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref ushort field, ushort value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref uint field, uint value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref ulong field, ulong value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref char field, char value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref float field, float value, [CallerMemberName]string propertyName = "")
        {
            if (Math.Abs(field - value) < 0.0)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref double field, double value, [CallerMemberName]string propertyName = "")
        {
            if (Math.Abs(field - value) < 0.0)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref decimal field, decimal value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set(ref bool field, bool value, [CallerMemberName]string propertyName = "")
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = "") where T : class
        {
            if (field == value)
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal bool SetValueType<T>(ref T field, T value, [CallerMemberName]string propertyName = "") where T : struct
        {
            if (field.Equals(value))
                return false;

            field = value;

            RaisePropertyChanged(propertyName);
            return true;
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
