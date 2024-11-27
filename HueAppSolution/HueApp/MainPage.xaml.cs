using System.Diagnostics;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using HueApp.Infrastructure.HueApi;
using HueApp.ViewModels;

namespace HueApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
    }
}
