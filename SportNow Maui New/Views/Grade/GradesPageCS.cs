using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;


namespace SportNow.Views
{
	public class GradesPageCS : DefaultPage
	{

		protected override void OnDisappearing() {
			if (collectionViewExaminations != null)
			{
				collectionViewExaminations.SelectedItem = null;
            }
        }

		private CollectionView collectionViewExaminations;

		private Member member;

		Microsoft.Maui.Controls.StackLayout stackButtons;

		MenuButton programasExameButton, minhasGraduacoesButton;

		
		public void initLayout()
		{
			Title = "GRADUAÇÕES";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png"

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);
			NavigationPage.SetBackButtonTitle(this, "");
		}

		public async void initSpecificLayout()
		{
			CreateStackButtons();
			CreateOptionButtons();
		}

		public void CreateStackButtons()
		{
			var buttonWidth = (App.screenWidth - 10 * App.screenWidthAdapter) / 3;


			minhasGraduacoesButton = new MenuButton("MINHAS GRADUAÇÕES", buttonWidth, 60);
			minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;

			programasExameButton = new MenuButton("PROGRAMAS EXAME", buttonWidth, 60);
			programasExameButton.button.Clicked += OnProgramasExameButtonClicked;


			stackButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5 * App.screenHeightAdapter,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60 * App.screenHeightAdapter,
				Children =
				{
					minhasGraduacoesButton,
					programasExameButton,
				}
			};

			absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			programasExameButton.deactivate();
			minhasGraduacoesButton.activate();

		}

		public void CreateOptionButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 3;


			minhasGraduacoesButton = new MenuButton("MINHAS GRADUAÇÕES", buttonWidth, 60);
			minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;



			programasExameButton = new MenuButton("PROGRAMAS EXAME", buttonWidth, 60);
			programasExameButton.button.Clicked += OnProgramasExameButtonClicked;


			stackButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
				Children =
				{
					minhasGraduacoesButton,
					programasExameButton,
				}
			};

			absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));


            programasExameButton.deactivate();
			minhasGraduacoesButton.activate();

		}

		public async void CreateMinhasGraduacoesColletion()
		{

			//COLLECTION GRADUACOES

			var vsg = new VisualStateGroup();
			var vs = new VisualState {
				Name = "Selected"
			};

			collectionViewExaminations = new CollectionView {
				SelectionMode = SelectionMode.Single,
				ItemsSource = Constants.belts, //member.examinations,
				ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não tem exames registados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewExaminations.SelectionChanged += OnCollectionViewSelectionChanged;



			List<Belt> member_belts = Constants.belts;
			bool isNextGradeLocked= false;

			foreach (Belt member_belt in member_belts) {
				Debug.WriteLine("member_belt = "+ member_belt.gradecode);
				foreach (Examination member_examination in member.examinations)
				{
					
					if (member_belt.gradecode == member_examination.grade) {
						member_belt.hasgrade = true;
					}
				}

				if (isNextGradeLocked == true)
				{
					member_belt.image = "belt_" + member_belt.gradecode + "_bloq.png";
				}

                if (member_belt.gradecode == member.grade)
				{
					isNextGradeLocked = true;
				}
			}

			Debug.WriteLine("member.grade = " + member.grade);

			collectionViewExaminations.ItemTemplate = new DataTemplate(() =>
			{

				Microsoft.Maui.Controls.Grid grid = new Microsoft.Maui.Controls.Grid { Padding = 10 };
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				//grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

				Image image = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				image.SetBinding(Image.SourceProperty, "image");

				Label gradeLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
				gradeLabel.SetBinding(Label.TextProperty, "grade");

				//Label locationLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap};
				//locationLabel.SetBinding(Label.TextProperty, "hasgrade");

				//vs.Setters.Add(new Setter { Property = grid.BackgroundColor, Value = Colors.Red });

				grid.Add(image, 0, 0);
				grid.Add(gradeLabel, 0, 1);
				//grid.Add(locationLabel, 0, 2);

				return grid;
			});



			absoluteLayout.Add(collectionViewExaminations);
            absoluteLayout.SetLayoutBounds(collectionViewExaminations, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 80 * App.screenHeightAdapter));
		}

		public GradesPageCS()
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.initLayout();
			this.initSpecificLayout();

			//Parent.

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}


		async Task<int> GetExaminations(Member member)
		{
			Debug.WriteLine("GetExaminations");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetExaminations(member);
			if (result == -1)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return result;
			}
			return result;
		}

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewSelectionChanged member.examinations.Count " + member.examinations.Count);


			if ((sender as CollectionView).SelectedItem != null) { 

				Belt belt = (sender as CollectionView).SelectedItem as Belt;

				Debug.WriteLine("OnCollectionViewSelectionChanged belt.gradecode " + belt.gradecode);

				foreach (Examination examination in member.examinations)
				{
					if (belt.gradecode == examination.grade)
					{
						await Navigation.PushAsync(new DetalheGraduacaoPageCS(member, examination));
					}
				}

				//Debug.WriteLine("OnCollectionViewSelectionChanged examination = " + examination.grade);

				//await Navigation.PushAsync(new DetalheGraduacaoPageCS(member, examination));
				/*Navigation.InsertPageBefore(new DetalheGraduacaoPageCS(examination), this);
				await Navigation.PopAsync();*/

				//(sender as CollectionView).SelectedItem = null;
			}
		}

		async void OnMinhasGraduacoesButtonClicked(object sender, EventArgs e)
		{

			programasExameButton.deactivate();
			minhasGraduacoesButton.activate();

			absoluteLayout.Add(collectionViewExaminations);
            absoluteLayout.SetLayoutBounds(collectionViewExaminations, new Rect(0, 60 * App.screenHeightAdapter, App.screenWidth, App.screenHeight));

			//absoluteLayout.Remove(collectionViewProximosEventos);

			//collectionViewProximasCompeticoes.IsVisible = false;
			//collectionViewResultadosCompeticoes.IsVisible = true;
			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}

		async void OnProgramasExameButtonClicked(object sender, EventArgs e)
		{
			programasExameButton.activate();
			minhasGraduacoesButton.deactivate();
			absoluteLayout.Remove(collectionViewExaminations);
		}

	}

}

