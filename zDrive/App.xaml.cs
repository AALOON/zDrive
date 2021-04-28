using System;
using System.Diagnostics;
using System.Windows;

namespace zDrive
{
    /// <summary>
    /// cs for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Theme resource dictionary.
        /// </summary>
        private ResourceDictionary ThemeDictionary => this.Resources.MergedDictionaries[1]; // TODO: by name

        /// <summary>
        /// Change skin by uri.
        /// </summary>
        public void ChangeSkin(Uri uri)
        {
            this.ThemeDictionary.MergedDictionaries.Clear();
            this.ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                WpfSingleInstance.Make();
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                LogStartErrorSafe(ex);

                throw;
            }
        }

        private static void LogStartErrorSafe(Exception ex)
        {
            const string application = "Application";

            try
            {
                using var eventLog = new EventLog(application) { Source = ".NET Runtime" };
                eventLog.WriteEntry($"Error on start application. Error: [{ex.Message}] Stack: [{ex.StackTrace}]",
                    EventLogEntryType.Error, EventIds.ErrorOnStarting);
            }
            catch
            {
                // do not change exception on event log exceptions.
            }
        }
    }
}
