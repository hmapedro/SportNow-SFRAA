
using SportNow.CustomViews;

namespace SportNow.Views.Profile
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

			/*Frame backgroundFrame= new Frame
			{
				CornerRadius = 10,
				IsClippedToBounds = true,
				BackgroundColor = Color.FromRgb(60,60,60),
				HasShadow = false
			};

			absoluteLayout.Add(backgroundFrame,
				xConstraint: )10 * App.screenHeightAdapter),
				yConstraint: )0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (20 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Height) - (90 * App.screenHeightAdapter));
				})
			);*/

			Label labelSucesso = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			labelSucesso.Text = "OBRIGADO " + App.member.name.Split(' ')[0].ToUpper() + "!\n\n O teu treinador será avisado que concluíste o processo de registo e logo que ele aprove a tua inscrição poderás começar a utilizar a nossa App.";
			absoluteLayout.Add(labelSucesso);
			absoluteLayout.SetLayoutBounds(labelSucesso, new Rect(30 * App.screenHeightAdapter, 40 * App.screenHeightAdapter, App.screenWidth - 60 * App.screenWidthAdapter, 300 * App.screenHeightAdapter));


			Image logo_ippon = new Image
			{
				Source = "logo_aksl_round.png",
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 224 * App.screenHeightAdapter
			};
			absoluteLayout.Add(logo_ippon);
			absoluteLayout.SetLayoutBounds(logo_ippon, new Rect(30 * App.screenHeightAdapter, 350 * App.screenHeightAdapter, App.screenWidth - 60 * App.screenWidthAdapter, 224 * App.screenHeightAdapter));

			RoundButton confirmButton = new RoundButton("VOLTAR AO LOGIN", 100, 50);
			confirmButton.button.Clicked += confirmConsentButtonClicked;

			absoluteLayout.Add(confirmButton);
			absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenHeightAdapter, App.screenHeight - 60 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

        }

		public NewMemberSuccessPageCS()
		{
            this.initLayout();
            this.initSpecificLayout();
        }

		async void confirmConsentButtonClicked(object sender, EventArgs e)
		{
            Application.Current.MainPage = new NavigationPage(new LoginPageCS("Aguarda que o treinador aprove a tua inscrição."))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor
            };
        }
	}

}