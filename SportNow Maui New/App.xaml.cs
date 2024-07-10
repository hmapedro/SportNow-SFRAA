using SportNow.Views;
using System.Diagnostics;
using SportNow.Model;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
//using Plugin.BetterFirebasePushNotification;
using SportNow.Services.Data.JSON;
using Plugin.BetterFirebasePushNotification;

namespace SportNow
{
    public partial class App : Application
    {

        public static List<Member> members;
        public static Member original_member;
        public static Member member;

        public static string VersionNumber = "1.0";
        public static string BuildNumber = "5";

        public static Competition competition;

         public static Competition_Participation competition_participation;
         public static Event_Participation event_participation;

        public static double screenWidthAdapter = 1, screenHeightAdapter = 1;
         public static int consentFontSize = 0, superbigTitleFontSize = 0, bigTitleFontSize = 0, titleFontSize = 0, menuButtonFontSize = 0, formLabelFontSize = 0, formValueFontSize = 0, itemTitleFontSize = 0, itemTextFontSize = 0, smallTextFontSize = 0, formValueSmallFontSize = 0;

         public static int ItemWidth = 0, ItemHeight = 0, ServiceItemHeigth = 0;

         //SELECTED TABS
         public static string DO_activetab = "graduacoes";
         public static string EVENTOS_activetab = "estagios";
         public static string EQUIPAMENTOS_activetab = "karategis";

         public static string token = "";

         public static string notification = "";

         public static bool isToPop = false;

        public static Color backgroundColor = Color.FromRgb(255, 255, 255);
        public static Color backgroundOppositeColor = Color.FromRgb(0, 0, 0);
        public static Color topColor = Color.FromRgb(52, 145, 70); //24, 45, 91); 52, 145, 70
        public static Color bottomColor = Color.FromRgb(98, 98, 98); //Color.FromRgb(100, 150, 215);
        public static Color activeTitleTextColor = Color.FromRgb(52, 144, 69);
        public static Color inactiveTitleTextColor = Color.FromRgb(29, 93, 41);
        public static Color normalTextColor = Color.FromRgb(50, 50, 50);
        public static Color oppositeTextColor = Colors.White;

        public static double screenWidth = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 10 * App.screenWidthAdapter;
        public static double screenHeight = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 10 * App.screenWidthAdapter;


        public App(bool hasNotification = false, object notificationData = null)
         {
            

            Debug.Print("App Constructor");
             if (hasNotification)
             {
                 Debug.Print("App Constructor hasNotification true");
             }
             else
             {
                 Debug.Print("App Constructor hasNotification false");
                InitializeComponent();


                if ((Preferences.Default.Get("EMAIL", "") != "") & (Preferences.Default.Get("PASSWORD", "") != ""))
                {
                    MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                    };
                }
                else
                {

                    MainPage = new NavigationPage(new LoginPageCS(""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };
                }

                /*MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };*/

                //MainPage = new AppShell();
                //new App();
            }
         }

         public App()
         {
             //InitializeComponent();

             if (DeviceInfo.Platform == DevicePlatform.Android)
             {
                 startNotifications();
             }

             checkPreviousLoginOk();

             //--  Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzczMTU1QDMyMzAyZTMzMmUzMFp6SzZZNk5GS1ZwbHFpY1FZblZqUWM2cnc3WXY2WWxFdEJkQmZWVUdXcFE9");

             //MainPage = new MainPage();

             CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);

         }

         public void checkPreviousLoginOk()
         {

             if ((Preferences.Default.Get("EMAIL", "") != "") & (Preferences.Default.Get("PASSWORD", "") != ""))
             {
                /*-- MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                 {
                     BarBackgroundColor = App.backgroundColor,
                     BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                 };*/
    }
            else
            {

                /*--MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };*/
            }
        }


        protected void startNotifications()
        {
            BetterFirebasePushNotification.Current.Subscribe("General");
            
            BetterFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
             {
                 Debug.WriteLine($"TOKEN : {p.Token}");
                 Debug.Print("App.original_member = " + App.original_member + ". App.token =" + App.token + ". p.Token=" + p.Token);
                 if ((App.original_member != null) & (App.token != p.Token))
                 {
                     MemberManager memberManager = new MemberManager();
                     var result = await memberManager.updateToken(App.original_member.id, p.Token);
                 }
                 App.token = p.Token;
             };

            BetterFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
             {
                 Debug.Print("OnNotificationReceived Cross");
                 App.notification = "OnNotificationReceived";
             };

            BetterFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
             {
                 Debug.Print("OnNotificationOpened Cross");

                 App.notification = "OnNotificationOpened";

                 executionActionPushNotification(p.Data);

                 if (!string.IsNullOrEmpty(p.Identifier))
                 {
                     System.Diagnostics.Debug.WriteLine($"ActionID: {p.Identifier}");
                 }

             };
         }

         protected override async void OnStart()

         {
             Debug.Print("OnStart");

             if (DeviceInfo.Platform != DevicePlatform.Android)
             {
                 startNotifications();
             }

         }

         public void executionActionPushNotification(IDictionary<string, object> dataDict)
         {
             var actiontype = "";
             var actionid = "";
             Debug.Print("Opened");
             App.notification = App.notification + " executionActionPushNotification";
             foreach (var data in dataDict)
             {
                 Debug.Print("Push Notification " + data.Key.ToString() + " = " + data.Value.ToString());

                 if (data.Key == "actiontype")
                 {
                     Debug.Print("Push Notification Action = " + data.Value.ToString());
                     actiontype = data.Value.ToString();
                 }

                 if (data.Key == "actionid")
                 {
                     Debug.Print("Push Notification Message = " + data.Value.ToString());
                     actionid = data.Value.ToString();

                 }
             }

             App.notification = App.notification + " actiontype = " + actiontype;
             App.notification = App.notification + " actionid = " + actionid;
             if (((actiontype == "openevent") | (actiontype == "opencompetition") | (actiontype == "openexaminationsession")) & (actionid != ""))
             {
                /*-- MainPage = new NavigationPage(new MainTabbedPageCS(actiontype, actionid))
                 {
                     BarBackgroundColor = App.backgroundColor,
                     BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                 };*/
                 //MainPage = new NavigationPage(new DetailEventPageCS(event_i));
             }
             else if ((actiontype == "openevaluationclass") | (actiontype == "openweekclass") | (actiontype == "opentodayclass") | (actiontype == "opentodayclassinstructor") | (actiontype == "authorizeregistration"))
             {
                /*--MainPage = new NavigationPage(new MainTabbedPageCS(actiontype, actionid))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };*/
                //MainPage = new NavigationPage(new DetailEventPageCS(event_i));
            }

        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        public static void screenWidthAdapterCalculator()
        {

            screenWidthAdapter = 1;
        }

        public static void screenHeightAdapterCalculator()
        {
            screenHeightAdapter = 1;
        }



        public static void AdaptScreen()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // App.screenWidth = mainDisplayInfo.Width;
            //App.screenHeight = mainDisplayInfo.Height;
            //Debug.Print("AQUI 1 - ScreenWidth = " + App.screenWidth + " ScreenHeight = " + App.screenHeight + "mainDisplayInfo.Density = " + mainDisplayInfo.Density);

            double fontresize = 0;
            if (Application.Current.MainPage.Width > Application.Current.MainPage.Height)
            {

                //App.screenHeight = Application.Current.MainPage.Width;
                //App.screenWidth = Application.Current.MainPage.Height;
                App.screenHeightAdapter = ((mainDisplayInfo.Width) / mainDisplayInfo.Density) / 850;
                App.screenWidthAdapter = ((mainDisplayInfo.Height) / mainDisplayInfo.Density) / 400;

                App.screenWidth = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 20 * App.screenHeightAdapter;
                App.screenHeight = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 20 * App.screenWidthAdapter;

                App.ItemWidth = (int)(120 * App.screenHeightAdapter);
                App.ItemHeight = (int)(App.screenWidth / 3 - 10 * App.screenWidthAdapter); //(int)(155 * App.screenWidthAdapter);
                App.ServiceItemHeigth = App.ItemHeight / 3 * 2;
                fontresize = screenHeightAdapter;
                if (fontresize > 2)
                {
                    fontresize = 2;
                }

            }
            else
            {
                //App.screenHeight = Application.Current.MainPage.Height;
                //App.screenWidth = Application.Current.MainPage.Width;
                App.screenWidthAdapter = ((mainDisplayInfo.Width) / mainDisplayInfo.Density) / 400;
                App.screenHeightAdapter = ((mainDisplayInfo.Height) / mainDisplayInfo.Density) / 850;
                App.screenWidth = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 10 * App.screenWidthAdapter;
                App.screenHeight = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 10 * App.screenHeightAdapter;

                App.ItemWidth = (int)(App.screenWidth / 2 - 10 * App.screenWidthAdapter); //(int)(155 * App.screenWidthAdapter);
                App.ItemHeight = (int)(120 * App.screenHeightAdapter);
                App.ServiceItemHeigth = App.ItemHeight / 3 * 2;
                fontresize = screenWidthAdapter;
                if (fontresize > 2)
                {
                    fontresize = 2;
                }

            }
            App.superbigTitleFontSize = (int)(28 * fontresize);
            App.bigTitleFontSize = (int)(24 * fontresize);
            App.titleFontSize = (int)(18 * fontresize);
            App.menuButtonFontSize = (int)(14 * fontresize);
            App.formLabelFontSize = (int)(16 * fontresize);
            App.formValueFontSize = (int)(16 * fontresize);
            App.formValueSmallFontSize = (int)(12 * fontresize);
            App.itemTitleFontSize = (int)(16 * fontresize);
            App.itemTextFontSize = (int)(12 * fontresize);
            App.consentFontSize = (int)(16 * fontresize);
            App.smallTextFontSize = (int)(10 * fontresize);

        }

        public static async Task<string> SendWhatsApp(string phoneNumber, string message = null)
        {
            try
            {
                string text = "whatsapp://send?phone=" + phoneNumber;
                if (!string.IsNullOrWhiteSpace(message))
                {
                    text = text + "&text=" + message;
                }

                await Browser.OpenAsync(new Uri(text), BrowserLaunchMode.External);
            }
            catch (Exception ex)
            {
                Debug.Print("Can't send Whatsapp message " + ex.Message);
                return "-1";
            }
            return "1";
        }

        public static string getSeason()
        {
            int currentMonth = Convert.ToInt16(DateTime.Now.Month.ToString());
            int currentYear = Convert.ToInt16(DateTime.Now.Year.ToString());
            if (currentMonth < 8)
            {
                return (currentYear - 1) + "_" + currentYear;
            }
            else
            {
                return currentYear + "_" + (currentYear + 1);
            }
            //return "";
        }

        public static string getSeasonString()
        {
            int currentMonth = Convert.ToInt16(DateTime.Now.Month.ToString());
            int currentYear = Convert.ToInt16(DateTime.Now.Year.ToString());
            if (currentMonth < 8)
            {
                return (currentYear - 1) + "-" + currentYear;
            }
            else
            {
                return currentYear + "-" + (currentYear + 1);
            }
            //return "";
        }

        public static string ToTitleCase(string str)
        {
            var firstword = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.Split(' ')[0].ToLower());
            str = str.Replace(str.Split(' ')[0], firstword);
            return str;
        }

        /*protected override Window CreateWindow(IActivationState activationState)
        {
            Debug.Print("OLAAAA");
            if (this.MainPage == null)
            {
                this.MainPage = new MainTabbedPageCS(null,null);
            }

                return base.CreateWindow(activationState);
        }*/

    }

}
