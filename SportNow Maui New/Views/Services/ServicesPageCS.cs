using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SportNow.ViewModel;
using Microsoft.Maui;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using SportNow.Views.Profile;
using SportNow.CustomViews;
using SportNow.Views.Personal;
using System.Xml;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Plugin.BetterFirebasePushNotification;

namespace SportNow.Views.Services
{
	public class ServicesPageCS : DefaultPage
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

		private CollectionView currentServicesCollectionView, otherServicesCollectionView;

		Label currentServicesLabel, otherServicesLabel;

		private List<Event> currentServicesList, otherServicesList;

        Grid gridServices;

        public void CleanScreen()
		{
			if (currentServicesLabel != null)
			{
				//absoluteLayout.Clear();
				
				absoluteLayout.Children.Remove(currentServicesCollectionView);
				absoluteLayout.Children.Remove(otherServicesCollectionView);
				absoluteLayout.Children.Remove(currentServicesLabel);
				absoluteLayout.Children.Remove(otherServicesLabel);

                currentServicesCollectionView = null;
                otherServicesCollectionView = null;
                currentServicesLabel = null;
                otherServicesLabel = null;
			}
        }

        public void initLayout()
		{
			Title = "SERVIÇOS";

			ToolbarItem toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "perfil.png";

            toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public async void initSpecificLayout()
		{
            gridServices = new Microsoft.Maui.Controls.Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter };
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter});
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 200 * App.screenHeightAdapter });
            gridServices.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
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

			currentServicesLabel = new Label
			{
				Text = "SERVIÇOS ATUAIS",
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Start,
				FontSize = App.titleFontSize,
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
				ItemsSource = null,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

            currentServicesCollectionView.SelectionChanged += OnCurrentServicesCollectionViewSelectionChanged;

            currentServicesCollectionView.ItemTemplate = new DataTemplate(() =>
			{

				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = App.ItemHeight ,
					WidthRequest = App.ItemWidth
				};

				Debug.Print("App.ItemHeight  = " + (App.ItemHeight  - 10) * App.screenHeightAdapter);

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
				dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");


                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");


                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

				return itemabsoluteLayout;
			});

  		}


        public async Task<int> createOtherServices()
        {
            showActivityIndicator();

            otherServicesLabel = new Label
            {
                Text = "SERVIÇOS DISPONÍVEIS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            CreateOtherServicesColletion();
            hideActivityIndicator();
            return 1;
        }


        public void CreateOtherServicesColletion()
        {
            //COLLECTION TEACHER CLASSES
            otherServicesCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = null,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
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
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
                    Padding = new Thickness(0, 0, 0, 0),
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 }; //, HeightRequest = 60, WidthRequest = 60
                eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");


                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");


                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

                return itemabsoluteLayout;
            });

        }

        public ServicesPageCS ()
		{

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async void OnCurrentServicesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("ServicesPageCS.OnOtherServicesCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItems.Count != 0)
			{
				/*ClassManager classmanager = new ClassManager();

				Class_Schedule class_schedule = (sender as CollectionView).SelectedItems[0] as Class_Schedule;
				if (class_schedule.classattendanceid == null)
				{
                    /*Task.Run(async () =>
                    {
                        string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                        class_schedule.classattendanceid = class_attendance_id;
                        return true;
                    });*/
                    //string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                    /*                    string class_attendance_id =  classmanager.CreateClass_Attendance_sync(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                                        */
                   // class_schedule.classattendancestatus = "confirmada";
                   // class_schedule.participationimage = "iconcheck.png";


                //}

				((CollectionView)sender).SelectedItems.Clear();

				hideActivityIndicator();
			}
		}


        async void OnOtherServicesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("ServicesPageCS.OnOtherServicesCollectionViewSelectionChanged");

        }
    }
}
