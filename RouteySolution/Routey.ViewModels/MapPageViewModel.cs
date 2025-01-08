using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Routey.Domain.Models;

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
        private ObservableCollection<Pin> routePins;

        public bool CanStartListening() => !IsListening;

        public bool CanStopListening() => IsListening;

        public MapPageViewModel(IGeolocation geolocation)
        {
            this.geolocation = geolocation;

            this.geolocation.LocationChanged += LocationChanged;

            this.routePins = new ObservableCollection<Pin>();
        }

        private int checksTillMarker = 0;

        private async void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            var user_location = await this.geolocation.GetLastKnownLocationAsync();

            if (user_location != null)
            {
                checksTillMarker++;
                this.CurrentPosition = user_location.ToString();
                this.CurrentMapSpan = MapSpan.FromCenterAndRadius(user_location, Distance.FromMeters(500));

                //3 * 10 seconds = 30 seconds between each marker put on the map
                if (checksTillMarker < 3)
                    return;

                int routePointNumber = RoutePins.Count+1; //+1 because the new marker increments the list
                Pin pin = new Pin
                {
                    Type = PinType.Generic,
                    Label = $"Route Point {routePointNumber}",
                    Location = user_location
                };
                RoutePins.Add(pin);

                Pin oldPoint = null;
                if (RoutePins.Count > 1)
                    oldPoint = RoutePins.ElementAt(RoutePins.Count-1);

                if (oldPoint != null)
                {
                    double distance = Location.CalculateDistance(oldPoint.Location.Latitude, oldPoint.Location.Longitude, e.Location, DistanceUnits.Kilometers);
                    string distanceMessage = $"(You have walked {distance} from your previous Point!)";
                    Debug.WriteLine(distanceMessage);
                }
                checksTillMarker = 0;
            }
        }

        [RelayCommand(CanExecute = nameof(CanStartListening))]
        public async Task StartListening()
        {
            IsListening = true;
            await this.geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
            {
                MinimumTime = TimeSpan.FromSeconds(10),
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
