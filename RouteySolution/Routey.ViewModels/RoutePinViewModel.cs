using CommunityToolkit.Mvvm.ComponentModel;

namespace Routey.ViewModels
{
    public partial class RoutePinViewModel : ObservableObject
    {
        [ObservableProperty]
        private string labelText;

        [ObservableProperty]
        private Location location;
    
        public RoutePinViewModel(string routePoint, Location location)
        {
            this.labelText = routePoint;
            this.location = location;
        }
    }
}
