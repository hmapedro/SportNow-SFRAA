using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;
//using SportNow.Services.Camera;
using SkiaSharp;

namespace SportNow.Views.CompleteRegistration
{
	public class DocumentsPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}

		//Image estadoQuotaImage;


		RegisterButton confirmDocumentsButton;
		Image loadedDocument;

		bool documentSubmitted;

		Stream stream;

		public void initLayout()
		{
			Title = "DOCUMENTOS";
		}

		public async void initSpecificLayout()
		{


			Label titleLabel = new Label { FontFamily = "futuracondensedmedium", Text = "CASO POSSUA EXAME MÉDICO DESPORTIVO PODE SUBMETER O MESMO:", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Color.FromRgb(100, 100, 100), LineBreakMode = LineBreakMode.WordWrap };

            absoluteLayout.Add(titleLabel);
            absoluteLayout.SetLayoutBounds(titleLabel, new Rect(10 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 65 * App.screenHeightAdapter));

			Image imageGallery = new Image
			{
				Source = "iconabrirgaleria.png",
				HorizontalOptions = LayoutOptions.Center,
			};

			var imageGallery_tap = new TapGestureRecognizer();
			imageGallery_tap.Tapped += OpenGalleryTapped;
			imageGallery.GestureRecognizers.Add(imageGallery_tap);

            absoluteLayout.Add(imageGallery);
            absoluteLayout.SetLayoutBounds(imageGallery, new Rect(App.screenWidth/2 - 110 * App.screenHeightAdapter, 80 * App.screenHeightAdapter, 100 * App.screenHeightAdapter, 100 * App.screenHeightAdapter));


            /*relativeLayout.Children.Add(imageGallery,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) - (110 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant(120 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter)
			);*/

            Image imagePhoto = new Image
			{
				Source = "icontirarfoto.png",
				HorizontalOptions = LayoutOptions.Center,
			};
			var imagePhoto_tap = new TapGestureRecognizer();
			imagePhoto_tap.Tapped += TakeAPhotoTapped;
			imagePhoto.GestureRecognizers.Add(imagePhoto_tap);

            absoluteLayout.Add(imagePhoto);
            absoluteLayout.SetLayoutBounds(imagePhoto, new Rect(App.screenWidth / 2 + 10 * App.screenHeightAdapter, 80 * App.screenHeightAdapter, 100 * App.screenHeightAdapter, 100 * App.screenHeightAdapter));


			Label atestadoLabel = new Label { Text = "Fazer download do formulário do Exame Médico", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

			var atestadoLabel_tap = new TapGestureRecognizer();
			atestadoLabel_tap.Tapped += async (s, e) =>
			{
				try
				{
					await Browser.OpenAsync("https://www.adcpn.pt/wp-content/uploads/2019/05/Exame-Medico-Desportivo.pdf", BrowserLaunchMode.SystemPreferred);
				}
				catch (Exception ex)
				{
				}
			};
			atestadoLabel.GestureRecognizers.Add(atestadoLabel_tap);

			/*relativeLayout.Children.Add(atestadoLabel,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(270 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);*/

			confirmDocumentsButton = new RegisterButton("CONTINUAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			confirmDocumentsButton.button.Clicked += confirmDocumentsButtonClicked;

            absoluteLayout.Add(confirmDocumentsButton);
            absoluteLayout.SetLayoutBounds(confirmDocumentsButton, new Rect(10 * App.screenHeightAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
		}


		public DocumentsPageCS()
		{
			documentSubmitted = false;
			this.initLayout();
			this.initSpecificLayout();

            loadedDocument = new Image() { };

            absoluteLayout.Add(loadedDocument);
            absoluteLayout.SetLayoutBounds(loadedDocument, new Rect(20 * App.screenHeightAdapter, 200 * App.screenHeightAdapter, App.screenWidth - 40 * App.screenWidthAdapter, App.screenHeight - 370 * App.screenHeightAdapter));

        }

        async void confirmDocumentsButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("confirmDocumentsButtonClicked");

			
			//if (documentSubmitted == false)
			//{
			if (stream != null)
			{
				MemberManager memberManager = new MemberManager();

				string documentname = "";
				string type = "";
					

				documentname = "Exame Médico - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
				string filename = documentname + ".jpeg";
				string status = "Under Review";
				type = "atestado_medico";
				string startdate = DateTime.Now.ToString("yyyy-MM-dd");// "2022-07-22";
				string enddate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
				showActivityIndicator();
				//documentSubmitted = true;
				_ = await memberManager.Upload_Member_Document(stream, App.member.id, filename, documentname, status, type, startdate, enddate);
                hideActivityIndicator();
				await Navigation.PushAsync(new NewMemberPageCS());

			}
			else
			{
                bool result = await DisplayAlert("Documento não submetido", "Tens a certeza que pretendes continuar sem submeter o Exame Médico", "Sim", "Não");

				if (result == true)
				{
					await Navigation.PushAsync(new NewMemberPageCS());
				}
				else
				{
				}
			}

		}

        /*void ImageTapped(object sender, System.EventArgs e)
		{
			LoadFromStream((sender as Image).Source);
		}

		private async void LoadFromStream(ImageSource source)
		{
			await Navigation.PushAsync(new SfImageEditorPage() { ImageSource = source });
		}

		void TakeAPhotoTapped(object sender, System.EventArgs e)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "documentPhoto";//SetImageFileName();
				DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
			});
		}

		void OpenGalleryTapped(object sender, System.EventArgs e)
		{
			Debug.Print("OpenGalleryTapped");
			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "documentPhoto";//SetImageFileName();
				Debug.Print("OpenGalleryTapped fileName = " + fileName);
				Debug.Print("OpenGalleryTapped FileFormatEnum.JPEG = " + FileFormatEnum.JPEG);
				try
				{
					CameraInterface cameraInterface = DependencyService.Get<CameraInterface>();
					//cameraInterface = new DependencyService.Get<CameraInterface>();
					cameraInterface.LaunchGallery(FileFormatEnum.JPEG, fileName);
					//DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, "teste.jpeg");
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.Print(ex.Message);
					System.Diagnostics.Debug.Print(ex.StackTrace);
				}
				//loadedDocument.Source = fileName;
			});
		}*/


        async void OpenGalleryTapped(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Por favor escolha uma foto"
            });

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                loadedDocument.Source = ImageSource.FromStream(() => localstream);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    loadedDocument.Rotation = 0;
                    stream = RotateBitmap(stream_aux, 0);
                }
                else
                {
                    loadedDocument.Rotation = 90;
                    stream = RotateBitmap(stream_aux, 90);
                }
                //documentSubmitted = true;
                showConfirmDocumentsButton();
            }
        }

        async void TakeAPhotoTapped(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                loadedDocument.Source = ImageSource.FromStream(() => localstream);
                loadedDocument.Rotation = 90;
                stream = RotateBitmap(stream_aux, 90);
				//documentSubmitted = true;
                showConfirmDocumentsButton();
            }

        }

        public Stream RotateBitmap(Stream _stream, int angle)
        {
            Stream streamlocal = null;
            SKBitmap bitmap = SKBitmap.Decode(_stream);
            SKBitmap rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);
            if (angle != 0)
            {
                using (var surface = new SKCanvas(rotatedBitmap))
                {
                    surface.Translate(rotatedBitmap.Width, 0);
                    surface.RotateDegrees(angle);
                    surface.DrawBitmap(bitmap, 0, 0);
                }
            }
            else
            {
                rotatedBitmap = bitmap;
            }

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                rotatedBitmap.Encode(wstream, SKEncodedImageFormat.Jpeg, 40);
                byte[] data = memStream.ToArray();
                streamlocal = new MemoryStream(data);
            }
            return streamlocal;

        }

        public void showConfirmDocumentsButton()
        {
           /* relativeLayout.Children.Add(confirmDocumentsButton,
                xConstraint: Constraint.Constant(10),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - (60 * App.screenHeightAdapter); // 
                }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
                }),
                heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
            );*/
        }
    }

}