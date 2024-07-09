using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;

namespace SportNow.Views
{
	public class CompetitionPaymentPageCS : DefaultPage
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

		private Competition competition_v;

		private List<Payment> payments;

		private Microsoft.Maui.Controls.Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{


			if (competition_v.value == 0)
			{
				createRegistrationConfirmed();
			}
			else
			{
				createPaymentOptions();
			}
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
				Text = "A sua Inscrição na Competição " + competition_v.name + " está Confirmada. \n Boa sorte e nunca te esqueças de te divertir!",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 200,
				FontSize = 30
			};

			absoluteLayout.Add(inscricaoOKLabel);		
			absoluteLayout.SetLayoutBounds(inscricaoOKLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, 200 * App.screenHeightAdapter));

			Image competitionImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
			competitionImage.Source = competition_v.imagemSource;

			absoluteLayout.Add(competitionImage);
            absoluteLayout.SetLayoutBounds(competitionImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

			CompetitionManager competitionManager = new CompetitionManager();

			await competitionManager.Update_Competition_Participation_Status(competition_v.participationid, "confirmado");
			competition_v.participationconfirmed = "confirmado";

		}

		public void createPaymentOptions() {

			Label selectPaymentModeLabel = new Label
			{
				Text = "Escolhe o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			absoluteLayout.Add(selectPaymentModeLabel);
            absoluteLayout.SetLayoutBounds(selectPaymentModeLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenHeightAdapter), 80 * App.screenHeightAdapter));

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
                Text = "Ao valor da Competição é acrescido 1.7% e 0.22€.", // \n Total a pagar:" + CalculateMBPayment(monthFeeValue) + "€",
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
                Text = "Ao valor da Competição é acrescido 0.7% e 0.07€ (+ IVA).",
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

        public CompetitionPaymentPageCS(Competition competition_v)
		{

			this.competition_v = competition_v;

			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CompetitionMBPageCS(this.competition_v));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("OnMBWayButtonClicked");
			await Navigation.PushAsync(new CompetitionMBWayPageCS(this.competition_v));
		}

	}
}

