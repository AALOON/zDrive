using System;
using System.Collections.Generic;
using System.Linq;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    internal class InfosService : IInfosService
    {
        private readonly IInfoFormatService _infoFormatService;
        private readonly ICollection<IInfoViewModel> _infos;

        public InfosService(ICollection<IInfoViewModel> infos, IInfoFormatService infoFormatService)
        {
            _infos = infos;
            _infoFormatService = infoFormatService;
        }

        public void Add(InfoWidget widget, params object[] param)
        {
            switch (widget)
            {
                case InfoWidget.RamDisk:
                {
                    var exists = _infos.FirstOrDefault(info => info is RamInfoViewModel);
                    if (exists != null)
                        throw new ArgumentException(nameof(InfoWidget.RamDisk) + " already exists!");
                    var t = new RamInfoViewModel(_infoFormatService);
                    _infos.Add(t);
                    t.RaiseChanges();
                }
                    break;
                case InfoWidget.Displays:
                {
                    var exists = _infos.FirstOrDefault(info => info is DisplaysViewModel);
                    if (exists != null)
                        throw new ArgumentException(nameof(InfoWidget.Displays) + " already exists!");
                    var t = new DisplaysViewModel(_infoFormatService);
                    _infos.Add(t);
                    t.RaiseChanges();
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(widget), widget, null);
            }
        }
    }
}