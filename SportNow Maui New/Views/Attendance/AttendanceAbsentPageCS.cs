using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Xml;

namespace SportNow.Views
{
	public class AttendanceAbsentPageCS : DefaultPage
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

		private CollectionView studentAbsentCollectionView;

		List<Student_Absence> student_Absence;

        public void initLayout()
		{
			Title = "FALTAS ALUNOS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			student_Absence = await GetStudentAbsence();
			CreateTitle();
			CreateStudentAbsentColletion();

			hideActivityIndicator();
		}


		public void CreateTitle()
		{

			Label absentLabel = new Label()
			{
                FontFamily = "futuracondensedmedium",
                Text = "Alunos a faltar há mais de 1 semana:",
				FontSize = App.bigTitleFontSize,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};

			absoluteLayout.Add(absentLabel);
            absoluteLayout.SetLayoutBounds(absentLabel, new Rect(0, 0, App.screenWidth, 80 * App.screenHeightAdapter));
		}

		public void CreateStudentAbsentColletion()
		{
			//COLLECTION GRADUACOES
			studentAbsentCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = student_Absence,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem alunos a faltar há mais de 1 semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

            //class_attendanceCollectionView.BindingContext = attendanceCollection;
            studentAbsentCollectionView.SelectionChanged += OnStudent_AbsentCollectionViewSelectionChanged;

            studentAbsentCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
					HeightRequest = 30 * App.screenHeightAdapter,
					WidthRequest = App.screenWidth
				};

				FormValue nicknameLabel = new FormValue("", 30 * App.screenHeightAdapter);
                nicknameLabel.label.SetBinding(Label.TextProperty, "name");
				nicknameLabel.label.FontFamily = "futuracondensedmedium";



                itemabsoluteLayout.Add(nicknameLabel);

                itemabsoluteLayout.SetLayoutBounds(nicknameLabel, new Rect(0, 0, App.screenWidth - 220 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                FormValue dojoLabel = new FormValue("", 30 * App.screenHeightAdapter);
                dojoLabel.label.FontSize = App.smallTextFontSize;
                dojoLabel.label.SetBinding(Label.TextProperty, "dojo");
				dojoLabel.label.FontFamily = "futuracondensedmedium";

				itemabsoluteLayout.Add(dojoLabel);
				itemabsoluteLayout.SetLayoutBounds(dojoLabel, new Rect((App.screenWidth - (215 * App.screenWidthAdapter)), 0, 100 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                FormValue dataUltimaAulaLabel = new FormValue("", 30 * App.screenHeightAdapter);
                dataUltimaAulaLabel.label.FontSize = App.smallTextFontSize;
                dataUltimaAulaLabel.label.SetBinding(Label.TextProperty, "ultima_presenca");
				dataUltimaAulaLabel.label.FontFamily = "futuracondensedmedium";

                itemabsoluteLayout.Add(dataUltimaAulaLabel);
	            itemabsoluteLayout.SetLayoutBounds(dataUltimaAulaLabel, new Rect(App.screenWidth - (110 * App.screenWidthAdapter), 0, 60 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                FormValue diasUltimaAulaLabel = new FormValue("", 30 * App.screenHeightAdapter);
				diasUltimaAulaLabel.label.FontSize = App.smallTextFontSize;
                diasUltimaAulaLabel.label.SetBinding(Label.TextProperty, "n_dias_ultima_presenca");
                diasUltimaAulaLabel.label.FontFamily = "futuracondensedmedium";

                itemabsoluteLayout.Add(diasUltimaAulaLabel);
				itemabsoluteLayout.SetLayoutBounds(diasUltimaAulaLabel, new Rect(App.screenWidth - (45 * App.screenWidthAdapter), 0, 40 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                return itemabsoluteLayout;
			});
			absoluteLayout.Add(studentAbsentCollectionView);
            absoluteLayout.SetLayoutBounds(studentAbsentCollectionView, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - (80 * App.screenHeightAdapter)));
		}

		public AttendanceAbsentPageCS()
		{
			//this.class_schedule = class_schedule;
			this.initLayout();
		}

		async Task<List<Student_Absence>> GetStudentAbsence()
		{
			ClassManager classManager = new ClassManager();
			List<Student_Absence> student_Absences = await classManager.GetStudentAbsence();
			if (student_Absences == null)
			{
				Debug.Print("class_attendances_i é null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			Debug.Print("student_Absences não é null");
			return student_Absences;
		}
		

		async void OnStudent_AbsentCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnStudent_AbsentCollectionViewSelectionChanged ");


			if ((sender as CollectionView).SelectedItem != null)
			{
				Student_Absence student_Absence = (sender as CollectionView).SelectedItem as Student_Absence;

                var actionSheet = await DisplayActionSheet("Contactar o Sócio " + student_Absence.name, "Cancelar", null, "Telefonar", "SMS", "WhatsApp");

                string result = "";
                switch (actionSheet)
                {
                    case "Cancelar":
                        break;
                    case "Telefonar":
                        PhoneDialer.Open(student_Absence.phoneNumber);
                        break;
                    case "SMS":
                        var message = new SmsMessage("Olá " + student_Absence.name + ", já não vens treinar há alguns dias. Está tudo bem contigo? Obrigado", new[] { student_Absence.phoneNumber });
                        await Sms.ComposeAsync(message);
                        break;
                    case "WhatsApp":
                        App.SendWhatsApp(student_Absence.phoneNumber, "Olá "+ student_Absence.name+", já não vens treinar há alguns dias. Está tudo bem contigo? Obrigado");
                        break;
                }

                Debug.WriteLine("OnStudent_AbsentCollectionViewSelectionChanged selected item = " + student_Absence.name);

			}

			else
            {
				Debug.WriteLine("OnStudent_AbsentCollectionViewSelectionChanged selected item = nulll");
			}
		}

    }
}
