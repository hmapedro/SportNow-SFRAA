﻿using SportNow.Model;
using System.Diagnostics;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;

using SportNow.Services.Data.JSON;
using SportNow.CustomViews;

namespace SportNow.Views.Services
{
	public class SportsPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
            
            showActivityIndicator();

			initSpecificLayout();

			hideActivityIndicator();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private CollectionView currentServicesCollectionView, otherServicesCollectionView;

		Label currentServicesLabel, otherServicesLabel;

		private List<Event> currentServicesList, otherServicesList;

        RoundButton confirmButton;

        Grid gridServices;
        List<Service> other_services, current_services;

        public void CleanScreen()
		{
			if (currentServicesLabel != null)
			{
				//absoluteLayout.Clear();
				
				absoluteLayout.Children.Remove(gridServices);
                gridServices = null;
			}
        }

        public void initLayout()
		{
			Title = "MODALIDADES";

			ToolbarItem toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "perfil.png";

            toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public async void initSpecificLayout()
		{
            gridServices = new Microsoft.Maui.Controls.Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 5 * App.screenHeightAdapter };
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 40 * App.screenHeightAdapter});
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 200 * App.screenHeightAdapter });
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 40 * App.screenHeightAdapter });
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 400 * App.screenHeightAdapter });
            gridServices.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            _ = await createCurrentServices();
			_ = await createOtherServices();

            gridServices.Add(currentServicesLabel, 0, 0);
            gridServices.Add(currentServicesCollectionView, 0, 1);
            gridServices.Add(otherServicesLabel, 0, 2);
            gridServices.Add(otherServicesCollectionView, 0, 3);

            absoluteLayout.Add(gridServices);
            absoluteLayout.SetLayoutBounds(gridServices, new Rect(0 * App.screenWidthAdapter, 0, App.screenWidth - 0 * App.screenWidthAdapter, App.screenHeight - 100));

        }

        public async Task<int> createCurrentServices()
		{
            showActivityIndicator();
            showActivityIndicator();

            ServicesManager servicesManager = new ServicesManager();

            current_services = await servicesManager.GetCurrentServices(App.member.id);
            current_services = CompleteServices(current_services);

            currentServicesLabel = new Label
			{
				Text = "MODALIDADES ATUAIS",
                TextColor = App.activeTitleTextColor,
                HorizontalTextAlignment = TextAlignment.Start,
				FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };

			CreateCurrentServicesColletion();
			hideActivityIndicator();
			return 1;
		}



		public void CreateCurrentServicesColletion()
		{
            //COLLECTION TEACHER CLASSES
            currentServicesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = current_services,
                 
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Serviços.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

            currentServicesCollectionView.SelectionChanged += OnCurrentServicesCollectionViewSelectionChanged;

            currentServicesCollectionView.ItemTemplate = new DataTemplate(() =>
            {

                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth
                };

                Debug.Print("App.ItemHeight  = " + (App.ItemHeight - 10) * App.screenHeightAdapter);

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = Colors.Transparent,
                    BackgroundColor = App.backgroundColor,
                    Padding = new Thickness(0, 0, 0, 0),
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
                eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextTransform = TextTransform.Uppercase, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "nome");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, App.ItemHeight / 2 - 40 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 80 * App.screenHeightAdapter));

                return itemabsoluteLayout;
            });

        }


        public async Task<int> createOtherServices()
        {
            showActivityIndicator();

            ServicesManager servicesManager = new ServicesManager();

            other_services = await servicesManager.GetServices();
            other_services = CompleteServices(other_services);
            otherServicesLabel = new Label
            {
                Text = "MODALIDADES DISPONÍVEIS",
                TextColor = App.activeTitleTextColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            CreateOtherServicesColletion();
            hideActivityIndicator();
            return 1;
        }

        public List<Service> CompleteServices(List<Service> services_local)
        {
            List<Service> services_new = new List<Service>();

            if (services_local != null)
            {
                foreach (Service service in services_local)
                {

                    if ((service.imagem == "") | (service.imagem is null))
                    {
                        service.imagemSource = "company_logo_square.png";
                    }
                    else
                    {
                        service.imagemSource = new UriImageSource
                        {
                            Uri = new Uri(Constants.images_URL + service.id + "_imagem"),
                            CachingEnabled = false,
                            CacheValidity = new TimeSpan(0, 0, 0, 0)
                        };
                    }
                    if ((service.tipo == "desporto") | (service.tipo == "cultura"))
                    {
                        services_new.Add(service);
                    }
                }
            }

            return services_new;
        }


        public void CreateOtherServicesColletion()
        {
            //COLLECTION TEACHER CLASSES
            otherServicesCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = other_services,
                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem Serviços.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            otherServicesCollectionView.SelectionChanged += OnOtherServicesCollectionViewSelectionChanged;

            otherServicesCollectionView.ItemTemplate = new DataTemplate(() =>
            {

                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth
                };

                Debug.Print("App.ItemHeight  = " + (App.ItemHeight - 10) * App.screenHeightAdapter);

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = Colors.Transparent,
                    BackgroundColor = App.backgroundColor,
                    Padding = new Thickness(0, 0, 0, 0),
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
                eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextTransform = TextTransform.Uppercase, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "nome");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, App.ItemHeight / 2 - 40 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 80 * App.screenHeightAdapter));

                return itemabsoluteLayout;
            });

        }

        public void CreateConfirmButton()
        {

            confirmButton = new RoundButton("CONFIRMAR PRESENÇAS", App.screenWidth - 20 * App.screenWidthAdapter, 50);
            confirmButton.button.Clicked += OnConfirmButtonClicked;

        }


        public SportsPageCS()
		{

			this.initLayout();
		//	this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async void OnCurrentServicesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("ServicesPageCS.OnCurrentServicesCollectionViewSelectionChanged");

            if ((sender as CollectionView).SelectedItem != null)
            {
                Service service = (sender as CollectionView).SelectedItem as Service;
                (sender as CollectionView).SelectedItem = null;
                await Navigation.PushAsync(new CurrentServicesDetailPageCS(service));
            }
            hideActivityIndicator();
        }


        async void OnOtherServicesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("ServicesPageCS.OnOtherServicesCollectionViewSelectionChanged");
            showActivityIndicator();

            if ((sender as CollectionView).SelectedItem != null)
            {
                Service service = (sender as CollectionView).SelectedItem as Service;
                (sender as CollectionView).SelectedItem = null;
                await Navigation.PushAsync(new ServicesDetailPageCS(service));

            }
            hideActivityIndicator();
        }

        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            showActivityIndicator();

            Debug.WriteLine("OnConfirmButtonClicked");

            hideActivityIndicator();

        }
    }
}
