using Routey.ViewModels;

namespace Routey;

public partial class MapPage : ContentPage
{
	private MapPageViewModel viewModel;

    public MapPage(MapPageViewModel mapPageViewModel)
	{
		InitializeComponent();
		BindingContext = mapPageViewModel;
		this.viewModel = mapPageViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		await viewModel.SetStartLocation();
    }
}