using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class MonthFeeStudentListPageCS : DefaultPage
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

		public Microsoft.Maui.Controls.StackLayout stackMonthSelector;

		DateTime selectedTime;

		ObservableCollection<MonthFee> monthFees;

		public void initLayout()
		{
			Title = "MENSALIDADES";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (monthFeesCollectionView != null)
			{
				absoluteLayout.Remove(monthFeesCollectionView);
				monthFeesCollectionView = null;
			}
			if (stackMonthSelector != null)
			{
				absoluteLayout.Remove(stackMonthSelector);
				stackMonthSelector = null;
			}
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateMonthSelector();
			monthFees = await GetMonthFeesbyStudent();
			CreateMonthFeesColletion();

			hideActivityIndicator();
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
                Text = "<",
                FontSize = App.titleFontSize,
                TextColor = App.topColor,
                BackgroundColor = App.backgroundColor,
                VerticalOptions = LayoutOptions.Center
            };


            previousMonthButton.Clicked += OnPreviousButtonClicked;

			currentMonth = new Label()
			{
				Text = selectedTime.Year.ToString(),
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				WidthRequest = 150,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			nextMonthButton = new Button()
			{
				Text = ">",
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				BackgroundColor = App.backgroundColor,
				VerticalOptions = LayoutOptions.Center
			};

			nextMonthButton.Clicked += OnNextButtonClicked;

			stackMonthSelector = new Microsoft.Maui.Controls.StackLayout
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
            absoluteLayout.SetLayoutBounds(stackMonthSelector, new Rect(0, 20 * App.screenHeightAdapter, App.screenWidth, 40 * App.screenHeightAdapter));


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
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Mensalidades relativas a este ano.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
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
                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.screenWidth, 30 * App.screenHeightAdapter));

                //itemFrame.Content = itemabsoluteLayout;

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membernickname");

				itemabsoluteLayout.Add(nameLabel);
	            itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(5 * App.screenWidthAdapter, 0, App.screenWidth - (255 * App.screenWidthAdapter), 30 * App.screenHeightAdapter));

				Label monthLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				monthLabel.SetBinding(Label.TextProperty, "month");

				itemabsoluteLayout.Add(monthLabel);
				itemabsoluteLayout.SetLayoutBounds(monthLabel, new Rect(App.screenWidth - 250 * App.screenWidthAdapter, 0, 40 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

       
				Label valueLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				valueLabel.SetBinding(Label.TextProperty, "value");

				itemabsoluteLayout.Add(valueLabel);
				itemabsoluteLayout.SetLayoutBounds(valueLabel, new Rect(App.screenWidth - 205 * App.screenWidthAdapter, 0, 50 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				Label statusLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				statusLabel.SetBinding(Label.TextProperty, "status");
				statusLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				itemabsoluteLayout.Add(statusLabel);
				itemabsoluteLayout.SetLayoutBounds(statusLabel, new Rect(App.screenWidth - 150 * App.screenWidthAdapter, 0, 150 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

			absoluteLayout.Add(monthFeesCollectionView);
            absoluteLayout.SetLayoutBounds(monthFeesCollectionView, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 170 * App.screenHeightAdapter));

		}

		public MonthFeeStudentListPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}


		async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			selectedTime = selectedTime.AddYears(-1);
			currentMonth.Text = selectedTime.Year.ToString();

			monthFees = await GetMonthFeesbyStudent();

			monthFeesCollectionView.ItemsSource = monthFees;

			hideActivityIndicator();
		}

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.Print("selectedTime antes = " + selectedTime.ToShortDateString());
			selectedTime = selectedTime.AddYears(1);
			Debug.Print("selectedTime = " + selectedTime.ToShortDateString());
			currentMonth.Text = selectedTime.Year.ToString();
			monthFees = await GetMonthFeesbyStudent();

			monthFeesCollectionView.ItemsSource = monthFees;


			hideActivityIndicator();


		}

		async Task<ObservableCollection<MonthFee>> GetMonthFeesbyStudent()
		{
			MonthFeeManager monthFeeManager = new MonthFeeManager();
			ObservableCollection<MonthFee> monthFees = await monthFeeManager.GetMonthFeesbyStudent(App.member.id, selectedTime.Year.ToString());
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

			foreach (MonthFee monthFee in monthFees)
			{
				monthFee.selected = false;
				monthFee.selectedColor = App.normalTextColor;

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

			return monthFees;
		}

		void OncollectionViewMonthFeesSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
		{
			Debug.Print("OncollectionViewMonthFeesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				MonthFee selectedMonthFee = (sender as CollectionView).SelectedItem as MonthFee;

				Debug.WriteLine("OncollectionViewExaminationSessionCallSelectionChanged selected item = " + selectedMonthFee.membernickname);
				
				if ((selectedMonthFee.status == "Em pagamento") | (selectedMonthFee.status == "Pagamento em Atraso") | (selectedMonthFee.status == "Emitida"))
				{
					payMonthFee(selectedMonthFee);
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

		public async Task<bool> checkPreviusUnpaidMonthFeeAsync(MonthFee selectedMonthFee)
		{
			Debug.Print("checkPreviusUnPaidMonthFee");
			foreach (MonthFee monthFee in monthFees)
			{
				Debug.Print("selectedMonthFee = " + selectedMonthFee.name + " " + selectedMonthFee.status + " " + selectedMonthFee.year + " " + selectedMonthFee.month);
				Debug.Print("MonthFee = " + monthFee.name + " " + monthFee.status + " " + monthFee.year + " " + monthFee.month);
				if (((monthFee.status == "Em pagamento") | (monthFee.status == "Pagamento em Atraso") | (monthFee.status == "Emitida"))
					& ((Convert.ToInt32(selectedMonthFee.year) == Convert.ToInt32(monthFee.year)) & (Convert.ToInt32(selectedMonthFee.month) > Convert.ToInt32(monthFee.month)))
						| (Convert.ToInt32(selectedMonthFee.year) > Convert.ToInt32(monthFee.year)))
                {
					Debug.Print("Tem mensalidades anteriores sem pagamento!");

					var result = await DisplayAlert("Tens mensalidades anteriores em atraso. Confirmas que queres pagar esta mensalidade?", "MENSALIDADES EM ATRASO", "Sim", "Não");
					return result;
					/*if (result)
					{
						return true;
					}
					else
					{
						return false;
					}*/
				}
			}
			return true;
		}
		

		async void payMonthFee(MonthFee monthFee)
		{
			Debug.Print("payMonthFee");

			bool unpaid = await checkPreviusUnpaidMonthFeeAsync(monthFee);
			if (unpaid == true)
			{
				showActivityIndicator();
				await Navigation.PushAsync(new MonthFeePaymentPageCS(monthFee));
				hideActivityIndicator();
			}
		}

		async void changeMonthFeeStatusPrompt(MonthFee monthFee)
		{
			showActivityIndicator();
			var result = await DisplayAlert("Confirmas que pretendes colocar esta mensalidade como paga?", "Confirmar Pagamento", "Sim", "Não");
			if (result)
			{
				MonthFeeManager monthFeeManager = new MonthFeeManager();
				int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "paga");
				monthFee.status = "Paga";
				monthFeesCollectionView.ItemsSource = null;
				monthFeesCollectionView.ItemsSource = monthFees;
			}
			else
			{
			}

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
