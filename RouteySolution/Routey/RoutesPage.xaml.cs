using Routey.Domain.SQLiteDatabases.Entities;
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
}