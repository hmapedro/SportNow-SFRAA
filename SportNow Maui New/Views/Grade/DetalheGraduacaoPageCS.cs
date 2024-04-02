using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;


namespace SportNow.Views
{
	public class DetalheGraduacaoPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
            base.OnAppearing();
            this.CleanScreen();
		}

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (gridGrade != null)
			{
				absoluteLayout.Remove(gridGrade);
				gridGrade = null;
			}

		}

		private Microsoft.Maui.Controls.StackLayout stackLayout;

		private Microsoft.Maui.Controls.Grid gridGrade;

		public List<MainMenuItem> MainMenuItems { get; set; }

		private Examination examination;

		public void initLayout()
		{
			Title = "Graduação";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "iconshare.png",
			
			};
			toolbarItem.Clicked += OnShareButtonClicked;
			ToolbarItems.Add(toolbarItem);
			NavigationPage.SetBackButtonTitle(this, "");
		}


		public async Task initSpecificLayout()
		{

			gridGrade = new Microsoft.Maui.Controls.Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 140 * App.screenHeightAdapter });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 50 * App.screenHeightAdapter });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 50 * App.screenHeightAdapter });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 250 * App.screenHeightAdapter });
			
			gridGrade.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

			Label gradeLabel = new Label
			{
				Text = Constants.grades[examination.grade],
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 30 * App.screenHeightAdapter,
                FontFamily = "futuracondensedmedium"
            };

			Debug.Print("examination.image = " + examination.image);


            Image gradeImage = new Image
			{
				Source = examination.image.ToLower(),
			};

			var textdategrade = examination.place + " | " + examination.date + " | " + examination.examiner;
			Label dategradeLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = textdategrade,
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize
			};



			var browser = new WebView
			{
				//Source = "http://xamarin.com",
				HeightRequest = 230 * App.screenHeightAdapter,
				WidthRequest = 297 * App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Fill,
                //Source = "https://" + Constants.server +"/ services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                //Source = "https://www.google.com"
                //Source = "https://docs.google.com/viewer?url=https://www.nksl.org/nkslcrm/upload/planta.pdf"
                //Source = "https://www.nksl.org/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id

                /*#if __ANDROID__
								Source = "https://docs.google.com/gview?url=http://www.nksl.org/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id + "&embedded=true%22"
				#else
								Source = "https://www.nksl.org/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id
				#endif*/
            };

			var pdfUrl = "https://"+Constants.server+"/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id;
			var androidUrl = "https://docs.google.com/gview?url=" + pdfUrl + "&embedded=true";
			Debug.Print("androidUrl=" + androidUrl);
            browser.Source = pdfUrl;
            if (DeviceInfo.Platform != DevicePlatform.iOS)
			{
				browser.Source = pdfUrl;
			}
			else if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				browser.Source = new UrlWebViewSource() { Url = androidUrl };
			}

			if (browser.Source == null)
			{
				Debug.Print("browser.Source = null");
			}
			else {
				Debug.Print("browser.Source != null");
			}

			gridGrade.Add(gradeLabel, 0, 0);

			gridGrade.Add(gradeImage, 0, 1);

			gridGrade.Add(dategradeLabel, 0, 3);

			gridGrade.Add(browser, 0, 4);

			List<Payment> payments = await GetExamination_Payment(examination.id);

			if (payments.Count > 0)
			{
				if ((payments[0].invoiceid != null) & (payments[0].invoiceid != ""))
				{
					Label invoiceLabel = new Label
					{
                        FontFamily = "futuracondensedmedium",
                        Text = "Obter fatura",
						TextColor = App.normalTextColor,
						HorizontalTextAlignment = TextAlignment.Center,
						FontSize = App.titleFontSize
					};

					var invoiceLabel_tap = new TapGestureRecognizer();
					invoiceLabel_tap.Tapped += async (s, e) =>
					{
						await Navigation.PushAsync(new InvoiceDocumentPageCS(payments[0]));
					};
					invoiceLabel.GestureRecognizers.Add(invoiceLabel_tap);
					gridGrade.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
					gridGrade.Add(invoiceLabel, 0, 5);
				}
			}

			absoluteLayout.Add(gridGrade);
            absoluteLayout.SetLayoutBounds(gridGrade, new Rect((App.screenWidth/ 2) - 143.5 * App.screenHeightAdapter, 0, App.screenWidth, App.screenHeight));
		}

		public DetalheGraduacaoPageCS(Member member, Examination examination)
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.examination = examination;
			this.initLayout();
		}

		async void OnShareButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnShareButtonClicked");
			await Share.RequestAsync(new ShareTextRequest
			{
				//Uri = "https://plataforma.nksl.org/diploma_1.jpg",
				Uri = "https://"+Constants.server+"/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id,
				Title = "Partilha Diploma"
			});
		}

		async Task<List<Payment>> GetExamination_Payment(string examinationid)
		{
			Debug.WriteLine("GetExamination_Payment");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Payment> payments = await examination_sessionManager.GetExamination_Payment(examinationid);
			if (payments == null)
			{
				Debug.WriteLine("GetExamination_Payment is null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return payments;
		}


		
	}


}

