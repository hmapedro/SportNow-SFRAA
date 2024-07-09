using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class EventPaymentPageCS : DefaultPage
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

		
		private Event_Participation event_participation;
		private Event event_v;

		private List<Payment> payments;

		private Microsoft.Maui.Controls.Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			if (App.member.currentFee != null)
			{
				if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
				{
					payments = await GetEventParticipationPayment(event_participation);

					if (payments == null)
					{
						createRegistrationConfirmed();
					}
					else if (payments.Count == 0)
					{
						createRegistrationConfirmed();
					}
					else if (event_v.value == 0)
					{
						createRegistrationConfirmed();
					}
					else
					{
						createPaymentOptions();
					}
				}
			}
			else
			{
				await DisplayAlert("QUOTA ", "A sua quota não está válida.", "Ok" );
				await Navigation.PopAsync();
			}



			
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "A sua Inscrição no Evento \n " + event_participation.evento_name + " \n está Confirmada. \n\n BOA SORTE\n e nunca te esqueças de te divertir!",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 300 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

			absoluteLayout.Add(inscricaoOKLabel);
            absoluteLayout.SetLayoutBounds(inscricaoOKLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, 300 * App.screenHeightAdapter));

			Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
			eventoImage.Source = event_v.imagemSource;

			absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

			EventManager eventManager = new EventManager();

			await eventManager.Update_Event_Participation_Status(event_participation.id, "inscrito");
			event_participation.estado = "inscrito";

		}

		public void createPaymentOptions() {

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
                Text = "Ao valor do Evento é acrescido 1.7% e 0.22€ (+ IVA).",
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
                Text = "Ao valor do Evento é acrescido 0.7% e 0.07€ (+ IVA).",
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

        public EventPaymentPageCS(Event event_v, Event_Participation event_participation)
		{

			this.event_participation = event_participation;
			this.event_v = event_v;

			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EventMBPageCS(event_participation));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EventMBWayPageCS(event_v, event_participation));
		}

		async Task<List<Payment>> GetEventParticipationPayment(Event_Participation event_participation)
		{
			Debug.WriteLine("GetCompetitionParticipationPayment");
			EventManager eventManager = new EventManager();

			List<Payment> payments = await eventManager.GetEventParticipation_Payment(event_participation.id);
			if (payments == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return payments;
		}

	}
}

