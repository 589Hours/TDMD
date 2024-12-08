using HueApp.ViewModels;

namespace HueApp;

public partial class LightDetailPage : ContentPage
{
	public LightDetailPage(LightDetailPageViewModel lightDetailPageViewModel)
	{
		InitializeComponent();
		BindingContext = lightDetailPageViewModel;
	}

    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (BindingContext is LightDetailPageViewModel viewModel)
        {
            viewModel.HueSliderChanged();
        }
    }

    private void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (BindingContext is LightDetailPageViewModel viewModel)
        {
            viewModel.BrightnessSliderChanged();
        }
    }

    private void SaturationSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (BindingContext is LightDetailPageViewModel viewModel)
        {
            viewModel.SaturationSliderChanged();
        }
    }
}