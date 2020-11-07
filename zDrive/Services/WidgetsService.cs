using System;
using System.Collections.Generic;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    /// <inheritdoc />
    internal sealed class WidgetsService : IWidgetsService
    {
        private readonly IInfoFormatService _infoFormatService;
        private readonly IDictionary<string, IInfoViewModel> _infos;

        public WidgetsService(IDictionary<string, IInfoViewModel> infos, IInfoFormatService infoFormatService)
        {
            _infos = infos;
            _infoFormatService = infoFormatService;
        }

        /// <inheritdoc />
        public void Add(InfoWidget widget, params object[] param)
        {
            switch (widget)
            {
                case InfoWidget.RamDisk:
                {
                    var key = nameof(RamInfoViewModel);
                    var exists = _infos.TryGetValue(key, out _);
                    if (exists)
                        throw new ArgumentException(nameof(InfoWidget.RamDisk) + " already exists!");
                    var viewModel = new RamInfoViewModel(_infoFormatService);
                    _infos.Add(key, viewModel);
                    viewModel.RaiseChanges();
                }
                    break;
                case InfoWidget.Displays:
                {
                    var key = nameof(DisplayViewModel);
                    var exists = _infos.TryGetValue(key, out _);
                    if (exists)
                        throw new ArgumentException(nameof(InfoWidget.Displays) + " already exists!");
                    var viewModel = new DisplaysViewModel(_infoFormatService);
                    _infos.Add(key, viewModel);
                    viewModel.RaiseChanges();
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(widget), widget, null);
            }
        }
    }
}