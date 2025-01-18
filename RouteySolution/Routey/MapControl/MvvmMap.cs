namespace Routey.MapControl;

using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

/// <summary>
/// This class has been documented with the help of GitHub Copilot!
/// MvvmMap is a custom Map control that allows you to bind to the MapSpan property and the SelectedItem property.
/// Source: https://dev.to/symbiogenesis/use-net-maui-map-control-with-mvvm-dfl
/// </summary>
public class MvvmMap : Map
{
    /// <summary>
    /// This property keeps track of the map span. The map span is the place where the map is centered.
    /// </summary>
    public static readonly BindableProperty MapSpanProperty = BindableProperty.Create(nameof(MapSpan), typeof(MapSpan), typeof(MvvmMap), null, BindingMode.TwoWay, propertyChanged: (b, _, n) =>
    {
        if (b is MvvmMap map && n is MapSpan mapSpan)
        {
            MoveMap(map, mapSpan);
        }
    });

    /// <summary>
    /// This property keeps track of the selected item. The selected item is the item that is currently selected on the map.
    /// </summary>
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(MvvmMap), null, BindingMode.TwoWay);

    public MapSpan MapSpan
    {
        get => (MapSpan)this.GetValue(MapSpanProperty);
        set => this.SetValue(MapSpanProperty, value);
    }

    public object? SelectedItem
    {
        get => (object?)this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// This method moves the map to the specified map span.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="mapSpan"></param>
    private static void MoveMap(MvvmMap map, MapSpan mapSpan)
    {
        var timer = Application.Current!.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(500);
        timer.Tick += (s, e) =>
        {
            if (s is IDispatcherTimer timer)
            {
                timer.Stop();

                MainThread.BeginInvokeOnMainThread(() => map.MoveToRegion(mapSpan));
            }
        };

        timer.Start();
    }
}
