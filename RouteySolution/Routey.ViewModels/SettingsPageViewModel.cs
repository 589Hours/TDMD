using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalizationResourceManager.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace Routey.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private ILocalizationResourceManager localizationResourceManager; // To keep track of current language, and it's translations
        public SettingsPageViewModel(ILocalizationResourceManager manager) 
        { 
            localizationResourceManager = manager;
        }

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

                case "Dutch":
                    if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "nl") // Only change the language if it isn't already Dutch
                    {
                        localizationResourceManager.CurrentCulture = new CultureInfo("nl-NL");
                    }
                    break;

            }
        }
    }
}
