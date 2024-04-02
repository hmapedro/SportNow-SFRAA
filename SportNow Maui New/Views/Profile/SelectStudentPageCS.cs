using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;

namespace SportNow.Views.Profile
{
	public class SelectStudentPageCS : DefaultPage
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
		ObservableCollection<Member> studends_filtered;
		FormValueEdit searchEntry;

        //private List<Member> members;

        public void initLayout()
		{
			Debug.Print("SelectStudentPageCS.initLayout");
			Title = "ESCOLHER ALUNO";

            var toolbarItem = new ToolbarItem
            {
                //Text = "Logout",
                IconImageSource = "iconalunosafaltar.png",

            };
            toolbarItem.Clicked += OnPerfilButtonClicked;
            ToolbarItems.Add(toolbarItem);

        }


        public void CleanScreen()
		{
			Debug.Print("SelectMemberPageCS.CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
            {
				absoluteLayout.Remove(stackButtons);
				absoluteLayout.Remove(collectionViewMembers);

				stackButtons = null;
				collectionViewMembers = null;
			}

		}

		public async void initSpecificLayout()
		{
			Label titleLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "Podes contactar ou utilizar a aplicação com a conta de um dos teus alunos:";

			absoluteLayout.Add(titleLabel);
			absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			App.member.students = await GetMemberStudents();

            CreateSearchEntry();
            CreateStudentsColletion();

			if (App.original_member.id != App.member.id)
			{
				RoundButton confirmButton = new RoundButton("VOLTAR ORIGINAL", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
				confirmButton.button.Clicked += OnVoltarOriginalButtonClicked;

				absoluteLayout.Add(confirmButton);
				absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenHeightAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			}
		}

		public void CreateSearchEntry()
		{
            searchEntry = new FormValueEdit("",Keyboard.Text, 45);
			searchEntry.entry.Placeholder = "Pesquisa...";
            searchEntry.entry.TextChanged += onSearchTextChange;
            absoluteLayout.Add(searchEntry);
            absoluteLayout.SetLayoutBounds(searchEntry, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

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

			studends_filtered = new ObservableCollection<Member>(App.member.students);

            Debug.Print("SelectMemberPageCS.CreateStudentsColletion");
			//COLLECTION GRADUACOES
			collectionViewStudents = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = studends_filtered,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não tem membros associados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewStudents.SelectionChanged += OnCollectionViewStudentsSelectionChanged;

			collectionViewStudents.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = 35 * App.screenHeightAdapter
				};

				FormValue numberLabel = new FormValue("", 35 * App.screenHeightAdapter);
				numberLabel.label.SetBinding(Label.TextProperty, "number_member");


				itemabsoluteLayout.Add(numberLabel);
				itemabsoluteLayout.SetLayoutBounds(numberLabel, new Rect(0,0, 50 * App.screenWidthAdapter, 35 * App.screenHeightAdapter));

				FormValue nicknameLabel = new FormValue("", 35 * App.screenHeightAdapter);
				nicknameLabel.label.SetBinding(Label.TextProperty, "name");


				itemabsoluteLayout.Add(nicknameLabel);
				itemabsoluteLayout.SetLayoutBounds(nicknameLabel, new Rect(55 * App.screenWidthAdapter, 0, (App.screenWidth - (55 * App.screenWidthAdapter)), 35 * App.screenHeightAdapter));

/*				FormValue dojoLabel = new FormValue("", 35 * App.screenHeightAdapter);
				dojoLabel.label.FontSize = App.formValueSmallFontSize;

                dojoLabel.label.SetBinding(Label.TextProperty, "dojo");

				itemabsoluteLayout.Add(dojoLabel);
				itemabsoluteLayout.SetLayoutBounds(dojoLabel, new Rect((((App.screenWidth) - ((App.screenWidth - (55 * App.screenWidthAdapter)) / 2))), 0, ((App.screenWidth - (55 * App.screenWidthAdapter)) / 2) - (5 * App.screenWidthAdapter), 35 * App.screenHeightAdapter));
*/
				return itemabsoluteLayout;
			});
			if (App.original_member.id != App.member.id)
			{
				absoluteLayout.Add(collectionViewStudents);
                absoluteLayout.SetLayoutBounds(collectionViewStudents, new Rect(0, 110 * App.screenHeightAdapter, App.screenWidth , App.screenHeight - 280 * App.screenHeightAdapter));
			}
			else
			{
				absoluteLayout.Add(collectionViewStudents);
                absoluteLayout.SetLayoutBounds(collectionViewStudents, new Rect(0, 110 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));
            }

		}

		public SelectStudentPageCS()
		{
			Debug.WriteLine("SelectStudentPageCS");
			this.initLayout();
			//this.initSpecificLayout();

		}


		async Task<List<Member>> GetMemberStudents()
		{
			MemberManager memberManager = new MemberManager();
			List<Member> students = await memberManager.GetMemberStudents(App.original_member.id);

			return students;
		}

		async void OnCollectionViewStudentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Member member = (sender as CollectionView).SelectedItem as Member;


                var actionSheet = await DisplayActionSheet("Sócio " + member.nickname, "Cancelar", null, "Login", "Telefonar", "SMS", "WhatsApp");

                string result = "";
				switch (actionSheet)
				{
					case "Cancelar":
						break;
					case "Login":
						App.member = member;
						App.member.students_count = await GetMemberStudents_Count(App.member.id);

						App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
						{
							BarBackgroundColor = App.backgroundColor,
							BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
						};

						break;
					case "Telefonar":
						PhoneDialer.Open(member.phone);
						break;
					case "SMS":
						var message = new SmsMessage("Olá " + member.nickname + ", ", new[] { member.phone });
						await Sms.ComposeAsync(message);
						break;
					case "WhatsApp":
						result = await App.SendWhatsApp(member.phone, "Olá " + member.nickname + ", ");
						if (result != "1")
						{
							await DisplayAlert("Problema com Whatsapp", "Não foi possível enviar mensagem por Whatsapp. Confirme que tem a aplicação correctamente instalada.", "Ok");
						}
						break;
				}


				/*Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
				await Navigation.PopToRootAsync();*/

				//await Navigation.PopAsync();
			}
		}

		async Task<int> GetMemberStudents_Count(string memberid)
		{
			Debug.WriteLine("MainTabbedPageCS.GetMemberStudents_Count");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetMemberStudents_Count(memberid);

			return result;
		}

        async void OnVoltarOriginalButtonClicked(object sender, EventArgs e)
        {
            App.member = App.original_member;


            /*await Navigation.PopAsync();
            await Navigation.PopAsync();
            await Navigation.PushAsync(new ProfilePageCS());*/
            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };

            //await Navigation.PopAsync();
            //await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
        }

        async void OnPerfilButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AttendanceAbsentPageCS());
        }
    }
}
