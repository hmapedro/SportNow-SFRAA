
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;

namespace SportNow.Views
{
	public class AddPersonAttendancePageCS : DefaultPage
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

		MenuButton proximosEventosButton;
		MenuButton participacoesEventosButton;

		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private CollectionView collectionViewMembers, collectionViewStudents;

		Class_Schedule class_Schedule;

        ObservableCollection<Member> studends_filtered;
        FormValueEdit searchEntry;

        //private List<Member> members;

        public void initLayout()
		{
			Title = "ESCOLHER ALUNO";
		}


		public void CleanScreen()
		{
			Debug.Print("AddPersonAttendancePageCS.CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			/*if (collectionViewMembers != null)
            {
				absoluteLayout.Remove(collectionViewMembers);
				collectionViewMembers = null;
			}*/

		}

		public async void initSpecificLayout()
		{

			showActivityIndicator();

			Label titleLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "Escolhe o aluno para o qual pretendes adicionar uma presença:";


			absoluteLayout.Add(titleLabel);
            absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0,0, App.screenWidth, 40 * App.screenHeightAdapter));

            _ = await CreateDojoPicker();
            CreateSearchEntry();

            App.member.students = await GetStudentsDojo(class_Schedule.dojo);

            studends_filtered = new ObservableCollection<Member>(App.member.students);


            CreateStudentsColletion();

			hideActivityIndicator();
		}

        public async Task<int> CreateDojoPicker()
        {
			List<string> dojoList = new List<string>();
			List<Dojo> dojos = await GetAllDojos();
			int selectedIndex = 0;
			int selectedIndex_temp = 0;

			foreach (Dojo dojo in dojos)
			{
				dojoList.Add(dojo.name);
				Debug.Print("dojo.name = " + dojo.name + " class_Schedule.dojo=" + class_Schedule.dojo);
				if (dojo.name == class_Schedule.dojo)
				{
					selectedIndex = selectedIndex_temp;
				}
				selectedIndex_temp++;
			}


			Debug.Print("selectedIndex = " + selectedIndex);

			var dojoPicker = new Picker
			{
                FontFamily = "futuracondensedmedium",
                Title = "",
				TitleColor = App.normalTextColor,
				BackgroundColor = Colors.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize

			};
			dojoPicker.ItemsSource = dojoList;
			dojoPicker.SelectedIndex = selectedIndex;

			dojoPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{

				showActivityIndicator();

				Debug.Print("DojoPicker selectedItem = " + dojoPicker.SelectedItem.ToString());
                App.member.students = await GetStudentsDojo(dojoPicker.SelectedItem.ToString());
                onSearchTextChange(null, null);

				absoluteLayout.Remove(collectionViewStudents);
				collectionViewStudents = null;
				CreateStudentsColletion();

				hideActivityIndicator();

			};

			absoluteLayout.Add(dojoPicker);
			absoluteLayout.SetLayoutBounds(dojoPicker, new Rect(0, 40 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

			return 1;
		}

        public void CreateSearchEntry()
        {
            searchEntry = new FormValueEdit("", Keyboard.Text, 45);
            searchEntry.entry.Placeholder = "Pesquisa...";
            searchEntry.entry.TextChanged += onSearchTextChange;
            absoluteLayout.Add(searchEntry);
            absoluteLayout.SetLayoutBounds(searchEntry, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

        }

        async void onSearchTextChange(object sender, EventArgs e)
        {
            Debug.WriteLine("SelectStudentPageCS.onSearchTextChange");
            if (searchEntry.entry.Text == "")
            {
                studends_filtered = new ObservableCollection<Member>(App.member.students);

            }
            else
            {
                studends_filtered = new ObservableCollection<Member>(App.member.students.Where(i => i.nickname.ToLower().Contains(searchEntry.entry.Text.ToLower())));
            }

            collectionViewStudents.ItemsSource = null;
            collectionViewStudents.ItemsSource = studends_filtered;
        }

        public void CreateStudentsColletion()
		{
			Debug.Print("AddPersonAttendancePageCS.CreateStudentsColletion");
			//COLLECTION GRADUACOES
			collectionViewStudents = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = studends_filtered,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não tem membros associados.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewStudents.SelectionChanged += OnCollectionViewStudentsSelectionChanged;

			collectionViewStudents.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
					HeightRequest = 30 * App.screenHeightAdapter
				};

				FormValue numberLabel = new FormValue("", 30 * App.screenHeightAdapter);
				numberLabel.label.SetBinding(Label.TextProperty, "number_member");


				itemabsoluteLayout.Add(numberLabel);
				itemabsoluteLayout.SetLayoutBounds(numberLabel, new Rect(0, 0, 50 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));


				FormValue nicknameLabel = new FormValue("", 30 * App.screenHeightAdapter);
                nicknameLabel.label.SetBinding(Label.TextProperty, "nickname");


				itemabsoluteLayout.Add(nicknameLabel);
                itemabsoluteLayout.SetLayoutBounds(nicknameLabel, new Rect(55 * App.screenWidthAdapter, 0, ((App.screenWidth - (55 * App.screenWidthAdapter)) / 2) - (5 * App.screenWidthAdapter), 30 * App.screenHeightAdapter));

				FormValue dojoLabel = new FormValue("", 30 * App.screenHeightAdapter);
                dojoLabel.label.SetBinding(Label.TextProperty, "dojo");

				itemabsoluteLayout.Add(dojoLabel);
				itemabsoluteLayout.SetLayoutBounds(dojoLabel, new Rect((((App.screenWidth) - ((App.screenWidth - (55 * App.screenWidthAdapter)) / 2))), 0, ((App.screenWidth - (55 * App.screenWidthAdapter)) / 2), 30 * App.screenHeightAdapter));


				return itemabsoluteLayout;
			});

			absoluteLayout.Add(collectionViewStudents);
            absoluteLayout.SetLayoutBounds(collectionViewStudents, new Rect(0, 160 * App.screenHeightAdapter, App.screenWidth, (App.screenHeight - (270 * App.screenHeightAdapter))));
		}

		public AddPersonAttendancePageCS(Class_Schedule class_Schedule)
		{
			Debug.WriteLine("AddPersonAttendancePageCS");
			this.class_Schedule = class_Schedule;
			this.initLayout();
			//this.initSpecificLayout();

		}


		async Task<List<Member>> GetStudentsDojo(string dojo)
		{
			MemberManager memberManager = new MemberManager();
			List<Member> students = await memberManager.GetStudentsDojo(App.original_member.id, dojo);

			return students;
		}

		async Task<List<Dojo>> GetAllDojos()
		{
			DojoManager dojoManager = new DojoManager();
			List<Dojo> dojos = await dojoManager.GetAllDojos();

			return dojos;
		}

		async void OnCollectionViewStudentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("AddPersonAttendancePageCS.OnCollectionViewMembersSelectionChanged");

			showActivityIndicator();

			if ((sender as CollectionView).SelectedItem != null)
			{

				Member member = (sender as CollectionView).SelectedItem as Member;

				ClassManager classmanager = new ClassManager();
				string class_attendance_id = await classmanager.CreateClass_Attendance(member.id, class_Schedule.classid, "fechada", class_Schedule.date);
				Debug.Print("class_attendance_id=" + class_attendance_id);

                bool res = await DisplayAlert("Associar definitivamente?", "Pretendes adicionar este aluno a esta aula de forma definitiva?", "Sim", "Não");

				if (res == true)
				{
					ClassManager classManager = new ClassManager();
					classManager.Update_Member_Add_To_Class(member.id, class_Schedule.classid);
                }

                await Navigation.PopAsync();

				/*Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
				await Navigation.PopToRootAsync();*/

				//await Navigation.PopAsync();
			}

			hideActivityIndicator();
		}
	}
}
