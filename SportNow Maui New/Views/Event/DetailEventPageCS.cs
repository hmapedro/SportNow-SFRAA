using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views
{
	public class DetailEventPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            refreshEventStatus();
			
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}


		FormValue estadoValue = new FormValue("");

		private Event event_v;

		//private Event_Participation event_participation;

		private List<Payment> payments;

        RoundButton registerButton;

		private Microsoft.Maui.Controls.Grid gridEvent;
		Image eventoImage;


        public void initLayout()
		{
			Title = event_v.name;
		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (gridEvent != null)
			{
				absoluteLayout.Remove(gridEvent);
				gridEvent = null;
			}
			if (registerButton != null)
			{
				absoluteLayout.Remove(registerButton);
				registerButton = null;
			}

            if (eventoImage != null)
            {
                absoluteLayout.Remove(eventoImage);
                eventoImage = null;
            }
        }


		public async void initSpecificLayout()
		{

            this.BackgroundColor = App.backgroundOppositeColor;

            eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
            eventoImage.Source = event_v.imagemSource;

            absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

            gridEvent = new Microsoft.Maui.Controls.Grid { Padding = 0, ColumnSpacing = 2 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 }); //GridLength.Auto
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 * 4}); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
            dateLabel.TextColor = App.oppositeTextColor;
            FormValue dateValue = new FormValue(event_v.detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
            placeLabel.TextColor = App.oppositeTextColor;
            FormValue placeValue = new FormValue(event_v.place);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
            typeLabel.TextColor = App.oppositeTextColor;
            FormValue typeValue = new FormValue(Constants.event_type[event_v.type]);

			FormLabel valueLabel = new FormLabel { Text = "VALOR" };
            valueLabel.TextColor = App.oppositeTextColor;
            FormValue valueValue = new FormValue(String.Format("{0:0.00}", event_v.value + "€"));

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
            websiteLabel.TextColor = App.oppositeTextColor;
            FormValue websiteValue = new FormValue(event_v.website);

			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(event_v.website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});

			FormLabel estadoLabel = new FormLabel { Text = "ESTADO" };
            estadoLabel.TextColor = App.oppositeTextColor;
            estadoValue = new FormValue("");

			DateTime currentTime = DateTime.Now.Date;
			DateTime registrationbegindate_datetime = new DateTime();
			DateTime registrationlimitdate_datetime = new DateTime();

			Debug.Print("event_v.registrationbegindate = " + event_v.registrationbegindate);

			if ((event_v.registrationbegindate != "") & (event_v.registrationbegindate != null))
			{	
				registrationbegindate_datetime = DateTime.Parse(event_v.registrationbegindate).Date;
			}	
			if ((event_v.registrationlimitdate != "") &(event_v.registrationlimitdate != null))
			{
				registrationlimitdate_datetime = DateTime.Parse(event_v.registrationlimitdate).Date;
			}
			

			bool registrationOpened = false;
			string limitDateLabelText = "";

            if ((event_v.registrationbegindate == "") | (event_v.registrationbegindate == null))
			{
				limitDateLabelText = "Este evento não tem inscrições ou as inscrições ainda não estão abertas.";
			}
			else if ((currentTime - registrationbegindate_datetime).Days < 0)
			{
				limitDateLabelText = "As inscrições abrem no dia " + event_v.registrationbegindate + ".";
			}
			else
			{
				if ((registrationlimitdate_datetime - currentTime).Days < 0)
				{
					limitDateLabelText = "Ohhh...As inscrições já terminaram.";
				}
				else
				{
					registrationOpened = true;
					limitDateLabelText = "As inscrições estão abertas e terminam no dia " + event_v.registrationlimitdate+". Contamos contigo!";
				}
			}


			if (event_v.participationconfirmed == "inscrito")
			{
				estadoValue = new FormValue("INSCRITO");
				estadoValue.label.TextColor = Color.FromRgb(96, 182, 89);
				limitDateLabelText = "BOA SORTE!";

			}
			else if ((event_v.participationconfirmed == null) | (event_v.participationconfirmed == "nao_inscrito"))
			{
				estadoValue = new FormValue("NÃO INSCRITO");
				estadoValue.label.TextColor = Colors.Red;

				if (registrationOpened == true)
				{

					registerButton = new RoundButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);

					registerButton.button.Clicked += OnRegisterButtonClicked;

                    absoluteLayout.Add(registerButton);
                    absoluteLayout.SetLayoutBounds(registerButton, new Rect(0 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));


                    /*gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
					gridEvent.Add(registerButton, 0, 7);
					Grid.SetColumnSpan(registerButton, 2);*/
				}
			}

			Label limitDateLabel = new Label
			{
				Text = limitDateLabelText,
				TextColor = App.oppositeTextColor,
                WidthRequest = 300 * App.screenWidthAdapter,
                HeightRequest = 50 * App.screenHeightAdapter,
                FontSize = App.titleFontSize,
                HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
                FontFamily = "futuracondensedmedium",
            };


			gridEvent.Add(dateLabel, 0, 0);
			gridEvent.Add(dateValue, 1, 0);

			gridEvent.Add(placeLabel, 0, 1);
			gridEvent.Add(placeValue, 1, 1);

			gridEvent.Add(typeLabel, 0, 2);
			gridEvent.Add(typeValue, 1, 2);

			gridEvent.Add(valueLabel, 0, 3);
			gridEvent.Add(valueValue, 1, 3);

			gridEvent.Add(websiteLabel, 0, 4);
			gridEvent.Add(websiteValue, 1, 4);

			gridEvent.Add(estadoLabel, 0, 5);
			gridEvent.Add(estadoValue, 1, 5);

			gridEvent.Add(limitDateLabel, 0, 6);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(limitDateLabel, 2);

			absoluteLayout.Add(gridEvent);
            absoluteLayout.SetLayoutBounds(gridEvent, new Rect(0 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter));

			//absoluteLayout.Add(registerButton);
            //absoluteLayout.SetLayoutBounds(registerButton, new Rect(10 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
		}

		public DetailEventPageCS(Event event_v )
		{
			App.notification = App.notification + " DetailEventPageCS ";
			this.event_v = event_v;
			this.initLayout();
		}

		async void refreshEventStatus()
		{
			if (event_v.participationid != null)
            {
				EventManager eventManager = new EventManager();
				Event_Participation event_participation = await eventManager.GetEventParticipation(event_v.participationid);
				if (event_participation == null)
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor, BarTextColor = App.normalTextColor
					};
					return;
				}
				event_v.participationconfirmed = event_participation.estado;
				//Debug.Print("refreshEventStatus event_v.participationconfirmed=" + event_v.participationconfirmed);
			}
			initSpecificLayout();
		}


		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
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
                showActivityIndicator();
                registerButton.IsEnabled = false;

                Event_Participation event_participation = new Event_Participation();

                EventManager eventManager = new EventManager();

                //O Event Participation ainda não existe, temos de criar. Caso contrário não é necessário
                if (event_v.participationconfirmed == null)
                {
                    string new_event_participationid = await eventManager.CreateEventParticipation(App.member.id, event_v.id);
                    if ((new_event_participationid != "0") & (new_event_participationid != "-1"))
                    {
                        event_participation = await eventManager.GetEventParticipation(new_event_participationid);
                    }

                    event_v.participationconfirmed = event_participation.estado;
                }
                else
                {
                    event_participation = await eventManager.GetEventParticipation(event_v.participationid);
                }

                if (event_participation != null)
                {
                    Debug.WriteLine("OnRegisterButtonClicked event_participation.name " + event_participation.name);
                    //await Navigation.PushAsync(new EventMBPageCS(event_participation));

                    if (event_participation.permite_acompanhantes == "1")
                    {
                        event_participation.numero_acompanhantes = await DisplayPromptAsync("Número de Acompanhantes", "Indica por favor o número de acompanhantes que vais levar para o(a) " + event_participation.evento_name + ". \n (atenção: coloca apenas o número de acompanhantes sem contar contigo)", "Ok", "Cancelar", initialValue: "0");
                        await eventManager.Update_Event_Participation_Numero_Acompanhantes(event_participation.id, event_participation.numero_acompanhantes);
                    }


                    await Navigation.PushAsync(new EventPaymentPageCS(event_v, event_participation));

                }

                hideActivityIndicator();
            }
            else
            {
                bool answer = await DisplayAlert("Inscrição Evento", "Para poderes efetuar a sua inscrição no Evento tem de ter a Quota Associativa em dia.", "Pagar Quota", "Cancelar");
                if (answer == true)
                {
                    await Navigation.PushAsync(new QuotasPageCS());
                }
            }

            
			//registerButton.IsEnabled = true;
		}

		async Task<List<Event_Participation>> GetFutureEventParticipations()
		{
			Debug.WriteLine("GetFutureCompetitionParticipation");
			EventManager eventManager = new EventManager();

			List<Event_Participation> futureEventParticipations = await eventManager.GetFutureEventParticipations(App.member.id);

			if (futureEventParticipations == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return futureEventParticipations;
		}




	}
}

