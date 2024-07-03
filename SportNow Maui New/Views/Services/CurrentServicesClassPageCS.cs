using SportNow.Model;
using System.Diagnostics;
using Microsoft.Maui.Controls.Shapes;
using SportNow.Services.Data.JSON;
using System.Collections.ObjectModel;
using SportNow.ViewModel;
using SportNow.CustomViews;
//using static System.Net.Mime.MediaTypeNames;
//using Microsoft.Maui.Controls;

namespace SportNow.Views.Services
{
	public class CurrentServicesClassPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
            
            showActivityIndicator();

			//initSpecificLayout();

			hideActivityIndicator();
		}

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

        Service service;
        private ObservableCollection<Class_Schedule> importantClass_Schedule;
        private ObservableCollection<Class_Schedule> cleanClass_Schedule;

        private List<Class_Schedule> teacherClass_Schedules;

        Label attendanceLabel, teacherClassesLabel, otherOptionsLabel;

        private CollectionView importantClassesCollectionView;
        private CollectionView teacherClassesCollectionView;

        ScheduleCollection scheduleCollection;

        Grid gridService, gridPresencasButtons;

        private OptionButton marcarAulaButton, estatisticasButton, presencasButton, mensalidadesButton, mensalidadesStudentButton;

        int service_students_count;

        public void CleanScreen()
		{
            if (importantClassesCollectionView != null)
            {
                //absoluteLayout.Clear();

                absoluteLayout.Children.Remove(importantClassesCollectionView);
                importantClassesCollectionView = null;
            }
            if (teacherClassesCollectionView != null)
            {

                absoluteLayout.Children.Remove(teacherClassesCollectionView);
                teacherClassesCollectionView = null;
                teacherClassesLabel = null;
            }
        }

        public void initLayout()
		{
			Title = service.nome.ToUpper();
		}


		public async void initSpecificLayout()
		{
            gridService = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter, ColumnSpacing = 10 * App.screenWidthAdapter };

            gridService.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            service_students_count = await GetStudents_Service_Count();

            Debug.Print("service students_countt = " + service_students_count);
            if (service_students_count > 0)
            {
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridService.RowDefinitions.Add(new RowDefinition { Height = App.ItemHeight + 20 * App.screenHeightAdapter });
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridService.RowDefinitions.Add(new RowDefinition { Height = App.ItemHeight + 20 * App.screenHeightAdapter });
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                _ = await createImportantTeacherClasses();
                gridService.Add(teacherClassesLabel, 0, 0);
                gridService.Add(teacherClassesCollectionView, 0, 1);
                _ = await createImportantClasses();
                gridService.Add(attendanceLabel, 0, 2);
                gridService.Add(importantClassesCollectionView, 0, 3);
                _ = await CreatePresencasOptionButtonsAsync();
                gridService.Add(otherOptionsLabel, 0, 4);
                gridService.Add(gridPresencasButtons, 0, 5);

            }
            else
            {
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridService.RowDefinitions.Add(new RowDefinition { Height = App.ItemHeight + 20 * App.screenHeightAdapter });
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                _ = await createImportantClasses();
                gridService.Add(attendanceLabel, 0, 0);
                gridService.Add(importantClassesCollectionView, 0, 1);
                _ = await CreatePresencasOptionButtonsAsync();
                gridService.Add(otherOptionsLabel, 0, 2);
                gridService.Add(gridPresencasButtons, 0, 3);

            }
            absoluteLayout.Add(gridService);
            absoluteLayout.SetLayoutBounds(gridService, new Rect(0 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100));
        }

        public async Task<int> createImportantClasses()
        {
            showActivityIndicator();
            int result = await getClass_DetailData();

            //AULAS LABEL
            attendanceLabel = new Label
            {
                
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            if (App.member.gender == "female")
            {
                attendanceLabel.Text = "PRÓXIMAS AULAS COMO ALUNA";
            }
            else
            {
                attendanceLabel.Text = "PRÓXIMAS AULAS COMO ALUNO";
            }
            scheduleCollection = new ScheduleCollection();
            scheduleCollection.Items = importantClass_Schedule;
            createClassesCollection();
            hideActivityIndicator();
            return result;
        }

        public async Task<int> getClass_DetailData()
        {
            DateTime currentTime = DateTime.Now.Date;
            DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

            importantClass_Schedule = await GetStudentClass_Schedules(currentTime.ToString("yyyy-MM-dd"), currentTime.AddDays(7).ToString("yyyy-MM-dd"));//  new List<Class_Schedule>();
            cleanClass_Schedule = new ObservableCollection<Class_Schedule>();

            CompleteClass_Schedules();

            return 1;
        }


        public void CompleteClass_Schedules()
        {
            foreach (Class_Schedule class_schedule in importantClass_Schedule)
            {
                /*if (class_schedule.classattendancestatus == "confirmada")
				{
					class_schedule.participationimage = "iconcheck.png";
				}*/
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

        public void createClassesCollection()
        {
            importantClassesCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Multiple,
                //ItemsSource = importantClass_Schedule,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium",FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };
            this.BindingContext = scheduleCollection;
            importantClassesCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Items");


            importantClassesCollectionView.SelectionChanged += OnClassScheduleCollectionViewSelectionChanged;

            importantClassesCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
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
                    HeightRequest = App.ItemHeight,// -(10 * App.screenHeightAdapter),
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image
                {
                    Aspect = Aspect.AspectFill,
                    Opacity = 0.40,
                };
                eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));


                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

                return itemabsoluteLayout;
            });
        }

        public async Task<int> createImportantTeacherClasses()
        {
            showActivityIndicator();
            DateTime currentTime = DateTime.Now.Date;
            DateTime currentTime_add7 = DateTime.Now.AddDays(7).Date;

            string firstDay = currentTime.ToString("yyyy-MM-dd");
            string lastday = currentTime_add7.AddDays(6).ToString("yyyy-MM-dd");

            teacherClass_Schedules = await GetAllClass_Schedules_byService(firstDay, lastday);
            CompleteTeacherClass_Schedules();

            //AULAS LABEL
            teacherClassesLabel = new Label
            {
                Text = "PRÓXIMAS AULAS COMO RESPONSÁVEL",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            CreateTeacherClassesColletion();
            hideActivityIndicator();
            return 1;
        }

        public void CompleteTeacherClass_Schedules()
        {
            foreach (Class_Schedule class_schedule in teacherClass_Schedules)
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


        public void CreateTeacherClassesColletion()
        {
            //COLLECTION TEACHER CLASSES
            teacherClassesCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = teacherClass_Schedules,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            teacherClassesCollectionView.SelectionChanged += OnTeacherClassesCollectionViewSelectionChanged;

            teacherClassesCollectionView.ItemTemplate = new DataTemplate(() =>
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

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");


                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");


                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

                return itemabsoluteLayout;
            });

        }


        public async Task<int> CreatePresencasOptionButtonsAsync()
        {


            //AULAS LABEL
            otherOptionsLabel = new Label
            {
                Text = "OUTRAS OPÇÕES",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            //showActivityIndicator();
            var width = App.screenWidth;
            var buttonWidth = (width) / 2 - 10 * App.screenWidthAdapter;


            marcarAulaButton = new OptionButton("MARCAR AULAS", "confirmclasses.png", buttonWidth, 80 * App.screenHeightAdapter);
            //minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
            var marcarAulaButton_tap = new TapGestureRecognizer();
            marcarAulaButton_tap.Tapped += (s, e) =>
            {

                Navigation.PushAsync(new AttendancePageCS(this.service));
            };
            marcarAulaButton.GestureRecognizers.Add(marcarAulaButton_tap);

            estatisticasButton = new OptionButton("ESTATÍSTICAS", "classstats.png", buttonWidth, 80 * App.screenHeightAdapter);
            var estatisticasButton_tap = new TapGestureRecognizer();
            estatisticasButton_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new AttendanceStatsPageCS(this.service));
            };
            estatisticasButton.GestureRecognizers.Add(estatisticasButton_tap);

            presencasButton = new OptionButton("PRESENÇAS", "attendances.png", buttonWidth, 80 * App.screenHeightAdapter);
            //minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
            var presencasButton_tap = new TapGestureRecognizer();
            presencasButton_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new AttendanceManagePageCS(this.service));
            };
            presencasButton.GestureRecognizers.Add(presencasButton_tap);

            mensalidadesButton = new OptionButton("MENSALIDADES INSTRUTOR", "mensalidades_alunos.png", buttonWidth, 80 * App.screenHeightAdapter);
            var mensalidadesButton_tap = new TapGestureRecognizer();
            mensalidadesButton_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new MonthFeeListPageCS(this.service));
            };
            mensalidadesButton.GestureRecognizers.Add(mensalidadesButton_tap);

            mensalidadesStudentButton = new OptionButton("MENSALIDADES", "monthfees.png", buttonWidth, 80 * App.screenHeightAdapter);
            var mensalidadesStudentButton_tap = new TapGestureRecognizer();
            mensalidadesStudentButton_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new MonthFeeStudentListPageCS(this.service));
            };
            mensalidadesStudentButton.GestureRecognizers.Add(mensalidadesStudentButton_tap);


            string monthFeeStudentCount = await Get_has_StudentMonthFees();


            gridPresencasButtons = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 5 * App.screenHeightAdapter, ColumnSpacing = 0 * App.screenWidthAdapter };
            gridPresencasButtons.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPresencasButtons.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPresencasButtons.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPresencasButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridPresencasButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto


            if (service_students_count > 0)
            {
                if (monthFeeStudentCount != "0")
                {
                    gridPresencasButtons.Add(presencasButton, 0, 0);
                    gridPresencasButtons.Add(marcarAulaButton, 1, 0);
                    gridPresencasButtons.Add(mensalidadesButton, 0, 1);
                    gridPresencasButtons.Add(mensalidadesStudentButton, 1, 1);
                    gridPresencasButtons.Add(estatisticasButton, 0, 2);
                    Grid.SetColumnSpan(estatisticasButton, 2);
                }
                else
                {
                    gridPresencasButtons.Add(presencasButton, 0, 0);
                    gridPresencasButtons.Add(marcarAulaButton, 1, 0);
                    gridPresencasButtons.Add(estatisticasButton, 0, 1);
                    gridPresencasButtons.Add(mensalidadesButton, 1, 1);

                }

            }
            else
            {
                if (monthFeeStudentCount != "0")
                {
                    gridPresencasButtons.Add(marcarAulaButton, 0, 0);
                    gridPresencasButtons.Add(estatisticasButton, 1, 0);
                    gridPresencasButtons.Add(mensalidadesStudentButton, 0, 1);
                    Grid.SetColumnSpan(mensalidadesStudentButton, 2);
                }
                else
                {
                    gridPresencasButtons.Add(marcarAulaButton, 0, 0);
                    gridPresencasButtons.Add(estatisticasButton, 1, 0);
                }
            }
            return 0;
        }

        public CurrentServicesClassPageCS(Service service)
		{
            this.service = service;
			this.initLayout();
			this.initSpecificLayout();

		}

        async Task<ObservableCollection<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
        {
            Debug.WriteLine("GetStudentClass_Schedules");
            ClassManager classManager = new ClassManager();
            ObservableCollection<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules_obs(App.member.id, this.service.id, begindate, enddate);
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

        async Task<List<Class_Schedule>> GetAllClass_Schedules_byService(string begindate, string enddate)
        {
            ClassManager classManager = new ClassManager();
            List<Class_Schedule> class_schedules_i = await classManager.GetAllClass_Schedules_byService(App.member.id, this.service.id, begindate, enddate);

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

        async Task<int> GetStudents_Service_Count()
        {
            ServicesManager servicesManager = new ServicesManager();
            int count = await servicesManager.GetStudents_Service_Count(App.member.id, this.service.id);

            if (count < 0)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
                return -1;
            }
            return count;
        }

        async Task<string> Get_has_StudentMonthFees()
        {
            Debug.WriteLine("GetStudentClass_Schedules");
            MonthFeeManager monthFeeManager = new MonthFeeManager();
            string count = await monthFeeManager.Has_MonthFeesStudent(App.member.id, service.id);
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
                    //string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                    /*                    string class_attendance_id =  classmanager.CreateClass_Attendance_sync(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                                        */
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
                        await DisplayAlert("PRESENÇA EM AULA", "A tua presença nesta aula já foi validada pelo instrutor pelo que não é possível alterar o seu estado.", "Ok");
                    }
                    //int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
                }

                ((CollectionView)sender).SelectedItems.Clear();
                /*importantClassesCollectionView.ItemsSource = cleanClass_Schedule;
				importantClassesCollectionView.ItemsSource = importantClass_Schedule;*/

                hideActivityIndicator();
            }
        }

        async void OnTeacherClassesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("MainPageCS.OnClassAttendanceCollectionViewSelectionChanged");

            if ((sender as CollectionView).SelectedItem != null)
            {
                Class_Schedule class_schedule = (sender as CollectionView).SelectedItem as Class_Schedule;
                (sender as CollectionView).SelectedItem = null;
                await Navigation.PushAsync(new AttendanceClassPageCS(class_schedule));

            }
        }
        

    }
}
