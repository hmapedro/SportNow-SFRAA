using System.Diagnostics;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.BetterFirebasePushNotification;
using Syncfusion.Maui.Core.Hosting;


namespace SportNow;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Microsoft.Maui.Hosting.MauiAppBuilder builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
        builder
#if DEBUG
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
#else
			.UseMauiCommunityToolkit(options =>
			{
				options.SetShouldEnableSnackbarOnWindows(true);
				options.SetShouldSuppressExceptionsInConverters(true);
				options.SetShouldSuppressExceptionsInBehaviors(true);
				options.SetShouldSuppressExceptionsInAnimations(true);
			})
#endif
            .UseMauiCommunityToolkitMarkup()//.UseMauiCommunityToolkitMarkup()
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("futura medium condensed bt.ttf", "futuracondensedmedium");
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();

#endif
        //builder.Services.AddTransient<IDeviceOrientationService, DeviceOrientationService>();
#if IOS
      
#endif
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE4OTkwN0AzMjM1MmUzMDJlMzBQVWp6TUNFNEllMG9FQjRMK1Y0TEQ2SHgyRWwwbCtOVmZzc3BUTGpwL3BzPQ==");


        builder.ConfigureLifecycleEvents(events => {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((_, options) =>
            {
                var usercategories = new NotificationUserCategory[]
            {
                new NotificationUserCategory("StillAlive", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Yes","Yes", NotificationActionType.Foreground),
                    new NotificationUserAction("No","No", NotificationActionType.Foreground)

                })
               

            };
                //If you dont want useractions call one of the other initialize options
                FirebasePushNotificationManager.Initialize(options, usercategories, true);
                return false;
            }));
           
#elif ANDROID
            var usercategories = new NotificationUserCategory[]
            {
                new NotificationUserCategory("StillAlive", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Yes","Yes", NotificationActionType.Foreground),
                    new NotificationUserAction("No","No", NotificationActionType.Foreground)

                })


            };


            events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                Firebase.FirebaseApp.InitializeApp(activity);
            }));

            /*events.AddAndroid(android => android.OnCreate((activity, _) =>
            //If you dont want useractions call one of the other initialize options
            FirebasePushNotificationManager.Initialize(usercategories, true, false, true)

            ));*/
#endif
        });
        builder.Services.AddSingleton<IPushNotificationHandler, DefaultPushNotificationHandler>();


        return builder.Build();
    }
}

