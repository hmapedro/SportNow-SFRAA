using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class MonthFeeListPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private CollectionView monthFeesCollectionView;

		public Label currentMonth;

		public StackLayout stackMonthSelector;

		DateTime selectedTime;

		ObservableCollection<MonthFee> monthFees;

		Picker dojoPicker;

		RoundButton approveAllButton;

		public void initLayout()
		{
			Title = "MENSALIDADES INSTRUTOR";
		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");

			if (monthFeesCollectionView != null)
			{
                absoluteLayout.Remove(monthFeesCollectionView);
                monthFeesCollectionView = null;

				absoluteLayout.Remove(dojoPicker);
				absoluteLayout.Remove(stackMonthSelector);
            }
        }

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateMonthSelector();
			_ = await CreateDojoPicker();
			monthFees = await GetMonthFeesbyDojo();
			CreateMonthFeesColletion();

			hideActivityIndicator();
		}


		public async Task<int> CreateDojoPicker()
		{
			Debug.Print("CreateDojoPicker");
			List<string> dojoList = new List<string>();
			List<Dojo> dojos = await GetAllDojos();
			int selectedIndex = 0;
			int selectedIndex_temp = 0;

			foreach (Dojo dojo in dojos)
			{
				dojoList.Add(dojo.name);
				if (dojo.name == App.member.dojo)
				{
					selectedIndex = selectedIndex_temp;
				}
				selectedIndex_temp++;
			}

			dojoPicker = new Picker
			{
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

				monthFees = await GetMonthFeesbyDojo();

				monthFeesCollectionView.ItemsSource = monthFees;

				/*students = await GetStudentsDojo(dojoPicker.SelectedItem.ToString());
				absoluteLayout.Remove(collectionViewStudents);
				collectionViewStudents = null;
				CreateStudentsColletion();*/

				hideActivityIndicator();

			};

			absoluteLayout.Add(dojoPicker);
            absoluteLayout.SetLayoutBounds(dojoPicker, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));

			return 1;
		}

		public void CreateMonthSelector()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 3;

			//DateTime currentTime = DateTime.Now;
			selectedTime = DateTime.Now;

			Button previousMonthButton = new Button();
			Button nextMonthButton = new Button();

            previousMonthButton = new Button()
            {
                FontFamily = "futuracondensedmedium",
                Text = "<",
                FontSize = App.titleFontSize,
                TextColor = App.topColor,
                BackgroundColor = App.backgroundColor,
                VerticalOptions = LayoutOptions.Center
            };
            previousMonthButton.Clicked += OnPreviousButtonClicked;

			currentMonth = new Label()
			{
                FontFamily = "futuracondensedmedium",
                Text = selectedTime.Year + " - " + selectedTime.Month,
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				WidthRequest = 150,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

            nextMonthButton = new Button()
            {
                FontFamily = "futuracondensedmedium",
                Text = ">",
                FontSize = App.titleFontSize,
                TextColor = App.topColor,
                BackgroundColor = App.backgroundColor,
                VerticalOptions = LayoutOptions.Center
            };

            nextMonthButton.Clicked += OnNextButtonClicked;

			stackMonthSelector = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5 * App.screenHeightAdapter,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40 * App.screenHeightAdapter,
				Children =
				{
					previousMonthButton,
					currentMonth,
					nextMonthButton
				}
			};

			absoluteLayout.Add(stackMonthSelector);
            absoluteLayout.SetLayoutBounds(stackMonthSelector, new Rect(0, 40 * App.screenHeightAdapter, App.screenWidth, 40 * App.screenHeightAdapter));
		}

		public void CreateMonthFeesColletion()
		{
			//COLLECTION GRADUACOES
			monthFeesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = monthFees,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Mensalidades deste Dojo para este mês.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			monthFeesCollectionView.SelectionChanged += OncollectionViewMonthFeesSelectionChangedAsync;

			monthFeesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = 30 * App.screenHeightAdapter,
                    WidthRequest = App.screenWidth
                };

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = Colors.Transparent,
                    Padding = new Thickness(2, 2, 2, 2),
                    HeightRequest = 30 * App.screenHeightAdapter,
                    VerticalOptions = LayoutOptions.Center,
                };

                //itemFrame.Content = itemabsoluteLayout;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.screenWidth, 30 * App.screenHeightAdapter));

                Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membernickname");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(5 * App.screenWidthAdapter, 0, App.screenWidth - 280 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				Label valueLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				valueLabel.SetBinding(Label.TextProperty, "value");

				itemabsoluteLayout.Add(valueLabel);
				itemabsoluteLayout.SetLayoutBounds(valueLabel, new Rect(App.screenWidth - (215 * App.screenWidthAdapter), 0, 60 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				Label statusLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				statusLabel.SetBinding(Label.TextProperty, "status");
				statusLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				itemabsoluteLayout.Add(statusLabel);
                itemabsoluteLayout.SetLayoutBounds(statusLabel, new Rect(App.screenWidth - (150 * App.screenWidthAdapter), 0, 150 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

 				return itemabsoluteLayout;
			});

			absoluteLayout.Add(monthFeesCollectionView);
            absoluteLayout.SetLayoutBounds(monthFeesCollectionView, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 270 * App.screenHeightAdapter));
		}

		public void createApproveButtons()
		{
            /*approveSelectedButton = new Button
			{
				Text = "APROVAR SELECCIONADOS",
				BackgroundColor = Color.FromRgb(96, 182, 89),
				TextColor = App.normalTextColor,
				FontSize = App.itemTitleFontSize,
				WidthRequest = 100,
				HeightRequest = 50
			};

			Frame frame_approveSelectedButton = new Frame
			{
				BorderColor = Color.FromRgb(96, 182, 89),
				WidthRequest = 100,
				HeightRequest = 50,
				CornerRadius = 10,
				IsClippedToBounds = true,
				Padding = 0
			};

			frame_approveSelectedButton.Content = approveSelectedButton;
			approveSelectedButton.Clicked += approveSelectedButtonClicked;

			absoluteLayout.Add(frame_approveSelectedButton,
				xConstraint: )0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 60; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width/2)-5; // center of image (which is 40 wide)
				}),
				heightConstraint: )50)
			);*/


            approveAllButton = new RoundButton("APROVAR TODOS", App.screenWidth - 20 * App.screenWidthAdapter, 50);
            approveAllButton.button.Clicked += approveAllButtonClicked;


            absoluteLayout.Add(approveAllButton);
            absoluteLayout.SetLayoutBounds(approveAllButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
        }

        public MonthFeeListPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Dojo>> GetAllDojos()
		{
			DojoManager dojoManager = new DojoManager();
			List<Dojo> dojos = await dojoManager.GetAllDojos();

			return dojos;
		}


		async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			selectedTime = selectedTime.AddMonths(-1);
			currentMonth.Text = selectedTime.Year + " - " + selectedTime.Month;

			monthFees = await GetMonthFeesbyDojo();

			monthFeesCollectionView.ItemsSource = monthFees;

			hideActivityIndicator();
		}

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.Print("selectedTime antes = " + selectedTime.ToShortDateString());
			selectedTime = selectedTime.AddMonths(1);
			Debug.Print("selectedTime = " + selectedTime.ToShortDateString());
			currentMonth.Text = selectedTime.Year + " - " + selectedTime.Month;
			monthFees = await GetMonthFeesbyDojo();

			monthFeesCollectionView.ItemsSource = monthFees;

			hideActivityIndicator();
		}

		async Task<ObservableCollection<MonthFee>> GetMonthFeesbyDojo()
		{
			Debug.WriteLine("GetMonthFeesbyDojo " + dojoPicker.SelectedItem.ToString());
			MonthFeeManager monthFeeManager = new MonthFeeManager();
			ObservableCollection<MonthFee> monthFees = await monthFeeManager.GetMonthFeesbyDojo(dojoPicker.SelectedItem.ToString(), selectedTime.Year.ToString(), selectedTime.Month.ToString());
			if (monthFees == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}

			monthFees = correctMonthFees(monthFees);

			return monthFees;
		}


		public ObservableCollection<MonthFee> correctMonthFees(ObservableCollection<MonthFee> monthFees)
		{
			bool hasMonthFeesToApprove = false;

			Debug.Print("correctMonthFees");

			foreach (MonthFee monthFee in monthFees)
			{
				monthFee.selected = false;
				monthFee.selectedColor = Colors.White;
				Debug.Print("monthFee.status = " + monthFee.status);

				if (monthFee.status == "por_aprovar")
				{
					monthFee.selectedColor = Colors.DarkOrange;
					monthFee.status = "Por Aprovar";
					hasMonthFeesToApprove = true;
				}
                if (monthFee.status == "emitida")
                {
                    monthFee.selectedColor = Colors.DarkBlue;
                    monthFee.status = "Emitida";
                }
				else if (monthFee.status == "paga")
				{
					monthFee.selectedColor = Colors.DarkGreen;
					monthFee.status = "Paga";
				}
			}

			if (hasMonthFeesToApprove == true)
			{
				createApproveButtons();
			}

			return monthFees;
		}

		void OncollectionViewMonthFeesSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
		{
			Debug.Print("OncollectionViewMonthFeesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				MonthFee selectedMonthFee = (sender as CollectionView).SelectedItem as MonthFee;

				Debug.WriteLine("OncollectionViewExaminationSessionCallSelectionChanged selected item = " + selectedMonthFee.membernickname);
				if (selectedMonthFee.status == "Por Aprovar")
				{
					changeMonthFeeValuePrompt(selectedMonthFee);
				}
				else if ((selectedMonthFee.status == "Em pagamento") | (selectedMonthFee.status == "Pagamento em Atraso") | (selectedMonthFee.status == "Emitida"))
				{
					changeMonthFeeStatusPrompt(selectedMonthFee);
				}
				else if (selectedMonthFee.status == "Paga")
				{
					InvoiceDocument(selectedMonthFee);
				}
				(sender as CollectionView).SelectedItem = null;
			}
			else
			{
				Debug.WriteLine("OncollectionViewMonthFeesSelectionChanged selected item = nulll");
			}
		}


		async void changeMonthFeeValuePrompt(MonthFee monthFee)
		{
			showActivityIndicator();

			var input = await DisplayPromptAsync("Mensalidade", "Indica o valor da mensalidade a aplicar a este aluno", "Ok", "Cancelar", initialValue: monthFee.value, keyboard: Keyboard.Text);

            /*var promptConfig = new PromptConfig();
			promptConfig.InputType = InputType.Default;
			promptConfig.Text = monthFee.value;
			promptConfig.IsCancellable = true;
			promptConfig.Message = "Indica o valor da mensalidade a aplicar a este aluno";
			promptConfig.Title = "Mensalidade";
			promptConfig.OkText = "Ok";
			promptConfig.CancelText = "Cancelar";
			var input = await UserDialogs.Instance.PromptAsync(promptConfig);*/

			if (input != null)
			{
				string new_value = input;
				var charsToRemove = new string[] { "$", "€"};
				foreach (var c in charsToRemove)
				{
					new_value = new_value.Replace(c, string.Empty);
				}

                var charsToRemoveComma = new string[] { ","};
                foreach (var c in charsToRemoveComma)
                {
                    new_value = new_value.Replace(c, ".");
                }

                Debug.Print("O Valor da Mensalidade é " + new_value);

				if (IsDoubleRealNumber(new_value))
				{
					MonthFeeManager monthFeeManager = new MonthFeeManager();
					int i = await monthFeeManager.Update_MonthFee_Value_byID(monthFee.id, new_value);
					monthFee.value = input;
					absoluteLayout.Remove(monthFeesCollectionView);
					monthFeesCollectionView = null;
					CreateMonthFeesColletion();
				}
				else
				{
                    await DisplayAlert("Valor incorreto", "O valor introduzido não é válido.", "Ok");
                }


				/*string global_evaluation = await UpdateExamination_Result(examination_Result.id, input.Text);
				examination_Result.description = input.Text;*/
			}

			hideActivityIndicator();
		}

        public static bool IsDoubleRealNumber(string valueToTest)
        {
            if (double.TryParse(valueToTest, out double d) && !Double.IsNaN(d) && !Double.IsInfinity(d))
            {
                return true;
            }

            return false;
        }

        async void changeMonthFeeStatusPrompt(MonthFee monthFee)
		{
			showActivityIndicator();
			var result = await DisplayAlert("Confirmas que pretendes colocar esta mensalidade como paga?", "Confirmar Pagamento", "Sim", "Não");
			if (result)
			{
				MonthFeeManager monthFeeManager = new MonthFeeManager();
				int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "paga");
				monthFee.selectedColor = Colors.LightGreen;
				monthFee.status = "Paga";
				absoluteLayout.Remove(monthFeesCollectionView);
				monthFeesCollectionView = null;
				CreateMonthFeesColletion();
			}
			else
			{
			}

			hideActivityIndicator();
		}

		public async void InvoiceDocument(MonthFee monthFee)
		{
			Debug.Print("InvoiceDocument");
			Payment payment = await GetMonthFeePayment(monthFee);
			if (payment.invoiceid != null)
			{
				Debug.Print("InvoiceDocument != null");
				if (payment.value != 0)
				{
					Debug.Print("InvoiceDocument != 0");
					await Navigation.PushAsync(new InvoiceDocumentPageCS(payment));
				}

			}
		}

		async void approveSelectedButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("approveSelectedButtonClicked");
			showActivityIndicator();
			
			//approveSelectedButton.IsEnabled = false;
			//approveAllButton.IsEnabled = false;


			int selectedMonthFeeCount = 0;

			foreach (MonthFee monthFee in monthFees)
			{
				if (monthFee.selected == true)
				{
					selectedMonthFeeCount++;
					MonthFeeManager monthFeeManager = new MonthFeeManager();
					//int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida_a_pagamento");
                    int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida");
                }
			}

			if (selectedMonthFeeCount == 0)
			{
				await DisplayAlert("ESCOLHA VAZIA", "Tens de escolher pelo menos uma mensalidade para aprovar.", "Ok" );
			}

			//approveSelectedButton.IsEnabled = true;
			//approveAllButton.IsEnabled = true;

			hideActivityIndicator();
		}

		async void approveAllButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("approveAllButtonClicked");
			showActivityIndicator();


			foreach (MonthFee monthFee in monthFees)
			{
				Debug.Print("monthFee "+ monthFee.name + " status = "+ monthFee.status);
				if (monthFee.status == "Por Aprovar")
				{
					MonthFeeManager monthFeeManager = new MonthFeeManager();

					int currentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
					int currentYear = Convert.ToInt32(DateTime.Now.Year.ToString());

					if (((Convert.ToInt32(monthFee.year) == currentYear) & (Convert.ToInt32(monthFee.month) < currentMonth)) | (Convert.ToInt32(monthFee.year) < currentYear))
					{
                        //int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida_pagamento_em_atraso");
                        //monthFee.status = "Pagamento em Atraso";
                        
						int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida");
                        monthFee.status = "Emitida";
                        monthFee.selectedColor = Colors.IndianRed;
                    }
					else
					{
						/*int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida_a_pagamento");
						monthFee.status = "Em pagamento";*/
                        int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida");
                        monthFee.status = "Emitida";
                        monthFee.selectedColor = Colors.DarkBlue;
					}
				}
			}
			absoluteLayout.Remove(monthFeesCollectionView);
			monthFeesCollectionView = null;
			CreateMonthFeesColletion();


			hideActivityIndicator();
		}

		public async Task<Payment> GetMonthFeePayment(MonthFee monthFee)
		{
			Debug.WriteLine("GetMonthFeePayment");
			MonthFeeManager monthFeeManager = new MonthFeeManager();

			List<Payment> result = await monthFeeManager.GetMonthFee_Payment(monthFee.id);
			if (result == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return result[0];
			}
			return result[0];
		}
	}
}
