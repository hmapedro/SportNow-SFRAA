using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.ViewModel;
using System.Collections.ObjectModel;


namespace SportNow.Views
{
	public class ExaminationEvaluationCallPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            //competition_participation = App.competition_participation;
            initSpecificLayout();

		}

		protected override void OnDisappearing()
		{
			Debug.Print("OnDisappearing ExaminationSessionCallPageCS");
			absoluteLayout.Remove(collectionViewExaminationSessionCall);
			absoluteLayout.Remove(doExaminationButton);
			collectionViewExaminationSessionCall = null;
			doExaminationButton = null;
		}

		private CollectionView collectionViewExaminationSessionCall;

		private Examination_Session examination_session;

		private ObservableCollection<Examination> examination_sessionCall;

		private List<Examination> selectedExaminations;

		private ExaminationCollection examinationCollection;

		Button doExaminationButton;

		Label examinationSessionNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "CONVOCATÓRIA";
		}


		public async void initSpecificLayout()
		{
			examination_sessionCall = await GetExamination_SessionCall();

			CompleteExamination();

			examinationCollection = new ExaminationCollection();
			examinationCollection.Items = examination_sessionCall;
			CreateExamination_SessionCallColletionView();

		}


		public void CompleteExamination()
		{
			foreach (Examination examination_i in examination_sessionCall)
			{
				examination_i.selectedColor = Colors.White;
			}
		}

		public void CreateExamination_SessionCallColletionView()
		{

			foreach (Examination examination in examination_sessionCall)
			{
				Debug.Print("examination.estado=" + examination.estado);
				if (examination.estado == "confirmado")
				{
					examination.estadoTextColor = Color.FromRgb(96, 182, 89) ;
				}
				examination.gradeLabel = Constants.grades[examination.grade];
			}

			examinationSessionNameLabel = new Label
			{
				Text = examination_session.name,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.normalTextColor
			};

			absoluteLayout.Add(examinationSessionNameLabel);
            absoluteLayout.SetLayoutBounds(examinationSessionNameLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));


			if (examination_sessionCall.Count > 0)
            {
				nameTitleLabel = new Label
				{
					Text = "NOME",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = Color.FromRgb(246, 220, 178),
					LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(nameTitleLabel);
                absoluteLayout.SetLayoutBounds(examinationSessionNameLabel, new Rect(0, 50 * App.screenHeightAdapter, (App.screenWidth / 3 * 2) - 10 * App.screenHeightAdapter, 40 * App.screenHeightAdapter));

				categoryTitleLabel = new Label
				{
					Text = "EXAME PARA",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = Color.FromRgb(246, 220, 178),
					LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(categoryTitleLabel);
                absoluteLayout.SetLayoutBounds(categoryTitleLabel, new Rect((App.screenWidth / 3 * 2), 50 * App.screenHeightAdapter, (App.screenWidth / 3 * 2) - 10 * App.screenHeightAdapter, 40 * App.screenHeightAdapter));
			}

			collectionViewExaminationSessionCall = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = examination_sessionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não foi criada convocatória para esta Sessão de Exames.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Colors.Red, FontSize = 20 },
			}
					}
				}
			};

			this.BindingContext = examinationCollection;
			collectionViewExaminationSessionCall.SetBinding(ItemsView.ItemsSourceProperty, "Items");

			collectionViewExaminationSessionCall.SelectionChanged += OncollectionViewExaminationSessionCallSelectionChanged;

			collectionViewExaminationSessionCall.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "membername");
				nameLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame nameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Colors.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemabsoluteLayout.Add(nameFrame);
	            itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 0, (App.screenWidth / 3 * 2) - 10 * App.screenHeightAdapter, 40 * App.screenHeightAdapter));

				Label categoryLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "gradeLabel");
				categoryLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame categoryFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Colors.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				categoryFrame.Content = categoryLabel;

				itemabsoluteLayout.Add(categoryFrame);
				itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect((App.screenWidth / 3 * 2), 0, (App.screenWidth / 3) - 10 * App.screenHeightAdapter, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

			absoluteLayout.Add(collectionViewExaminationSessionCall);
            absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 170 * App.screenHeightAdapter));

			doExaminationButton = new Button
			{
				Text = "FAZER EXAME",
				BackgroundColor = Color.FromRgb(96, 182, 89),
				TextColor = App.normalTextColor,
				FontSize = App.itemTitleFontSize,
				WidthRequest = 100,
				HeightRequest = 50
			};

			Frame frame_doExaminationButton = new Frame
			{
				BorderColor = Color.FromRgb(96, 182, 89),
				WidthRequest = 100,
				HeightRequest = 50,
				CornerRadius = 10,
				IsClippedToBounds = true,
				Padding = 0
			};

			frame_doExaminationButton.Content = doExaminationButton;
			doExaminationButton.Clicked += doExaminationButtonClicked;

			absoluteLayout.Add(frame_doExaminationButton);
            absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, App.screenHeight - 60 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
		}

		public ExaminationEvaluationCallPageCS(Examination_Session examination_session)
		{
			this.examination_session = examination_session;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<ObservableCollection<Examination>> GetExamination_SessionCall()
		{
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			ObservableCollection<Examination> examination_sessionCall = await examination_sessionManager.GetExamination_SessionCall_obs(examination_session.id);
			if (examination_sessionCall == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return examination_sessionCall;
		}

		async void doExaminationButtonClicked(object sender, EventArgs e)
		{
			doExaminationButton.IsEnabled = false;

			int selectedExaminationCount = 0;

			foreach (Examination examination_i in examination_sessionCall)
			{
				if (examination_i.selected == true)
				{
					selectedExaminationCount++;
				}
				Debug.Print("Examinado = " + examination_i.membername + " selected = " + examination_i.selected);
			}

			if (selectedExaminationCount == 0)
			{
				await DisplayAlert("ESCOLHA VAZIA", "Tens de escolher pelo menos um exame.", "Ok" );
				doExaminationButton.IsEnabled = true;
			}
			else if (selectedExaminationCount > 4)
			{
                await DisplayAlert("MÁXIMO EXAMES A AVALIAR", "Não podes escolher mais de 4 exames para avaliar.", "Ok" );
				doExaminationButton.IsEnabled = true;
			}
			else
			{
				/*doExaminationButton.IsEnabled = true;
				await Navigation.PushAsync(new ExaminationEvaluationConfirmPageCS(examination_session, examination_sessionCall));
				*/
				var result = await DisplayAlert("Confirmas que pretendes avaliar estes exames?", "Confirmar seleção", "Sim", "Não");
				if (result)
				{
					await Navigation.PushAsync(new ExaminationEvaluationPageCS(examination_session, examination_sessionCall));
				}
				else
				{
					doExaminationButton.IsEnabled = true;
				}
			}


			
		}


		void OncollectionViewExaminationSessionCallSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.Print("OncollectionViewExaminationSessionCallSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Examination selectedExamination = (sender as CollectionView).SelectedItem as Examination;

				Debug.WriteLine("OncollectionViewExaminationSessionCallSelectionChanged selected item = " + selectedExamination.membername);
				if (selectedExamination.selected == true)
				{
					selectedExamination.selected = false;
					selectedExamination.selectedColor = Colors.White;
				}
				else
				{
					selectedExamination.selected = true;
					selectedExamination.selectedColor = Colors.Green;
				}

				(sender as CollectionView).SelectedItem = null;
			}
			else
			{
				Debug.WriteLine("OncollectionViewExaminationSessionCallSelectionChanged selected item = nulll");
			}
		}
	}
}
