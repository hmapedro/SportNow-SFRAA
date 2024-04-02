using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;


namespace SportNow.Views
{
	public class myGradesPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			if (collectionViewExaminations != null)
			{
				collectionViewExaminations.SelectedItem = null;
			}
		}


		private AbsoluteLayout graduacaoTimingabsoluteLayout;

		private CollectionView collectionViewExaminations;

		//private Member member;

		Microsoft.Maui.Controls.StackLayout stackButtons;
		Microsoft.Maui.Controls.StackLayout stackProgramasExameButtons;

		MenuButton programasExameButton, minhasGraduacoesButton;

		OptionButton under6OptionButton, under12OptionButton, over12OptionButton;

		Examination_Timing examination_Timing;


        public void initLayout()
		{
			Title = "GRADUAÇÕES";
		}

		public async void initSpecificLayout(string type)
		{
			//member = App.member;

			var result = await GetExaminations(App.member);

            examination_Timing = await GetExamination_Timing(App.member);

            CreateStackButtons(type);
			CreateMinhasGraduacoesColletion();
			CreateGraduacoesTiming();
			CreateProgramasExame();
			//CreateParticipacoesEventosColletion();

			if (type == "ProgramasExame")
			{
				OnProgramasExameButtonClicked(null, null);

			}
			else if (type == "MinhasGraduaçoes")
			{
				OnMinhasGraduacoesButtonClicked(null, null);

			}
		}

		public void CreateStackButtons(string type)
		{
			var width = App.screenWidth;
			var buttonWidth = (width - 5) / 2;


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

		}


		public async void CreateGraduacoesTiming()
		{
			if (examination_Timing != null)
			{
                Debug.Print("examination_Timing.neededmonths = " + examination_Timing.neededmonths);
                Debug.Print("examination_Timing.doneclasses = " + examination_Timing.doneclasses);
                Debug.Print("examination_Timing.neededclassesok = " + examination_Timing.neededclassesok);
                graduacaoTimingabsoluteLayout = new AbsoluteLayout
                {
                    Margin = new Thickness(0)
                };

                Label nextExaminationLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };
                nextExaminationLabel.Text = "PRÓXIMO EXAME";

                graduacaoTimingabsoluteLayout.Add(nextExaminationLabel);
                graduacaoTimingabsoluteLayout.SetLayoutBounds(nextExaminationLabel, new Rect((App.screenWidth / 2) - 100 * App.screenWidthAdapter, 0, 200 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

                Label monthsLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
                monthsLabel.Text = "Tempo Necessário";

                graduacaoTimingabsoluteLayout.Add(monthsLabel);
                graduacaoTimingabsoluteLayout.SetLayoutBounds(monthsLabel, new Rect(0, 40 * App.screenHeightAdapter, 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                Label monthsValueLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
                monthsValueLabel.Text = examination_Timing.donemonths + "/" + examination_Timing.neededmonths + " meses";

                if (examination_Timing.neededmonthsok == "OK")
                {
                    monthsValueLabel.TextColor = Colors.Green;
                }
                else
                {
                    monthsValueLabel.TextColor = Colors.Red;
                }

                graduacaoTimingabsoluteLayout.Add(monthsValueLabel);
                graduacaoTimingabsoluteLayout.SetLayoutBounds(monthsValueLabel, new Rect(0, 70 * App.screenHeightAdapter, 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                Label classesLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
                classesLabel.Text = "Aulas Necessárias";

                graduacaoTimingabsoluteLayout.Add(classesLabel);
                absoluteLayout.SetLayoutBounds(classesLabel, new Rect((App.screenWidth) - 120 * App.screenWidthAdapter, 40 * App.screenHeightAdapter, 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                Label classesValueLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
                classesValueLabel.Text = examination_Timing.doneclasses + "/" + examination_Timing.neededclasses;

                if (examination_Timing.neededclassesok == "OK")
                {
                    classesValueLabel.TextColor = Colors.Green;
                }
                else
                {
                    classesValueLabel.TextColor = Colors.Red;
                }

                graduacaoTimingabsoluteLayout.Add(classesValueLabel);
                graduacaoTimingabsoluteLayout.SetLayoutBounds(classesValueLabel, new Rect((App.screenWidth) - 120 * App.screenWidthAdapter, 70 * App.screenHeightAdapter, 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));



                absoluteLayout.Add(graduacaoTimingabsoluteLayout);
                absoluteLayout.SetLayoutBounds(graduacaoTimingabsoluteLayout, new Rect(0, App.screenHeight - 200 * App.screenHeightAdapter, App.screenWidth, 120 * App.screenHeightAdapter));

            }

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

			Debug.WriteLine("member.grade = " + App.member.grade + " "+isNextGradeLocked);
			if (App.member.grade == "10_Kyu") {
				
				isNextGradeLocked = true;
			}

			foreach (Belt member_belt in member_belts) {
				//Debug.WriteLine("member_belt = "+ member_belt.gradecode);
				foreach (Examination member_examination in App.member.examinations)
				{
					
					if (member_belt.gradecode == member_examination.grade) {
						member_belt.hasgrade = true;
					}
				}

				//Debug.WriteLine("member_belt.grade = " + member_belt.gra + " " + isNextGradeLocked);

				if (isNextGradeLocked == true)
				{
					member_belt.image = "belt_" + member_belt.gradecode.ToLower() + "_bloq.png";
				}
				else
				{
					member_belt.image = "belt_" + member_belt.gradecode.ToLower() + ".png";
				}

                if (member_belt.gradecode == App.member.grade)
				{
					Debug.WriteLine("member_belt.gradecode == member.grade poe a true " + member_belt.gradecode);
					isNextGradeLocked = true;
				}
			}

			collectionViewExaminations.ItemTemplate = new DataTemplate(() =>
			{

				Microsoft.Maui.Controls.Grid grid = new Microsoft.Maui.Controls.Grid { Padding = 10 };
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				//grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

				Image image = new Image { Aspect = Aspect.AspectFit, WidthRequest = (App.screenWidth / 3) - (30 * App.screenWidthAdapter) }; //, HeightRequest = 60, WidthRequest = 60
				image.SetBinding(Image.SourceProperty, "image");

				Label gradeLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };
				gradeLabel.SetBinding(Label.TextProperty, "grade");

				//Label locationLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap};
				//locationLabel.SetBinding(Label.TextProperty, "hasgrade");

				//vs.Setters.Add(new Setter { Property = grid.BackgroundColor, Value = Colors.Red });

				grid.Add(image, 0, 0);
				grid.Add(gradeLabel, 0, 1);
				//grid.Add(locationLabel, 0, 2);

				return grid;
			});



			/*absoluteLayout.Add(collectionViewExaminations);
            absoluteLayout.SetLayoutBounds(collectionViewExaminations, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 320 * App.screenHeightAdapter));*/
		}

		public void CreateProgramasExame()
        {
			var buttonWidth = (App.screenWidth) / 2;

			under6OptionButton = new OptionButton("LÚDICO", "ludico.png", buttonWidth, 100 * App.screenHeightAdapter);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var under6OptionButton_tap = new TapGestureRecognizer();
			under6OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("ludico"));
			};
			under6OptionButton.GestureRecognizers.Add(under6OptionButton_tap);

			under12OptionButton = new OptionButton("TRADICIONAL", "tradicional.png", buttonWidth, 100 * App.screenHeightAdapter);
			var under12OptionButton_tap = new TapGestureRecognizer();
			under12OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("tradicional"));
			};
			under12OptionButton.GestureRecognizers.Add(under12OptionButton_tap);

			over12OptionButton = new OptionButton("COMPETIÇÃO", "competicao.png", buttonWidth, 100 * App.screenHeightAdapter);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var over12OptionButton_tap = new TapGestureRecognizer();
			over12OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("competicao"));
			};
			over12OptionButton.GestureRecognizers.Add(over12OptionButton_tap);

			
			stackProgramasExameButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 50 * App.screenHeightAdapter,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 430 * App.screenHeightAdapter,
				Children =
				{
					under6OptionButton,
					under12OptionButton,
					over12OptionButton
				}
			};

			/*absoluteLayout.Add(stackProgramasExameButtons);
            absoluteLayout.SetLayoutBounds(stackProgramasExameButtons, new Rect((App.screenWidth/ 4), 0 * App.screenHeightAdapter, App.screenWidth / 2, 430 * App.screenHeightAdapter));*/
		}

		public myGradesPageCS(string type)
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.initLayout();
			this.initSpecificLayout(type);

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


        async Task<Examination_Timing> GetExamination_Timing(Member member)
        {
            Debug.WriteLine("GetExamination_Timing");
            ExaminationManager examinationManager = new ExaminationManager();

            Examination_Timing examination_timing = await examinationManager.GetExamination_Timing(member.id);
            /*if (examination_timing == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
                return examination_timing;
            }*/
            return examination_timing;
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewSelectionChanged member.examinations.Count " + App.member.examinations.Count);


			if ((sender as CollectionView).SelectedItem != null) { 

				Belt belt = (sender as CollectionView).SelectedItem as Belt;

				Debug.WriteLine("OnCollectionViewSelectionChanged belt.gradecode " + belt.gradecode);

				foreach (Examination examination in App.member.examinations)
				{
					if (belt.gradecode == examination.grade)
					{
						await Navigation.PushAsync(new DetalheGraduacaoPageCS(App.member, examination));
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
            absoluteLayout.SetLayoutBounds(collectionViewExaminations, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));

			absoluteLayout.Remove(stackProgramasExameButtons);

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

			absoluteLayout.Add(stackProgramasExameButtons);
            absoluteLayout.SetLayoutBounds(stackProgramasExameButtons, new Rect(App.screenWidth / 4, 80 * App.screenHeightAdapter, App.screenWidth / 2, 430 * App.screenHeightAdapter));

			absoluteLayout.Remove(collectionViewExaminations);

		}

	}

}

