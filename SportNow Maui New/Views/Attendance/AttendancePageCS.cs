using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class AttendancePageCS : DefaultPage
	{
		private string week = "current";

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private AbsoluteLayout presencasrelativeLayout;


		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private MenuButton week0Button, week1Button, week2Button, week3Button;

		private CollectionView weekClassesCollectionView;

		private List<Class_Schedule> weekClass_Schedule, cleanClass_Schedule;

		public void initLayout()
		{
			Title = "MARCAR AULAS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{
				absoluteLayout.Remove(stackButtons);
				stackButtons = null;
			}
			if (weekClassesCollectionView != null)
			{
                absoluteLayout.Remove(weekClassesCollectionView);
                weekClassesCollectionView = null;

            }

        }

		public async void initSpecificLayout()
		{
			showActivityIndicator();
			DateTime currentTime = DateTime.Now.Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);
			

			CreateStackButtons();
			CreateClassesColletion();


			if (week == "current")
			{
				OnWeek0ButtonClicked(null, null);
			}
			else if (week == "next")
			{
				OnWeek1ButtonClicked(null, null);
			}

            hideActivityIndicator();
        }

		public void CreateStackButtons()
		{
			var buttonWidth = (App.screenWidth - 15 * App.screenWidthAdapter) / 4;

			DateTime currentTime = DateTime.Now;

			Debug.Print("current Time = "+currentTime.ToString());
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

			var week0ButtonText = firstDayWeek.Day + " " + Constants.months[firstDayWeek.Month] + " - "+ firstDayWeek.AddDays(6).Day + " " + Constants.months[firstDayWeek.AddDays(6).Month];
			var week1ButtonText = firstDayWeek.AddDays(7).Day + " " + Constants.months[firstDayWeek.AddDays(7).Month] + " - " + firstDayWeek.AddDays(13).Day + " " + Constants.months[firstDayWeek.AddDays(13).Month];
			var week2ButtonText = firstDayWeek.AddDays(14).Day + " " + Constants.months[firstDayWeek.AddDays(14).Month] + " - " + firstDayWeek.AddDays(20).Day + " " + Constants.months[firstDayWeek.AddDays(20).Month];
			var week3ButtonText = firstDayWeek.AddDays(21).Day + " " + Constants.months[firstDayWeek.AddDays(21).Month] + " - " + firstDayWeek.AddDays(27).Day + " " + Constants.months[firstDayWeek.AddDays(27).Month];

			week0Button = new MenuButton(week0ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week0Button.button.Clicked += OnWeek0ButtonClicked;

			week1Button = new MenuButton(week1ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week1Button.button.Clicked += OnWeek1ButtonClicked;

			week2Button = new MenuButton(week2ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week2Button.button.Clicked += OnWeek2ButtonClicked;

			week3Button = new MenuButton(week3ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week3Button.button.Clicked += OnWeek3ButtonClicked;

			stackButtons = new Microsoft.Maui.Controls.StackLayout
			{
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40 * App.screenHeightAdapter,
				Children =
				{
					week0Button,
					week1Button,
					week2Button,
					week3Button
				}
			};

			absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));
        

		}


		public async Task<int> getClass_DetailData(DateTime startDate)
		{

			weekClass_Schedule = await GetStudentClass_Schedules(startDate.ToString("yyyy-MM-dd"), startDate.AddDays(6).ToString("yyyy-MM-dd"));//  new List<Class_Schedule>();
			cleanClass_Schedule = new List<Class_Schedule>();

			CompleteClass_Schedules();

			return 1;
		}


		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in weekClass_Schedule)
			{
				if (class_schedule.classattendancestatus == "confirmada")
				{
					class_schedule.participationimage = "iconcheck.png";
				}
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;
                if ((class_schedule.imagesource == "") | (class_schedule.imagesource is null))
                {
					class_schedule.imagesourceObject = "company_logo_square.png";
				}
				else
				{
					class_schedule.imagesourceObject = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + class_schedule.classid + "_imagem_c"),
						CachingEnabled = false,
						CacheValidity = new TimeSpan(0, 0, 0, 0)
					};
				}


				if ((class_schedule.classattendancestatus == "confirmada") | (class_schedule.classattendancestatus == "fechada"))
				{
					class_schedule.participationimage = "iconcheck.png";
				}
				else
				{
					class_schedule.participationimage = "iconinativo.png";
				}

			}

		}



		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			weekClassesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = weekClass_Schedule,
                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 10 * App.screenWidthAdapter, },
                EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label {FontFamily = "futuracondensedmedium", Text = "Não existem Aulas agendadas nesta semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			weekClassesCollectionView.SelectionChanged += OnClassScheduleCollectionViewSelectionChanged;

			weekClassesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth
                };

                Border itemFrame = new Border
				{
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
                    Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");
				
				itemFrame.Content = eventoImage;

                /*var itemFrame_tap = new TapGestureRecognizer();
                itemFrame_tap.Tapped += (s, e) =>
                {
                    Navigation.PushAsync(new EquipamentsPageCS("protecoescintos"));
                };
                itemFrame.GestureRecognizers.Add(itemFrame_tap);*/

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label dateLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = App.oppositeTextColor };
                dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(3 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - 6 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

                Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20 * App.screenWidthAdapter, TextColor = App.oppositeTextColor };
                nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - 6 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect(App.ItemWidth - (25 * App.screenHeightAdapter), 5 * App.screenHeightAdapter, 20 * App.screenHeightAdapter, 20 * App.screenHeightAdapter));

                //itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect(App.ItemWidth - (25 * App.screenWidthAdapter), 3 * App.screenHeightAdapter, 20 * App.screenHeightAdapter, 20 * App.screenHeightAdapter));

                return itemabsoluteLayout;
			});
			absoluteLayout.Add(weekClassesCollectionView);
            absoluteLayout.SetLayoutBounds(weekClassesCollectionView, new Rect(0, 70 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - (170 * App.screenHeightAdapter)));
		}


		public AttendancePageCS()
		{
			this.initLayout();
		}


		public AttendancePageCS(string week)
		{
			this.week = week;
			this.initLayout();
		}



		async void OnWeek0ButtonClicked(object sender, EventArgs e)
		{
			week0Button.activate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
			//_collection.Items = week0Class_Detail;
		}

		async void OnWeek1ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.activate();
			week2Button.deactivate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.AddDays(7).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async void OnWeek2ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.deactivate();
			week2Button.activate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.AddDays(14).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async void OnWeek3ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.activate();

			DateTime currentTime = DateTime.Now.AddDays(21).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);


			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async Task<List<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
		{
			Debug.WriteLine("GetStudentClass_Schedules");
			showActivityIndicator();
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules(App.member.id, begindate, enddate);
            hideActivityIndicator();
            if (class_schedules_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Colors.White,
					BarTextColor = Colors.Black
				};
				return null;
			}
			
			return class_schedules_i;
		}


		async void OnClassScheduleCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("MainPageCS.OnClassScheduleCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItems.Count != 0)
			{
				ClassManager classmanager = new ClassManager();

				Class_Schedule class_schedule = (sender as CollectionView).SelectedItems[0] as Class_Schedule;
				if (class_schedule.classattendanceid == null)
				{
                    Task.Run(async () =>
                    {
                        string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                        class_schedule.classattendanceid = class_attendance_id;
                        return true;
                    });
                    
					class_schedule.classattendancestatus = "confirmada";
					class_schedule.participationimage = "iconcheck.png";
				}
				else
				{
					if (class_schedule.classattendancestatus == "anulada")
					{
						class_schedule.classattendancestatus = "confirmada";
						class_schedule.participationimage = "iconcheck.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "confirmada")
					{
						class_schedule.classattendancestatus = "anulada";
						class_schedule.participationimage = "iconinativo.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "fechada")

					{
                        await Navigation.PushAsync(new AttendanteEvaluationPageCS(class_schedule.classattendanceid));
                        //await DisplayAlert("PRESENÇA EM AULA", "A tua presença nesta aula já foi validada pelo treinador pelo que não é possível alterar o seu estado.", "OK");
					}
					
				}

				((CollectionView)sender).SelectedItems.Clear();
				weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
				weekClassesCollectionView.ItemsSource = weekClass_Schedule;

                hideActivityIndicator();
            }
		}



	}
}
