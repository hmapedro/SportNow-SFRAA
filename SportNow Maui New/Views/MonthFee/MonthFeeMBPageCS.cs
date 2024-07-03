
using Microsoft.Maui.Controls.Shapes;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;

namespace SportNow.Views
{
	public class monthFeeMBPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			//App.competition_participation = competition_participation;

			//registerButton.IsEnabled = true;
			//estadoValue.entry.Text = App.competition_participation.estado;
		}

		private MonthFee monthFee;

		private List<Payment> payments;

		private Microsoft.Maui.Controls.Grid gridMBPayment;

		public void initLayout()
		{
            Title = "MENSALIDADE - PAGAMENTO MB";
        }


		public async void initSpecificLayout()
		{

			payments = await GetMonthFee_Payment(monthFee);

            PaymentManager paymentManager = new PaymentManager();
			await paymentManager.Update_Payment_Mode(payments[0].id, "mb");

            payments = await GetMonthFee_Payment(monthFee);


            if ((payments == null) | (payments.Count == 0))
			{
				//createRegistrationConfirmed();
			}
			else {
				createMBPaymentLayout();
			}
		}

		public async void createRegistrationConfirmed()
		{

		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Microsoft.Maui.Controls.Grid { Padding = 10, ColumnSpacing = 20 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label competitionParticipationNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Para efetuares o pagamento da tua " + monthFee.name + " - "+ payments[0].value + "€ usa os dados indicados em baixo.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
            };

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				WidthRequest = 100,
				HeightRequest = 100
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

			Grid gridMBDataPayment = new Microsoft.Maui.Controls.Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
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
				FontSize = App.titleFontSize
            };
			Label referenceLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label valueLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };

			Label entityValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = payments[0].entity,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label referenceValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = payments[0].reference,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label valueValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = String.Format("{0:0.00}", payments[0].value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };

            Border MBDataFrame= new Border {
				BackgroundColor = App.backgroundColor,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5 * (float)App.screenHeightAdapter,
                },
                Stroke = App.topColor,
			};
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Add(entityLabel, 0, 0);
			gridMBDataPayment.Add(entityValue, 1, 0);
			gridMBDataPayment.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Add(referenceValue, 1, 1);
			gridMBDataPayment.Add(valueLabel, 0, 2);
			gridMBDataPayment.Add(valueValue, 1, 2);

            gridMBPayment.Add(competitionParticipationNameLabel, 0, 0);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(competitionParticipationNameLabel, 2);

			gridMBPayment.Add(MBLogoImage, 0, 2);
			gridMBPayment.Add(referenciaMBLabel, 1, 2);

			gridMBPayment.Add(MBDataFrame, 0, 4);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(MBDataFrame, 2);

            absoluteLayout.Add(gridMBPayment);
            absoluteLayout.SetLayoutBounds(gridMBPayment, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 10 * App.screenHeightAdapter));
        }
		

        public monthFeeMBPageCS(MonthFee monthFee)
		{

			this.monthFee = monthFee;
			//App.competition_participation = competition_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async Task<List<Payment>> GetMonthFee_Payment(MonthFee monthFee)
		{
			Debug.WriteLine("GetMonthFee_Payment");
			MonthFeeManager monthFeeManager = new MonthFeeManager();

			List<Payment> payments = await monthFeeManager.GetMonthFee_Payment(monthFee.id);
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

