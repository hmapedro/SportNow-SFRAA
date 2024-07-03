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
	public class CurrentServicesEducationPageCS : DefaultPage
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

        Grid gridService, gridPresencasButtons;

        private OptionButton eventosButton, mensalidadesButton, mensalidadesStudentButton;

        Label otherOptionsLabel;

        int service_students_count;

        public void CleanScreen()
		{

        }

        public void initLayout()
		{
			Title = service.nome.ToUpper();
		}


		public async void initSpecificLayout()
		{
            gridService = new Grid { BackgroundColor = Colors.Blue, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter, ColumnSpacing = 10 * App.screenWidthAdapter };

            gridService.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star}); //GridLength.Auto

            service_students_count = await GetStudents_Service_Count();

            _ = await CreatePresencasOptionButtonsAsync();

            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.Add(gridPresencasButtons, 0, 0);
           
            absoluteLayout.Add(gridService);
            absoluteLayout.SetLayoutBounds(gridService, new Rect(0 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100));
        }


        public async Task<int> CreatePresencasOptionButtonsAsync()
        {


            //showActivityIndicator();
            var width = App.screenWidth;
            var buttonWidth = (width) / 2 - 10 * App.screenWidthAdapter;

            eventosButton = new OptionButton("EVENTOS", "attendances.png", buttonWidth, 80 * App.screenHeightAdapter);
            //minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
            var eventosButton_tap = new TapGestureRecognizer();
            eventosButton_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new AttendanceManagePageCS(this.service));
            };
            eventosButton.GestureRecognizers.Add(eventosButton_tap);

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

            if (service_students_count > 0)
            {
                if (monthFeeStudentCount != "0")
                {
                    gridPresencasButtons.Add(mensalidadesButton, 0, 0);
                    gridPresencasButtons.Add(mensalidadesStudentButton, 0, 1);
                    gridPresencasButtons.Add(eventosButton, 0, 2);
                }
                else
                {
                    gridPresencasButtons.Add(mensalidadesButton, 0, 0);
                    gridPresencasButtons.Add(eventosButton, 0, 1);

                }
            }
            else
            {
                if (monthFeeStudentCount != "0")
                {
                    gridPresencasButtons.Add(mensalidadesStudentButton, 0, 0);
                    gridPresencasButtons.Add(eventosButton, 0, 1);
                    
                }
                else
                {
                    gridPresencasButtons.Add(eventosButton, 0, 0);
                }
            }
            return 0;
        }

        public CurrentServicesEducationPageCS(Service service)
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
