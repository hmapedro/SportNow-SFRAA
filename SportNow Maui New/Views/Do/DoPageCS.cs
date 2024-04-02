using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Model.Charts;
using SportNow.Views.Profile;
using Syncfusion.Maui.Charts;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class DoPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
			this.CleanScreen();
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}



		MenuButton premiosButton, graduacoesButton, palmaresButton, participacoesEventosButton;
		private AbsoluteLayout premiosrelativeLayout, graduacoesrelativeLayout, palmaresrelativeLayout, participacoesEventosrelativeLayout;

		private StackLayout stackButtons;

		private CollectionView collectionViewPremios, collectionViewParticipacoesCompeticoes, collectionViewParticipacoesEventos;
		private SfCircularChart chart;

		private List<Award> awards;
		private List<Competition_Participation>  pastCompetitionParticipations;
		private List<Event_Participation> pastEventParticipations;

		private OptionButton minhasGraduacoesButton, programasExameButton;

		public void initLayout()
		{
			Title = "HISTÓRICO!";
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
			if (collectionViewPremios != null)
            {
                absoluteLayout.Remove(collectionViewPremios);
                collectionViewPremios = null;
            }
            if (graduacoesrelativeLayout != null)
            {
                absoluteLayout.Remove(graduacoesrelativeLayout);
                graduacoesrelativeLayout = null;
            }
            if (collectionViewParticipacoesCompeticoes != null)
            {
                absoluteLayout.Remove(collectionViewParticipacoesCompeticoes);
                collectionViewParticipacoesCompeticoes = null;
            }
            if (palmaresrelativeLayout != null)
            {
                absoluteLayout.Remove(palmaresrelativeLayout);
                palmaresrelativeLayout = null;
            }
            if (collectionViewParticipacoesEventos != null)
            {
                absoluteLayout.Remove(collectionViewParticipacoesEventos);
                collectionViewParticipacoesEventos = null;
            }

			if (chart != null)
			{
				absoluteLayout.Remove(chart);
				chart = null;
			}

		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "DO VISIT", "Visit Do Page");


            pastEventParticipations = await GetPastEventParticipations();
			pastCompetitionParticipations = await GetPastCompetitionParticipations();

			CreateStackButtons();
            if (premiosrelativeLayout == null)
            {
                CreatePremios();
            }

            if (graduacoesrelativeLayout == null)
            {
                CreateGrades();
            }

            if (participacoesEventosrelativeLayout == null)
            {
                CreateParticipacoesEventosColletion();
            }
            if (palmaresrelativeLayout == null)
            {
                CreateParticipacoesCompeticoesColletion();
            }

            activateLastSelectedTab();

            hideActivityIndicator();
		}

		public void activateLastSelectedTab() {

			if (App.DO_activetab == "graduacoes")
			{
				OnGraduacoesButtonClicked(null, null);
			}
			else if (App.DO_activetab == "palmares")
			{
				OnPalmaresButtonClicked(null, null);
			}
			else if (App.DO_activetab == "participacoesevento")
			{
				OnParticipacoesEventosButtonClicked(null, null);
			}
			else if (App.DO_activetab == "premios")
			{
				OnPremiosButtonClicked(null, null);
			}
			else
			{
				OnGraduacoesButtonClicked(null, null);
			}
		}

		public void CreateStackButtons()
		{

			Debug.Print("CreateStackButtons Constants.ScreenWidth = "+ App.screenWidth);
			var buttonWidth = (App.screenWidth - 15 * App.screenWidthAdapter) / 4;

			graduacoesButton = new MenuButton("GRADUAÇÕES", buttonWidth, 60 * App.screenHeightAdapter);
			graduacoesButton.button.Clicked += OnGraduacoesButtonClicked;


			palmaresButton = new MenuButton("PALMARÉS", buttonWidth, 60);
			palmaresButton.button.Clicked += OnPalmaresButtonClicked;

			participacoesEventosButton = new MenuButton("EVENTOS", buttonWidth, 60 * App.screenHeightAdapter);
			participacoesEventosButton.button.Clicked += OnParticipacoesEventosButtonClicked;

			premiosButton = new MenuButton("PRÉMIOS", buttonWidth, 60 * App.screenHeightAdapter);
			premiosButton.button.Clicked += OnPremiosButtonClicked;


			stackButtons = new StackLayout
			{
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60 * App.screenHeightAdapter,
				Children =
				{
					graduacoesButton,
					palmaresButton,
					participacoesEventosButton,
					premiosButton
				}
			};

            //Content = stackButtons;
			absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));
			
			graduacoesButton.activate();
			palmaresButton.deactivate();
			participacoesEventosButton.deactivate();
			premiosButton.deactivate();
		}

		public void CreatePremios() {
			premiosrelativeLayout = new AbsoluteLayout
			{
				Margin = new Thickness(5)
			};

			CreatePremiosCollection();

			//absoluteLayout.Add(premiosrelativeLayout);
            //absoluteLayout.SetLayoutBounds(premiosrelativeLayout, new Rect(0, 30 * App.screenHeightAdapter, App.screenWidth, (App.screenHeight) - (80 * App.screenHeightAdapter)));
		}


		public async void CreatePremiosCollection()
		{			
			var result = await GetAwards_Student(App.member);

			Label premiosLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Color.FromRgb(182, 145, 89), LineBreakMode = LineBreakMode.WordWrap };
			premiosLabel.Text = "PRÉMIOS";

			/*premiosrelativeLayout.Add(premiosLabel,
				xConstraint: )0),
				yConstraint: )0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: )50 * App.screenHeightAdapter));*/

			//COLLECTION PREMIOS
			collectionViewPremios = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = awards,
				ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Ainda não tens prémios.\n Continua a treinar e vais ver que vais conseguir.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewPremios.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
					HeightRequest = 220 * App.screenHeightAdapter,
                    WidthRequest = App.screenWidth / 3 - 30 * App.screenWidthAdapter
                };


				Image premioImage= new Image {};
				premioImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemabsoluteLayout.Add(premioImage);
				itemabsoluteLayout.SetLayoutBounds(premioImage, new Rect(10 * App.screenHeightAdapter, 0, 90 * App.screenHeightAdapter, 90 * App.screenHeightAdapter));

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(10 * App.screenHeightAdapter, 100 * App.screenHeightAdapter, 90 * App.screenHeightAdapter, 90 * App.screenHeightAdapter));				
				
				return itemabsoluteLayout;
			});

			premiosrelativeLayout.Add(collectionViewPremios);
            premiosrelativeLayout.SetLayoutBounds(collectionViewPremios, new Rect(0, 0, App.screenWidth, (App.screenHeight) - (70 * App.screenHeightAdapter)));
		}

		public void CreateGrades()
		{
            Debug.Print("CreateGrades");
            graduacoesrelativeLayout = new AbsoluteLayout
			{
				Margin = new Thickness(5)
			};

			CreateGraduacoesOptionButtons();

            if (App.DO_activetab == "graduacoes")
			{
				OnGraduacoesButtonClicked(null, null);
            }
            

			//absoluteLayout.Add(graduacoesrelativeLayout);
            //absoluteLayout.SetLayoutBounds(graduacoesrelativeLayout, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, (App.screenHeight) - (80 * App.screenHeightAdapter)));
		}

		public void CreateGraduacoesOptionButtons()
		{
			Debug.Print("CreateGraduacoesOptionButtons");

            var buttonWidth = ((App.screenWidth) / 2);

            minhasGraduacoesButton = new OptionButton("MINHAS GRADUAÇÕES", "mygrades.png", buttonWidth, 100 * App.screenHeightAdapter);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var minhasGraduacoesButton_tap = new TapGestureRecognizer();
			minhasGraduacoesButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new myGradesPageCS("MinhasGraduaçoes"));
			};
			minhasGraduacoesButton.GestureRecognizers.Add(minhasGraduacoesButton_tap);

			programasExameButton = new OptionButton("PROGRAMAS EXAME", "examinationprograms.png", buttonWidth, 100 * App.screenHeightAdapter);
			var programasExameButton_tap = new TapGestureRecognizer();
			programasExameButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new myGradesPageCS("ProgramasExame"));
			};
			programasExameButton.GestureRecognizers.Add(programasExameButton_tap);


			StackLayout stackGraduacaoesButtons = new Microsoft.Maui.Controls.StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 50,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 280,
				Children =
				{
					minhasGraduacoesButton,
					programasExameButton,
				}
			};

			graduacoesrelativeLayout.Add(stackGraduacaoesButtons);
            graduacoesrelativeLayout.SetLayoutBounds(stackGraduacaoesButtons, new Rect((App.screenWidth-20)/4, 80 * App.screenHeightAdapter, (App.screenWidth-20)/2, 280 * App.screenHeightAdapter));
			
		}


		public void CreateParticipacoesEventosColletion()
		{

			//COLLECTION PARTICIPAÇÕES EVENTOS
			collectionViewParticipacoesEventos = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
                //HorizontalOptions = LayoutOptions.Center,
				
				//BackgroundColor = Colors.Blue,
				ItemsSource = pastEventParticipations,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 10 * App.screenWidthAdapter },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem participações em eventos.\n Inscreve-te já nos próximos eventos.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize},
							}
					}
				}
			};

			foreach (Event_Participation event_participation in pastEventParticipations)
			{
				if ((event_participation.imagemNome == "") | (event_participation.imagemNome is null))
				{
					event_participation.imagemSource = "company_logo_square.png";
				}
				else
				{
					event_participation.imagemSource = Constants.images_URL + event_participation.evento_id + "_imagem_c";
				}
			}

			collectionViewParticipacoesEventos.SelectionChanged += OnCollectionViewParticipacoesEventosSelectionChanged;

			collectionViewParticipacoesEventos.ItemTemplate = new DataTemplate(() =>
			{
                //double itemWidth = App.screenWidth / 2 - 20 * App.screenWidthAdapter;

                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth
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
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "evento_name");

				itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), (((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2))));

                Label dateLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.WordWrap };
				dateLabel.SetBinding(Label.TextProperty, "evento_detailed_date");

				itemabsoluteLayout.Add(dateLabel);
				itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(3 * App.screenWidthAdapter, App.ItemHeight - (15 * App.screenHeightAdapter) - ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 3), App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 3));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.Source = "iconcheck.png";

				itemabsoluteLayout.Add(participationImagem);
				itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect(App.ItemWidth - (21 * App.screenHeightAdapter), 1 * App.screenHeightAdapter, 20 * App.screenHeightAdapter, 20 * App.screenHeightAdapter));
				
				return itemabsoluteLayout;

			});

		}


		public void CreateParticipacoesCompeticoesColletion()
		{
			palmaresrelativeLayout = new AbsoluteLayout
			{
				Margin = new Thickness(5)
			};

			//COLLECTION PARTICIPAÇÕES EVENTOS
			collectionViewParticipacoesCompeticoes = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = pastCompetitionParticipations,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem resultados de competições.\n Fala com o teu treinador para participares na tua primeira competição.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			foreach (Competition_Participation competition_participation in pastCompetitionParticipations)
			{
				if ((competition_participation.imagemNome == "") | (competition_participation.imagemNome is null))
				{
					competition_participation.imagemSource = "company_logo_square.png";
				}
				else
				{
					competition_participation.imagemSource = Constants.images_URL + competition_participation.competicao_id + "_imagem_c";
				}

				if (competition_participation.classificacao == "1")
				{
					competition_participation.classificacaoColor = Color.FromRgb(231, 188, 64);
				}
				else if (competition_participation.classificacao == "2")
				{
					competition_participation.classificacaoColor = Color.FromRgb(174, 174, 174);
				}
				else if (competition_participation.classificacao == "3")
				{
					competition_participation.classificacaoColor = Color.FromRgb(179, 144, 86);
				}
				else
				{
					competition_participation.classificacaoColor = Color.FromRgb(88, 191, 237);
					if ((competition_participation.classificacao == "") | (competition_participation.classificacao is null))
					{
						competition_participation.classificacao = "P";

					}
					else
					{
						competition_participation.classificacao = competition_participation.classificacao.ToUpper();
					}
				}
			}

			collectionViewParticipacoesCompeticoes.SelectionChanged += OnCollectionViewParticipacoesCompeticoesSelectionChanged;

			collectionViewParticipacoesCompeticoes.ItemTemplate = new DataTemplate(() =>
			{
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
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
				/*
                Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40, HorizontalOptions = LayoutOptions.Center }; //, HeightRequest = 60, WidthRequest = 60
                eventoImage.SetBinding(Image.SourceProperty, "imagemSource");
                */
                itemFrame.Content = eventoImage;

				itemabsoluteLayout.Add(itemFrame);
	            itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "competicao_name");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 2));

				Label categoryLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "categoria");

				itemabsoluteLayout.Add(categoryLabel);
				itemabsoluteLayout.SetLayoutBounds(categoryLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter + (App.ItemHeight - (15 * App.screenHeightAdapter)) / 2, App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));

				Label dateLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "competicao_detailed_date");

				itemabsoluteLayout.Add(dateLabel);
			    itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(3 * App.screenWidthAdapter, 15 * App.screenHeightAdapter + (App.ItemHeight - (15 * App.screenHeightAdapter)) * 3 / 4, App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));

				Label classificacaoLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, LineBreakMode = LineBreakMode.NoWrap, HeightRequest = 20 * App.screenHeightAdapter, WidthRequest = 20 * App.screenHeightAdapter,};
				classificacaoLabel.SetBinding(Label.TextProperty, "classificacao");

				Border classificacaoFrame = new Border
				{
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Padding = 0,
					HeightRequest = 20 * App.screenHeightAdapter,
                    WidthRequest = 20 * App.screenHeightAdapter,
                };
				classificacaoFrame.SetBinding(Label.BackgroundColorProperty, "classificacaoColor");
				classificacaoFrame.Content = classificacaoLabel;

				itemabsoluteLayout.Add(classificacaoFrame);
				itemabsoluteLayout.SetLayoutBounds(classificacaoFrame, new Rect(App.ItemWidth - (21 * App.screenHeightAdapter), 0, 20 * App.screenHeightAdapter, 20 * App.screenHeightAdapter));

				return itemabsoluteLayout;

			});

			palmaresrelativeLayout.Add(collectionViewParticipacoesCompeticoes);

            palmaresrelativeLayout.SetLayoutBounds(collectionViewParticipacoesCompeticoes, new Rect(0, 0, App.screenWidth, App.screenHeight - (500 * App.screenHeightAdapter)));

			if (pastCompetitionParticipations.Count > 0) { 
				chart = createChart();

				palmaresrelativeLayout.Add(chart);
                palmaresrelativeLayout.SetLayoutBounds(chart, new Rect(0, (App.screenHeight) - 500 * App.screenHeightAdapter, App.screenWidth, 200 * App.screenHeightAdapter));
			}
		}

		public SfCircularChart createChart() {
            SfCircularChart chart = new SfCircularChart();

			chart.BackgroundColor = App.backgroundColor;


            //Initializing Primary Axis
            /*CategoryAxis primaryAxis = new CategoryAxis();
			primaryAxis.Title.Text = "Classificacao";
			chart.X = primaryAxis;

			NumericalAxis secondaryAxis = new NumericalAxis();
			secondaryAxis.Title.Text = "#";
			chart.SecondaryAxis = secondaryAxis;*/


            //this.BindingContext = competition_results;

            this.BindingContext = new Competition_Results(pastCompetitionParticipations);

			//Initializing column series
			PieSeries series = new PieSeries();
			series.PaletteBrushes = new List<Brush>()
			{
				Color.FromRgb(231, 188, 64),
				Color.FromRgb(174, 174, 174),
				Color.FromRgb(179, 144, 86),
				Color.FromRgb(88, 191, 237)
			};

			series.SetBinding(ChartSeries.ItemsSourceProperty, "Data");
			series.XBindingPath = "classificacao";
			series.YBindingPath = "count";

			/*series. .EnableSmartLabels = true;
			series.DataMarkerPosition = CircularSeriesDataMarkerPosition.OutsideExtended;
			series.ConnectorLineType = ConnectorLineType.Bezier;*/

			series.StartAngle = 75;
			series.EndAngle = 435;

			series.EnableTooltip = true;
            series.ShowDataLabels = true;
            //series.DataMarker = new ChartDataMarker();

            /*series.DataMarker.ShowLabel = true;
			series.DataMarker.LabelStyle.TextColor = App.normalTextColor;
			series.DataMarker.LabelStyle.Margin = 0;
			series.DataMarker.LabelStyle.FontSize = 12;

			series.DataMarker.LabelContent = LabelContent.YValue;*/

            //series.EnableDataPointSelection = true;
            //series.SelectedDataPointColor = Colors.Red;

            chart.Series.Add(series);

			


			/*ChartLegend legend = new ChartLegend();
			legend.BackgroundColor = Color.FromRgb(245, 245, 240);
			legend.StrokeColor = Colors.Black;
			legend.StrokeWidth = 2;
			legend.Margin = new Thickness(5);
			legend.CornerRadius = new ChartCornerRadius(5);
			legend.StrokeDashArray = new double[] { 3, 3 };

			chart.Legend = legend;*/

			return chart;
		}

		public DoPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async Task<List<Competition_Participation>> GetPastCompetitionParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> pastCompetitionParticipations = await competitionManager.GetPastCompetitionParticipations(App.member.id);


			Debug.WriteLine("GetPastCompetitionParticipations pastCompetitionParticipations.count "+ pastCompetitionParticipations.Count);
			return pastCompetitionParticipations;
		}

		async Task<List<Event_Participation>> GetPastEventParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			EventManager eventManager = new EventManager();

			List<Event_Participation> pastEventParticipations = await eventManager.GetPastEventParticipations(App.member.id);

			return pastEventParticipations;
		}


		async void OnPremiosButtonClicked(object sender, EventArgs e)
		{
            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "DO AWARDS VISIT", "Visit Do Awards Page");

            App.DO_activetab = "premios";
			premiosButton.activate();
			graduacoesButton.deactivate();
			palmaresButton.deactivate();
			participacoesEventosButton.deactivate();

            absoluteLayout.Remove(premiosrelativeLayout);
            absoluteLayout.Remove(graduacoesrelativeLayout);
            absoluteLayout.Remove(palmaresrelativeLayout);
            absoluteLayout.Remove(collectionViewParticipacoesEventos);

            absoluteLayout.Add(premiosrelativeLayout);
            //absoluteLayout.SetLayoutBounds(premiosrelativeLayout, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - (80 * App.screenHeightAdapter)));
            absoluteLayout.SetLayoutBounds(premiosrelativeLayout, new Rect(0 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - (200 * App.screenHeightAdapter)));

        }

		async void OnGraduacoesButtonClicked(object sender, EventArgs e)
		{

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "DO GRADES VISIT", "Visit Do Grades Page");

            App.DO_activetab = "graduacoes";
			premiosButton.deactivate();
			graduacoesButton.activate();
			palmaresButton.deactivate();
			participacoesEventosButton.deactivate();

            absoluteLayout.Remove(graduacoesrelativeLayout);
            absoluteLayout.Remove(palmaresrelativeLayout);
            absoluteLayout.Remove(collectionViewParticipacoesEventos);
            absoluteLayout.Remove(premiosrelativeLayout);

            if (graduacoesrelativeLayout != null)
			{
                absoluteLayout.Add(graduacoesrelativeLayout);
                absoluteLayout.SetLayoutBounds(graduacoesrelativeLayout, new Rect(0 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - (200 * App.screenHeightAdapter)));
            }
        }

		async void OnPalmaresButtonClicked(object sender, EventArgs e)
		{

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "DO PALMARES VISIT", "Visit Do Palmares Page");

            App.DO_activetab = "palmares";
			premiosButton.deactivate();
			graduacoesButton.deactivate();
			palmaresButton.activate();
			participacoesEventosButton.deactivate();

            absoluteLayout.Remove(graduacoesrelativeLayout);
            absoluteLayout.Remove(palmaresrelativeLayout);
            absoluteLayout.Remove(collectionViewParticipacoesEventos);
            absoluteLayout.Remove(premiosrelativeLayout);

            absoluteLayout.Add(palmaresrelativeLayout);
            //absoluteLayout.SetLayoutBounds(palmaresrelativeLayout, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - (80 * App.screenHeightAdapter)));
            absoluteLayout.SetLayoutBounds(palmaresrelativeLayout, new Rect(0 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - (200 * App.screenHeightAdapter)));

        }

		async void OnParticipacoesEventosButtonClicked(object sender, EventArgs e)
		{

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "DO PAST EVENTS VISIT", "Visit Do Past Events Page");

            App.DO_activetab = "participacoesevento";
			premiosButton.deactivate();
			graduacoesButton.deactivate();
			palmaresButton.deactivate();
			participacoesEventosButton.activate();

            absoluteLayout.Remove(graduacoesrelativeLayout);
            absoluteLayout.Remove(palmaresrelativeLayout);
            absoluteLayout.Remove(collectionViewParticipacoesEventos);
            absoluteLayout.Remove(premiosrelativeLayout);

            absoluteLayout.Add(collectionViewParticipacoesEventos);
            //absoluteLayout.SetLayoutBounds(collectionViewParticipacoesEventos, new Rect(0 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - (200 * App.screenHeightAdapter)));
            absoluteLayout.SetLayoutBounds(collectionViewParticipacoesEventos, new Rect(10 * App.screenWidthAdapter, 80 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - (200 * App.screenHeightAdapter)));

        }


        async void OnCollectionViewParticipacoesEventosSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewParticipacoesEventosSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Event_Participation event_participation = (sender as CollectionView).SelectedItem as Event_Participation;
				await Navigation.PushAsync(new DetailEventParticipationPageCS(event_participation));
			}
		}

		async void OnCollectionViewParticipacoesCompeticoesSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewParticipacoesCompeticoesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Competition_Participation competition_participation = (sender as CollectionView).SelectedItem as Competition_Participation;
				await Navigation.PushAsync(new DetailCompetitionResultPageCS(competition_participation));
			}
		}

		async Task<int> GetAwards_Student(Member member)
		{
			Debug.WriteLine("GetAwards_Student");
			AwardManager awardManager = new AwardManager();

			awards = await awardManager.GetAwards_Student(member.id);
			if (awards == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return -1;
			}

			foreach (Award award in awards)
			{
				if ((award.imagem == "") | (award.imagem is null))

                {
					award.imagemSource = "company_logo_square.png";
				}
				else
				{
                    award.imagemSource = Constants.images_URL + award.id+ "_imagem_c";
                }
                Debug.Print("Imagem Premio  = " + award.imagem);
                Debug.Print("Imagem Premio Source = " + award.imagemSource);
			}

			return 1;
		}

	}
}
