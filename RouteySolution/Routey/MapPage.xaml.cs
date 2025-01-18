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

    bool hasStarted = false;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!hasStarted) // Activate this one time once the app has started
            await viewModel.SetStartLocation();

        hasStarted = true;
    }
}