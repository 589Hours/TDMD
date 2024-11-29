using HueApp.ViewModels;

namespace HueApp;

public partial class LightPage : ContentPage
{
	public LightPage(LightPageViewModel lightPageViewModel)
	{
		InitializeComponent();
        BindingContext = lightPageViewModel;
    }

}