using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class CompetitionMBWayPageCS : DefaultPage
    {

		protected override void OnDisappearing()
		{
		}

		private Competition competition_v;

		private Payment payment;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{
			showActivityIndicator();
			payment = await GetCompetitionParticipationPayment(this.competition_v);

            PaymentManager paymentManager = new PaymentManager();
            await paymentManager.Update_Payment_Mode(payment.id, "mbway");

            payment = await GetCompetitionParticipationPayment(this.competition_v);

			hideActivityIndicator();
            createLayoutPhoneNumber();
			/*
			if ((payments == null) | (payments.Count == 0))
			{
				createRegistrationConfirmed();
			}
			else {
				createMBPaymentLayout();
			}*/
		}

		public async void createLayoutPhoneNumber()
		{

			Label eventParticipationNameLabel = new Label
			{
				Text = "Para confirmares a tua presença no(a) " + competition_v.name + " efetua o pagamento de " + String.Format("{0:0.00}", payment.value) + "€.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			absoluteLayout.Add(eventParticipationNameLabel);
            absoluteLayout.SetLayoutBounds(eventParticipationNameLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 160 * App.screenHeightAdapter));


			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				WidthRequest = 184 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter

			};

			absoluteLayout.Add(MBWayLogoImage);
            absoluteLayout.SetLayoutBounds(MBWayLogoImage, new Rect((App.screenWidth / 2) - ((184 * App.screenHeightAdapter) / 2), 180 * App.screenHeightAdapter, 184 * App.screenWidthAdapter, 120 * App.screenHeightAdapter));

			Label phoneNumberLabel = new Label
			{
				Text = "Confirma o teu número de telefone",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 50 * App.screenHeightAdapter,
				FontSize = App.titleFontSize
            };

			absoluteLayout.Add(phoneNumberLabel);
            absoluteLayout.SetLayoutBounds(phoneNumberLabel, new Rect(0, 290 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			phoneValueEdit = new FormValueEdit(App.member.phone);
			phoneValueEdit.entry.HorizontalTextAlignment = TextAlignment.Center;
			phoneValueEdit.entry.FontSize = App.titleFontSize;


			absoluteLayout.Add(phoneValueEdit);
            absoluteLayout.SetLayoutBounds(phoneValueEdit, new Rect(10, 340 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));


            payButton = new RegisterButton("PAGAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            payButton.button.Clicked += OnPayButtonClicked;


			absoluteLayout.Add(payButton);
            absoluteLayout.SetLayoutBounds(payButton, new Rect(10 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

            Label labelTax = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O valor total desta transação incluiu uma taxa de " + String.Format("{0:0.00}", payment.value - competition_v.value) + "€",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.itemTextFontSize
            };

            absoluteLayout.Add(labelTax);
            absoluteLayout.SetLayoutBounds(labelTax, new Rect(10 * App.screenWidthAdapter, 380 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
        }


        public CompetitionMBWayPageCS(Competition competition_v)
		{
			this.competition_v = competition_v;
			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPayButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			payButton.IsEnabled = false;

			await CreateMbWayPayment(payment);

			hideActivityIndicator();
			payButton.IsEnabled = true;
		}

		async Task<Payment> GetCompetitionParticipationPayment(Competition competition)
		{
			Debug.WriteLine("GetCompetitonParticipationPayment");
			CompetitionManager competitionManager = new CompetitionManager();

			Payment payment = await competitionManager.GetCompetitionParticipation_Payment(competition);
			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return payment;
		}

		async Task<string> CreateMbWayPayment(Payment payment)
		{
			Debug.WriteLine("CreateMbWayPayment");
            showActivityIndicator();

            PaymentManager paymentManager = new PaymentManager();

			string value_string = Convert.ToString(payment.value);
			string result = await paymentManager.CreateMbWayPayment(App.original_member.id, payment.id, payment.orderid, phoneValueEdit.entry.Text, value_string, App.member.email);
			if ((result == "-2") | (result == "-3"))
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				hideActivityIndicator();
				return null;
			}
			hideActivityIndicator();
            await DisplayAlert("VALIDAÇÃO DE PAGAMENTO?", "Valida o pagamento na App MBWay ou no teu Home Banking. Logo que o faças podes voltar a consultar o estado da tua inscrição e verificares que já te encontras inscrito.", "Ok");
            //await UserDialogs.Instance.AlertAsync(new AlertConfig() { Title = "VALIDAÇÃO DE PAGAMENTO", "Valida o pagamento na App MBWay ou no teu Home Banking. Logo que o faças podes voltar a consultar o estado da tua inscrição e verificares que já te encontras inscrito.", "Ok" );

/*			App.isToPop = true;
			await Navigation.PopAsync();*/

			App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
			{
				BarBackgroundColor = App.backgroundColor,
				BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
			};



			/*			Application.Current.MainPage = new NavigationPage(new DetailEventPageCS(event_v))
						{
							BarBackgroundColor = App.backgroundColor,
							BarTextColor = App.normalTextColor
						};*/

			return result;
		}

	}
}

