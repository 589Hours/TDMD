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
    }

    private async void ListViewLights_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (BindingContext is LightPageViewModel viewModel)
        {
            string item = e.SelectedItem as string;
            await viewModel.IsItemSelected(item);
        }
    }
}