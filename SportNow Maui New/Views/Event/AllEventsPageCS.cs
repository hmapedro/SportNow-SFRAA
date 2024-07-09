﻿using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;

namespace SportNow.Views
{
	public class AllEventsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			base.OnAppearing();
			initSpecificLayout();

        }

        protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		MenuButton proximosEspetaculosButton;
		MenuButton proximosOutrosEventosButton;

		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private CollectionView proximosEventosCollectionView;


		private List<Event> proximosEventosAll, proximosEspetaculos, proximosOutrosEventos;


        public void initLayout()
		{
			Title = "EVENTOS";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png",

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{
				absoluteLayout.Remove(stackButtons);
				stackButtons = null;
			}
			CleanProximosEventosCollectionView();
			//CleanProximasCompeticoesCollectionView();
			//CleanProximasSessoesExameCollectionView();


        }

		public async void initSpecificLayout()
		{
			showActivityIndicator();
			

			int result = await getEventData();
			CreateStackButtons();
			CreateProximosEventosColletion();
			activateLastSelectedTab();
			//CreateCalendarioLink();
			//OnProximosEstagiosButtonClicked(null, null);

			hideActivityIndicator();

		}


		public void activateLastSelectedTab()
		{
			Debug.Print("activateLastSelectedTab");
			if (App.EVENTOS_activetab == "espetaculos")
			{
				OnProximosEspetaculosButtonClicked(null, null);
			}
			else if (App.EVENTOS_activetab == "outroseventos")
			{
				OnProximosOutrosEventosButtonClicked(null, null);
			}
			else
			{
                OnProximosEspetaculosButtonClicked(null, null);
			}
		}

		public async Task<int> getEventData()
		{
			proximosEventosAll = await GetFutureEventsAll();
			proximosEspetaculos= new List<Event>();
			proximosOutrosEventos= new List<Event>();
			if (proximosEventosAll != null)
			{

				foreach (Event event_i in proximosEventosAll)
				{
					if ((event_i.imagemNome == "") | (event_i.imagemNome is null))
					{
						event_i.imagemSource = "company_logo_square.png";
					}
					else
					{
						event_i.imagemSource = Constants.images_URL + event_i.id + "_imagem_c";

					}

					if (event_i.participationconfirmed == "inscrito")
					{
						event_i.participationimage = "iconcheck.png";
					}
                    else if (event_i.participationconfirmed == "cancelado")
                    {
                        event_i.participationimage = "iconinativo.png";
                    }

                    if (event_i.type == "espetaculo")
					{
                        proximosEspetaculos.Add(event_i);
					}
					else
					{
						proximosOutrosEventos.Add(event_i);
					}
				}
			}

			return 1;
		}


		public void CreateStackButtons()
		{
			Debug.Print("AllEventsPageCS - CreateStackButtons");
            var buttonWidth = (App.screenWidth - 5 * App.screenWidthAdapter) / 2;


            proximosEspetaculosButton = new MenuButton("ESPECTÁCULOS", buttonWidth, 60);
            proximosEspetaculosButton.button.Clicked += OnProximosEspetaculosButtonClicked;

			proximosOutrosEventosButton = new MenuButton("OUTROS", buttonWidth, 60);
			proximosOutrosEventosButton.button.Clicked += OnProximosOutrosEventosButtonClicked;

			stackButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60,
				Children =
				{
                    proximosEspetaculosButton,
					proximosOutrosEventosButton
				}
			};

			absoluteLayout.Add(stackButtons);
			
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

		}


		public void CreateProximosEventosColletion()
		{
			//COLLECTION GRADUACOES
			proximosEventosCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = proximosEventosSelected,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter,  },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Eventos deste tipo agendados", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			proximosEventosCollectionView.SelectionChanged += OnProximosEventosCollectionViewSelectionChanged;

            proximosEventosCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				double itemWidth = App.screenWidth / 2 - 10 * App.screenWidthAdapter;

                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
					HeightRequest= App.ItemHeight,
                    WidthRequest = itemWidth
                };

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
				eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemFrame.Content = eventoImage;

				itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, itemWidth, App.ItemHeight));

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), (((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2))));

				Label categoryLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 * App.screenWidthAdapter, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.NoWrap };
				categoryLabel.SetBinding(Label.TextProperty, "participationcategory");

				itemabsoluteLayout.Add(categoryLabel);
                itemabsoluteLayout.SetLayoutBounds(categoryLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter + (App.ItemHeight - (15 * App.screenHeightAdapter)) / 2, App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));
                //itemabsoluteLayout.SetLayoutBounds(categoryLabel, new Rect(3 * App.screenWidthAdapter, ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2), App.ItemWidth - (6 * App.screenWidthAdapter), ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));

                Label dateLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 * App.screenWidthAdapter, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "detailed_date");

				itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter + (App.ItemHeight - (15 * App.screenHeightAdapter)) * 3 / 4, App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));
                //itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(5 * App.screenHeightAdapter, ((App.ItemHeight - 15) - ((App.ItemHeight - 15) / 4)), itemWidth - (10 * App.screenHeightAdapter), ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemabsoluteLayout.Add(participationImagem);
				itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect(itemWidth - (21 * App.screenHeightAdapter), 1 * App.screenHeightAdapter, 20 * App.screenHeightAdapter, 20 * App.screenHeightAdapter));
				
				return itemabsoluteLayout;
			});
			absoluteLayout.Add(proximosEventosCollectionView);
            absoluteLayout.SetLayoutBounds(proximosEventosCollectionView, new Rect(5 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, App.screenHeight - 280 * App.screenHeightAdapter));

		}

		public AllEventsPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		public void CleanProximosEventosCollectionView()
		{
			Debug.Print("CleanProximosEstagiosCollectionView");
			//valida se os objetos já foram criados antes de os remover
			if (proximosEventosCollectionView != null)
			{
				absoluteLayout.Remove(proximosEventosCollectionView);
				proximosEventosCollectionView = null;
			}

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}


		async Task<List<Event>> GetFutureEventsAll()
		{
			Debug.WriteLine("GetFutureEventsAll");
			EventManager eventManager = new EventManager();
			List<Event> futureEvents = await eventManager.GetFutureEventsAll(App.member.id);
			if (futureEvents == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return futureEvents;
		}


		async void OnProximosEspetaculosButtonClicked(object sender, EventArgs e)
		{
			App.EVENTOS_activetab = "espetaculos";
			proximosEspetaculosButton.activate();
			proximosOutrosEventosButton.deactivate();

			proximosEventosCollectionView.ItemsSource = proximosEspetaculos;
		}


		async void OnProximosOutrosEventosButtonClicked(object sender, EventArgs e)
		{
			App.EVENTOS_activetab = "outroseventos";
            proximosEspetaculosButton.deactivate();
			proximosOutrosEventosButton.activate();

			proximosEventosCollectionView.ItemsSource = proximosOutrosEventos;
		}


		async void OnProximosEventosCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewProximosEstagiosSelectionChanged "+ (sender as CollectionView).SelectedItem.GetType().ToString());

			if ((sender as CollectionView).SelectedItem != null)
			{

				if ((sender as CollectionView).SelectedItem.GetType().ToString() == "SportNow.Model.Event")
                {
					Event event_v = (sender as CollectionView).SelectedItem as Event;
					await Navigation.PushAsync(new DetailEventPageCS(event_v));
				}
				else if ((sender as CollectionView).SelectedItem.GetType().ToString() == "SportNow.Model.Competition")
				{
					Competition competition = (sender as CollectionView).SelectedItem as Competition;
					await Navigation.PushAsync(new DetailCompetitionPageCS(competition));
				}
				else if ((sender as CollectionView).SelectedItem.GetType().ToString() == "SportNow.Model.Examination_Session")
				{

					Examination_Session examination_session = (sender as CollectionView).SelectedItem as Examination_Session;

                    Debug.Print("examination_session.imagemSource ANTES = " + examination_session.imagemSource);

                    await Navigation.PushAsync(new ExaminationSessionPageCS(examination_session));
				}

			}
		}

	}
}
