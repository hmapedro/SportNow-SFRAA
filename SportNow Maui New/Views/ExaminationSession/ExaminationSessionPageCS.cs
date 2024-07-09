using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui;
using System.Globalization;


namespace SportNow.Views
{
	public class ExaminationSessionPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            refreshExaminationStatus(examination_session.id);
		}

		protected override void OnDisappearing()
		{
			CleanScreen();
		}



		FormValue estadoValue = new FormValue("");

		private Examination_Session examination_session;

		private List<Payment> payments;

		RegisterButton registerButton;
        CancelButton cancelButton;

        private Microsoft.Maui.Controls.Grid gridCompetiton;

		public void initLayout()
		{
			Title = examination_session.name;
		}


		public void CleanScreen()
		{
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

            Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
            eventoImage.Source = examination_session.imagemSource;

			Debug.Print("examination_session.imagemSource = " + examination_session.imagemSource);

            absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

            gridCompetiton = new Microsoft.Maui.Controls.Grid { Padding = 0, ColumnSpacing = 10 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter});
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 120 * App.screenHeightAdapter});
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = (App.screenWidth - 10 * App.screenWidthAdapter) / 5 }); //GridLength.Auto
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = (App.screenWidth - 10 * App.screenWidthAdapter) / 5 * 4}); //GridLength.Auto 

            FormLabel dateLabel = new FormLabel { Text = "DATA" };
			dateLabel.TextColor = App.oppositeTextColor;
			FormValue dateValue = new FormValue(examination_session.date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
            placeLabel.TextColor = App.oppositeTextColor;
            FormValue placeValue = new FormValue(examination_session.place);

			FormLabel gradeLabel = new FormLabel { Text = "EXAME PARA" };
            gradeLabel.TextColor = App.oppositeTextColor;
            FormValue gradeValue;
			if (examination_session.participationgrade != null)
			{
				gradeValue = new FormValue(Constants.grades[examination_session.participationgrade]);
			}
			else
			{
				gradeValue = new FormValue("-");
			}


			FormLabel valueLabel = new FormLabel { Text = "VALOR" };
            valueLabel.TextColor = App.oppositeTextColor;

            FormValue valueValue;
			if (examination_session.participationvalue != null)
			{
				Debug.Print("examination_session.participationvalue = " + examination_session.participationvalue+".");
				double participationvalue_double = Double.Parse(examination_session.participationvalue.Replace(',', '.'), CultureInfo.InvariantCulture);
				valueValue = new FormValue(String.Format("{0:0.00}", participationvalue_double + "€"));
			}
			else
			{
				valueValue = new FormValue("-");
			}


			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
            websiteLabel.TextColor = App.oppositeTextColor;
            FormValue websiteValue = new FormValue(examination_session.website);

			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(examination_session.website, BrowserLaunchMode.SystemPreferred);
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

			List<Examination> examination_sessionCall = await GetExamination_SessionCall();

			DateTime currentTime = DateTime.Now.Date;

			DateTime registrationbegindate_datetime = new DateTime();
			DateTime registrationlimitdate_datetime = new DateTime();

			Debug.Print("examination_session.registrationbegindate = " + examination_session.registrationbegindate);

			if ((examination_session.registrationbegindate != "") & (examination_session.registrationbegindate != null))
			{
				registrationbegindate_datetime = DateTime.Parse(examination_session.registrationbegindate).Date;
			}
			if ((examination_session.registrationlimitdate != "") & (examination_session.registrationlimitdate != null))
			{
				registrationlimitdate_datetime = DateTime.Parse(examination_session.registrationlimitdate).Date;
			}

			bool registrationOpened = false;
			string limitDateLabelText = "";

			if (examination_session.registrationbegindate == "")
			{
				Debug.Print("Data início de inscrições ainda não está definida");
				limitDateLabelText = "As inscrições ainda não estão abertas.";
			}
			else if ((currentTime - registrationbegindate_datetime).Days < 0)
			{
				Debug.Print("Inscrições ainda não abriram");
				limitDateLabelText = "As inscrições abrem no dia " + examination_session.registrationbegindate + ".";
			}
			else
			{
				
				Debug.Print("Inscrições já abriram " + (registrationlimitdate_datetime - currentTime).Days);
				if ((registrationlimitdate_datetime - currentTime).Days < 0)
				{
					Debug.Print("Inscrições já fecharam");
					limitDateLabelText = "Ohhh...As inscrições já terminaram.";
				}
				else
				{
					registrationOpened = true;
					Debug.Print("Inscrições estão abertas!");
					limitDateLabelText = "As inscrições estão abertas e terminam no dia " + examination_session.registrationlimitdate + ". Contamos contigo!";
				}
			}

			Debug.Print("examination_session.participationconfirmed = " + examination_session.participationconfirmed);

			bool hasCall = false;

			if (examination_sessionCall != null)
			{
				Debug.Print("examination_sessionCall is not null");
				if (examination_sessionCall.Count != 0)
				{
					Debug.Print("examination_sessionCall count is not 0 - " + examination_sessionCall.Count);
					hasCall = true;
				}
				else {
					Debug.Print("examination_sessionCall count is 0");
				}
			}
			else
			{
				Debug.Print("examination_sessionCall is null");
			}

			//registerButton = new RegisterButton();


			if (examination_session.participationconfirmed == "confirmado")
			{
				estadoValue = new FormValue("INSCRITO");
				estadoValue.label.TextColor = Color.FromRgb(96, 182, 89);
				limitDateLabelText = "BOA SORTE!";
			}
			else if (examination_session.participationconfirmed == "convocado")
			{
				estadoValue = new FormValue("NÃO INSCRITO");
				estadoValue.label.TextColor = Colors.Red;

				if (registrationOpened == true)
				{

                    registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
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
            else if (examination_session.participationconfirmed == "cancelado")
            {
                estadoValue = new FormValue("INSCRIÇÃO CANCELADA");
                estadoValue.label.TextColor = Color.FromRgb(233, 93, 85);

                if (registrationOpened == true)
                {

                    registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                    registerButton.button.Clicked += OnRegisterButtonClicked;


                    gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    gridCompetiton.Add(registerButton, 0, 9);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(registerButton, 2);
				}
            }
            else if (examination_session.participationconfirmed == null)
			{
				if (hasCall == false)
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

			if (hasCall == false)
            {
				Label convocatoriaLabel = new Label
				{
                    FontFamily = "futuracondensedmedium",
                    Text = "Ainda não existe Convocatória para esta Sessão de Exames.",
                    TextColor = App.oppositeTextColor,
                    FontSize = 20 * App.screenHeightAdapter,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center
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
                    FontFamily = "futuracondensedmedium",
                    Text = "Convocatória",
					TextColor = App.oppositeTextColor,
					FontSize = App.titleFontSize,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start
				};

				Microsoft.Maui.Controls.StackLayout convocatoriaStackLayout = new Microsoft.Maui.Controls.StackLayout
				{
					//BackgroundColor = Colors.Green,
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Spacing = 20 * App.screenHeightAdapter,
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
					Navigation.PushAsync(new ExaminationSessionCallPageCS(examination_session));
				};
				convocatoriaStackLayout.GestureRecognizers.Add(convocatoriaStackLayout_tap);
			}
			
			gridCompetiton.Add(dateLabel, 0, 0);
			gridCompetiton.Add(dateValue, 1, 0);

			gridCompetiton.Add(placeLabel, 0, 1);
			gridCompetiton.Add(placeValue, 1, 1);

			gridCompetiton.Add(websiteLabel, 0, 2);
			gridCompetiton.Add(websiteValue, 1, 2);

			gridCompetiton.Add(gradeLabel, 0, 3);
			gridCompetiton.Add(gradeValue, 1, 3);

			gridCompetiton.Add(valueLabel, 0, 4);
			gridCompetiton.Add(valueValue, 1, 4);

			gridCompetiton.Add(estadoLabel, 0, 5);
			gridCompetiton.Add(estadoValue, 1, 5);

			gridCompetiton.Add(limitDateLabel, 0, 6);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(limitDateLabel, 2);

		    /*if (App.member.isExaminador == "1")
			{
				Image examinadorImage = new Image
				{
					Source = "iconexames.png",
					HorizontalOptions = LayoutOptions.Start,
					HeightRequest = 60 * App.screenHeightAdapter,
				};

				Label examinadorLabel = new Label
				{
					Text = "Examinador",
					TextColor = App.normalTextColor,
					FontSize = App.titleFontSize,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start
				};

				Microsoft.Maui.Controls.StackLayout examinadorStackLayout = new Microsoft.Maui.Controls.StackLayout
				{
					//BackgroundColor = Colors.Green,
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Spacing = 20 * App.screenHeightAdapter,
					Children =
					{
						examinadorImage,
						examinadorLabel
					}
				};
				gridCompetiton.Add(examinadorStackLayout, 0, 8);
				Microsoft.Maui.Controls.Grid.SetColumnSpan(examinadorStackLayout, 2);

				var examinadorStackLayout_tap = new TapGestureRecognizer();
				examinadorStackLayout_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new ExaminationEvaluationCallPageCS(examination_session));
				};
				examinadorStackLayout.GestureRecognizers.Add(examinadorStackLayout_tap);
			}*/


			absoluteLayout.Add(gridCompetiton);
            absoluteLayout.SetLayoutBounds(gridCompetiton, new Rect(0, 0, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 115 * App.screenHeightAdapter));

		}



		public ExaminationSessionPageCS(Examination_Session examination_session)
		{
			this.examination_session = examination_session;
			Debug.Print("AQUI 0 examination_session.participationid = " + examination_session.participationid);
			this.initLayout();
			//this.initSpecificLayout();
		}

		public ExaminationSessionPageCS(string examination_session_id)
		{
			this.examination_session = new Examination_Session();
			examination_session.id = examination_session_id;
			//refreshExaminationStatus(examination_session_id);
			this.initLayout();
			//this.initSpecificLayout();
		}

		async void refreshExaminationStatus(string examination_session_id)
		{
			Debug.Print("AQUI 1 examination_session_id = " + examination_session_id);
			if (examination_session_id != null)
			{
				ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();
				examination_session = await examination_sessionManager.GetExamination_SessionByID(App.member.id, examination_session_id);

                if ((examination_session.imagemNome == "") | (examination_session.imagemNome is null))
                {
                    examination_session.imagemSource = "company_logo_square.png";
                }
                else
                {
                    examination_session.imagemSource = Constants.images_URL + examination_session.id + "_imagem_c";
                }

            }
			initSpecificLayout();
		}


        async Task<Examination_Session> refreshExaminationStatus_async(string examination_session_id)
        {
            Debug.Print("AQUI 1 examination_session_id = " + examination_session_id);
            if (examination_session_id != null)
            {
                ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();
                examination_session = await examination_sessionManager.GetExamination_SessionByID(App.member.id, examination_session_id);

                if ((examination_session.imagemNome == "") | (examination_session.imagemNome is null))
                {
                    examination_session.imagemSource = "company_logo_square.png";
                }
                else
                {
                    examination_session.imagemSource = Constants.images_URL + examination_session.id + "_imagem_c";
                }

            }
            return examination_session;
        }

        async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("OnRegisterButtonClicked");
			registerButton.IsEnabled = false;
			Debug.Print("examination_session.participationconfirmed" + examination_session.participationconfirmed);

            bool hasQuotaPayed = false;

            if (App.member.currentFee != null)
            {
                if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
                {
                    hasQuotaPayed = true;
                }
            }

			if ((hasQuotaPayed == true) & ((examination_session.participationconfirmed == "convocado") | (examination_session.participationconfirmed == "cancelado")))
			{
				Debug.Print("AQUI examination_session.participationid" + examination_session.participationid);
				await Navigation.PushAsync(new ExaminationSessionPaymentPageCS(examination_session));
			}
			else
			{
                bool answer = await DisplayAlert("Inscrição Exame", "Para poderes efetuar a sua inscrição na Sessão de Exames tem de ter a Quota Associativa em dia.", "Pagar Quota", "Cancelar");
				if (answer == true)
				{
                    await Navigation.PushAsync(new QuotasPageCS());
                }
            }
		}

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
			showActivityIndicator();
            cancelButton.IsEnabled = false;

			examination_session = await refreshExaminationStatus_async(examination_session.id);

			PaymentManager paymentManager = new PaymentManager();
			string result = await paymentManager.Update_Payment_Status(examination_session.paymentid, "anulado");

            if ((result == "-1") | (result == "-2"))
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
				hideActivityIndicator();
                
            }
			hideActivityIndicator();
            CleanScreen();
            refreshExaminationStatus(examination_session.id);

            /*CompetitionManager competitionManager = new CompetitionManager();

            await competitionManager.Update_Competition_Participation_Status(competition.participationid, "cancelado");
            competition.participationconfirmed = "cancelado";
            CleanScreen();
            refreshCompetitionStatus(competition);*/

        }

        async Task<List<Examination>> GetExamination_SessionCall()
		{
			Debug.WriteLine("AQUI GetExamination_SessionCall");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Examination> examination_sessionCall_i = await examination_sessionManager.GetExamination_SessionCall(examination_session.id);
			if (examination_sessionCall_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return examination_sessionCall_i;
		}


	}
}

