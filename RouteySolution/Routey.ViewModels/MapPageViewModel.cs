using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Maps;

namespace Routey.ViewModels
{
    public partial class MapPageViewModel : ObservableObject
    {
        private readonly IGeolocation geolocation;

        [ObservableProperty]
        public string currentPosition;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartListeningCommand))]
        [NotifyCanExecuteChangedFor(nameof(StopListeningCommand))]
        public bool isListening = false;

        [ObservableProperty]
        public MapSpan currentMapSpan;

        [ObservableProperty]
        private ObservableCollection<RoutePinViewModel> routePins;

        [ObservableProperty]
        private RoutePinViewModel currentRoutePin;

        public bool CanStartListening() => !IsListening;

        public bool CanStopListening() => IsListening;

        public MapPageViewModel(IGeolocation geolocation)
        {
            this.geolocation = geolocation;

            this.geolocation.LocationChanged += LocationChanged;

            this.routePins = new ObservableCollection<RoutePinViewModel>();
        }

        private int checksTillMarker = 0;

        private void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            if (e.Location != null)
            {
                checksTillMarker++;
                this.CurrentPosition = e.Location.ToString();
                this.CurrentMapSpan = MapSpan.FromCenterAndRadius(e.Location, Distance.FromMeters(500));

                //60 * 500 = 30 seconds between each marker put on the map
                if (checksTillMarker < 60)
                    return;

                int routePointNumber = RoutePins.Count+1; //+1 because the new marker increments the list
                RoutePins.Add(new RoutePinViewModel($"Route Point {routePointNumber}", e.Location));

                //todo make routepoint
            }
        }

        [RelayCommand(CanExecute = nameof(CanStartListening))]
        public async Task StartListening()
        {
            IsListening = true;
            await this.geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
            {
                MinimumTime = TimeSpan.FromSeconds(100),
                DesiredAccuracy = GeolocationAccuracy.Default
            });
        }

        [RelayCommand(CanExecute = nameof(CanStopListening))]
        public void StopListening()
        {
            IsListening = false;
            this.geolocation.StopListeningForeground();
        }

    }
}
