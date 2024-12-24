using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Routey.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> colorModeOptions;

        [ObservableProperty]
        private List<string> darkModeOptions;

        public SettingsPageViewModel()
        {
            
        }

        public void OnLanguageChanged(string language)
        {
            CultureInfo lang;
            switch (language)
            {
                case "Dutch":
                    lang = CultureInfo.GetCultureInfo("nl");
                    break;

                case "English":
                    lang = CultureInfo.GetCultureInfo("en");
                    break;
            }
            // Set language (and make new AppShell for localization)

        }

        public void OnDarkModeChanged(string darkMode)
        {
            //CultureInfo i = CultureInfo.Dar
            throw new NotImplementedException();
        }

    }
}
