using HueApp.ViewModels;

namespace HueApp;

public partial class LightPage : ContentPage
{
    private LightPageViewModel lightPageViewModel;
	public LightPage(LightPageViewModel lightPageViewModel)
	{
		InitializeComponent();
        this.lightPageViewModel = lightPageViewModel;
        BindingContext = lightPageViewModel;
    }

    private async void LightPageLoaded(object sender, EventArgs e)
    {
        // Get request for lights
        await lightPageViewModel.FetchLights();
        // For each light, create an element and add to ListViewLight
        BoxView boxToAdd = new();
        Label labelTest = new();
        labelTest.Text = "Hoi!";
        boxToAdd.AddLogicalChild(labelTest);
        ListViewLights.AddLogicalChild(boxToAdd);

    }
}