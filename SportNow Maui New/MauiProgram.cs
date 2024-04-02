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
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
                fonts.AddFont("futura medium condensed bt.ttf", "futuracondensedmedium");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
        builder.Logging.AddDebug();

#endif
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk4MzIzN0AzMjM0MmUzMDJlMzBLSEZ0a3U0Rlk3UVNjZWxBWmNtclJkOW5jVG1tWm52aGlUNng2THJsWkhnPQ==");

        //RegisterFirebase(builder);
        
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

            events.AddAndroid(android => android.OnCreate((activity, _) =>
            //If you dont want useractions call one of the other initialize options
            FirebasePushNotificationManager.Initialize(usercategories, true, false, true)

            ));
#endif
        });
        builder.Services.AddSingleton<IPushNotificationHandler, DefaultPushNotificationHandler>();
        

        return builder.Build();
	}

    private static MauiAppBuilder RegisterFirebase(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                Firebase.Core.App.Configure();
                return false;
            }));
#else
            events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                Firebase.FirebaseApp.InitializeApp(activity);
            }));
#endif
        });

        return builder;
    }
}

