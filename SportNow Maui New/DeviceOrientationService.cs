using System.Diagnostics;
using System.Runtime.Versioning;
namespace SportNow
{
    public partial class DeviceOrientationService
    {
        //DisplayOrientation CurrentOrientation { get; }

        public void LockPortraitInterface()
        {
           // Debug.Print("LockPortraitInterface");
            LockPortrait();
        }

        public void LockLandscapeInterface()
        {
            //Debug.Print("LockLandscapeInterface");
            LockLandscape();
        }

        partial void LockPortrait();


        partial void LockLandscape();

        [SupportedOSPlatform("ios16.0")]
        partial void UnlockOrientation();

        public void UnlockOrientationInterface()
        {
            //Debug.Print("UnlockOrientationInterface");
            UnlockOrientation();
        }
    }
    
    
}
