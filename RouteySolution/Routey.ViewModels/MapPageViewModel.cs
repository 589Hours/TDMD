using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Platform;
using Routey.Domain.Models;

namespace Routey.ViewModels
{
    public partial class MapPageViewModel : ObservableObject
    {
        private readonly IGeolocation geolocation;

        [ObservableProperty]
        public string routeName;

        [ObservableProperty]
        public string currentPosition;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartListeningCommand))]
        [NotifyCanExecuteChangedFor(nameof(StopListeningCommand))]
        public bool isListening = false;

        [ObservableProperty]
        public bool isNotListening = true;

        [ObservableProperty]
        public MapSpan currentMapSpan;

        [ObservableProperty]
        private ObservableCollection<Pin> routePins;

        [ObservableProperty]
        private ObservableCollection<Pin> routePoints;

        [ObservableProperty]
        private string totalDistance; // To show distance of the Route

        [ObservableProperty]
        private double distanceTracker; // To show distance of the Route

        [ObservableProperty]
        private string distancePrev; // To show distance to previous Pin

        [ObservableProperty]
        private string totalDuration; // To show passed time

        [ObservableProperty]
        private TimeSpan timePassed; // To track passed time

        public bool CanStartListening() => !IsListening;

        public bool CanStopListening() => IsListening;

        private System.Timers.Timer timer;

        public MapPageViewModel(IGeolocation geolocation)
        {
            this.geolocation = geolocation;
            this.geolocation.LocationChanged += LocationChanged;

            this.routePins = new ObservableCollection<Pin>();
            this.routePoints = new ObservableCollection<Pin>();

            timer = new System.Timers.Timer();
            timer.Interval = 1000; // Interval each second
            timer.Elapsed += OnTimerInterval;

            // Reset UI elements
            TotalDuration = "No Route Started!";
            TotalDistance = "Total Distance Covered: 0 Kilometers";
            DistancePrev = "No Previous Pin Availible!";
        }

        private void OnTimerInterval(object? sender, ElapsedEventArgs e)
        {
            // Add a second every Timer interval (also every second)
            TimeSpan add = new TimeSpan(0, 0, 1);
            TimePassed = TimePassed.Duration() + add;
            string time = TimePassed.ToFormattedString("HH:mm:ss");
            TotalDuration = $"Route Duration: {time}";
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

                // Create a new Point to track distance
                Pin pinPoint = MapPageViewModel.CreatePin(user_location, "Walk Point");
                RoutePoints.Add(pinPoint);

                if (RoutePoints.Count <= 1)
                    return;

                //UpdateTotalDistance(pinPoint, user_location);

                //3 * 10 seconds = 30 seconds between each marker put on the map
                if (checksTillMarker < 3)
                    return;

                // Create a new Pin to show on the map
                int routePointNumber = RoutePins.Count + 1; //+1 because the new marker increments the list
                Pin pinPin = MapPageViewModel.CreatePin(user_location, $"Route Point {routePointNumber}");
                RoutePins.Add(pinPin);

                // If there is an old pin availible, calculate the distance between that and the new pin
                Pin oldPin = null;
                if (RoutePins.Count > 1)
                    oldPin = RoutePins.ElementAt(RoutePins.Count-2); // Previous pin = -1, but the list starts at 0

                UpdateDistanceBetweenPoints(oldPin, user_location);

                checksTillMarker = 0;
            }
        }

        private static Pin CreatePin(Location user_location, string labelText)
        {
            Pin newPin = new Pin
            {
                Type = PinType.Generic,
                Label = labelText,
                Location = user_location
            };
            return newPin;
        }

        private void UpdateTotalDistance(Pin pinPoint, Location user_location)
        {
            // If there is a previous user location availible, calculate the distance between these points
            double distance = CalculateDistanceBetweenLocations(RoutePoints.ElementAt(RoutePoints.Count - 1), user_location);
            if (distance == 0)
                return;

            int comma = distance.ToString().IndexOf('.') + 1; // Get the index of the dot (including the dot itself)
            if (comma < 1)
            {
                DistanceTracker += Double.Parse(distance.ToString()); // Show 2 digits after comma
                TotalDistance = $"Total Distance Covered: {DistanceTracker.ToString()} Kilometers!";
            }
            DistanceTracker += Double.Parse(distance.ToString().Substring(0, comma + 2)); // Show 2 digits after comma
            TotalDistance = $"Total Distance Covered: {DistanceTracker.ToString().Substring(0, comma + 2)} Kilometers!";
        }

        private void UpdateDistanceBetweenPoints(Pin? oldPin, Location user_location)
        {
            // If there is a previous pin availible, calculate the distance between these pins
            if (oldPin != null)
            {
                double distance = CalculateDistanceBetweenLocations(oldPin, user_location);
                int comma = distance.ToString().IndexOf('.') + 1; // Get the index of the dot (including the dot itself)
                
                string distanceMessage;
                if (distance < 1)
                {
                    distance = distance * 1000; // If the distance is less than 1 Kilometer(s), convert to Meter(s)
                    comma = distance.ToString().IndexOf('.') + 1;
                    distanceMessage = $"You have walked {distance.ToString().Substring(0, comma + 2)} Meter(s) from your previous Point!"; // Show 2 digits after comma
                }
                else
                    distanceMessage = $"You have walked {distance.ToString().Substring(0, comma + 2)} Kilometer(s) from your previous Point!"; // Show 2 digits after comma

                DistancePrev = distanceMessage;
                //TODO: Update total distance

            }
        }

        private double CalculateDistanceBetweenLocations(Pin oldLocation, Location newLocation)
        {
            return Location.CalculateDistance(oldLocation.Location.Latitude,
                    oldLocation.Location.Longitude,
                    newLocation, DistanceUnits.Kilometers);
            
        }

        [RelayCommand(CanExecute = nameof(CanStartListening))]
        public async Task StartListening()
        {
            if (RouteName == null || RouteName == "Enter route name...")
                return;

            IsListening = true;
            IsNotListening = !IsListening;
            TimePassed = TimeSpan.Zero;
            timer.Start();
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
            IsNotListening = !IsListening;
            timer.Stop();
            this.geolocation.StopListeningForeground();

            // Reset UI elements
            TotalDuration = "No Route Started!";
            TotalDistance = "Total Distance Covered: 0 Kilometers";
            DistancePrev = "No Previous Pin Availible!";
            //TODO: Save Route in database

        }

    }
}
