using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;
using SportNow.Views.Personal;
using SportNow.Model;

namespace SportNow.Views
{
	public class AttendanceOptionsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private AbsoluteLayout presencasabsoluteLayout;

		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private OptionButton marcarAulaButton, estatisticasButton, presencasButton, mensalidadesButton, mensalidadesStudentButton;

		RoundButton personalClassesButton;

		Service service;

        public void initLayout()
		{
			Title = service.nome;
			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png",

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (presencasabsoluteLayout != null)
			{
				absoluteLayout.Remove(presencasabsoluteLayout);

                presencasabsoluteLayout = null;
			}

		}

		public async void initSpecificLayout()
		{
            showActivityIndicator();
            presencasabsoluteLayout = new AbsoluteLayout
			{
				Margin = new Thickness(0),
			};

			await CreatePresencasOptionButtonsAsync();

			absoluteLayout.Add(presencasabsoluteLayout);
            absoluteLayout.SetLayoutBounds(presencasabsoluteLayout, new Rect(0, 0 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));
			hideActivityIndicator();
		}


		public async Task CreatePresencasOptionButtonsAsync()
		{

			//showActivityIndicator();
			var width = App.screenWidth;
			var buttonWidth = (width) / 2;


			marcarAulaButton = new OptionButton("MARCAR AULAS", "confirmclasses.png", buttonWidth, 80 * App.screenHeightAdapter);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var marcarAulaButton_tap = new TapGestureRecognizer();
			marcarAulaButton_tap.Tapped += (s, e) =>
			{
				
				//Navigation.PushAsync(new AttendancePageCS());
			};
			marcarAulaButton.GestureRecognizers.Add(marcarAulaButton_tap);

			estatisticasButton = new OptionButton("ESTATÍSTICAS", "classstats.png", buttonWidth, 80 * App.screenHeightAdapter);
			var estatisticasButton_tap = new TapGestureRecognizer();
			estatisticasButton_tap.Tapped += (s, e) =>
			{
				//Navigation.PushAsync(new AttendanceStatsPageCS());
			};
			estatisticasButton.GestureRecognizers.Add(estatisticasButton_tap);

			presencasButton = new OptionButton("PRESENÇAS", "attendances.png", buttonWidth, 80 * App.screenHeightAdapter);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var presencasButton_tap = new TapGestureRecognizer();
			presencasButton_tap.Tapped += (s, e) =>
			{
				//Navigation.PushAsync(new AttendanceManagePageCS());
			};
			presencasButton.GestureRecognizers.Add(presencasButton_tap);

			mensalidadesButton = new OptionButton("MENSALIDADES INSTRUTOR", "mensalidades_alunos.png", buttonWidth, 80 * App.screenHeightAdapter);
			var mensalidadesButton_tap = new TapGestureRecognizer();
			mensalidadesButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new MonthFeeListPageCS(new Service()));
			};
			mensalidadesButton.GestureRecognizers.Add(mensalidadesButton_tap);

			mensalidadesStudentButton = new OptionButton("MENSALIDADES", "monthfees.png", buttonWidth, 80 * App.screenHeightAdapter);
			var mensalidadesStudentButton_tap = new TapGestureRecognizer();
			mensalidadesStudentButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new MonthFeeStudentListPageCS(new Service()));
			};
			mensalidadesStudentButton.GestureRecognizers.Add(mensalidadesStudentButton_tap);


			string monthFeeStudentCount = await Get_has_StudentMonthFees();


			Microsoft.Maui.Controls.StackLayout stackPresencasButtons = new StackLayout();
			if (App.member.students_count > 0)
			{
				if (monthFeeStudentCount != "0")
				{
					Debug.Print("screenHeightAdapter = " + App.screenHeightAdapter);
					stackPresencasButtons = new Microsoft.Maui.Controls.StackLayout
					{
						
						Spacing = 20 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						//VerticalAlignment = LayoutAlignment.Start,
						//HorizontalOptions = LayoutOptions.FillAndExpand,
						//VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 480 * App.screenHeightAdapter,
						MaximumHeightRequest = 480 * App.screenHeightAdapter,
                        Children =
							{
                                presencasButton,
								marcarAulaButton,
								estatisticasButton,
								mensalidadesButton,
                                mensalidadesStudentButton
							}
					};
					//stackPresencasButtons.Add(presencasButton);

                }
				else
				{

					stackPresencasButtons = new Microsoft.Maui.Controls.StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 20 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 380 * App.screenHeightAdapter,
						Children =
							{
								presencasButton,
								marcarAulaButton,
								estatisticasButton,
								mensalidadesButton
							}
					};
				}

			}
			else
			{
				if (monthFeeStudentCount != "0")
				{
					stackPresencasButtons = new Microsoft.Maui.Controls.StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 50 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						//HeightRequest = 400 * App.screenHeightAdapter,
						Children =
						{
							marcarAulaButton,
							estatisticasButton,
							mensalidadesStudentButton
						}
					};
				}
				else
				{
					stackPresencasButtons = new Microsoft.Maui.Controls.StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 50 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						//HeightRequest = 400 * App.screenHeightAdapter,
						Children =
						{
							marcarAulaButton,
							estatisticasButton,
						}
					};
				}
			}

            presencasabsoluteLayout.Add(stackPresencasButtons);
            presencasabsoluteLayout.SetLayoutBounds(stackPresencasButtons, new Rect(App.screenWidth / 4, 10 * App.screenHeightAdapter, App.screenWidth / 2, App.screenHeight - 300 * App.screenHeightAdapter));


            Label personalClassesLabel = new Label
            {
				
                Text = "Agora já podes agendar aulas pessoais com os teus treinadores preferidos!",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = App.itemTitleFontSize,
                FontFamily = "futuracondensedmedium",				
            };
            presencasabsoluteLayout.Add(personalClassesLabel);
            presencasabsoluteLayout.SetLayoutBounds(personalClassesLabel, new Rect(0, App.screenHeight - 275 * App.screenHeightAdapter, App.screenWidth, 40 * App.screenHeightAdapter));

            personalClassesButton = new RoundButton("SABER MAIS!", App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            personalClassesButton.button.BackgroundColor = App.topColor;
            personalClassesButton.button.Clicked += OnPersonalClassesButtonClicked;

            presencasabsoluteLayout.Add(personalClassesButton);
            presencasabsoluteLayout.SetLayoutBounds(personalClassesButton, new Rect(5 * App.screenWidthAdapter, App.screenHeight - 230 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			/*BoxView separator1 = new BoxView()
            {
                HeightRequest = 10,
                BackgroundColor = Color.FromRgb(255, 0, 0),
                Color = Color.FromRgb(0, 240, 0),
            };*/

			//absoluteLayout.Add(separator1);
			//absoluteLayout.SetLayoutBounds(separator1, new Rect(0, App.screenHeight - 165 * App.screenHeightAdapter, App.screenWidth, 10 * App.screenHeightAdapter));

			//hideActivityIndicator();
        }

        public AttendanceOptionsPageCS(Service service)
		{
			this.service = service;
            this.initLayout();
		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async Task<string> Get_has_StudentMonthFees()
		{
			Debug.WriteLine("GetStudentClass_Schedules");
			MonthFeeManager monthFeeManager = new MonthFeeManager();
			string count = await monthFeeManager.Has_MonthFeesStudent(App.member.id, "");
			if (count == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return count;
		}

        async void OnPersonalClassesButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MainPageCS.OnPersonalClassesButtonClicked");
            await Navigation.PushAsync(new PersonalInfoPageCS());
        }

    }
}
