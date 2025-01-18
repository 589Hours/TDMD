using CommunityToolkit.Mvvm.ComponentModel;

namespace Routey.ViewModels
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// This class is used to represent a route pin on the map.
    /// </summary>
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