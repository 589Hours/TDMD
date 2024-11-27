namespace HueApp.ViewModels
{
    // All the code in this file is included in all platforms.
    public class MainPageViewModel
    {
        IPreferences preferences;
        public MainPageViewModel(IPreferences preferences) 
        {
            this.preferences = preferences;
        }

    }
}
