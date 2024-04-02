using Microsoft.Maui.Controls.Shapes;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;

namespace SportNow.Views
{
	public class AttendanceManagePageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            //initSpecificLayout();
        }

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

		private AbsoluteLayout presencasrelativeLayout;

		private Microsoft.Maui.Controls.StackLayout stackButtons, stackWeekSelector;

		private CollectionView classAttendanceCollectionView;

		private List<Class_Schedule> allClass_Schedules;
		private static List<Class_Schedule> dummyClass_Schedules = new List<Class_Schedule>();

		DateTime firstDayWeek_datetime;
		Label currentWeek;

		public void initLayout()
		{
			Title = "ESCOLHER AULAS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (classAttendanceCollectionView != null)
			{
				classAttendanceCollectionView = null;
				absoluteLayout.Remove(classAttendanceCollectionView);
			}
		}

		public async void initSpecificLayout()
		{
			App.AdaptScreen();
			showActivityIndicator();
			DateTime currentTime = DateTime.Now.Date;

			firstDayWeek_datetime = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");

			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);
			CompleteClass_Schedules();

			//CreateStackButtons();

			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];

			CreateWeekSelector(firstDayWeek_formatted, lastdayLastWeek_formatted);

			CreateClassesColletion();

			//OnWeek0ButtonClicked(null, null);

			hideActivityIndicator();
		}


		public void CreateWeekSelector(string firstDayWeek, string lastdayLastWeek)
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 3;

			DateTime currentTime = DateTime.Now;

			Button previousWeekButton = new Button();
			Button nextWeekButton = new Button();

            previousWeekButton = new Button()
            {
                FontFamily = "futuracondensedmedium",
                Text = "<",
                FontSize = App.titleFontSize,
                TextColor = App.topColor,
                BackgroundColor = App.backgroundColor,
				VerticalOptions = LayoutOptions.Center
            };

			previousWeekButton.Clicked += OnPreviousButtonClicked;

			currentWeek = new Label()
			{
                FontFamily = "futuracondensedmedium",
                Text = firstDayWeek + " - " + lastdayLastWeek,
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				WidthRequest = 150,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

            nextWeekButton = new Button()
            {
                FontFamily = "futuracondensedmedium",
                Text = ">",
                FontSize = App.titleFontSize,
                TextColor = App.topColor,
                BackgroundColor = App.backgroundColor,
                VerticalOptions = LayoutOptions.Center
            };

			nextWeekButton.Clicked += OnNextButtonClicked;

			stackWeekSelector = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5 * App.screenHeightAdapter,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40 * App.screenHeightAdapter,
				Children =
				{
					previousWeekButton,
					currentWeek,
					nextWeekButton
				}
			};

			absoluteLayout.Add(stackWeekSelector);
            absoluteLayout.SetLayoutBounds(stackWeekSelector, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));

		}

		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in allClass_Schedules)
			{
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
			}
		}

		
		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classAttendanceCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = allClass_Schedules,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 10 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Aulas agendadas nesta semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			classAttendanceCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;

			classAttendanceCollectionView.ItemTemplate = new DataTemplate(() =>
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
	            

				return itemabsoluteLayout;
			});
			absoluteLayout.Add(classAttendanceCollectionView);
            absoluteLayout.SetLayoutBounds(classAttendanceCollectionView, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 150 * App.screenHeightAdapter));
		}


		public AttendanceManagePageCS()
		{
			this.initLayout();
			initSpecificLayout();
		}



		async void OnWeek0ButtonClicked(object sender, EventArgs e)
		{
			/*week0Button.activate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.deactivate();*/

			//weekClassesCollectionView.ItemsSource = week0Class_Detail;
			//_collection.Items = week0Class_Detail;
		}

		async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			firstDayWeek_datetime = firstDayWeek_datetime.AddDays(-7);
			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.AddDays(6).Month];

			currentWeek.Text = firstDayWeek_formatted + " - " + lastdayLastWeek_formatted;

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");

			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);
			CompleteClass_Schedules();
			classAttendanceCollectionView.ItemsSource = dummyClass_Schedules;
			classAttendanceCollectionView.ItemsSource = allClass_Schedules;
			hideActivityIndicator();
		}

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			firstDayWeek_datetime = firstDayWeek_datetime.AddDays(7);
			
			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.AddDays(6).Month];

			currentWeek.Text = firstDayWeek_formatted + " - " + lastdayLastWeek_formatted;

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");

			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);

			CompleteClass_Schedules();
			classAttendanceCollectionView.ItemsSource = dummyClass_Schedules;
			classAttendanceCollectionView.ItemsSource = allClass_Schedules;
			hideActivityIndicator();
		}

		async Task<List<Class_Schedule>> GetAllClass_Schedules(string begindate, string enddate)
		{
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetAllClass_Schedules(App.member.id, begindate, enddate);
			if (class_schedules_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return class_schedules_i;
		}


		async void OnClassAttendanceCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("AttendanceManagePageCS OnClassAttendanceCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Class_Schedule class_schedule = (sender as CollectionView).SelectedItem as Class_Schedule;
				classAttendanceCollectionView.SelectedItem = null;
				await Navigation.PushAsync(new AttendanceClassPageCS(class_schedule));
				
			}
		}
	}

}
