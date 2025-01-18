using Routey.ViewModels;

namespace Routey;

public partial class RoutesPage : ContentPage
{
    private RoutesPageViewModel routesPageViewModel;
	public RoutesPage(RoutesPageViewModel routesPageViewModel)
	{
		InitializeComponent();

		this.routesPageViewModel = routesPageViewModel;
		BindingContext = routesPageViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await routesPageViewModel.GetRoutesAsync();
    }

    private void RouteEntityTapped(object sender, TappedEventArgs e)
    {
        routesPageViewModel.OnRouteSelected(sender);
    }

    private async void DeleteButtonClicked(object sender, EventArgs e)
    {
        await routesPageViewModel.OnDeleteButtonPressed();
    }
}