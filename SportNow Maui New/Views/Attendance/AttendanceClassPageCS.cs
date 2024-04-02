
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;
using SportNow.ViewModel;
using SportNow.Views.Technical;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class AttendanceClassPageCS : DefaultPage
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

		private AbsoluteLayout presencasrelativeLayout;


		private CollectionView class_attendanceCollectionView;

		private Class_Schedule class_schedule;
		private ObservableCollection<Class_Attendance> class_attendances;
		private List<Class_Attendance> class_attendances_dummy = new List<Class_Attendance>();

		private AttendanceCollection attendanceCollection;

		private int alunosAusentes, alunosMarcados, alunosConfirmados;
		Label ausentesLabel, marcadosLabel, confirmadosLabel;
		private Microsoft.Maui.Controls.Grid gridCount;

		RoundButton confirmButton;
		Label className;

        RoundButton classProgramButton;

        public void initLayout()
		{
			Title = "PRESENÇAS AULA";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "add_person.png"

			};
			toolbarItem.Clicked += OnAddPersonButtonClicked;
			ToolbarItems.Add(toolbarItem);
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");

			if (confirmButton != null)
			{
				absoluteLayout.Remove(confirmButton);
				confirmButton = null;
			}
			if (className != null)
			{
				absoluteLayout.Remove(className);
				className = null;
			}

			if (gridCount != null)
            {
				absoluteLayout.Remove(gridCount);
				gridCount = null;
				ausentesLabel = null;
				marcadosLabel = null;
				confirmadosLabel = null;
			}

			if (class_attendanceCollectionView != null)
			{
				absoluteLayout.Remove(class_attendanceCollectionView);
				class_attendanceCollectionView = null;
			}

			alunosMarcados = 0;
			alunosAusentes = 0;
			alunosConfirmados = 0;
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			class_attendances = await GetClass_Attendances();
			if (class_attendances == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return;
			}
			CreateTitle();
			//CreateClassProgramButton();
            CompleteClass_Attendances();

			attendanceCollection = new AttendanceCollection();
			attendanceCollection.Items = class_attendances;

			CreateClassesColletion();
			CreateConfirmButton();

			hideActivityIndicator();
		}


		public void CreateTitle()
		{

			Label className = new Label()
			{
                FontFamily = "futuracondensedmedium",
                Text = this.class_schedule.name + "\n" + class_schedule.dojo + "\n" + class_schedule.date,
				FontSize = 20,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};

			absoluteLayout.Add(className);
            absoluteLayout.SetLayoutBounds(className, new Rect(0, 0, App.screenWidth, 80 * App.screenHeightAdapter));
		}

        public void CreateClassProgramButton()
        {

            classProgramButton = new RoundButton("Programa de Treino", App.screenWidth, 50);
            classProgramButton.button.BackgroundColor = App.topColor;
            classProgramButton.button.Clicked += OnClassProgramButtonButtonClicked;

			absoluteLayout.Add(classProgramButton);
            absoluteLayout.SetLayoutBounds(classProgramButton, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
        }

        public void CompleteClass_Attendances()
		{
			foreach (Class_Attendance class_attendance in class_attendances)
			{
				Debug.Print("class_attendance.membernickname=" + class_attendance.membernickname + " " + class_attendance.status);
				if (class_attendance.status == "confirmada")
				{
					alunosMarcados++;
					class_attendance.imagesource = "iconinativo.png";
					class_attendance.color = Colors.Blue;
                    class_attendance.colorImage = "blue.png";
                }
				else if (class_attendance.status == "fechada")
				{
					alunosConfirmados++;
					class_attendance.imagesource = "iconcheck.png";
					class_attendance.color = Colors.Green;
                    class_attendance.colorImage = "green.png";
                }
				else if (class_attendance.status == "anulada")
				{
					alunosAusentes++;
					class_attendance.imagesource = "";
					class_attendance.color = Colors.Yellow;
                    class_attendance.colorImage = "yellow.png";
                }
				else 
				{
					alunosAusentes++;
					class_attendance.color = Colors.Transparent;
                    class_attendance.colorImage = "transparent.png";
                }
			}
		}

		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			class_attendanceCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = class_attendances,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Alunos inscritos nesta aula.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			//class_attendanceCollectionView.BindingContext = attendanceCollection;
			this.BindingContext = attendanceCollection;
			class_attendanceCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Items");

			class_attendanceCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;

			class_attendanceCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = 30 * App.screenHeightAdapter
                };

				FormValue nameLabel = new FormValue("", 30 * App.screenHeightAdapter);
				nameLabel.label.SetBinding(Label.TextProperty, "membernickname");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(0, 0, App.screenWidth - (45 * App.screenHeightAdapter), 30 * App.screenHeightAdapter));

				Border attendanceStatus_Frame = new Border()
				{
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    //Padding = new Thickness(2, 2, 2, 2),
					HeightRequest = 30 * App.screenHeightAdapter,
					VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.Transparent,
                };
                Image participationImagem = new Image { Aspect = Aspect.AspectFill, BackgroundColor = Colors.Transparent }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "colorImage");

				attendanceStatus_Frame.Content = participationImagem;

				itemabsoluteLayout.Add(attendanceStatus_Frame);
				itemabsoluteLayout.SetLayoutBounds(attendanceStatus_Frame, new Rect(App.screenWidth - (35 * App.screenHeightAdapter), 0, 30 * App.screenHeightAdapter, 30 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});
			absoluteLayout.Add(class_attendanceCollectionView);
			absoluteLayout.SetLayoutBounds(class_attendanceCollectionView, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 360 * App.screenHeightAdapter));

			gridCount = new Microsoft.Maui.Controls.Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCount.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			ausentesLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Ausentes - " + alunosAusentes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				FontSize = 18
			};

			marcadosLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Marcados - " + alunosMarcados,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				FontSize = 18
			};

			confirmadosLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Confirmados - " + alunosConfirmados,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				FontSize = 18
			};
			gridCount.Add(ausentesLabel, 0, 0);
			gridCount.Add(marcadosLabel, 1, 0);
			gridCount.Add(confirmadosLabel, 2, 0);

			absoluteLayout.Add(gridCount);
            absoluteLayout.SetLayoutBounds(gridCount, new Rect(0, App.screenHeight - 210 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
            absoluteLayout.SetLayoutBounds(gridCount, new Rect(0, App.screenHeight - 210 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
		}


		public void CreateConfirmButton()
		{

			confirmButton = new RoundButton("CONFIRMAR PRESENÇAS", App.screenWidth - 20 * App.screenWidthAdapter, 50);
			confirmButton.button.Clicked += OnConfirmButtonClicked;

			absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
		}

		public AttendanceClassPageCS(Class_Schedule class_schedule)
		{
			this.class_schedule = class_schedule;
			this.initLayout();
		}

		async Task<ObservableCollection<Class_Attendance>> GetClass_Attendances()
		{
			ClassManager classManager = new ClassManager();
			ObservableCollection<Class_Attendance> class_attendances_i = await classManager.GetClass_Attendances_obs(this.class_schedule.classid, this.class_schedule.date);
			if (class_attendances_i == null)
			{
				Debug.Print("class_attendances_i é null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			Debug.Print("class_attendances_i não é null");
			return class_attendances_i;
		}

		async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();

			Debug.WriteLine("OnConfirmButtonClicked");
			ClassManager classmanager = new ClassManager();
			foreach (Class_Attendance class_attendance in class_attendances)
			{
				Debug.WriteLine("OnConfirmButtonClicked class_attendance.classattendanceid=" + class_attendance.classattendanceid + " class_attendance.status="+ class_attendance.status);
				if ((class_attendance.classattendanceid == null) & (class_attendance.status == "confirmada"))
				{ // SE NÃO EXISTIA E O INSTRUTOR METEU CONFIRMADA
                    Task.Run(async () =>
                    {
                        string class_attendance_id = await classmanager.CreateClass_Attendance(class_attendance.memberid, class_attendance.classid, "fechada", class_attendance.date);
                        class_schedule.classattendanceid = class_attendance_id;
                        return true;
                    });

                    
					class_attendance.status = "fechada";
					class_attendance.color = Colors.Green;
                    class_attendance.colorImage = "green.png";

                    alunosMarcados--;
					marcadosLabel.Text = "Marcados - " + alunosMarcados;
					alunosConfirmados++;
					confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;
				}
				else if ((class_attendance.classattendanceid != null) &
					((class_attendance.statuschanged == true) | ((class_attendance.statuschanged == false) & (class_attendance.status == "confirmada"))))
				{
					Debug.Print("FAZ UPDATE");
					if (class_attendance.status == "confirmada")
					{
						class_attendance.status = "fechada";
						class_attendance.color = Colors.Green;
						class_attendance.colorImage = "green.png";

                        alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
						alunosConfirmados++;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;

					}
					/*else if (class_attendance.status == "anulada")
					{
						alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
						alunosConfirmados++;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;
					}*/
					_ = await classmanager.UpdateClass_Attendance(class_attendance.classattendanceid, class_attendance.status);
					//ATUALIZA ESTADO
				}
			}

			hideActivityIndicator();

		}


		

		async void OnClassAttendanceCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged ");


			if ((sender as CollectionView).SelectedItem != null)
			{
				Class_Attendance class_attendance = (sender as CollectionView).SelectedItem as Class_Attendance;

				Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged selected item = "+ class_attendance.classattendanceid);
				class_attendance.statuschanged = true;

				if ((class_attendance.status == "confirmada") | (class_attendance.status == "fechada"))
				{
					if (class_attendance.status == "confirmada")
					{
						alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
					}
					else if (class_attendance.status == "fechada")
					{
						alunosConfirmados--;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;

					}
					alunosAusentes++;
					ausentesLabel.Text = "Ausentes - " + alunosAusentes;

					if (class_attendance.classattendanceid != null)
					{
						Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = anulada");
						class_attendance.status = "anulada";
						class_attendance.color = Colors.Yellow;
                        class_attendance.colorImage = "yellow.png";
                    }
					else
					{
						Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = vazio");
						class_attendance.status = "";
						class_attendance.color = Colors.Transparent;
						class_attendance.colorImage = "transparent.png";

                    }


				}
				else
				{
					Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = confirmada");
					alunosAusentes--;
					ausentesLabel.Text = "Ausentes - " + alunosAusentes;
					alunosMarcados++;
					marcadosLabel.Text = "Marcados - " + alunosMarcados;

					class_attendance.status = "confirmada";
					class_attendance.color = Colors.Blue;
                    class_attendance.colorImage = "blue.png";

                }

				class_attendanceCollectionView.SelectedItem = null;
			}
			else
            {
				Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged selected item = nulll");
			}
		}

		async void OnAddPersonButtonClicked(object sender, EventArgs e)
		{
			OnConfirmButtonClicked(null, null);
			await Navigation.PushAsync(new AddPersonAttendancePageCS(this.class_schedule));
		}


        async void OnClassProgramButtonButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("AttendanceMonthPageCS.OnClassProgramButtonButtonClicked");
            await Navigation.PushAsync(new ClassProgramPageCS(class_schedule));
        }
    }
}
