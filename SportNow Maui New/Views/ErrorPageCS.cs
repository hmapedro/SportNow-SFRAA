using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views
{
	public class ErrorPageCS : DefaultPage
	{
		public List<MainMenuItem> MainMenuItems { get; set; }

		private Member member;

		Label msg;
		Button btn;


		protected override void OnAppearing()
		{
            base.OnAppearing();
            Constants.ScreenWidth = Application.Current.MainPage.Width;//DeviceDisplay.MainDisplayInfo.Width;
			Constants.ScreenHeight = Application.Current.MainPage.Height; //DeviceDisplay.MainDisplayInfo.Height;
			Debug.Print("ScreenWidth = "+ Constants.ScreenWidth + " ScreenHeight = " + Constants.ScreenHeight);
		}

		public void initLayout()
		{
			Title = "Home";

			Label errorLabel = new Label
			{
				Text = "Verifique a sua ligação à Internet. No caso do problema persistir entre em contacto connosco através de info@nksl.org",
				TextColor = App.normalTextColor,
				FontSize = 25,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			absoluteLayout.Add(errorLabel);
            absoluteLayout.SetLayoutBounds(errorLabel, new Rect(0, App.screenHeight/2 - 150 * App.screenHeightAdapter, App.screenWidth, 300 * App.screenHeightAdapter));

			RoundButton retryButton = new RoundButton("Tentar novamente", 100, 40);
			retryButton.button.Clicked += OnRetryButtonClicked;


			absoluteLayout.Add(retryButton);
            absoluteLayout.SetLayoutBounds(errorLabel, new Rect(App.screenWidth / 2 - 20 * App.screenWidthAdapter, App.screenHeight / 2 + 150 * App.screenHeightAdapter, 100 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

		}

		async void OnRetryButtonClicked(object sender, EventArgs e)
		{
            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }


		public ErrorPageCS()
		{
			this.initLayout();
			
		}
	}
}
