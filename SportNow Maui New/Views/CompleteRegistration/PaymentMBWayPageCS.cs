
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views.CompleteRegistration
{

	public class PaymentMBWayPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}

		private Payment payment;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		string paymentID;

		bool paymentDetected;

	
        public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			//payment = await GetPayment(this.paymentID);

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

			Label paymentNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = payment.name,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize
			};

            absoluteLayout.Add(paymentNameLabel);
            absoluteLayout.SetLayoutBounds(paymentNameLabel, new Rect(0 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 100 * App.screenHeightAdapter));


			Label valorLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Valor: "+payment.value.ToString("0.00") + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize
			};

            absoluteLayout.Add(valorLabel);
            absoluteLayout.SetLayoutBounds(valorLabel, new Rect(0 * App.screenWidthAdapter, 110 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				WidthRequest = 184 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
			};

            absoluteLayout.Add(MBWayLogoImage);
            absoluteLayout.SetLayoutBounds(MBWayLogoImage, new Rect((App.screenWidth / 2) - (184/2 * App.screenHeightAdapter), 170 * App.screenHeightAdapter, 184 * App.screenHeightAdapter, 120 * App.screenHeightAdapter));

			Label phoneNumberLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Confirme o seu número de telefone",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 50 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

            absoluteLayout.Add(phoneNumberLabel);
            absoluteLayout.SetLayoutBounds(phoneNumberLabel, new Rect(0 * App.screenHeightAdapter, 280 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			phoneValueEdit = new FormValueEdit(App.member.phone);
			phoneValueEdit.entry.HorizontalTextAlignment = TextAlignment.Center;
			phoneValueEdit.entry.FontSize = App.titleFontSize;

            absoluteLayout.Add(phoneValueEdit);
            absoluteLayout.SetLayoutBounds(phoneValueEdit, new Rect(0 * App.screenHeightAdapter, 330 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			payButton = new RegisterButton("PAGAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			payButton.button.Clicked += OnPayButtonClicked;

            absoluteLayout.Add(payButton);
            absoluteLayout.SetLayoutBounds(payButton, new Rect(10 * App.screenWidthAdapter, (App.screenHeight) - (160 * App.screenHeightAdapter), (App.screenWidth - 20 * App.screenHeightAdapter), 50 * App.screenHeightAdapter));



        }


        public PaymentMBWayPageCS(Payment payment)
		{
			//App.event_participation = event_participation;
			this.payment = payment;
			this.initLayout();
			this.initSpecificLayout();

			paymentDetected = false;

            int sleepTime = 5;
            Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
            {
                if (payment != null)
                {
                    this.checkPaymentStatus();
                    if (paymentDetected == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        async void checkPaymentStatus()
        {
            Debug.Print("checkPaymentStatus");
            this.payment = await GetPayment(payment.id);
            if ((payment.status == "confirmado") | (payment.status == "fechado") | (payment.status == "recebido"))
            {
                App.member.estado = "activo";
                App.original_member.estado = "activo";

                if (paymentDetected == false)
                {
                    paymentDetected = true;

                    await DisplayAlert("Pagamento Confirmado", "O seu pagamento foi recebido com sucesso. Já pode aceder à nossa App!", "Ok");
                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };

                }
            }
        }

        async void OnPayButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			payButton.IsEnabled = false;

			await CreateMbWayPayment(payment);

			hideActivityIndicator();
			payButton.IsEnabled = true;
		}

		async Task<Payment> GetPayment(string paymentID)
		{
			Debug.WriteLine("GetPayment");
			PaymentManager paymentManager = new PaymentManager();

			Payment payment = await paymentManager.GetPayment(this.paymentID);

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

				
            await DisplayAlert("VALIDAÇÃO DE PAGAMENTO", "Valide o pagamento na App MBWay ou no seu Home Banking. Logo que o faça pode voltar a consultar o estado da sua inscrição e verificar se já se encontra inscrito.", "OK");
            //await UserDialogs.Instance.AlertAsync(new AlertConfig() { Title = "VALIDAÇÃO DE PAGAMENTO", Message = "Valide o pagamento na App MBWay ou no seu Home Banking. Logo que o faça pode voltar a consultar o estado da sua inscrição e verificar se já se encontra inscrito.", OkText = "Ok" });

/*			App.isToPop = true;
			await Navigation.PopAsync();*/

			App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
			{
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor
            };



			/*			Application.Current.MainPage = new NavigationPage(new DetailEventPageCS(event_v))
						{
							BarBackgroundColor = Color.FromRgb(15, 15, 15),
							BarTextColor = Color.White
						};*/

			return result;
		}

	}
}

