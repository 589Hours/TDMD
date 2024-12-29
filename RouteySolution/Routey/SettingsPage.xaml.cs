using System.Diagnostics;
using System.Globalization;
using Routey.ViewModels;

namespace Routey;

public partial class SettingsPage : ContentPage
{
    private SettingsPageViewModel settingsPageViewModel;
	public SettingsPage(SettingsPageViewModel settingsPageViewModel)
	{
		InitializeComponent();

        this.settingsPageViewModel = settingsPageViewModel;
        BindingContext = settingsPageViewModel;
	}

    private void LanguageChanged(object sender, EventArgs e)
    {
        string language = pLanguage.SelectedItem.ToString();
        if (this.settingsPageViewModel is SettingsPageViewModel)
            settingsPageViewModel.OnLanguageChanged(language);
    }

    //private void DarkModeChanged(object sender, EventArgs e)
    //{
    //    string darkMode = pDarkMode.SelectedItem.ToString();
    //    if (this.settingsPageViewModel is SettingsPageViewModel)
    //        settingsPageViewModel.OnDarkModeChanged(darkMode);
    //}
}