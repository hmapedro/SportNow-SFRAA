using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;


namespace SportNow.Views
{
	public class CompetitionMBPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			//App.competition_participation = competition_participation;

			//registerButton.IsEnabled = true;
			//estadoValue.entry.Text = App.competition_participation.estado;
		}

		private Competition competition;
		//private List<Competition> competitions;

		private Payment payment;

		private Microsoft.Maui.Controls.Grid gridMBPayment;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			showActivityIndicator();
			payment = await GetCompetitionParticipationPayment(this.competition);

            PaymentManager paymentManager = new PaymentManager();
            await paymentManager.Update_Payment_Mode(payment.id, "mb");

            payment = await GetCompetitionParticipationPayment(this.competition);

			hideActivityIndicator();

            if (payment == null)
			{
				createRegistrationConfirmed();
			}
			else if (payment.value == 0)
			{
				createRegistrationConfirmed();
			}
			else
			{
				createMBPaymentLayout();
			}
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
				Text = "A sua Inscrição na Competição " + competition.name + " está Confirmada. \n Boa sorte e nunca te esqueças de te divertir!",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 200,
				FontSize = 30
			};

			absoluteLayout.Add(inscricaoOKLabel);
            absoluteLayout.SetLayoutBounds(inscricaoOKLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, 200 * App.screenHeightAdapter));

			CompetitionManager competitionManager = new CompetitionManager();

			await competitionManager.Update_Competition_Participation_Status(competition.participationid, "confirmado");
			competition.participationconfirmed = "confirmado";

		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Microsoft.Maui.Controls.Grid { Padding = 10, ColumnSpacing = 20 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 160 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label competitionParticipationNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Para confirmares a sua presença no\n " + competition.name + "\n efetua o pagamento no MB com os dados apresentados em baixo:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
            };

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
                WidthRequest = 164 * App.screenHeightAdapter,
                HeightRequest = 142 * App.screenHeightAdapter
            };

			Label referenciaMBLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Pagamento por\n Multibanco",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
                //LineBreakMode = LineBreakMode.NoWrap,
                HeightRequest = 142 * App.screenHeightAdapter,
                FontSize = App.bigTitleFontSize
            };

			gridMBPayment.Add(competitionParticipationNameLabel, 0, 0);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(competitionParticipationNameLabel, 2);

			gridMBPayment.Add(MBLogoImage, 0, 2);
			gridMBPayment.Add(referenciaMBLabel, 1, 2);


            Label labelTax = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O valor total desta transação incluiu uma taxa de " + String.Format("{0:0.00}", payment.value - competition.value) + "€",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.itemTextFontSize
            };

            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMBPayment.Add(labelTax, 0, 5);
            Grid.SetColumnSpan(labelTax, 2);

            createMBGrid(payment, competition.participationcategory);

			absoluteLayout.Add(gridMBPayment);
            absoluteLayout.SetLayoutBounds(gridMBPayment, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 10 * App.screenHeightAdapter));
        
		}

		public void createMBGrid(Payment payment, string category)
		{
			Microsoft.Maui.Controls.Grid gridMBDataPayment = new Microsoft.Maui.Controls.Grid { Padding = 10 * App.screenWidthAdapter, ColumnSpacing = 5 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

			Label categoryLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Categoria:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };
			Label categoryValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = category,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.titleFontSize
            };

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
                Text = payment.entity,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };
			Label referenceValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = payment.reference,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };
			Label valueValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = String.Format("{0:0.00}", payment.value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };

			Frame MBDataFrame = new Frame { BackgroundColor = App.backgroundColor, BorderColor = App.topColor, CornerRadius = 10, IsClippedToBounds = true, Padding = 0 };
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Add(categoryLabel, 0, 0);
			gridMBDataPayment.Add(categoryValue, 1, 0);
			gridMBDataPayment.Add(entityLabel, 0, 1);
			gridMBDataPayment.Add(entityValue, 1, 1);
			gridMBDataPayment.Add(referenceLabel, 0, 2);
			gridMBDataPayment.Add(referenceValue, 1, 2);
			gridMBDataPayment.Add(valueLabel, 0, 3);
			gridMBDataPayment.Add(valueValue, 1, 3);

			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			gridMBPayment.Add(MBDataFrame, 0, 4);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(MBDataFrame, 2);
		}

		public CompetitionMBPageCS(Competition competition)
		{

			this.competition = competition;
			//App.competition_participation = competition_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async Task<Payment> GetCompetitionParticipationPayment(Competition competition)
		{
			Debug.WriteLine("GetCompetitionParticipationPayment");
			CompetitionManager competitionManager = new CompetitionManager();

			Payment payment = await competitionManager.GetCompetitionParticipation_Payment(competition);
			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor, BarTextColor = App.normalTextColor
				};
				return null;
			}
			return payment;
		}

	}
}

