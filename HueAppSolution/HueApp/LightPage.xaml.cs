using HueApp.ViewModels;

namespace HueApp;

public partial class LightPage : ContentPage
{
	public LightPage(LightPageViewModel lightPageViewModel)
	{
		InitializeComponent();
        BindingContext = lightPageViewModel;
    }

    private void LightPageLoaded(object sender, EventArgs e)
    {
        // Get request for lights

        // For each light, create an element and add to ListViewLight
        BoxView boxToAdd = new();
        Label labelTest = new();
        labelTest.Text = "Hoi!";
        boxToAdd.AddLogicalChild(labelTest);
        ListViewLights.AddLogicalChild(boxToAdd);

    }
}