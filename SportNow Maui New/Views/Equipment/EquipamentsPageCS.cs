using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class EquipamentsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();

        }

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

		private AbsoluteLayout equipamentosrelativeLayout;
		
		private Microsoft.Maui.Controls.StackLayout stackButtons;

		List<Equipment> equipments, equipmentsKarategi, equipmentsProtecoesCintos, equipmentsMerchandising;
		public List<EquipmentGroup> equipmentsGroupSelected, equipmentsGroupKarategi, equipmentsGroupProtecoesCintos, equipmentsGroupMerchandising;

		private MenuButton karategiButton, protecoescintosButton, merchandisingButton;

		private CollectionView collectionViewEquipments;

		public void initLayout()
		{
			Title = "EQUIPAMENTOS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{
				absoluteLayout.Remove(stackButtons);
				//absoluteLayout.Remove(equipamentosrelativeLayout);

				stackButtons = null;
				equipamentosrelativeLayout = null;
			}

		}

		public async void initSpecificLayout()
		{
			await GetEquipmentsData();
			
			CreateStackButtons();
			CreateEquipmentColletionView();

			if (App.EQUIPAMENTOS_activetab == "karategis")
            {
				OnKarategiButtonClicked(null, null);
            }
			else if (App.EQUIPAMENTOS_activetab == "protecoescintos")
			{
				OnProtecoesCintosButtonClicked(null, null);
			}
			else if (App.EQUIPAMENTOS_activetab == "merchandising")
			{
				OnMerchandisingButtonClicked(null, null);
			}
		}


		public async Task GetEquipmentsData()
		{
			equipments = await GetEquipments();
			equipmentsKarategi = new List<Equipment>();
			equipmentsProtecoesCintos = new List<Equipment>();
			equipmentsMerchandising = new List<Equipment>();
			equipmentsGroupKarategi = new List<EquipmentGroup>();
			equipmentsGroupProtecoesCintos= new List<EquipmentGroup>();
			equipmentsGroupMerchandising = new List<EquipmentGroup>();

			foreach (Equipment equipment in equipments)
			{
				equipment.valueFormatted = String.Format("{0:0.00}", equipment.value) + "€";

				if (equipment.type == "karategi")
				{
					equipmentsKarategi.Add(equipment);
					EquipmentGroup equipmentGroup = getSubTypeEquipmentGroup(equipmentsGroupKarategi, equipment.subtype);

					if (equipmentGroup == null)
					{
						List<Equipment> equipments = new List<Equipment>();
						equipments.Add(equipment);
						equipmentsGroupKarategi.Add(new EquipmentGroup(equipment.subtype.ToUpper(), equipments));
					}
					else
					{
						equipmentGroup.Add(equipment);
					}
				}
				else if ((equipment.type == "protecao") | (equipment.type == "cinto"))
				{
					equipmentsProtecoesCintos.Add(equipment);
					EquipmentGroup equipmentGroup = getSubTypeEquipmentGroup(equipmentsGroupProtecoesCintos, equipment.subtype);

					if (equipmentGroup == null)
					{
						List<Equipment> equipments = new List<Equipment>();
						equipments.Add(equipment);
						equipmentsGroupProtecoesCintos.Add(new EquipmentGroup(equipment.subtype.ToUpper(), equipments));
					}
					else
					{
						equipmentGroup.Add(equipment);
					}
				}
				else if (equipment.type == "merchandising")
				{
					equipmentsMerchandising.Add(equipment);
					EquipmentGroup equipmentGroup = getSubTypeEquipmentGroup(equipmentsGroupMerchandising, equipment.subtype);

					if (equipmentGroup == null)
					{
						List<Equipment> equipments = new List<Equipment>();
						equipments.Add(equipment);
						equipmentsGroupMerchandising.Add(new EquipmentGroup(equipment.subtype.ToUpper(), equipments));
					}
					else
					{
						equipmentGroup.Add(equipment);
					}
				}
			}

			/*foreach (EquipmentGroup equipmentGroup in equipmentsGroupKarategi) {
				equipmentGroup.Print();
			}*/
		}

		public EquipmentGroup getSubTypeEquipmentGroup(List<EquipmentGroup> equipmentGroups, string subtype)
		{
			foreach (EquipmentGroup equipmentGroup in equipmentGroups)
			{
				if (equipmentGroup.Name.ToUpper() == subtype.ToUpper())
				{
					return equipmentGroup;
				}
			}
			return null;
		}

		public void CreateEquipmentColletionView()
		{
			collectionViewEquipments = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = equipments,
				IsGrouped = true,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
						{
							new Label { FontFamily = "futuracondensedmedium", Text = "Não existem equipamentos disponíveis de momento.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.topColor, FontSize = App.titleFontSize },
						}
					}
				}
			};

			
			collectionViewEquipments.SelectionChanged += OnCollectionViewEquipmentsSelectionChanged;

			collectionViewEquipments.GroupHeaderTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "Name");
				//nameLabel.SetBinding(Label.TextColorProperty, "estadoTextColor");

				itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(0, 0, App.screenWidth / 5 * 4 - 10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				Label valueLabel = new Label { FontFamily = "futuracondensedmedium",  Text = "VALOR", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor =App.topColor, LineBreakMode = LineBreakMode.WordWrap };

				itemabsoluteLayout.Add(valueLabel);
				itemabsoluteLayout.SetLayoutBounds(valueLabel, new Rect(App.screenWidth / 5 * 4, 0, App.screenWidth / 5 - 10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));
			
				return itemabsoluteLayout;
			});
			
			collectionViewEquipments.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");
				//nameLabel.SetBinding(Label.TextColorProperty, "estadoTextColor");
				
				Border nameFrame = new Border
				{
					
					BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemabsoluteLayout.Add(nameFrame);
				itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 0, (App.screenWidth / 5) * 4 - 10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));
				
				Label valueLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				valueLabel.SetBinding(Label.TextProperty, "valueFormatted");

				Border valueFrame = new Border
				{ 
					BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    Padding = new Thickness(5, 0, 0, 0)
				};
				valueFrame.Content = valueLabel;

				Debug.Print("(App.screenWidth / 5) * 4 = " + (App.screenWidth / 5) * 4);
				itemabsoluteLayout.Add(valueFrame);
				itemabsoluteLayout.SetLayoutBounds(valueFrame, new Rect((App.screenWidth / 5) * 4, 0, (App.screenWidth / 5) - (10 * App.screenWidthAdapter), 30 * App.screenHeightAdapter));


                return itemabsoluteLayout;
			});

			absoluteLayout.Add(collectionViewEquipments);
            absoluteLayout.SetLayoutBounds(collectionViewEquipments, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));

		}


		public void CreateStackButtons()
		{
			var buttonWidth = (App.screenWidth - 10 * App.screenWidthAdapter) / 3;

			karategiButton = new MenuButton("KARATE GIs", buttonWidth, 60);
			karategiButton.button.Clicked += OnKarategiButtonClicked;

			protecoescintosButton = new MenuButton("PROTEÇÕES E CINTOS", buttonWidth, 60);
			protecoescintosButton.button.Clicked += OnProtecoesCintosButtonClicked;

			merchandisingButton = new MenuButton("MERCHANDISING", buttonWidth, 60);
			merchandisingButton.button.Clicked += OnMerchandisingButtonClicked;

			stackButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
				Children =
				{
					karategiButton,
					protecoescintosButton,
					merchandisingButton
				}
			};

			absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

        }



		public EquipamentsPageCS(string type)
		{
			App.EQUIPAMENTOS_activetab = type;
			this.initLayout();
			initSpecificLayout();
		}

		async Task<List<Equipment>> GetEquipments()
		{
			EquipmentManager equipmentManager = new EquipmentManager();
			List<Equipment> equipments = await equipmentManager.GetEquipments();
			if (equipments == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return equipments;
		}

		async void OnKarategiButtonClicked(object sender, EventArgs e)
		{
			karategiButton.activate();
			protecoescintosButton.deactivate();
			merchandisingButton.deactivate();

			collectionViewEquipments.ItemsSource = equipmentsGroupKarategi;
			App.EQUIPAMENTOS_activetab = "karategis";

		}

		async void OnProtecoesCintosButtonClicked(object sender, EventArgs e)
		{
			karategiButton.deactivate();
			protecoescintosButton.activate();
			merchandisingButton.deactivate();

			collectionViewEquipments.ItemsSource = equipmentsGroupProtecoesCintos;

			App.EQUIPAMENTOS_activetab = "protecoescintos";
		}

		async void OnMerchandisingButtonClicked(object sender, EventArgs e)
		{
			karategiButton.deactivate();
			protecoescintosButton.deactivate();
			merchandisingButton.activate();

			collectionViewEquipments.ItemsSource = equipmentsGroupMerchandising;
			App.EQUIPAMENTOS_activetab = "merchandising";
		}

		async void OnCollectionViewEquipmentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewEquipmentsSelectionChanged");
			if ((sender as CollectionView).SelectedItem != null)
			{

				Equipment equipment = (sender as CollectionView).SelectedItem as Equipment;

				
				await Navigation.PushAsync(new EquipamentsOrderPageCS(equipment));
				

				Debug.WriteLine("OnCollectionViewEquipmentsSelectionChanged equipment = " + equipment.name);

			}
		}

	}
}
