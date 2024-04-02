using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views.CompleteRegistration
{
	public class BeginPageCS : DefaultPage
	{
		bool dialogShowing;

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}


		public void initLayout()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS - initLayout");
			Title = "INSCRIÇÃO";

            //NavigationPage.SetHasNavigationBar(this, false);

            App.AdaptScreen();
            Image company_logo = new Image
			{
				Source = "company_logo.png",
				HorizontalOptions = LayoutOptions.Center,
				Opacity = 0.8,
			};

            absoluteLayout.Add(company_logo);
            absoluteLayout.SetLayoutBounds(company_logo, new Rect(50 * App.screenWidthAdapter, App.screenHeight/2 - (App.screenWidth - 100 * App.screenWidthAdapter)/2, App.screenWidth - 100 * App.screenWidthAdapter, App.screenWidth - 100 * App.screenWidthAdapter));

			var toolbarItem = new ToolbarItem
			{
				Text = "Logout",
				
			};
			toolbarItem.Clicked += OnLogoutButtonClicked;
			ToolbarItems.Add(toolbarItem);

			NavigationPage.SetHasBackButton(this, false);
		}


		public void initSpecificLayout()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS - initSpecificLayout");

			Label welcomeLabel = new Label
			{
				Text = "BEM-VINDO AO NK SANGALHOS",
				TextColor = App.topColor,
				FontSize = App.bigTitleFontSize,
				HorizontalOptions = LayoutOptions.Center,
                FontFamily = "futuracondensedmedium",
            };
            absoluteLayout.Add(welcomeLabel);
            absoluteLayout.SetLayoutBounds(welcomeLabel, new Rect(5 * App.screenWidthAdapter, 30 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			Label welcomeLabel1 = new Label
			{
				Text = "Para terminares o teu processo de inscrição, clica em 'Continuar'",
				TextColor = App.topColor,
				FontSize = App.titleFontSize,
				HorizontalOptions = LayoutOptions.Center,
                FontFamily = "futuracondensedmedium",
            };
            absoluteLayout.Add(welcomeLabel1);
            absoluteLayout.SetLayoutBounds(welcomeLabel1, new Rect(5 * App.screenWidthAdapter, 70 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			RoundButton confirmButton = new RoundButton("CONTINUAR", App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			confirmButton.button.Clicked += OnConfirmButtonClicked;

            absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(5 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
		}

		public BeginPageCS()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS() " + App.member.gender);
            Debug.Print("CompleteRegistration_Begin_PageCS() " + App.member.member_type);
            this.initLayout();
			this.initSpecificLayout();
		}

		async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ConsentPageCS());
			//await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
		}

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnLogoutButtonClicked");

            Preferences.Default.Remove("EMAIL");
            Preferences.Default.Remove("PASSWORD");
            Preferences.Default.Remove("SELECTEDUSER");
            App.member = null;
            App.members = null;



            Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }
    }

}