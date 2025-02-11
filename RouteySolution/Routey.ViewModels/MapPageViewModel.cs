﻿using System.Collections.ObjectModel;
using System.Timers;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Platform;
using Plugin.LocalNotification;
using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases;

namespace Routey.ViewModels
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// ViewModel for the MapPage. This ViewModel is responsible for the logic of the MapPage.
    /// </summary>
    public partial class MapPageViewModel : ObservableObject
    {
        private readonly IGeolocation geolocation;
        private readonly ILocalizationResourceManager localizationResourceManager;
        private readonly IRouteDatabase routeDatabase;

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
        private ObservableCollection<Pin> routePins; // Pins on the map

        [ObservableProperty]
        private ObservableCollection<Pin> routePoints; // Points while walking

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
        private Route activeRoute;

        public MapPageViewModel(IGeolocation geolocation, ILocalizationResourceManager manager, IRouteDatabase routeDatabase)
        {
            this.geolocation = geolocation;
            this.geolocation.LocationChanged += LocationChanged;

            this.routeDatabase = routeDatabase;

            this.localizationResourceManager = manager;

            RoutePins = new ObservableCollection<Pin>();
            RoutePoints = new ObservableCollection<Pin>();

            timer = new System.Timers.Timer();
            timer.Interval = 1000; // Interval each second
            timer.Elapsed += OnTimerInterval;

            // Reset UI elements
            DistanceTracker = 0;
            TotalDuration = string.Format(localizationResourceManager["Duration"], "00:00:00");
            TotalDistance = string.Format(localizationResourceManager["Distance"], "0 Kilometer(s)");
            DistancePrev = string.Format(localizationResourceManager["Previous"], "0 Meter(s)");
        }

        /// <summary>
        /// Method to update the time every second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerInterval(object? sender, ElapsedEventArgs e)
        {
            // Add a second every Timer interval (also every second)
            TimeSpan add = new TimeSpan(0, 0, 1);
            TimePassed = TimePassed.Duration() + add;

            // Format the TimeSpan to a string
            string time = TimePassed.ToFormattedString("HH:mm:ss");
            TotalDuration = string.Format(localizationResourceManager["Duration"], time);
        }

        /// <summary>
        /// Helper method to create a new Pin.
        /// </summary>
        /// <param name="user_location"></param>
        /// <param name="labelText"></param>
        /// <returns></returns>
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

        #region Location Events
        /// <summary>
        /// When starting the app, set the start location of the user.
        /// </summary>
        /// <returns></returns>
        public async Task SetStartLocation()
        {
            try
            {
                var userLocation = await this.geolocation.GetLocationAsync();
                if (userLocation != null)
                {
                    this.CurrentPosition = userLocation.ToString();
                    this.CurrentMapSpan = MapSpan.FromCenterAndRadius(userLocation, Distance.FromMeters(250));
                }
            } catch (UnauthorizedAccessException ex)
            {
                await NoPermissionNotification();
            } catch (Exception)
            {
                await GeneralErrorNotification();
            }
        }

        /// <summary>
        /// Method to update the location of the user and change UI elements accordingly. Also updates the Route.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            try
            {
                var user_location = await this.geolocation.GetLastKnownLocationAsync();

                if (user_location != null)
                {
                    this.CurrentPosition = user_location.ToString();
                    this.CurrentMapSpan = MapSpan.FromCenterAndRadius(user_location, Distance.FromMeters(250));

                    // Create a new Point to track distance
                    Pin pinPoint = MapPageViewModel.CreatePin(user_location, "Walk Point");
                    RoutePoints.Add(pinPoint);

                    // Add +1 to counter and add the speed
                    activeRoute.AmountOfRoutePoints++;
                    activeRoute.SumOfSpeeds += user_location.Speed;

                    if (RoutePoints.Count <= 1)
                        return;

                    UpdateTotalDistance(pinPoint, user_location);

                    //3 * 10 seconds = 30 seconds between each marker put on the map
                    if (activeRoute.AmountOfRoutePoints % 3 == 0 || activeRoute.AmountOfRoutePoints % 3 == 2)
                        return;

                    // Create a new Pin to show on the map
                    int routePointNumber = RoutePins.Count + 1; //+1 because the new marker increments the list
                    Pin pinPin = MapPageViewModel.CreatePin(user_location, $"Route Point {routePointNumber}");
                    RoutePins.Add(pinPin);

                    // If there is an old pin availible, calculate the distance between that and the new pin
                    Pin oldPin = null;
                    if (RoutePins.Count > 1)
                        oldPin = RoutePins.ElementAt(RoutePins.Count - 2); // Previous pin = -1, but the list starts at 0

                    UpdateDistanceBetweenPoints(oldPin, user_location);
                }
            } catch (UnauthorizedAccessException)
            {
                await NoPermissionNotification();
                StopListeningCrashed();
            } catch (Exception)
            {
                await GeneralErrorNotification();
                StopListeningCrashed();
            } 
        }
        #endregion

        #region Methods for updating distance
        /// <summary>
        /// Updates the total distance of the Route and changes the UI element accordingly.
        /// </summary>
        /// <param name="pinPoint"></param>
        /// <param name="user_location"></param>
        private void UpdateTotalDistance(Pin pinPoint, Location user_location)
        {
            // If there is a previous user location availible, calculate the distance between these points
            double distance = CalculateDistanceBetweenLocations(RoutePoints.ElementAt(RoutePoints.Count - 2), user_location); // Previous pin = -1, but the list starts at 0
            if (distance == 0)
                return;

            // Add to total distance of the route and to property
            DistanceTracker += distance;

            if (DistanceTracker < 1)
                TotalDistance = string.Format(localizationResourceManager["Distance"], $"{Double.Round(DistanceTracker * 1000, 2)} Meter(s)");
            else
                TotalDistance = string.Format(localizationResourceManager["Distance"], $"{Double.Round(DistanceTracker, 2)} Kilometer(s)");
        }

        /// <summary>
        /// Updates the distance between the previous pin and the current user location. Also changes the UI element accordingly.
        /// </summary>
        /// <param name="oldPin"></param>
        /// <param name="user_location"></param>
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
                    DistancePrev = string.Format(localizationResourceManager["Previous"], $"{distance.ToString().Substring(0, comma + 2)} Meter(s)"); // Show 2 digits after comma
                }
                else
                    DistancePrev = string.Format(localizationResourceManager["Previous"], $"{distance.ToString().Substring(0, comma + 2)} Kilometer(s)"); // Show 2 digits after comma
            }
        }

        /// <summary>
        /// Helper method to calculate the distance between two locations.
        /// </summary>
        /// <param name="oldLocation"></param>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        private double CalculateDistanceBetweenLocations(Pin oldLocation, Location newLocation)
        {
            return Location.CalculateDistance(oldLocation.Location.Latitude,
                    oldLocation.Location.Longitude,
                    newLocation, DistanceUnits.Kilometers);
            
        }
        #endregion

        #region Listening Methods

        /// <summary>
        /// When we start the route, start listening to the user's location and set the active Route.
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanStartListening))]
        public async Task StartListening()
        {
            if (RouteName == null || RouteName == "Enter route name..." || RouteName == "Voer route naam in...")
            {
                Toast message = (Toast)Toast.Make("Fill in a Route name before starting!", ToastDuration.Short, 14);
                await message.Show();
                return;
            }

            activeRoute = new Route(RouteName, DateTime.UtcNow);

            IsListening = true;
            IsNotListening = !IsListening;
            TimePassed = TimeSpan.Zero;
            timer.Start();
            try
            {
                await this.geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
                {
                    MinimumTime = TimeSpan.FromSeconds(10),
                    DesiredAccuracy = GeolocationAccuracy.Default
                });
            } catch (UnauthorizedAccessException)
            {
                await NoPermissionNotification();
                StopListeningCrashed();
            } catch (Exception)
            {
                await GeneralErrorNotification();
                StopListeningCrashed();
            }
        }

        /// <summary>
        /// When we stop the route, stop listening to the user's location and save the Route in the database.
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanStopListening))]
        public async Task StopListening()
        {
            // Standard items
            IsListening = false;
            IsNotListening = !IsListening;
            timer.Stop();
            this.geolocation.StopListeningForeground();

            // Ready Route for saving
            activeRoute.TotalDuration = TimePassed.ToFormattedString("HH:mm:ss");
            activeRoute.TotalDistance = DistanceTracker;

            //Save Route in database
            await routeDatabase.AddRouteAsync(activeRoute);

            // Reset (UI) elements
            DistanceTracker = 0;
            activeRoute = null;
            TotalDuration = string.Format(localizationResourceManager["Duration"], "00:00:00");
            TotalDistance = string.Format(localizationResourceManager["Distance"], "0 Kilometer(s)");
            DistancePrev = string.Format(localizationResourceManager["Previous"], "0 Meter(s)");
            RouteName = "";

            await RouteStoppedNotificationLater();
        }

        /// <summary>
        /// When the app crashes while listening, stop listening and reset UI elements.
        /// </summary>
        private void StopListeningCrashed()
        {
            // Standard items
            IsListening = false;
            IsNotListening = !IsListening;
            timer.Stop();
            this.geolocation.StopListeningForeground();

            // Reset (UI) elements
            DistanceTracker = 0;
            activeRoute = null;
            TotalDuration = string.Format(localizationResourceManager["Duration"], "00:00:00");
            TotalDistance = string.Format(localizationResourceManager["Distance"], "0 Kilometer(s)");
            DistancePrev = string.Format(localizationResourceManager["Previous"], "0 Meter(s)");
        }
        #endregion

        #region Notifications
        /// <summary>
        /// General method to show a notification.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ShowNotification(string title, string subTitle, string message, NotificationCategoryType type, DateTime timeToNotify)
        {
            var request = new NotificationRequest
            {
                NotificationId = 1337,
                Title = title,
                Subtitle = subTitle,
                Description = message,
                CategoryType = type,
                BadgeNumber = 42,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = timeToNotify
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }

        /// <summary>
        /// Specific pre-made notifications.
        /// </summary>
        /// <returns></returns>
        private async Task NoPermissionNotification()
        {
            await ShowNotification("Location Permission", "Error", "Please enable location services to use this feature.", NotificationCategoryType.Error, DateTime.UtcNow);
        }
        private async Task RouteStoppedNotificationLater()
        {
            await ShowNotification("Route Stopped", "Status", "You have succesfully stopped your route!", NotificationCategoryType.Status, DateTime.UtcNow.AddSeconds(15));
        }
        private async Task GeneralErrorNotification()
        {
            await ShowNotification("Unforseen Action", "Error", "An unforseen action has occured!", NotificationCategoryType.Error, DateTime.UtcNow);
        }
        #endregion
    }
}