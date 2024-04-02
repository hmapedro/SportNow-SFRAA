using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;


namespace SportNow.Views.Personal
{
	public class PersonalCoachPageCS : DefaultPage
	{
		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private CollectionView coachsCollectionView;

		private List<Member> coachesMemberList;

        string personalClass_type;

        public void initLayout()
		{
			Title = "ESCOLHER TREINADOR";

			absoluteLayout = new AbsoluteLayout
			{	
				Margin = new Thickness(20)
			};
			Content = absoluteLayout;
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH SELECT", "Visit Personal Coach Selection Page with class type = "+ personalClass_type);

            MemberManager memberManager = new MemberManager();
			coachesMemberList = await memberManager.GetPersonalCoaches();
			CompletecoachesMemberList();
            CreateCoachColletion();

            hideActivityIndicator();
        }

        public void CompletecoachesMemberList()
        {
            foreach (Member coachMember in coachesMemberList)
            {
                coachMember.imagesourceObject = new UriImageSource
				{
					Uri = new Uri(Constants.images_URL + coachMember.id + "_photo"),
					CachingEnabled = false,
					CacheValidity = new TimeSpan(0, 0, 0, 1)
				};
                //double valorMinimo = Convert.ToDouble(coachMember.valor_hora_minino);// double.Parse(selectedCoach.valor_hora_minino.Replace(".", ","));
                //double valorMaximo = Convert.ToDouble(coachMember.valor_hora_maximo); //double.Parse(selectedCoach.valor_hora_maximo.Replace(".", ","));

                double valorMinimo = double.Parse(coachMember.valor_hora_minino, CultureInfo.InvariantCulture);
                double valorMaximo = double.Parse(coachMember.valor_hora_maximo, CultureInfo.InvariantCulture);


                coachMember.valor_intervalo = "Valor Hora: "+Convert.ToInt32(valorMinimo).ToString("0.00") + "€ - " + Convert.ToInt32(valorMaximo).ToString("0.00") + "€";
            }
        }


        public void CreateCoachColletion()
		{
            //COLLECTION COACHES
            coachsCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                ItemsSource = coachesMemberList,

                /*ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    SnapPointsType = SnapPointsType.MandatorySingle,
                    SnapPointsAlignment = SnapPointsAlignment.End
                },*/
                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter },
                EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Treinadores Pessoais disponíveis.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

            coachsCollectionView.SelectionChanged += OnCoachCollectionViewSelectionChanged;

			coachsCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				var coachItemWidth = (App.screenWidth / 2) - 10 * App.screenWidthAdapter;


                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout()
				{
					HeightRequest = 340 * App.screenHeightAdapter,
                    WidthRequest = coachItemWidth
                };

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "nickname");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(0, 0, coachItemWidth, 40 * App.screenHeightAdapter));

				RoundImage coachMemberImage = new RoundImage();
				coachMemberImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemabsoluteLayout.Add(coachMemberImage);
				itemabsoluteLayout.SetLayoutBounds(coachMemberImage, new Rect(0, 40 * App.screenHeightAdapter, coachItemWidth, 160 * App.screenHeightAdapter));

                Label personalCV_Label = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.menuButtonFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                personalCV_Label.SetBinding(Label.TextProperty, "personal_cv");

				itemabsoluteLayout.Add(personalCV_Label);
				itemabsoluteLayout.SetLayoutBounds(personalCV_Label, new Rect(0, 215 * App.screenHeightAdapter, coachItemWidth, 40 * App.screenHeightAdapter));

                Label disponibilidadeLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.menuButtonFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                disponibilidadeLabel.SetBinding(Label.TextProperty, "disponibilidade");

				itemabsoluteLayout.Add(disponibilidadeLabel);
				itemabsoluteLayout.SetLayoutBounds(disponibilidadeLabel, new Rect(0, 255 * App.screenHeightAdapter, coachItemWidth, 40 * App.screenHeightAdapter));

                Label valor_intervaloLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.menuButtonFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                valor_intervaloLabel.SetBinding(Label.TextProperty, "valor_intervalo");

				itemabsoluteLayout.Add(valor_intervaloLabel);
				itemabsoluteLayout.SetLayoutBounds(valor_intervaloLabel, new Rect(0, 295 * App.screenHeightAdapter, coachItemWidth, 40 * App.screenHeightAdapter));
                
                return itemabsoluteLayout;
			});

			absoluteLayout.Add(coachsCollectionView);
			absoluteLayout.SetLayoutBounds(coachsCollectionView, new Rect(0, 0, App.screenWidth, App.screenHeight - 100 * App.screenHeightAdapter));

		}


		public PersonalCoachPageCS(string personalClass_type)
		{
			this.personalClass_type = personalClass_type;

            this.initLayout();
            this.initSpecificLayout();
        }

		async void OnCoachCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("MainPageCS.OnCoachCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
                LogManager logManager = new LogManager();
                


                Member selectedCoach = (Member) (sender as CollectionView).SelectedItem;
				(sender as CollectionView).SelectedItem = null;
                await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH SELECTED", "Selected Personal Coach  = " + selectedCoach.nickname);

                await Navigation.PushAsync(new PersonalConfirmPageCS(selectedCoach, personalClass_type));
                hideActivityIndicator();
            }
		}

	}
}
