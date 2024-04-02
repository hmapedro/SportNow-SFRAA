
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;


namespace SportNow.Views
{
	public class InvoiceDocumentPageCS : DefaultPage
	{

		private Microsoft.Maui.Controls.StackLayout stackLayout;

		private Microsoft.Maui.Controls.Grid gridGrade;

		public List<MainMenuItem> MainMenuItems { get; set; }

		private Payment payment;

		public void initLayout()
		{
			Title = "Fatura";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "iconshare.png",
			
			};
			toolbarItem.Clicked += OnShareButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public void initSpecificLayout()
		{

			gridGrade= new Microsoft.Maui.Controls.Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star});
			gridGrade.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

			var browser = new WebView
			{
				BackgroundColor = App.backgroundColor,
                HeightRequest = App.screenHeight,
				WidthRequest = App.screenWidth,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
			};

			browser.Navigating += OnNavigating;
			browser.Navigated += OnNavigated;


			var pdfUrl = Constants.RestUrl_Get_Invoice_byID + "?invoiceid=" + payment.invoiceid;
			var androidUrl = "https://docs.google.com/gview?url=" + pdfUrl + "&embedded=true";
			Debug.Print("pdfUrl=" + pdfUrl);
			Debug.Print("androidUrl="+androidUrl);
            browser.Source = pdfUrl;

            /*if (DeviceInfo.Platform != DevicePlatform.iOS)
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
			}*/

			gridGrade.Add(browser, 0, 0);

			absoluteLayout.Add(gridGrade);
            absoluteLayout.SetLayoutBounds(gridGrade, new Rect(0, 0, App.screenWidth, App.screenHeight));

			/*Image diplomaImage = new Image
			{
				Source = new UriImageSource
				{
					Uri = new Uri("https://www.nksl.org/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id),
					CachingEnabled = false,
					CacheValidity = new TimeSpan(5, 0, 0, 0)
				}
			};*/

			}

		public InvoiceDocumentPageCS(Payment payment)
		{
			this.payment = payment;
			this.initLayout();
			this.initSpecificLayout();
			//CreateDiploma(member, examination);

			
		}

		public void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			showActivityIndicator();


		}

		public void OnNavigated(object sender, WebNavigatedEventArgs e)
		{

			hideActivityIndicator();

		}

		async void OnShareButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnShareButtonClicked");
			await Share.RequestAsync(new ShareTextRequest
			{
				//Uri = "https://plataforma.nksl.org/diploma_1.jpg",
				Uri = Constants.RestUrl_Get_Invoice_byID + "?invoiceid=" + payment.invoiceid,
				Title = "Partilha Fatura"
			});
		}

	}


}

