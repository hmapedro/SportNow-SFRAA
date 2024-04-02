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
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Microsoft.Maui;


namespace SportNow.Views
{
	public class ExaminationEvaluationPageCS : DefaultPage
	{


		protected async override void OnAppearing()
		{
			CrossDeviceOrientation.Current.UnlockOrientation();

			await initSpecificLayout();
			AdaptScreen();
		}
		//during page close setting back to portrait
		protected override void OnDisappearing()
		{
			CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
		}

		private CollectionView collectionViewExaminationSessionCall;

		ObservableCollection<Examination> examinations;
		ObservableCollection<Examination_Result> examination_results;
		Examination_Session examination_session;
		Examination_ResultCollection examination_resultCollection;

		Label examinationMemberNameLabel;
		Label examinationGradeLabel;
		Label examinationTypeLabel;

		int currentExaminationIndex;
		double numberExaminationsToShow;
		double screenwidth;
		double screenheight;
		double sizeAdapter;
		ScrollView scrollView;
        AbsoluteLayout absoluteLayoutExamination;

		public void initLayout()
		{
			Title = "AVALIAÇÃO EXAMES";
		}


		public async Task<int> initSpecificLayout()
		{
			cleanExaminations();

			await CreateExamination_Result();
			examination_resultCollection = new Examination_ResultCollection();
			examination_resultCollection.Items = examination_results;
			return 0;
			//	CreateExamination_SessionCallColletionView();
			//CreateExamination_SessionCallView();
		}

		public void cleanExaminations()
		{

			ObservableCollection<Examination> examinations_new = new ObservableCollection<Examination>();
			foreach (Examination examination_i in examinations)
			{
				if (examination_i.selected == true)
				{
					examinations_new.Add(examination_i);
				}
			}
			this.examinations = examinations_new;
		}


		public async void CreateExamination_SessionCallView()
		{

            absoluteLayoutExamination = new AbsoluteLayout
			{
				Margin = new Thickness(10)
			};

			Debug.Print("currentExaminationIndex = "+ currentExaminationIndex);

			int examinationResultIndex = 0;
			double Xindex = 0;
			while (examinationResultIndex < numberExaminationsToShow)
			{
				Examination_Result examination_Result = examination_results[examinationResultIndex + currentExaminationIndex];

				Label memberNameLabel = new Label
				{
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = App.titleFontSize,
					TextColor = App.normalTextColor,
					Text = examination_Result.membername.ToUpper() + " - " + examination_Result.grade + " - " + examination_Result.type
				};

				Label evaluationLabel = new Label
				{
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = App.titleFontSize,
					TextColor = Color.FromRgb(246, 220, 178),
					Text = examination_Result.evaluation//Math.Round(double.Parse(examination_Result.evaluation), 2).ToString()
				};

				absoluteLayoutExamination.Add(memberNameLabel);
                absoluteLayoutExamination.SetLayoutBounds(memberNameLabel, new Rect(Xindex, 0, App.screenWidth / 5 * 4, 40 * App.screenHeightAdapter));

				absoluteLayoutExamination.Add(evaluationLabel);
                absoluteLayoutExamination.SetLayoutBounds(evaluationLabel, new Rect(Xindex + (screenwidth / 5 * 4), 0, App.screenWidth / 5, 40 * App.screenHeightAdapter));

				double Yindex = (40 * App.screenHeightAdapter);
				var evaluationList = new List<string>();
				evaluationList.Add("0");
				evaluationList.Add("1");
				evaluationList.Add("2");
				evaluationList.Add("3");
				evaluationList.Add("4");
				evaluationList.Add("5");

				Image youtubeImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60, Source = "youtube.png" };
				youtubeImage.SetBinding(Image.AutomationIdProperty, "video");

				var youtubeImage_tap = new TapGestureRecognizer();
				youtubeImage_tap.Tapped += async (s, e) =>
				{
					try
					{
						await Browser.OpenAsync(((Image)s).AutomationId, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
					}
				};
				youtubeImage.GestureRecognizers.Add(youtubeImage_tap);


				string currentTechnicType = "";
				foreach (Examination_Technic_Result examination_Technic_Result in examination_Result.examination_technics_result)
				{
					examination_Technic_Result.examinationResultId = examination_results[examinationResultIndex + currentExaminationIndex].id;

					if (currentTechnicType != examination_Technic_Result.type)
					{
						Label technicTypeLabel = new Label
						{
							BackgroundColor = Colors.Transparent,
							VerticalTextAlignment = TextAlignment.Center,
							HorizontalTextAlignment = TextAlignment.Start,
							FontSize = App.itemTitleFontSize,
							TextColor = Color.FromRgb(246, 220, 178),
							Text = examination_Technic_Result.type.ToUpper()
						};

                        absoluteLayoutExamination.Add(technicTypeLabel);
                        absoluteLayoutExamination.SetLayoutBounds(technicTypeLabel, new Rect(Xindex, Yindex, App.screenWidth / 5 * 4, 40 * App.screenHeightAdapter));

						Yindex = Yindex + 40 * App.screenHeightAdapter;
						currentTechnicType = examination_Technic_Result.type;
					}
					Label technicNameLabel = new Label
					{
						BackgroundColor = Colors.Transparent,
						VerticalTextAlignment = TextAlignment.Center,
						HorizontalTextAlignment = TextAlignment.Start,
						FontSize = App.itemTitleFontSize,
						TextColor = App.normalTextColor,
						Text = examination_Technic_Result.order + " - " + examination_Technic_Result.name
					};

					var tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.NumberOfTapsRequired = 2;
					
					tapGestureRecognizer.Tapped += (object sender, TappedEventArgs e) => {
						
						Debug.Print("Pressionou Técnica "+ examination_Technic_Result.name +" - "+ examination_Technic_Result.examinationResultId +" "+ examination_Technic_Result.id);
						openDescriptionWindow(examination_Technic_Result);
					};
					technicNameLabel.GestureRecognizers.Add(tapGestureRecognizer);
					var evaluationPicker = new Picker
					{
						Title = "",//examination_Technic_Result.name,
						TitleColor = Colors.White,
						BackgroundColor = Colors.Transparent,
						HorizontalTextAlignment = TextAlignment.Center,
						FontSize = App.itemTitleFontSize

					};
					evaluationPicker.ItemsSource = evaluationList;
					//Debug.Print("examination_Technic_Result.grade = " + examination_Technic_Result.grade);
					int evaluation = (int)double.Parse(examination_Technic_Result.grade, System.Globalization.CultureInfo.InvariantCulture);
					evaluationPicker.SelectedIndex = evaluation;
					if (evaluation != 0)
					{
						evaluationPicker.TextColor = Colors.LimeGreen;
					}
					else
					{
						evaluationPicker.TextColor = Color.FromRgb(246, 220, 178);
					}

					evaluationPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
					{
						int new_evaluation = Int32.Parse(evaluationPicker.SelectedItem.ToString());
						examination_Technic_Result.grade = new_evaluation.ToString();
						string global_evaluation = await UpdateExamination_Technic_Result(examination_Technic_Result.examinationResultId, examination_Technic_Result.id, new_evaluation,"");
						//examination_results[examinationResultIndex+currentExaminationIndex].evaluation = global_evaluation;
						updateGlobalEvaluation(examination_Technic_Result.examinationResultId, global_evaluation);
						evaluationLabel.Text = examination_Result.evaluation;//Math.Round(double.Parse(global_evaluation), 2).ToString();
						if (new_evaluation != 0)
						{
							evaluationPicker.TextColor = Colors.LimeGreen;
						}
						else
						{
							evaluationPicker.TextColor = Color.FromRgb(246, 220, 178);
						}
					};
                    absoluteLayoutExamination.Add(technicNameLabel);
                    absoluteLayoutExamination.SetLayoutBounds(technicNameLabel, new Rect(Xindex, Yindex, App.screenWidth / 5 * 4, 40 * App.screenHeightAdapter));


                    absoluteLayoutExamination.Children.Add(evaluationPicker);
                    absoluteLayoutExamination.SetLayoutBounds(evaluationPicker, new Rect(Xindex + (App.screenWidth / 5 * 4), Yindex, App.screenWidth / 5, 40 * App.screenHeightAdapter));

					Yindex = Yindex + 40 * App.screenHeightAdapter;
				}


				RoundButton confirmButton = new RoundButton("TERMINAR EXAME", 100, 50);
				confirmButton.button.Clicked += async (sender, ea) =>
				{
					Debug.Print("Examination to Close: examinationResultIndex = " + examinationResultIndex + " currentExaminationIndex = " + currentExaminationIndex);
					Debug.Print("Examination to Close:"+ examination_Result.name);
					_ = await openDescriptionExaminationResultWindow(examination_Result);
				};


                absoluteLayoutExamination.Add(confirmButton);
                absoluteLayoutExamination.SetLayoutBounds(confirmButton, new Rect(Xindex, getMaxYIndex() + 5 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));



				examinationResultIndex++;
				Xindex = Xindex + screenwidth;
			}


			scrollView = new ScrollView
			{
				//BackgroundColor = Colors.Green,
				Content = absoluteLayout,
				Orientation = ScrollOrientation.Vertical,
				WidthRequest = screenwidth * numberExaminationsToShow,
				HeightRequest = screenheight,
				MinimumWidthRequest = screenwidth * numberExaminationsToShow,
				MinimumHeightRequest = screenheight,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			
			scrollView.Content = absoluteLayoutExamination;

			absoluteLayout.Add(scrollView);
            absoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, App.screenWidth* numberExaminationsToShow, App.screenHeight));


			Content = scrollView;

			if (numberExaminationsToShow == 1) {
				var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
				leftSwipeGesture.Swiped += async (sender, e) => {
					Debug.Print("Swipe left");
					if (currentExaminationIndex < (examination_results.Count - 1))
					{
						currentExaminationIndex++;
						if (scrollView != null)
						{
							absoluteLayout.Remove(scrollView);
							scrollView = null;
						}
						CreateExamination_SessionCallView();
					}
				};
				var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
				rightSwipeGesture.Swiped += async (sender, e) => {
					Debug.Print("Swipe right");
					if (currentExaminationIndex > 0)
					{
						currentExaminationIndex--;
						if (scrollView != null)
						{
							absoluteLayout.Remove(scrollView);
							scrollView = null;
						}
						CreateExamination_SessionCallView();

					}
				};
				scrollView.GestureRecognizers.Add(leftSwipeGesture);
				scrollView.GestureRecognizers.Add(rightSwipeGesture);
			}

		}

		public double getMaxYIndex()
		{
			int examinationResultIndex = 0;
			double Yindex_Max = 0; // (40 * sizeAdapter);
			while (examinationResultIndex < numberExaminationsToShow)
			{
				double Yindex = examination_results[examinationResultIndex + currentExaminationIndex].examination_technics_result.Count * (40 * App.screenHeightAdapter) + (160 * App.screenHeightAdapter);
				if (Yindex > Yindex_Max)
				{
					Yindex_Max = Yindex;
				}
				examinationResultIndex++;
			}
			return Yindex_Max + (40 * App.screenHeightAdapter);
		}

		public async void openDescriptionWindow(Examination_Technic_Result examination_Technic_Result)
		{

            var input = await DisplayPromptAsync("Comentários", "Adicione Comentários sobre esta Técnica", "Ok", "Cancelar", initialValue: examination_Technic_Result.description, keyboard: Keyboard.Text);

            /*var promptConfig = new PromptConfig();
			promptConfig.InputType = InputType.Default;
			promptConfig.Text = examination_Technic_Result.description;
			promptConfig.IsCancellable = true;
			promptConfig.Message = "Adicione Comentários sobre esta Técnica";
			promptConfig.Title = "Comentários";
			promptConfig.OkText = "Ok";
			promptConfig.CancelText = "Cancelar";
			var input = await UserDialogs.Instance.PromptAsync(promptConfig);*/

			//var input = await UserDialogs.Instance.PromptAsync("Adicione Comentários sobre esta Técnica", "Comentários", "Ok", "Cancelar");
			

			if (input != null)
			{
				Debug.Print("O Comentário é " + input);
				string global_evaluation = await UpdateExamination_Technic_Result(examination_Technic_Result.examinationResultId, examination_Technic_Result.id, -1, input);
				examination_Technic_Result.description = input;
			}
		}

		public bool checkAllTechnicsEvaluated(Examination_Result examination_Result)
		{
			bool isFinished = true;

			foreach (Examination_Technic_Result examination_Technic_Result in examination_Result.examination_technics_result)
			{
				Debug.Print("examination_Technic_Result.grade = " + examination_Technic_Result.grade);
				if  (examination_Technic_Result.grade == "0.00")
				{
					isFinished = false;
				}
			}
			return isFinished;
		}

		public async Task<string> openDescriptionExaminationResultWindow(Examination_Result examination_Result)
		{

			bool isFinished = checkAllTechnicsEvaluated(examination_Result);
			if (isFinished == false)
			{
				await DisplayAlert("Exame ainda não finalizado", "É necessário avaliar todas as técnicas para poder finalizar o exame", "OK");
				return "";
			}
			else
			{

                var input = await DisplayPromptAsync("Comentários", "Adicione Comentários a este Exame", "Ok", "Cancelar", initialValue: examination_Result.description, keyboard: Keyboard.Text);
                /*var promptConfig = new PromptConfig();
				promptConfig.InputType = InputType.Default;
				promptConfig.Text = examination_Result.description;
				promptConfig.IsCancellable = true;
				promptConfig.Message = "Adicione Comentários a este Exame";
				promptConfig.Title = "Comentários";
				promptConfig.OkText = "Ok";
				promptConfig.CancelText = "Cancelar";
				var input = await UserDialogs.Instance.PromptAsync(promptConfig);*/

				//var input = await UserDialogs.Instance.PromptAsync("Adicione Comentários sobre esta Técnica", "Comentários", "Ok", "Cancelar");


				if (input != null)
				{
					Debug.Print("O Comentário é " + input);

					string global_evaluation = await UpdateExamination_Result(examination_Result.id, input);
					examination_Result.description = input;
				}
				return "";
			}
		}


		public void updateGlobalEvaluation(string examinationResultId, string global_evaluation)
		{
			foreach (Examination_Result examination_Result in examination_results)
			{
				if (examination_Result.id == examinationResultId) {
					examination_Result.evaluation = global_evaluation;
				}
			}
		}

		public ExaminationEvaluationPageCS(Examination_Session examination_session, ObservableCollection<Examination> examinations)
		{
			this.examinations = examinations;
			this.examination_session = examination_session;
			currentExaminationIndex = 0;
			this.initLayout();

			//this.initSpecificLayout();


			CrossDeviceOrientation.Current.OrientationChanged += (sender, args) =>
			{
				AdaptScreen();
			};
		}

		public async void AdaptScreen()
		{
			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
			Debug.Print("Orientation = " + CrossDeviceOrientation.Current.CurrentOrientation);
			Debug.Print("Height = " + (mainDisplayInfo.Height / mainDisplayInfo.Density) + " Width = " + (mainDisplayInfo.Width / mainDisplayInfo.Density));
			if (CrossDeviceOrientation.Current.CurrentOrientation.ToString() == "Portrait")
			{
				numberExaminationsToShow = 1;
				screenwidth = (mainDisplayInfo.Width - 20) / mainDisplayInfo.Density;
				screenheight = mainDisplayInfo.Height / mainDisplayInfo.Density;
				sizeAdapter = screenwidth / 400.0;
				Debug.Print("sizeAdapter =" + sizeAdapter);
				if (scrollView != null)
				{
					Content = null;
					if (absoluteLayout != null)
					{
						absoluteLayout.Remove(scrollView);
						absoluteLayout = null;
					}

                    absoluteLayoutExamination = null;
					scrollView = null;
					initLayout();
				}
				CreateExamination_SessionCallView();
			}
			else
			{
				currentExaminationIndex = 0;
				numberExaminationsToShow = examination_results.Count;
				Debug.Print("numberExaminationsToShow =" + numberExaminationsToShow);
				/*double screenwidth_aux = ((mainDisplayInfo.Width - 80) / mainDisplayInfo.Density) / numberExaminationsToShow;
				double screenheight_aux = mainDisplayInfo.Height / mainDisplayInfo.Density;

				if (screenheight_aux > screenwidth_aux)
				{
					screenwidth = screenheight_aux;
					screenheight = screenwidth_aux;
				}*/

				screenwidth = ((mainDisplayInfo.Width-80) / mainDisplayInfo.Density) / numberExaminationsToShow;
				screenheight = mainDisplayInfo.Height / mainDisplayInfo.Density;
				Debug.Print("screenwidth =" + screenwidth);
				sizeAdapter = screenwidth / 400.0;
				Debug.Print("sizeAdapter =" + sizeAdapter);
				if (scrollView != null)
				{
					Content = null;
					if (absoluteLayout != null)
                    {
						absoluteLayout.Remove(scrollView);
						absoluteLayout = null;
					}
                    absoluteLayoutExamination = null;
					scrollView = null;
					initLayout();
				}
				CreateExamination_SessionCallView();
			}
		}

		async Task<int> CreateExamination_Result()
        {
			showActivityIndicator();
			ExaminationResultManager examinationResultManager = new ExaminationResultManager();
			examination_results = new ObservableCollection<Examination_Result>();

			foreach (Examination examination in examinations)
			{
				var result = await examinationResultManager.CreateExamination_Result(App.original_member.id, examination.id);
				Debug.Print("Examination Result = " + result);
				if ((result == "-1") | (result == "-2"))
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor,
						BarTextColor = App.normalTextColor
					};
				}

				
				Examination_Result examination_Result = await examinationResultManager.GetExamination_Result_byID(App.original_member.id, result);
				if (examination_Result == null)
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor,
						BarTextColor = App.normalTextColor
					};
				}
				Debug.Print("examination_Result aqui =" + examination_Result.name + "examination_Result.type = "+ examination_Result.GetType());
				examination_results.Add((Examination_Result) examination_Result);
			}

			hideActivityIndicator();  //Hide loader
												  //UserDialogs.Instance.Alert(new AlertConfig() { Title = "EQUIPAMENTO SOLICITADO", Message = "A tua encomenda foi realizada com sucesso. Fala com o teu instrutor para saber quando te conseguirá entregar a mesma.", OkText = "Ok" });

			return 0;
		}

		async Task<string> UpdateExamination_Technic_Result(string examination_result_id, string examination_technic_id, int evaluation, string description)
		{
			showActivityIndicator();
			ExaminationResultManager examinationResultManager = new ExaminationResultManager();
			string media = "0";
			foreach (Examination examination in examinations)
			{
				media = await examinationResultManager.UpdateExamination_Technic_Result(App.original_member.id, examination_result_id, examination_technic_id, evaluation, description);
				if ((media == "-1") | (media == "-2"))
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor,
						BarTextColor = App.normalTextColor
					};
				}

				/*Examination_Result examination_Result = await examinationResultManager.GetExamination_Result_byID(App.original_member.id, result);
				if (examination_Result == null)
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor,
						BarTextColor = App.normalTextColor
					};
				}
				Debug.Print("examination_Result aqui =" + examination_Result.name + "examination_Result.type = " + examination_Result.GetType());
				examination_results.Add((Examination_Result)examination_Result);
				*/
			}
			hideActivityIndicator();

			return media;
		}

		async Task<string> UpdateExamination_Result(string examination_result_id, string description)
		{
			string result = "";

			showActivityIndicator();
			ExaminationResultManager examinationResultManager = new ExaminationResultManager();
			foreach (Examination examination in examinations)
			{
				result = await examinationResultManager.UpdateExamination_Result(App.original_member.id, examination_result_id, description);
				if ((result == "-1") | (result == "-2"))
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = App.backgroundColor,
						BarTextColor = App.normalTextColor
					};
				}
			}
			hideActivityIndicator();
			return result;
		}
	}
}
