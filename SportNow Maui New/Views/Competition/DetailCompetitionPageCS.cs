using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views
{
	public class DetailCompetitionPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            refreshCompetitionStatus(competition);
		}

		protected override void OnDisappearing()
		{
			CleanScreen();
		}


		FormValue estadoValue = new FormValue("");

		private Competition competition;
		private List<Competition> competitions;

		private List<Payment> payments;

		RegisterButton registerButton;
        CancelButton cancelButton;
        

        private Microsoft.Maui.Controls.Grid gridCompetiton;

		public void initLayout()
		{
			Title = competition.name;
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen AQUIIII");
			if (gridCompetiton != null)
			{
				absoluteLayout.Remove(gridCompetiton);
				gridCompetiton = null;

			}
			if (registerButton != null)
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
		
		public async void initSpecificLayout()
		{ 
            this.BackgroundColor = App.backgroundOppositeColor;

            Debug.Print("competition.imagemSource = "+ competition.imagemSource);
			int i;
			Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40};
            eventoImage.Source = competition.imagemSource;

            absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

            gridCompetiton = new Microsoft.Maui.Controls.Grid { Padding = 0, ColumnSpacing = 5 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 80 });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 120 });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 }); //GridLength.Auto
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 * 4 }); //GridLength.Auto 

            Label dateLabel = new FormLabel { Text = "DATA" };
            dateLabel.TextColor = App.oppositeTextColor;
            FormValue dateValue = new FormValue(competition.detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
            placeLabel.TextColor = App.oppositeTextColor;
            FormValue placeValue = new FormValue(competition.place);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
            typeLabel.TextColor = App.oppositeTextColor;
            FormValue typeValue = new FormValue(Constants.competition_type[competition.type]);

			FormLabel valueLabel = new FormLabel { Text = "VALOR" };
            valueLabel.TextColor = App.oppositeTextColor;
            FormValue valueValue = new FormValue(String.Format("{0:0.00}", competition.value + "€"));

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
            websiteLabel.TextColor = App.oppositeTextColor;
            //websiteLabel.MaximumWidthRequest = App.screenWidth;
            FormValue websiteValue = new FormValue(competition.website);

			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(competition.website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						Debug.Print(ex.Message);// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});

			FormLabel estadoLabel = new FormLabel { Text = "ESTADO" };
            estadoLabel.TextColor = App.oppositeTextColor;
            estadoValue = new FormValue("");

			List<Competition_Participation> competitionCall = await GetCompetitionCall();

			Debug.Print("examination_session.registrationbegindate = " + competition.registrationbegindate);
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
			string limitDateLabelText = "";

			if ((competition.registrationbegindate == "") | (competition.registrationbegindate == null))
			{
				Debug.Print("Data início de inscrições ainda não está definida");
				limitDateLabelText = "As inscrições ainda não estão abertas.";
			}
			else if ((currentTime - registrationbegindate_datetime).Days < 0)
			{
				Debug.Print("Inscrições ainda não abriram");
				limitDateLabelText = "As inscrições abrem no dia " + competition.registrationbegindate + ".";
			}
			else
			{

				Debug.Print("Inscrições já abriram " + (registrationlimitdate_datetime - currentTime).Days);
				if ((registrationlimitdate_datetime - currentTime).Days < 0)
				{
					Debug.Print("Inscrições já fecharam");
					limitDateLabelText = "Ohhh...As inscrições já terminaram.";
					registrationOpened = 0;
				}
				else
				{
					registrationOpened = 1;
					Debug.Print("Inscrições estão abertas!");
					limitDateLabelText = "As inscrições estão abertas e terminam no dia " + competition.registrationlimitdate + ".";
				}
			}

			if (competition.participationconfirmed == "confirmado")
			{
				estadoValue = new FormValue("CONVOCADO - CONFIRMADO");
				estadoValue.label.TextColor = Color.FromRgb(96, 182, 89);
				limitDateLabelText = "BOA SORTE!";
			}
			else if (competition.participationconfirmed == "convocado")
			{
				estadoValue = new FormValue("CONVOCADO - NÃO CONFIRMADO");
				estadoValue.label.TextColor = Colors.Red;

				if (registrationOpened == 1) {
                    registerButton = new RegisterButton("CONFIRMAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                    registerButton.button.Clicked += OnRegisterButtonClicked;


                    gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    gridCompetiton.Add(registerButton, 0, 9);
					Microsoft.Maui.Controls.Grid.SetColumnSpan(registerButton, 2);
                    gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });


                    cancelButton = new CancelButton("NÃO POSSO IR :(", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                    cancelButton.button.Clicked += OnCancelButtonClicked;

                    //gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    gridCompetiton.Add(cancelButton, 0, 10);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(cancelButton, 2);
                }
				
			}
            else if (competition.participationconfirmed == "cancelado")
            {
                estadoValue = new FormValue("INSCRIÇÃO CANCELADA");
                estadoValue.label.TextColor = Color.FromRgb(233, 93, 85);

                if (registrationOpened == 1)
                {
                    registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                    registerButton.button.Clicked += OnRegisterButtonClicked;


                    gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    gridCompetiton.Add(registerButton, 0, 9);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(registerButton, 2);
                }

            }
            else if (competition.participationconfirmed == null)
			{
				if ((competitionCall == null) | (competitionCall.Count == 0))
				{
					estadoValue = new FormValue("-");
				}

				else
				{
					estadoValue = new FormValue("NÃO CONVOCADO");
				}
				estadoValue.label.TextColor = App.normalTextColor;
			}


			Label limitDateLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = limitDateLabelText,
				TextColor = App.oppositeTextColor,
                WidthRequest = 300 * App.screenWidthAdapter,
                HeightRequest = 50 * App.screenHeightAdapter,
                FontSize = App.titleFontSize,
				HorizontalTextAlignment = TextAlignment.Center
			};


			if ((competitionCall == null) | (competitionCall.Count == 0) | (registrationOpened == -1)) 
			{
				Label convocatoriaLabel = new Label
				{
                    Text = "Ainda não existe Convocatória para esta Competição.",
					TextColor = App.oppositeTextColor,
					FontSize = 20,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
                    FontFamily = "futuracondensedmedium",
                };
				gridCompetiton.Add(convocatoriaLabel, 0, 7);
				Microsoft.Maui.Controls.Grid.SetColumnSpan(convocatoriaLabel, 2);
			}
			else
			{
				Image convocatoriaImage = new Image
				{
					Source = "iconconvocatoria.png",
					HorizontalOptions = LayoutOptions.Start,
                    HeightRequest = 50 * App.screenHeightAdapter,
                };

				Label convocatoriaLabel = new Label
				{
                    Text = "Convocatória",
					TextColor = App.oppositeTextColor,
                    FontSize = App.titleFontSize,
                    VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
                    FontFamily = "futuracondensedmedium",
                };

				Microsoft.Maui.Controls.StackLayout convocatoriaStackLayout = new Microsoft.Maui.Controls.StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Spacing = 20,
					Children =
				{
					convocatoriaImage,
					convocatoriaLabel
				}
				};

				gridCompetiton.Add(convocatoriaStackLayout, 0, 7);
				Microsoft.Maui.Controls.Grid.SetColumnSpan(convocatoriaStackLayout, 2);

				var convocatoriaStackLayout_tap = new TapGestureRecognizer();
				convocatoriaStackLayout_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new CompetitionCallPageCS(competitions, null));
				};
				convocatoriaStackLayout.GestureRecognizers.Add(convocatoriaStackLayout_tap);

			}



			gridCompetiton.Add(dateLabel, 0, 0);
			gridCompetiton.Add(dateValue, 1, 0);

			gridCompetiton.Add(placeLabel, 0, 1);
			gridCompetiton.Add(placeValue, 1, 1);

			gridCompetiton.Add(typeLabel, 0, 2);
			gridCompetiton.Add(typeValue, 1, 2);

			gridCompetiton.Add(valueLabel, 0, 3);
			gridCompetiton.Add(valueValue, 1, 3);

			gridCompetiton.Add(websiteLabel, 0, 4);
			gridCompetiton.Add(websiteValue, 1, 4);

			gridCompetiton.Add(estadoLabel, 0, 5);
			gridCompetiton.Add(estadoValue, 1, 5);

			gridCompetiton.Add(limitDateLabel, 0, 6);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(limitDateLabel, 2);


			/*Image competitionImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
			Debug.Print("competition image = " + competition.imagemSource);
			competitionImage.Source = competition.imagemSource;

			absoluteLayout.Add(competitionImage);
            absoluteLayout.SetLayoutBounds(competitionImage, new Rect(0, 0, App.screenWidth, App.screenHeight));*/


            gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 10 });

			absoluteLayout.Add(gridCompetiton);
            absoluteLayout.SetLayoutBounds(gridCompetiton, new Rect(0, 0, App.screenWidth, App.screenHeight - 110 * App.screenHeightAdapter));
        }



		public DetailCompetitionPageCS(Competition competition)
		{
			this.competition = competition;
            Debug.Print("DetailCompetitionPageCS competition with id = " + competition.id);
            //Debug.Print("AQUI 2 competition ImageSource = " + competition.imagemSource);
            this.initLayout();
			//this.initSpecificLayout();
		}

		public DetailCompetitionPageCS(string competitionid, string competitionname)
		{
			this.competition = new Competition();
            Debug.Print("DetailCompetitionPageCS competitionid = " + competitionid);
            competition.id = competitionid;
			competition.name = competitionname;
            this.initLayout();

			//this.initSpecificLayout();
		}

		public DetailCompetitionPageCS(string competitionid, string competitionname, string competitionparticipationid)
        {
            this.competition = new Competition();
            Debug.Print("DetailCompetitionPageCS competitionid = " + competitionid + " competitionparticipationid = " + competitionparticipationid);
            competition.id = competitionid;
            competition.name = competitionname;
            competition.participationid = competitionparticipationid;
            this.initLayout();
            //this.initSpecificLayout();
        }

        async void refreshCompetitionStatus(Competition competition)
		{
			if (competition != null)
			{
				Debug.Print("refreshCompetitionStatus competition.participationid=" + competition.participationid);
				CompetitionManager competitionManager = new CompetitionManager();
				if (competition.participationid != null)

				{
					competitions = await competitionManager.GetCompetitionByParticipationID(App.member.id, competition.participationid);
					this.competition = competitions[0];

				}
				else
				{
					competitions = await competitionManager.GetCompetitionByID(App.member.id, competition.id);
					this.competition = competitions[0];
				}
				
				if ((competition.imagemNome == "") | (competition.imagemNome is null))
				{
					this.competition.imagemSource = "company_logo_square.png";
				}
				else
				{
                    this.competition.imagemSource = Constants.images_URL + competition.id + "_imagem_c";
					Debug.Print("DEPOIS competition ImageSource = " + competition.imagemSource);
				}
			}
			initSpecificLayout();
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
			showActivityIndicator();

            cancelButton.IsEnabled = false;

            CompetitionManager competitionManager = new CompetitionManager();

            await competitionManager.Update_Competition_Participation_Status(competition.participationid, "cancelado");
            competition.participationconfirmed = "cancelado";
			CleanScreen();
            refreshCompetitionStatus(competition);

			hideActivityIndicator();

        }




    }
}

