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
	public class QuotasMBWayPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}

		private Member member;

		private List<Payment> payments;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		double quotaOriginalValue = 0;

		public void initLayout()
		{
            Title = "QUOTA - PAGAMENTO MBWAY";
        }


		public async void initSpecificLayout()
		{

            showActivityIndicator();
            payments = await GetFeePayment(this.member);

			PaymentManager paymentManager = new PaymentManager();
            await paymentManager.Update_Payment_Mode(payments[0].id, "dinheiro");

            payments = await GetFeePayment(this.member);
			quotaOriginalValue = payments[0].value;

            await paymentManager.Update_Payment_Mode(payments[0].id, "mbway");

            payments = await GetFeePayment(this.member);
            hideActivityIndicator();

            

			createLayoutPhoneNumber();
		}

		public async void createLayoutPhoneNumber()
		{

			Label eventParticipationNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Para ativares a tua Quota - " + member.currentFee.name + " efetua o pagamento de " + String.Format("{0:0.00}", payments[0].value) + "€",
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
                FontFamily = "futuracondensedmedium",
                Text = "Confirma o teu número de telefone",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 50 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

            Label Label = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O valor total desta transação incluiu uma taxa de " + String.Format("{0:0.00}", payments[0].value - quotaOriginalValue) + "€",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.itemTextFontSize
            };

            absoluteLayout.Add(phoneNumberLabel);
            absoluteLayout.SetLayoutBounds(phoneNumberLabel, new Rect(0, 290 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			phoneValueEdit = new FormValueEdit(this.member.phone);
			phoneValueEdit.entry.HorizontalTextAlignment = TextAlignment.Center;
			phoneValueEdit.entry.FontSize = App.titleFontSize;


			absoluteLayout.Add(phoneValueEdit);
            absoluteLayout.SetLayoutBounds(phoneValueEdit, new Rect(5, 340 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			payButton = new RegisterButton("PAGAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            payButton.button.Clicked += OnPayButtonClicked;
            absoluteLayout.Add(payButton);
            absoluteLayout.SetLayoutBounds(payButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

            absoluteLayout.Add(Label);
            absoluteLayout.SetLayoutBounds(Label, new Rect(22, -10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 10 * App.screenHeightAdapter));

        }


        public QuotasMBWayPageCS(Member member)
		{

			this.member = member;
			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPayButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			payButton.IsEnabled = false;

			await CreateMbWayPayment(payments[0]);

			hideActivityIndicator();
			payButton.IsEnabled = true;
		}

		async Task<List<Payment>> GetFeePayment(Member member)
		{
			Debug.WriteLine("GetFeePayment");
			MemberManager memberManager = new MemberManager();

			payments = await memberManager.GetFeePayment(member.currentFee.id);
			Debug.Print("OLA");
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
			await DisplayAlert("VALIDAÇÃO DE PAGAMENTO", "Valida o pagamento na App MBWay ou no teu Home Banking. Logo que o faças podes voltar a consultar o estado da tua inscrição e verificares que já te encontras inscrito.", "Ok" );

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

