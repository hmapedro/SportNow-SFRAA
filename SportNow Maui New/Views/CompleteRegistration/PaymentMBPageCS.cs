using SportNow.Model;
using SportNow.Services.Data.JSON;
using System;
using System.Diagnostics;

namespace SportNow.Views.CompleteRegistration
{
	public class PaymentMBPageCS : DefaultPage
	{

		private Grid gridMBPayment;

		string paymentID;
		Payment payment;

		bool paymentDetected;


        public void initLayout()
		{
			Title = "Inscrição";

			NavigationPage.SetBackButtonTitle(this, "");
		}


		public async void initSpecificLayout()
		{

			payment = await GetPayment(this.paymentID);
	
			createMBPaymentLayout();
		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 100 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 


			Label paymentNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = payment.name,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize,
			};

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				WidthRequest = 100,
				HeightRequest = 100,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
		};

			Label referenciaMBLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Pagamento por\n Multibanco",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 100,
				FontSize = App.bigTitleFontSize
			};

			Grid gridMBDataPayment = new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

			Label entityLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Entidade:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = 20
			};
			Label referenceLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = 20
			};
			Label valueLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = 20
			};

			Label entityValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = this.payment.entity,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = 20
			};
			Label referenceValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = this.payment.reference,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = 20
			};
			Label valueValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = String.Format("{0:0.00}", this.payment.value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = 20
			};

			Frame MBDataFrame= new Frame { BackgroundColor = App.backgroundColor, BorderColor = App.topColor, CornerRadius = 10, IsClippedToBounds = true, Padding = 0, HasShadow = false };
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Add(entityLabel, 0, 0);
			gridMBDataPayment.Add(entityValue, 1, 0);
			gridMBDataPayment.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Add(referenceValue, 1, 1);
			gridMBDataPayment.Add(valueLabel, 0, 2);
			gridMBDataPayment.Add(valueValue, 1, 2);

			gridMBDataPayment.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Add(valueLabel, 0, 2);

			gridMBPayment.Add(paymentNameLabel, 0, 0);
			Grid.SetColumnSpan(paymentNameLabel, 2);

			gridMBPayment.Add(MBLogoImage, 0, 2);
			gridMBPayment.Add(referenciaMBLabel, 1, 2);

			gridMBPayment.Add(MBDataFrame, 0, 4);
			Grid.SetColumnSpan(MBDataFrame, 2);


            absoluteLayout.Add(gridMBPayment);
            absoluteLayout.SetLayoutBounds(gridMBPayment, new Rect(0 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));

            Label Label = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O valor total desta transação incluiu uma taxa de 1.7% e 0.22€ (+ IVA).",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };

            absoluteLayout.Add(Label);
            absoluteLayout.SetLayoutBounds(Label, new Rect(22, 0 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 10 * App.screenHeightAdapter));

        }
    

		public PaymentMBPageCS(string paymentID)
		{
			this.paymentID = paymentID;
			this.initLayout();
			this.initSpecificLayout();

            paymentDetected = false;

			int sleepTime = 5;
			Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
			{
				if ((paymentID != null) & (paymentID != ""))
				{
					this.checkPaymentStatus(paymentID);
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

        async void checkPaymentStatus(string paymentID)
        {
            Debug.Print("checkPaymentStatus");
            this.payment = await GetPayment(paymentID);
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

	}
}

