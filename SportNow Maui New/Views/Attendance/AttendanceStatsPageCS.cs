using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.Model.Charts;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class AttendanceStatsPageCS : DefaultPage
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

		private AbsoluteLayout graphabsoluteLayout;

		private List<Class_Schedule> pastclass_schedules, pastclass_schedules_dummy;
		private CollectionView classesCollectionView;

        SfCircularChart chart;
		string centerViewText;
		Label centerView_Label;
        RadialBarSeries series;
		Attendance_Stats pastClass_Attendances;

		public void initLayout()
		{
			Title = "ESTATÍSTICAS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover

		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();
			DateTime currentTime = DateTime.Now.Date;
            string firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-MM-dd");
			string currentTime_string = currentTime.ToString("yyyy-MM-dd");
			Debug.Print("firstDayWeek = " + firstDayWeek+ " currentTime_string = "+ currentTime_string);
			pastclass_schedules = await GetStudentClass_Schedules(firstDayWeek, currentTime_string);
			pastclass_schedules_dummy = new List<Class_Schedule>();
			CompleteClass_Schedules();

			CreatePeriodSelection();
			createReport();
			CreateClassesColletion();

			hideActivityIndicator();

		}

		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in pastclass_schedules)
			{
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;
                //class_schedule.imagesource = "company_logo_square.png";

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

				if (class_schedule.classattendancestatus == "fechada")
                {
					class_schedule.participationimage = "iconcheck.png";
				}
				else
                {
					class_schedule.participationimage = "iconinativo.png";
				}
					
			}
		}


		public void CreatePeriodSelection()
		{

			Label periodLabel = new Label
			{
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.bigTitleFontSize,
				TextColor = App.topColor,
				Text = "Escolhe o Periodo",
                FontFamily = "futuracondensedmedium",
            };

			var periodList = new List<string>();
			periodList.Add("ESTA SEMANA");
			periodList.Add("ESTE MÊS");
			periodList.Add("ÚLTIMA SEMANA");
			periodList.Add("ÚLTIMO MÊS");
			//periodList.Add("DESDE ÚLTIMO EXAME");

			var periodPicker = new Picker
			{
                FontFamily = "futuracondensedmedium",
				Title = "",
				TitleColor = Colors.White,
				BackgroundColor = Colors.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.itemTitleFontSize

			};
			periodPicker.ItemsSource = periodList;
			periodPicker.SelectedIndex = 0;

			periodPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("periodPicker.SelectedItem.ToString() = " + periodPicker.SelectedItem.ToString());
				string begindate = "";
				string enddate = "";
				if (periodPicker.SelectedItem.ToString() == "ESTA SEMANA")
				{
					DateTime currentTime = DateTime.Now.Date;
					begindate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-MM-dd");
					enddate = currentTime.ToString("yyyy-MM-dd");
				}
				else if (periodPicker.SelectedItem.ToString() == "ESTE MÊS")
				{
					DateTime currentTime = DateTime.Now.Date;
					begindate = currentTime.ToString("yyyy-MM-") + "01";
					enddate = currentTime.ToString("yyyy-MM-dd");
					//searchDate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-mm-dd");
				}
				else if (periodPicker.SelectedItem.ToString() == "ÚLTIMA SEMANA")
				{
					DateTime lastWeek = DateTime.Now.AddDays(-7);
					DateTime firstdayLastWeek = lastWeek.AddDays(-Constants.daysofWeekInt[lastWeek.DayOfWeek.ToString()]);
					DateTime lastdayLastWeek = firstdayLastWeek.AddDays(6);

					begindate = firstdayLastWeek.ToString("yyyy-MM-dd");
					enddate = lastdayLastWeek.ToString("yyyy-MM-dd");

                    Debug.Print("Ultima semana = " + begindate + " - " + enddate);
                }
				else if (periodPicker.SelectedItem.ToString() == "ÚLTIMO MÊS")
				{
					DateTime lastMonth = DateTime.Now.AddMonths(-1).Date;
					DateTime fisrtDayMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-") + "01").Date;

					begindate = lastMonth.ToString("yyyy-MM-") + "01";
					enddate = fisrtDayMonth.AddDays(-1).ToString("yyyy-MM-dd");
                    Debug.Print("Ultimo mês = " + begindate + " - " + enddate);

                    //searchDate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-mm-dd");
                }
				Debug.Print("begindate = " + begindate + " enddate = " + enddate);
				showActivityIndicator();

				pastclass_schedules = await GetStudentClass_Schedules(begindate, enddate);
				CompleteClass_Schedules();
				classesCollectionView.ItemsSource = pastclass_schedules_dummy;
				classesCollectionView.ItemsSource = pastclass_schedules;

				
				chart.BindingContext = new Attendance_Stats(pastclass_schedules_dummy);
                //this.BindingContext = new Attendance_Stats(pastclass_schedules_dummy);
                pastClass_Attendances = new Attendance_Stats(pastclass_schedules);
				chart.BindingContext = pastClass_Attendances;
                //this.BindingContext = pastClass_Attendances;

                centerViewText = pastClass_Attendances.Data[0].class_count_presente.ToString() + "\n TOTAL";
				centerView_Label = new Label() { Text = centerViewText, FontSize = 25, TextColor = App.topColor, HorizontalTextAlignment = TextAlignment.Center };
				series.CenterView = centerView_Label;

                //CreatePastClassesColletion();
                //createReport();

                hideActivityIndicator();

			};

			absoluteLayout.Add(periodLabel);
            absoluteLayout.SetLayoutBounds(periodLabel, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));

			absoluteLayout.Add(periodPicker);
            absoluteLayout.SetLayoutBounds(periodPicker, new Rect(0, 40 * App.screenHeightAdapter, App.screenWidth, 40 * App.screenHeightAdapter));

		}

		public void createReport()
		{
			createChart();
			//chart.BackgroundColor = Colors.Red;

            absoluteLayout.Add(chart);
            absoluteLayout.SetLayoutBounds(chart, new Rect(0 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 0 * App.screenWidthAdapter, (App.screenHeight - 210 * App.screenHeightAdapter) * 3 / 5));

			BoxView separator = new BoxView()
			{
				HeightRequest = 3 * App.screenHeightAdapter,
				BackgroundColor = App.topColor,
				Color = App.topColor,

                //BackgroundColor = Colors.Green,
            };

            absoluteLayout.Add(separator);
            absoluteLayout.SetLayoutBounds(separator, new Rect(0 * App.screenWidthAdapter, 75 * App.screenHeightAdapter + (App.screenHeight - 190 * App.screenHeightAdapter) * 3 / 5, App.screenWidth - 0 * App.screenWidthAdapter, 3 * App.screenHeightAdapter));

		}

		public SfCircularChart createChart()
		{
			chart = new SfCircularChart();
			chart.BackgroundColor = App.backgroundColor;

			Attendance_Stats pastClass_Attendances = new Attendance_Stats(pastclass_schedules);
			this.BindingContext = pastClass_Attendances;


			series = new RadialBarSeries()
			{
				ShowDataLabels = true,
				EnableTooltip = true,
				CapStyle = CapStyle.BothCurve,
				InnerRadius = 0.4,
				GapRatio = 0.2,
				MaximumValue = 100,
				TrackStrokeWidth = 0,
				TrackStroke = Color.FromRgb(200, 200, 200),
				TrackFill = Color.FromRgb(200, 200, 200),
				PaletteBrushes = new List<Brush>() {
                    new SolidColorBrush(App.topColor),
                    new SolidColorBrush(Color.FromArgb("494C4B")),
					new SolidColorBrush(Color.FromArgb("9D3A35")),
					new SolidColorBrush(Color.FromArgb("986F00")),
					new SolidColorBrush(Color.FromArgb("8A865D")),
					new SolidColorBrush(Color.FromArgb("D9D394")),
					new SolidColorBrush(Color.FromArgb("6EA79E")),
					new SolidColorBrush(Color.FromArgb("68565D")),
                },
				StartAngle = -90,
				EndAngle = 270,
				LegendIcon = ChartLegendIconType.Circle,
				IsVisibleOnLegend = true
            };

			centerViewText = pastClass_Attendances.Data[0].class_count_presente.ToString() + "\n TOTAL";
			centerView_Label = new Label() { FontFamily = "futuracondensedmedium", Text = centerViewText, FontSize = 25, TextColor = App.topColor, HorizontalTextAlignment = TextAlignment.Center };
			series.CenterView = centerView_Label;

			series.SetBinding(ChartSeries.ItemsSourceProperty, "Data");
			series.XBindingPath = "name";
			series.YBindingPath = "attendance_percentage";
            //series.DataMarker = new ChartDataMarker();
            //chart.EnableSeriesSelection = true;
            //chart.SeriesSelectionColor = Colors.Red;
 
			chart.BackgroundColor = Colors.LightGrey;
            chart.Series.Add(series);

			ChartLegend legend = new ChartLegend();
			legend.Placement = LegendPlacement.Bottom;

			/*
			{
				BindingContext = pastClass_Attendances,
                IsVisible = true,
				Placement = LegendPlacement.Bottom,
                ItemTemplate = new DataTemplate(() =>
                {
                    StackLayout stack = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        WidthRequest = 100
                    };

                    BoxView boxView = new BoxView()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 13,
                        HeightRequest = 13,
                        //BackgroundColor = Colors.Green,

                    };
                    boxView.SetBinding(BoxView.BackgroundColorProperty, "IconColor");

                    Label name = new Label()
                    {
                        TextColor = App.normalTextColor,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 13,
						//Text = "OLA"
                    };
                    name.SetBinding(Label.TextProperty, "DataPoint.Name");
                    //name.SetBinding(Label.TextProperty, "Data.name");

                    stack.Children.Add(boxView);
                    stack.Children.Add(name);
                    return stack;
                }),
        };*/




            chart.Legend = legend;

            return chart;
		}

        /*public void createChart()
        {
            Attendance_Stats pastClass_Attendances = new Attendance_Stats(pastclass_schedules);
			pastClass_Attendances.Print();
            this.BindingContext = pastClass_Attendances;

            chart = new SfCartesianChart();
            //chart.IsTransposed = true;
            chart.WidthRequest = App.screenWidth - 40 * App.screenWidthAdapter;
            CategoryAxis primaryAxis = new CategoryAxis()
			{
                LabelStyle = new ChartAxisLabelStyle() { TextColor = App.normalTextColor },
                Title = new ChartAxisTitle
                {
                    Text = "Aulas",
                    TextColor = App.normalTextColor
                }
			};
            chart.XAxes.Add(primaryAxis);
            NumericalAxis secondaryAxis = new NumericalAxis()
            {
                LabelStyle = new ChartAxisLabelStyle () { TextColor = App.normalTextColor,  },
                Title = new ChartAxisTitle
                {
                    Text = "Presenças",
                    TextColor = App.normalTextColor
                }
            };
            chart.YAxes.Add(secondaryAxis);

            chart.PaletteBrushes = new List<Brush>() {
                new SolidColorBrush(Color.FromArgb("B5E48C")),
				new SolidColorBrush(Color.FromArgb("D9ED92")),
			};

            StackingColumnSeries series = new StackingColumnSeries()
            {
                XBindingPath = "name",
                YBindingPath = "class_count_presente",
                ItemsSource = pastClass_Attendances.Data,
                EnableTooltip = true,
                ShowDataLabels = true,
                //Label = " Aulas",
                DataLabelSettings = new CartesianDataLabelSettings
                {
                    //LabelPlacement = DataLabelPlacement.Inner,
                    LabelStyle = new ChartDataLabelStyle() { TextColor = App.normalTextColor,  },
                }
            };

            StackingColumnSeries series1 = new StackingColumnSeries()
			{
				XBindingPath = "name",
				YBindingPath = "class_count_ausente",
				ItemsSource = pastClass_Attendances.Data,
                EnableTooltip = true,
                ShowDataLabels = true,
                DataLabelSettings = new CartesianDataLabelSettings
                {
                    //LabelPlacement = DataLabelPlacement.Inner,
                    LabelStyle = new ChartDataLabelStyle() { TextColor = App.normalTextColor, },
                }
            };

            series.DataLabelSettings = new CartesianDataLabelSettings()
            {
                BarAlignment = DataLabelAlignment.Middle,
            };

            series1.DataLabelSettings = new CartesianDataLabelSettings()
            {
                BarAlignment = DataLabelAlignment.Top,
            };

            chart.Legend = new ChartLegend();

            chart.Series.Add(series);
            chart.Series.Add(series1);
        }*/

        public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = pastclass_schedules,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 6 * App.screenWidthAdapter, HorizontalItemSpacing = 10 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium",Text = "Não existem Aulas agendadas nesta semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			classesCollectionView.SelectionChanged += OnClassesCollectionViewSelectionChanged;

			classesCollectionView.ItemTemplate = new DataTemplate(() =>
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

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
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


                return itemabsoluteLayout;
			});
			absoluteLayout.Add(classesCollectionView);
            absoluteLayout.SetLayoutBounds(classesCollectionView, new Rect(3 * App.screenWidthAdapter, 80 * App.screenHeightAdapter + ((App.screenHeight - 180 * App.screenHeightAdapter) * 3 / 5), App.screenWidth - (0 * App.screenWidthAdapter), ((App.screenHeight - 180 * App.screenHeightAdapter) * 2 / 5)));
		}

		public AttendanceStatsPageCS()
		{
			this.initLayout();
		}

		async Task<List<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
		{
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules(App.member.id, begindate, enddate);
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


		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AttendanceAbsentPageCS());
		}


		async void OnClassesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

    }
}
