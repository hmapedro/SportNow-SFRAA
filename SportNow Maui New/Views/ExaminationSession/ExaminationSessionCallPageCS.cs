using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class ExaminationSessionCallPageCS : DefaultPage
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
			collectionViewExaminationSessionCall = null;	
			registerButton = null;
		}

		private CollectionView collectionViewExaminationSessionCall;

		private Examination_Session examination_session;

		private List<Examination> examination_sessionCall;

        RegisterButton registerButton;
        CancelButton cancelButton;

        Label examinationSessionNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "CONVOCATÓRIA";
		}

        public async void initSpecificLayout()
		{
			examination_sessionCall = await GetExamination_SessionCall();

			CreateExamination_SessionCallColletionView();

		}


		public void CreateExamination_SessionCallColletionView()
		{

			foreach (Examination examination in examination_sessionCall)
			{
				Debug.Print("examination.estado=" + examination.estado);
				if (examination.estado == "confirmado")
				{
					examination.estadoTextColor = Colors.Green;
				}
                if (examination.estado == "cancelado")
                {
                    examination.estadoTextColor = Colors.Red;
                }
                examination.gradeLabel = Constants.grades[examination.grade];
			}

			examinationSessionNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = examination_session.name,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.bigTitleFontSize,
				TextColor = App.normalTextColor
			};

			absoluteLayout.Add(examinationSessionNameLabel);
            absoluteLayout.SetLayoutBounds(examinationSessionNameLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			if (examination_sessionCall.Count > 0)
			{
				nameTitleLabel = new Label
				{
                    FontFamily = "futuracondensedmedium",
                    Text = "NOME",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(nameTitleLabel);
                absoluteLayout.SetLayoutBounds(nameTitleLabel, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth / 3 * 2 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				categoryTitleLabel = new Label
				{
                    FontFamily = "futuracondensedmedium",
                    Text = "EXAME PARA",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
                    FontSize = App.titleFontSize,
                    TextColor = App.topColor,
                    LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(categoryTitleLabel);
                absoluteLayout.SetLayoutBounds(categoryTitleLabel, new Rect(App.screenWidth / 3 * 2, 50 * App.screenHeightAdapter, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));
			}

			collectionViewExaminationSessionCall = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = examination_sessionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não foi criada convocatória para esta Sessão de Exames.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Colors.Red, FontSize = 20 },
			}
					}
				}
			};

			//collectionViewCompetitionCall.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;

			collectionViewExaminationSessionCall.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
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

				Label categoryLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "gradeLabel");

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
		        itemabsoluteLayout.SetLayoutBounds(categoryFrame, new Rect((App.screenWidth / 3 * 2), 0, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

			DateTime currentTime = DateTime.Now.Date;
			DateTime registrationbegindate_datetime = DateTime.Parse(examination_session.registrationbegindate).Date;
			DateTime registrationlimitdate_datetime = DateTime.Parse(examination_session.registrationlimitdate).Date;
			Debug.Print("event_v.registrationbegindate = " + examination_session.registrationbegindate + " " + registrationbegindate_datetime);
			Debug.Print("event_v.registrationlimitdate = " + examination_session.registrationlimitdate + " " + registrationlimitdate_datetime);
			Debug.Print("registrationlimitdate_datetime - currentTime).Days = " + (registrationlimitdate_datetime - currentTime).Days);
			bool registrationOpened = false;

			if (((currentTime - registrationbegindate_datetime).Days >= 0) & ((registrationlimitdate_datetime - currentTime).Days >= 0))
			{
				registrationOpened = true;
			}

			//Já inscrito ou não convocado
			if ((examination_session.participationconfirmed == "confirmado") | (examination_session.participationconfirmed == null) | (registrationOpened == false))
			{
				absoluteLayout.Add(collectionViewExaminationSessionCall);
                absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));
			}
			else if (examination_session.participationconfirmed == "convocado")
			{

				absoluteLayout.Add(collectionViewExaminationSessionCall);
                absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 310 * App.screenHeightAdapter));


				registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                registerButton.button.Clicked += OnRegisterButtonClicked;

				absoluteLayout.Add(registerButton);
                absoluteLayout.SetLayoutBounds(registerButton, new Rect(0, App.screenHeight - 212 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

				cancelButton = new CancelButton("NÃO POSSO IR :(", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                cancelButton.button.Clicked += OnCancelButtonClicked;

				absoluteLayout.Add(cancelButton);
                absoluteLayout.SetLayoutBounds(cancelButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
			}
			else if (examination_session.participationconfirmed == "cancelado")
			{
				absoluteLayout.Add(collectionViewExaminationSessionCall);
                absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 170 * App.screenHeightAdapter));

				registerButton = new RegisterButton("INSCREVER", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
                registerButton.button.Clicked += OnRegisterButtonClicked;

				absoluteLayout.Add(registerButton);
                absoluteLayout.SetLayoutBounds(registerButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
			}

		}

		public ExaminationSessionCallPageCS(Examination_Session examination_session)
		{
			this.examination_session = examination_session;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Examination>> GetExamination_SessionCall()
		{
			Debug.WriteLine("AQUI 1 GetExamination_SessionCall");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Examination> examination_sessionCall = await examination_sessionManager.GetExamination_SessionCall(examination_session.id);
			if (examination_sessionCall == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return examination_sessionCall;
		}

        async Task<Examination_Session> refreshExaminationStatus_async(string examination_session_id)
        {
            Debug.Print("AQUI 1 examination_session_id = " + examination_session_id);
            if (examination_session_id != null)
            {
                ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();
                examination_session = await examination_sessionManager.GetExamination_SessionByID(App.member.id, examination_session_id);
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
            await Navigation.PopAsync();
            //CleanScreen();
            //refreshExaminationStatus(examination_session.id);

            /*CompetitionManager competitionManager = new CompetitionManager();

            await competitionManager.Update_Competition_Participation_Status(competition.participationid, "cancelado");
            competition.participationconfirmed = "cancelado";
            CleanScreen();
            refreshCompetitionStatus(competition);*/

        }
    }
}
