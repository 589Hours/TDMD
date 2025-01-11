using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routey.Domain.SQLiteDatabases;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.ViewModels
{
    public partial class RoutesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<RouteEntity> routeHistory;

        [ObservableProperty]
        private RouteEntity selectedRoute;

        private IRouteDatabase database;

        public RoutesPageViewModel(IRouteDatabase database)
        {
            this.database = database;
        }

        public async Task GetRoutesAsync()
        {
            IEnumerable<RouteEntity> routes = await database.GetRoutesAsync();
            foreach (RouteEntity routeEntity in routes)
            {
                Debug.WriteLine($"Got route with:" +
                    $"\nName: {routeEntity.RouteName}" +
                    $"\nStart date: {routeEntity.RouteDateTime}" +
                    $"\nDistance: {routeEntity.TotalDistance}" +
                    $"\nDuration: {routeEntity.RouteDuration}" +
                    $"\nAverage Speed: {routeEntity.AverageSpeed}");
            }
            RouteHistory = new ObservableCollection<RouteEntity>(routes);
        }

        [RelayCommand]
        public async Task OnDeleteButtonPressed()
        {
            RouteHistory.Remove(SelectedRoute);
            await database.DeleteRouteAsync(SelectedRoute);
        }

    }
}
