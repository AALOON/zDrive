using System;
using System.Linq;
using System.Windows;

namespace zDrive
{
    /// <summary>
    ///     cs for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WpfSingleInstance.Make();
            base.OnStartup(e);
        }

        /// <summary>
        /// Theme resource dictionary.
        /// </summary>
        private ResourceDictionary ThemeDictionary =>
            Resources.MergedDictionaries[1]; // TODO: by name

        /// <summary>
        /// Change skin by uri.
        /// </summary>
        public void ChangeSkin(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }
    }
}