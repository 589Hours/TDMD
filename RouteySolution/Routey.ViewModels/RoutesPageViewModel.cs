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
        private IRouteDatabase database;

        [ObservableProperty]
        private ObservableCollection<RouteEntity> routeHistory;

        [ObservableProperty]
        private RouteEntity selectedRoute;

        [ObservableProperty]
        private Color selectedColor;

        [ObservableProperty]
        private Color unSelectedColor;

        public RoutesPageViewModel(IRouteDatabase database)
        {
            this.database = database;
            SelectedRoute = null;
        }
        
        public async Task GetRoutesAsync()
        {
            IEnumerable<RouteEntity> routes = await database.GetRoutesAsync();
            RouteHistory = new ObservableCollection<RouteEntity>(routes);
        }

        private Frame previousSelectedFrame;
        public void OnRouteSelected(object sender)
        {
            if (sender is Frame selectedFrame)
            {
                if (selectedFrame.BindingContext as RouteEntity == null)
                    return;

                selectedFrame.BorderColor = Color.FromRgb(0, 0, 255);

                if (previousSelectedFrame != null && previousSelectedFrame != selectedFrame)
                    previousSelectedFrame.BorderColor = Color.FromRgb(0,0,0);
                    previousSelectedFrame = selectedFrame;

                SelectedRoute = selectedFrame.BindingContext as RouteEntity;
                Debug.WriteLine(SelectedRoute.RouteName);
            }
        }

        public async Task OnDeleteButtonPressed()
        {
            RouteHistory.Remove(SelectedRoute);
            await database.DeleteRouteAsync(SelectedRoute);
        }

    }
}
