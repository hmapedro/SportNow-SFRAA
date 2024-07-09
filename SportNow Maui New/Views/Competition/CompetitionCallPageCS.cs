using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class CompetitionCallPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
			//competition_participation = App.competition_participation;
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			//absoluteLayout = null;
			if (collectionViewCompetitionCall != null)
			{
				absoluteLayout.Remove(collectionViewCompetitionCall);
				collectionViewCompetitionCall = null;
			}

			if (registerButton!= null)
			{
				absoluteLayout.Remove(registerButton);
                registerButton = null;
			}

            if (cancelButton != null)
            {
                absoluteLayout.Remove(cancelButton);
                cancelButton = null;
            }

        }

		private CollectionView collectionViewCompetitionCall;

		private Competition competition;
		private List<Competition> competitions;

		private Competition_Participation competition_participation; 

		private List<Competition_Participation> competitionCall;

		RegisterButton registerButton;
		CancelButton cancelButton;

        Label competitionNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "CONVOCATÓRIA";
		}


		public async void initSpecificLayout()
		{
			competitionCall = await GetCompetitionCall();

			CreateCompetitionCallColletionView();

		}

		
		public void CreateCompetitionCallColletionView()
		{

			foreach (Competition_Participation competition_participation in competitionCall)
			{
				Debug.Print("competition_participation name = " + competition_participation.name);

				if (competition_participation.estado == "confirmado")
				{
					competition_participation.estadoTextColor = Colors.Green;
				}
				if (competition_participation.estado == "cancelado")
				{
					competition_participation.estadoTextColor = Colors.Red;
				}
			}

			competitionNameLabel = new Label
			{
				Text = competition.name,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.normalTextColor,
                FontFamily = "futuracondensedmedium",
            };

			absoluteLayout.Add(competitionNameLabel);
            absoluteLayout.SetLayoutBounds(competitionNameLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			if (competitionCall.Count > 0)
            {
				nameTitleLabel = new Label
				{
					Text = "NOME",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = App.bigTitleFontSize,
					TextColor = App.topColor,
					LineBreakMode = LineBreakMode.WordWrap,
                    FontFamily = "futuracondensedmedium",
                };

				absoluteLayout.Add(nameTitleLabel);
	            absoluteLayout.SetLayoutBounds(nameTitleLabel, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth / 3 * 2 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				categoryTitleLabel = new Label
				{
					Text = "CATEGORIA",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
                    FontSize = App.bigTitleFontSize,
                    TextColor = App.topColor,
					LineBreakMode = LineBreakMode.WordWrap,
                    FontFamily = "futuracondensedmedium",
                };

				absoluteLayout.Add(categoryTitleLabel);
                absoluteLayout.SetLayoutBounds(categoryTitleLabel, new Rect((App.screenWidth / 3 * 2), 50 * App.screenHeightAdapter, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			}

			collectionViewCompetitionCall = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = competitionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Ainda não foi criada convocatória para esta competição.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Colors.Red, FontSize = 20 },
							}
					}
				}
			};

			//collectionViewCompetitionCall.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;

			collectionViewCompetitionCall.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "membername");
				nameLabel.SetBinding(Label.TextColorProperty, "estadoTextColor");

				Border nameFrame = new Border
				{
					BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
					Stroke = App.topColor,
					Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemabsoluteLayout.Add(nameFrame);
	            itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 0, App.screenWidth / 3 * 2 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));


				Label categoryLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "categoria");

                Border categoryFrame = new Border
				{
					BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    Padding = new Thickness(5, 0, 0, 0)
				};
				categoryFrame.Content = categoryLabel;

				itemabsoluteLayout.Add(categoryFrame);
				itemabsoluteLayout.SetLayoutBounds(categoryFrame, new Rect(App.screenWidth / 3 * 2, 0, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});


            DateTime currentTime = DateTime.Now.Date;
            DateTime registrationbegindate_datetime = new DateTime();
            DateTime registrationlimitdate_datetime = new DateTime();


            if ((competition.registrationbegindate != "") & (competition.registrationbegindate != null))
            {
                registrationbegindate_datetime = DateTime.Parse(competition.registrationbegindate).Date;
            }
            if ((competition.registrationlimitdate != "") & (competition.registrationlimitdate != null))
            {
                registrationlimitdate_datetime = DateTime.Parse(competition.registrationlimitdate).Date;
            }
            Debug.Print("event_v.registrationbegindate = " + competition.registrationbegindate + " " + registrationbegindate_datetime);
            Debug.Print("event_v.registrationlimitdate = " + competition.registrationlimitdate + " " + registrationlimitdate_datetime);

            int registrationOpened = -1;

            if ((competition.registrationbegindate == "") | (competition.registrationbegindate == null))
            {
                Debug.Print("Data início de inscrições ainda não está definida");
            }
            else if ((currentTime - registrationbegindate_datetime).Days < 0)
            {
                Debug.Print("Inscrições ainda não abriram");
            }
            else
            {

                Debug.Print("Inscrições já abriram " + (registrationlimitdate_datetime - currentTime).Days);
                if ((registrationlimitdate_datetime - currentTime).Days < 0)
                {
                    Debug.Print("Inscrições já fecharam");
                    registrationOpened = 0;
                }
                else
                {
                    Debug.Print("Inscrições estão abertas!");
                    registrationOpened = 1;
                }
            }


			if ((competition.participationconfirmed == "confirmado") | (competition.participationconfirmed == null))
			{
				absoluteLayout.Add(collectionViewCompetitionCall);
                absoluteLayout.SetLayoutBounds(collectionViewCompetitionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));
			}
			else if ((competition.participationconfirmed == "convocado") & (registrationOpened == 0))
			{
				absoluteLayout.Add(collectionViewCompetitionCall);
                absoluteLayout.SetLayoutBounds(collectionViewCompetitionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));
			}
			else if ((competition.participationconfirmed == "convocado") & (registrationOpened == 1))
			{
				absoluteLayout.Add(collectionViewCompetitionCall);
                absoluteLayout.SetLayoutBounds(collectionViewCompetitionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));

				registerButton = new RegisterButton("CONFIRMAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
				registerButton.button.Clicked += OnRegisterButtonClicked;


				absoluteLayout.Add(registerButton);
                absoluteLayout.SetLayoutBounds(registerButton, new Rect(0, App.screenHeight - 215 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

				cancelButton = new CancelButton("NÃO POSSO IR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
				cancelButton.button.Clicked += OnCancelButtonClicked;

				absoluteLayout.Add(cancelButton);
                absoluteLayout.SetLayoutBounds(cancelButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

			}
			else if ((competition.participationconfirmed == "cancelado") & (registrationOpened == 0))
			{
				absoluteLayout.Add(collectionViewCompetitionCall);
                absoluteLayout.SetLayoutBounds(collectionViewCompetitionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));
			}
			else if ((competition.participationconfirmed == "cancelado") & (registrationOpened == 1))
			{
				absoluteLayout.Add(collectionViewCompetitionCall);
				absoluteLayout.SetLayoutBounds(collectionViewCompetitionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 160 * App.screenHeightAdapter));

				registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
				registerButton.button.Clicked += OnRegisterButtonClicked;


				absoluteLayout.Add(registerButton);
                absoluteLayout.SetLayoutBounds(registerButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
			}

        }

		public CompetitionCallPageCS(List<Competition> competitions, Competition_Participation competition_participation)
		{
			this.competitions = competitions;
			this.competition = competitions[0];
			this.competition_participation = competition_participation;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Competition_Participation>> GetCompetitionCall()
		{
			Debug.WriteLine("GetCompetitionCall");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> futureCompetitionParticipations = await competitionManager.GetCompetitionCall(competition.id);
			if (futureCompetitionParticipations == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return futureCompetitionParticipations;
		}

		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
            registerButton.IsEnabled = false;

            bool hasQuotaPayed = false;

            if (App.member.currentFee != null)
            {
                if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
                {
                    hasQuotaPayed = true;
                }
            }

            if (hasQuotaPayed == true)
            {
                if ((competition.participationconfirmed == "convocado") | (competition.participationconfirmed == "cancelado"))
                {
                    await Navigation.PushAsync(new CompetitionPaymentPageCS(competition));
                }
            }
            else
            {
                bool answer = await DisplayAlert("Inscrição Competição", "Para poder efetuar a sua inscrição na Competição tem de ter a Quota Associativa em dia.", "Pagar Quota", "Cancelar");
                if (answer == true)
                {
                    await Navigation.PushAsync(new QuotasPageCS());
                }
                else
                {
                    registerButton.IsEnabled = true;
                }
            }
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            cancelButton.IsEnabled = false;

            CompetitionManager competitionManager = new CompetitionManager();

            await competitionManager.Update_Competition_Participation_Status(competition.participationid, "cancelado");
            competition.participationconfirmed = "cancelado";
			await Navigation.PopAsync();
        }

    }
}
