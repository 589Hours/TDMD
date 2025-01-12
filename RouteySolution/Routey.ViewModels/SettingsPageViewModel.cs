using LocalizationResourceManager.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace Routey.ViewModels
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// ViewModel for the SettingsPage. This class is responsible for handling the logic of the SettingsPage.
    /// </summary>
    public partial class SettingsPageViewModel : ObservableObject
    {
        private ILocalizationResourceManager localizationResourceManager; // To keep track of current language, and it's translations
        public SettingsPageViewModel(ILocalizationResourceManager manager) 
        { 
            localizationResourceManager = manager;
        }

        /// <summary>
        /// Handle the changing of the language. This can be either English or Dutch.
        /// </summary>
        /// <param name="language"></param>
        public void LanguageChanged(string language)
        {
            switch (language) // All languages are listed here
            {
                case "English":
                    if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "en") // Only change the language if it isn't already English 
                    {
                        localizationResourceManager.CurrentCulture = new CultureInfo("en-US");
                    }
                    break;

                case "Nederlands":
                    if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "nl") // Only change the language if it isn't already Dutch
                    {
                        localizationResourceManager.CurrentCulture = new CultureInfo("nl-NL");
                    }
                    break;

            }
        }
    }
}