using System.Collections.ObjectModel;
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
            Task.Run(async () =>
            {
                await database.GetRoutesAsync();
            });
        }

        [RelayCommand]
        public async Task OnDeleteButtonPressed()
        {
            RouteHistory.Remove(SelectedRoute);
            await database.DeleteRouteAsync(SelectedRoute);
        }

    }
}
