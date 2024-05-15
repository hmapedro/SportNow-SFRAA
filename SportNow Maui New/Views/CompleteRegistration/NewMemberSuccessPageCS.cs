
using SportNow.CustomViews;

namespace SportNow.Views.CompleteRegistration
{
    public class NewMemberSuccessPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			initLayout();
			initSpecificLayout();
		}


		protected async override void OnDisappearing()
		{
			if (absoluteLayout != null)
			{
				absoluteLayout = null;
				this.Content = null;
			}

		}

		//Image estadoQuotaImage;

		Label titleLabel;

		public void initLayout()
		{
			Title = "BEM-VINDO";
            NavigationPage.SetBackButtonTitle(this, "");

        }


		public async void initSpecificLayout()
		{
			if (absoluteLayout == null)
			{
				initBaseLayout();
			}

			Label labelSucesso = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			labelSucesso.Text = "OBRIGADO " + App.member.name.Split(' ')[0].ToUpper() + "!\n\n O seu processo de inscrição foi concluído com sucesso pelo que poderá começar a utilizar a nossa Aplicação.";
			absoluteLayout.Add(labelSucesso);
			absoluteLayout.SetLayoutBounds(labelSucesso, new Rect(30 * App.screenHeightAdapter, 40 * App.screenHeightAdapter, App.screenWidth - 60 * App.screenWidthAdapter, 300 * App.screenHeightAdapter));


			Image logo_ippon = new Image
			{
				Source = "company_logo.png",
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 224 * App.screenHeightAdapter
			};
			absoluteLayout.Add(logo_ippon);
			absoluteLayout.SetLayoutBounds(logo_ippon, new Rect(30 * App.screenHeightAdapter, 350 * App.screenHeightAdapter, App.screenWidth - 60 * App.screenWidthAdapter, 224 * App.screenHeightAdapter));

			RoundButton confirmButton = new RoundButton("VOLTAR AO LOGIN", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			confirmButton.button.Clicked += confirmConsentButtonClicked;

			absoluteLayout.Add(confirmButton);
			absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenHeightAdapter, App.screenHeight - 100 - 60 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

        }

		public NewMemberSuccessPageCS()
		{
            this.initLayout();
            this.initSpecificLayout();
        }

		async void confirmConsentButtonClicked(object sender, EventArgs e)
		{
            Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor
            };
        }
	}

}