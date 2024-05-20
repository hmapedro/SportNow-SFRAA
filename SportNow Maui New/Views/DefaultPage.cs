using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using SportNow.CustomViews;

namespace SportNow.Views
{
    public class DefaultPage : ContentPage
	{
        Border border;
        Frame background_frame;

        bool isRunning;
#if IOS
        GifImage loading;
#else
        public Image loading;
#endif


        private DeviceOrientationService _deviceOrientationService;
        private string _orientationLockState = "Unlocked";

        public ICommand LockPortraitCommand { get; }
        public ICommand LockLandscapeCommand { get; }
        public ICommand UnlockOrientationCommand { get; }

        protected async override void OnAppearing()
        {
#if ANDROID
            var currentActivity = ActivityStateManager.Default.GetCurrentActivity();
            if (currentActivity is not null)
            {
                currentActivity.RequestedOrientation = (Android.Content.PM.ScreenOrientation)DisplayOrientation.Portrait;
            }

            await Task.Delay(100);
            loading.IsAnimationPlaying = false;
            await Task.Delay(100);
            loading.IsAnimationPlaying = true;

#elif IOS
            this.LockOrientation(DisplayOrientation.Portrait);
#endif

        }

        public string OrientationLockState;/*
        {
            get => _orientationLockState;
            set => SetField(ref _orientationLockState, value);
        }*/



        private void LockOrientation(DisplayOrientation? orientation)
        {
            if (_deviceOrientationService == null)
            {
                //Debug.Print("_deviceOrientationService null");
                return;
            }


            switch (orientation)
            {
                case DisplayOrientation.Portrait:
                    //Debug.Print("Locked Portrait");
                    this.OrientationLockState = "Locked Portrait";
                    _deviceOrientationService.LockPortraitInterface();
                    break;
                case DisplayOrientation.Landscape:
                    //Debug.Print("Locked Landscape");
                    _deviceOrientationService.LockPortraitInterface();
                    this.OrientationLockState = "Locked Landscape";
                    break;
                case null:
                case DisplayOrientation.Unknown:
                default:
                   // Debug.Print("Unlocked");
                    _deviceOrientationService.LockPortraitInterface();
                    this.OrientationLockState = "Unlocked";
                    break;
            }
        }


        public DefaultPage()
        {
            isRunning = false;
            this.initBaseLayout();

            DeviceOrientationService deviceOrientationService = new DeviceOrientationService();
            _deviceOrientationService = deviceOrientationService;
            deviceOrientationService.LockPortraitInterface();

#if ANDROID
            var currentActivity = ActivityStateManager.Default.GetCurrentActivity();
            if (currentActivity is not null)
            {
                currentActivity.RequestedOrientation = (Android.Content.PM.ScreenOrientation)DisplayOrientation.Portrait;

            }
#elif IOS
            this.LockOrientation(DisplayOrientation.Portrait);
#endif
        }

        public AbsoluteLayout absoluteLayout;

		public void initBaseLayout()
		{    
			this.BackgroundColor = App.backgroundColor;

            absoluteLayout = new AbsoluteLayout
            {
				Margin = new Thickness(5 * App.screenWidthAdapter)
			};
			Content = absoluteLayout;

			NavigationPage.SetBackButtonTitle(this, "");


            if (Application.Current.MainPage != null)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = App.backgroundColor;
                navigationPage.BarTextColor = App.normalTextColor;
            }

            background_frame = new Frame() { BackgroundColor = App.backgroundColor, BorderColor = Colors.Transparent, Opacity = 0.3 };

            border = new Border
            {
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5 * (float)App.screenHeightAdapter,
                },
                BackgroundColor = Color.FromRgb(200, 200, 200),
                Opacity = 0.2,
                Padding = new Thickness(0, 0, 0, 0),
                HeightRequest = 80 * App.screenHeightAdapter,
                WidthRequest = 160 * App.screenWidthAdapter,
                VerticalOptions = LayoutOptions.Center,
            };

#if IOS
            loading = new GifImage() { Asset = "loading.gif", WidthRequest = 70 * App.screenHeightAdapter, HeightRequest = 70 * App.screenHeightAdapter}; 
#else
            loading = new Image() { Source = "loading.gif", IsAnimationPlaying = true };
#endif


            //indicator = new ActivityIndicator() { Color = App.topColor, HeightRequest = 100, WidthRequest = 100, MinimumHeightRequest = 100, MinimumWidthRequest = 100};
        }

        public void showActivityIndicator()
        {
            Debug.Print("showActivityIndicator isRunning = " + isRunning);
            if (isRunning == false)
            {
                isRunning = true;

                absoluteLayout.Add(background_frame);
                absoluteLayout.SetLayoutBounds(background_frame, new Rect(0, 0, App.screenWidth, App.screenHeight));

                absoluteLayout.Add(border);
                absoluteLayout.SetLayoutBounds(border, new Rect((App.screenWidth / 2) - 80 * App.screenWidthAdapter, (App.screenHeight / 2) - 140 * App.screenHeightAdapter, 160 * App.screenWidthAdapter, 80 * App.screenHeightAdapter));

                absoluteLayout.Add(loading);
                absoluteLayout.SetLayoutBounds(loading, new Rect((App.screenWidth / 2) - 80 * App.screenWidthAdapter, (App.screenHeight / 2) - 140 * App.screenHeightAdapter, 160 * App.screenWidthAdapter, 80 * App.screenHeightAdapter));
            }

        }

        public void hideActivityIndicator()
        {
            Debug.Print("hideActivityIndicator");
            absoluteLayout.Remove(background_frame);
            absoluteLayout.Remove(border);
            absoluteLayout.Remove(loading);
            isRunning = false;
            //indicator.IsRunning = false;
        }
    }
}
