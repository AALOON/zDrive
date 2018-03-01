using System.Globalization;
using zDrive.Converters;
using zDrive.Helpers;

namespace zDrive.ViewModels
{
    internal class DriveViewModel : ViewModelBase
    {
        #region Fields
        private string _key;
        private string _label;
        private string _name;
        private string _format;
        private System.IO.DriveType _type;
        private long _totalsize;
        private long _totalfreespace;
        private InfoFormat _formatOfInfo;

        #endregion Fields

        #region Properties

        public string Key
        {
            get => _key;
            set => Set(ref _key, value);
        }

        public string Label
        {
            get => _label;
            set => Set(ref _label, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string Format
        {
            get => _format;
            set => Set(ref _format, value);
        }

        public System.IO.DriveType Type
        {
            get => _type;
            set => SetValueType(ref _type, value);
        }

        public long TotalSize
        {
            get => _totalsize;
            set
            {
                if (_totalsize != value)
                {
                    _totalsize = value;
                    RaisePropertyChanged("TotalSize");
                    RaisePropertyChanged("Value");
                    RaisePropertyChanged("FreeText");
                }
            }
        }

        public long TotalFreeSpace
        {
            get => _totalfreespace;
            set
            {
                if (_totalfreespace != value)
                {
                    _totalfreespace = value;
                    RaisePropertyChanged("TotalFreeSpace");
                    RaisePropertyChanged("Value");
                }
            }
        }

        public double Value
        {
            get
            {
                if (_totalfreespace == 0)
                    return 0;

                return (_totalsize - _totalfreespace) / (_totalsize / 100d);
            }
        }

        public InfoFormat FormatOfInfo
        {
            get => _formatOfInfo;
            set
            {
                _formatOfInfo = value;
                RaisePropertyChanged(nameof(FormatOfInfo));
            }
        }

        public string Info => FormatOfInfo.ToFormatedString(TotalFreeSpace, TotalSize);

        #endregion Properties

        public DriveViewModel()
        {
            OpenCommand = new RelayCommand(Open);
            _formatOfInfo = InfoFormat.Free;
        }

        public RelayCommand OpenCommand { get; set; }

        void Open(object param)
        {
            System.Diagnostics.Process.Start(Name);
        }
    }
}
