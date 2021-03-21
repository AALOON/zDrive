using System;
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
            WpfSingleInstance.Make();
            base.OnStartup(e);
        }
    }
}
