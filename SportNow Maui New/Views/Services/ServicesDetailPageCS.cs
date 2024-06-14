using SportNow.Model;
using System.Diagnostics;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;

using SportNow.Services.Data.JSON;
using System.Net;
using SportNow.CustomViews;

namespace SportNow.Views.Services
{
	public class ServicesDetailPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
            
            showActivityIndicator();

			//initSpecificLayout();

			hideActivityIndicator();
		}

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

		Label serviceDescriptionLabel;
        Grid gridService, gridServiceDetail;
        ScrollView scrollView;
        Service service;
        Image serviceImage;
        RoundButton contactButton;

        public void CleanScreen()
		{
			if (serviceDescriptionLabel != null)
			{
                serviceDescriptionLabel = null;
			}
        }

        public void initLayout()
		{
			Title = service.nome.ToUpper();
		}


		public async void initSpecificLayout()
		{
            gridService = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter };
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            CreatePhoto();
            createServiceInfo();
            gridService.Add(serviceImage, 0, 0);
            gridService.Add(serviceDescriptionLabel, 0, 1);
            gridService.Add(gridServiceDetail, 0, 2);

            absoluteLayout.Add(gridService);
            absoluteLayout.SetLayoutBounds(gridService, new Rect(0 * App.screenWidthAdapter, 0, App.screenWidth, App.screenHeight - 100));

        }

        public int createServiceInfo()
		{
            showActivityIndicator();

            Debug.Print("service.descricao = " + service.descricao);

            //service.descricao = "<html> <b> OLA</b> ole <ul>  <li>Coffee</li>  <li>Tea</li>  <li>Milk</li></ul>";

            //service.descricao = service.descricao.Replace("upload/", "https://sfraa.ippon.pt/upload/");

            serviceDescriptionLabel = new Label
            {
                Text = service.descricao,
                TextColor = App.inactiveTitleTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.itemTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            scrollView = new ScrollView { Orientation = ScrollOrientation.Vertical, WidthRequest = App.screenWidth};

            gridServiceDetail = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter};
            gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridServiceDetail.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridServiceDetail.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            FormLabel serviceResponsavelLabel = new FormLabel { Text = "RESPONSÁVEL" };
            FormValue serviceResponsavelValue = new FormValue(service.responsavel);


            FormLabel serviceLocalLabel = new FormLabel { Text = "LOCAL" };
            FormValue serviceLocalValue = new FormValue(service.local);

            FormLabel serviceValoresLabel = new FormLabel { Text = "VALORES" };
            FormValue serviceValoresValue = new FormValue(service.valores, 100);
            serviceValoresValue.label.TextType = TextType.Html;

            FormLabel serviceHorarioLabel = new FormLabel { Text = "HORÁRIOS" };
            FormValue serviceHorarioValue = new FormValue(service.horario, 100);
            serviceHorarioValue.label.TextType = TextType.Html;

            FormLabel serviceObservacoesLabel = new FormLabel { Text = "OBSERVAÇÕES" };
            FormValue serviceObservacoesValue = new FormValue(service.observacoes, 100);
            serviceObservacoesValue.label.TextType = TextType.Html;

            gridServiceDetail.Add(serviceResponsavelLabel, 0, 0);
            gridServiceDetail.Add(serviceResponsavelValue, 1, 0);

            gridServiceDetail.Add(serviceLocalLabel, 0, 1);
            gridServiceDetail.Add(serviceLocalValue, 1, 1);

            gridServiceDetail.Add(serviceValoresLabel, 0, 2);
            gridServiceDetail.Add(serviceValoresValue, 1, 2);

            gridServiceDetail.Add(serviceHorarioLabel, 0, 3);
            gridServiceDetail.Add(serviceHorarioValue, 1, 3);

            if (service.observacoes != "")
            {
                gridServiceDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                gridServiceDetail.Add(serviceObservacoesLabel, 0, 4);
                gridServiceDetail.Add(serviceObservacoesValue, 1, 4);

            }

            contactButton = new RoundButton("MAIS INFORMAÇÕES", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            contactButton.button.Clicked += OnContactButtonClicked;

            gridServiceDetail.Add(contactButton, 0, 5);
            Grid.SetColumnSpan(contactButton, 2);

            scrollView.Content = gridServiceDetail;

            hideActivityIndicator();
			return 1;
		}


        public void CreatePhoto()
        {
            //memberPhotoImage = new RoundImage();

            serviceImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 100 * App.screenHeightAdapter }; //, HeightRequest = 60, WidthRequest = 60
            serviceImage.Source = "";



            WebResponse response;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Constants.images_URL + service.id + "_imagem");
            Debug.Print(Constants.images_URL + service.id + "_imagem");
            request.Method = "HEAD";
            bool exists;
            try
            {
                response = request.GetResponse();
                Debug.Print("response.Headers.GetType()= " + response.Headers.GetType());
                exists = true;
            }
            catch (Exception ex)
            {
                exists = false;
            }

            Debug.Print("Photo exists? = " + exists);

            if (exists)
            {

                serviceImage.Source = new UriImageSource
                {
                    Uri = new Uri(Constants.images_URL + service.id + "_imagem"),
                    CachingEnabled = false,
                    //CacheValidity = new TimeSpan(0, 0, 0, 0)
                };
            }
            else
            {
                serviceImage.Source = "";
            }

        }

        public ServicesDetailPageCS(Service service)
		{
            this.service = service;
			this.initLayout();
			this.initSpecificLayout();

		}

        async void OnContactButtonClicked(object sender, EventArgs e)
        {

            showActivityIndicator();
            contactButton.IsEnabled = false;
            bool answer = await DisplayAlert("Pedido Informações", "Será enviada uma mensagem ao responsável deste serviço com o pedido de informações agora realizado.", "Ok", "Cancelar");
            if (answer == true)
            {
                ServicesManager servicesManager = new ServicesManager();
                string result = await servicesManager.SendMailService(this.service.id, this.service.nome, this.service.responsavel, this.service.responsavel_email, App.member.id, App.member.nickname, App.member.email, App.member.phone);

                if (result == "0")
                {
                    await DisplayAlert("Pedido Informações Enviado", "Mensagem enviada com sucesso.", "Ok");
                }
            }
            contactButton.IsEnabled = true;
            
            hideActivityIndicator();
        }

    }
}
