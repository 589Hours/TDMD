using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routey.Domain.SQLiteDatabases;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.ViewModels
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// ViewModel for the RoutesPage. This class is responsible for handling the logic of the RoutesPage.
    /// </summary>
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

        /// <summary>
        /// Gets the Routes from the database and sets the RouteHistory property.
        /// </summary>
        /// <returns></returns>
        public async Task GetRoutesAsync()
        {
            IEnumerable<RouteEntity> routes = await database.GetRoutesAsync();
            RouteHistory = new ObservableCollection<RouteEntity>(routes);
        }

        /// <summary>
        /// When a Route is selected, update the borde color of the selected frame. Also, set the SelectedRoute property.
        /// </summary>
        /// <param name="sender"></param>
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

        /// <summary>
        /// Deletes a Route from the database and the RouteHistory.
        /// </summary>
        /// <returns></returns>
        public async Task OnDeleteButtonPressed()
        {
            RouteHistory.Remove(SelectedRoute);
            await database.DeleteRouteAsync(SelectedRoute);
        }

    }
}