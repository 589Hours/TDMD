using Routey.ViewModels;

namespace Routey;

public partial class MapPage : ContentPage
{
    public MapPage(MapPageViewModel mapPageViewModel)
	{
		InitializeComponent();
		BindingContext = mapPageViewModel;
	}
}