using Android.Content.PM;
using Microsoft.Maui.Devices;

namespace SportNow
{
    public partial class DeviceOrientationService
    {
        private static readonly IReadOnlyDictionary<DisplayOrientation, ScreenOrientation> _androidDisplayOrientationMap =
            new Dictionary<DisplayOrientation, ScreenOrientation>
            {
                [DisplayOrientation.Landscape] = ScreenOrientation.Landscape,
                [DisplayOrientation.Portrait] = ScreenOrientation.Portrait,
            };

        private void SetDeviceOrientation(DisplayOrientation displayOrientation)
        {
            var currentActivity = ActivityStateManager.Default.GetCurrentActivity();
            if (currentActivity is not null)
            {
                if (_androidDisplayOrientationMap.TryGetValue(displayOrientation, out ScreenOrientation screenOrientation))
                    currentActivity.RequestedOrientation = screenOrientation;
            }
        }

        partial void LockPortrait()
        {
            SetDeviceOrientation(DisplayOrientation.Portrait);

        }

        partial void LockLandscape()
        {
            SetDeviceOrientation(DisplayOrientation.Landscape);

        }

        partial void UnlockOrientation()
        {
            SetDeviceOrientation(DisplayOrientation.Unknown);
        }
    }
}
