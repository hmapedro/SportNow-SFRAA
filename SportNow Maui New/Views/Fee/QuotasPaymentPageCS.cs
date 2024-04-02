using SportNow.Model;


namespace SportNow.Views
{
	public class QuotasPaymentPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
			if (App.isToPop == true)
			{
				App.isToPop = false;
				Navigation.PopAsync();
			}

		}


		protected override void OnDisappearing()
		{
		}

		private Member member;

		private Microsoft.Maui.Controls.Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "QUOTA";
		}


		public async void initSpecificLayout()
		{

			createPaymentOptions();
		}


		public void createPaymentOptions()
		{

			Label selectPaymentModeLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Escolhe o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			absoluteLayout.Add(selectPaymentModeLabel);
            absoluteLayout.SetLayoutBounds(selectPaymentModeLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenHeightAdapter, 80 * App.screenHeightAdapter));

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				//WidthRequest = 100 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter,
				//BackgroundColor = Colors.Red,
			};

			var tapGestureRecognizerMB = new TapGestureRecognizer();
			tapGestureRecognizerMB.Tapped += OnMBButtonClicked;
			MBLogoImage.GestureRecognizers.Add(tapGestureRecognizerMB);

            Label TermsPaymentMBLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Ao valor da Quota é acrescido 1.7% e 0.22€ (+ IVA).", // \n Total a pagar:" + CalculateMBPayment(monthFeeValue) + "€",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.formLabelFontSize
            };

            absoluteLayout.Add(MBLogoImage);
            absoluteLayout.SetLayoutBounds(MBLogoImage, new Rect(0, 130 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenHeightAdapter, 115 * App.screenHeightAdapter));
            absoluteLayout.Add(TermsPaymentMBLabel);
            absoluteLayout.SetLayoutBounds(TermsPaymentMBLabel, new Rect(0, 210 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenHeightAdapter, 115 * App.screenHeightAdapter));

            Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				//BackgroundColor = Colors.Green,
				//WidthRequest = 184 * App.screenHeightAdapter,
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter
			};

			
			var tapGestureRecognizerMBWay = new TapGestureRecognizer();
			tapGestureRecognizerMBWay.Tapped += OnMBWayButtonClicked;
			MBWayLogoImage.GestureRecognizers.Add(tapGestureRecognizerMBWay);

            Label TermsPaymentMBWayLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Ao valor da Quota é acrescido 0.7% e 0.07€ (+ IVA).",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.formLabelFontSize
            };

            absoluteLayout.Add(MBWayLogoImage);
            absoluteLayout.SetLayoutBounds(MBWayLogoImage, new Rect(0, 300 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenHeightAdapter, 115 * App.screenHeightAdapter));
            absoluteLayout.Add(TermsPaymentMBWayLabel);
            absoluteLayout.SetLayoutBounds(TermsPaymentMBWayLabel, new Rect(0, 380 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenHeightAdapter, 115 * App.screenHeightAdapter));

        }

        public QuotasPaymentPageCS(Member member)
		{

			this.member = member;

			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new QuotasMBPageCS(member));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new QuotasMBWayPageCS(member));
		}

	}
}

