using System;
using System.Collections.Generic;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    /// <inheritdoc />
    internal sealed class WidgetsService : IWidgetsService
    {
        private readonly IInfoFormatService infoFormatService;
        private readonly IDictionary<string, IInfoViewModel> infos;

        public WidgetsService(IDictionary<string, IInfoViewModel> infos, IInfoFormatService infoFormatService)
        {
            this.infos = infos;
            this.infoFormatService = infoFormatService;
        }

        /// <inheritdoc />
        /// c
        public void Add(InfoWidget widget, params object[] param)
        {
            switch (widget)
            {
                case InfoWidget.RamDisk:
                {
                    var key = nameof(RamInfoViewModel);
                    var exists = this.infos.TryGetValue(key, out _);
                    if (exists)
                    {
                        throw new ArgumentException(nameof(InfoWidget.RamDisk) + " already exists!");
                    }

                    var viewModel = new RamInfoViewModel(this.infoFormatService);
                    this.infos.Add(key, viewModel);
                    viewModel.RaiseChanges();
                }
                    break;
                case InfoWidget.Displays:
                {
                    var key = nameof(DisplayViewModel);
                    var exists = this.infos.TryGetValue(key, out _);
                    if (exists)
                    {
                        throw new ArgumentException(nameof(InfoWidget.Displays) + " already exists!");
                    }

                    var viewModel = new DisplaysViewModel(this.infoFormatService);
                    this.infos.Add(key, viewModel);
                    viewModel.RaiseChanges();
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(widget), widget, null);
            }
        }
    }
}
