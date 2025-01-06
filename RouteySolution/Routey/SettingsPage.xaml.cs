using System.Globalization;
using LocalizationResourceManager.Maui;
using Routey.ViewModels;

namespace Routey;

public partial class SettingsPage : ContentPage
{
    private SettingsPageViewModel viewModel;
	public SettingsPage(SettingsPageViewModel settingsPageViewModel)
    {
		InitializeComponent();
        
        viewModel = settingsPageViewModel;
        BindingContext = settingsPageViewModel;
	}

    private void LanguageChanged(object sender, EventArgs e)
    {
        string language = pLanguage.SelectedItem.ToString(); // Get the current language
        if (viewModel is SettingsPageViewModel)
            viewModel.LanguageChanged(language); // Trigger Event to change language
    }
}