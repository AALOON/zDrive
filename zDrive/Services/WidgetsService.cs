using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    /// <inheritdoc />
    internal sealed class WidgetsService : IWidgetsService
    {
        private static readonly Dictionary<InfoWidget, string> WidgetToStringMap =
            Enum.GetValues<InfoWidget>().ToDictionary(p => p, p => p.ToString());

        private readonly IInfoFormatService infoFormatService;
        private readonly IDictionary<string, IInfoViewModel> infos;
        private readonly ILoggerFactory loggerFactory;

        public WidgetsService(IDictionary<string, IInfoViewModel> infos, IInfoFormatService infoFormatService,
            ILoggerFactory loggerFactory)
        {
            this.infos = infos;
            this.infoFormatService = infoFormatService;
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public void Add(InfoWidget widget)
        {
            IInfoViewModel viewModel = widget switch
            {
                InfoWidget.RamDisk => new RamInfoViewModel(this.infoFormatService,
                    this.loggerFactory.CreateLogger<RamInfoViewModel>()),
                InfoWidget.Displays => new DisplaysViewModel(),
                InfoWidget.Cpu => new CpuInfoViewModel(this.loggerFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(widget), widget, @"Specified widget not supported.")
            };
            this.AddWidget(widget, viewModel).RaiseChanges();
        }

        /// <inheritdoc />
        public void Remove(InfoWidget widget)
        {
            this.infos.Remove(WidgetToStringMap[widget], out var removedWidget);
            if (removedWidget is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private IInfoViewModel AddWidget(InfoWidget widget, IInfoViewModel viewModel)
        {
            var key = WidgetToStringMap[widget];
            if (!this.infos.TryAdd(key, viewModel))
            {
                throw new ArgumentException(key + " already exists!");
            }

            return viewModel;
        }
    }
}
