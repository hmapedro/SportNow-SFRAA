using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class QuotasListPageCS : DefaultPage
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
		private AbsoluteLayout quotasabsoluteLayout;

		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private CollectionView collectionViewPastQuotas;

		private List<Fee> pastQuotas;

		Image estadoQuotaImage;

        private Microsoft.Maui.Controls.Grid gridInactiveFee, gridActiveFee;
        RegisterButton activateButton;

        public void initLayout()
		{
			Title = "QUOTAS";

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
				absoluteLayout.Remove(quotasabsoluteLayout);

				stackButtons = null;
				collectionViewPastQuotas = null;
			}
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateQuotas();

			hideActivityIndicator();
		}


		public async void CreateQuotas() {
            quotasabsoluteLayout = new AbsoluteLayout
			{
				Margin = new Thickness(0)
			};

            _ = await CreatePastQuotas();
			CreateCurrentQuota(quotasabsoluteLayout);
            
			
			

			absoluteLayout.Add(quotasabsoluteLayout);
			absoluteLayout.SetLayoutBounds(quotasabsoluteLayout, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 80 * App.screenHeightAdapter));
		}

        public async void CreateCurrentQuota(AbsoluteLayout quotasabsoluteLayout)
		{
			if (App.member.currentFee == null)
			{
				var result = await GetCurrentFees(App.member);
			}
			

			bool hasQuotaPayed = false;

			if (App.member.currentFee != null)
			{
				if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
				{
					hasQuotaPayed = true;
				}
			}


            if (hasQuotaPayed)
            {
                createActiveFeeLayout();
            }
            else
            {
                createInactiveFeeLayout();
            }

		}

        public void createInactiveFeeLayout()
        {
            gridInactiveFee = new Microsoft.Maui.Controls.Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, RowSpacing = 10 * App.screenHeightAdapter };
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = 100 });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = 50 });
            gridInactiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //
            gridInactiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            Label feeYearLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = DateTime.Now.ToString("yyyy"),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            Image akslLogoFee = new Image
            {
                Source = "company_logo.png",
                WidthRequest = 100 * App.screenWidthAdapter,
                Opacity = 0.50
            };

            Image fnkpLogoFee = new Image
            {
                Source = "logo_fnkp.png",
                WidthRequest = 100 * App.screenWidthAdapter,
                Opacity = 0.50
            };

            Label feeInactiveLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Quota Inativa",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Colors.Red,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            Label feeInactiveCommentLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Atenção: Com as quotas inativas o aluno não poderá participar em eventos e não tem acesso a seguro desportivo em caso de lesão.",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.titleFontSize
            };

            activateButton = new RegisterButton("ATIVAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            activateButton.button.Clicked += OnActivateButtonClicked;


            gridInactiveFee.Add(feeYearLabel, 0, 0);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeYearLabel, 2);

            gridInactiveFee.Add(fnkpLogoFee, 0, 1);
            gridInactiveFee.Add(akslLogoFee, 1, 1);

            gridInactiveFee.Add(feeInactiveLabel, 0, 2);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeInactiveLabel, 2);

            gridInactiveFee.Add(feeInactiveCommentLabel, 0, 3);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeInactiveCommentLabel, 2);


            Label historicoQuotasLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
            historicoQuotasLabel.Text = "HISTÓRICO QUOTAS";

            gridInactiveFee.Add(historicoQuotasLabel, 0, 5);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(historicoQuotasLabel, 2);


            gridInactiveFee.Add(collectionViewPastQuotas, 0, 6);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(collectionViewPastQuotas, 2);

            gridInactiveFee.Add(activateButton, 0, 4);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(activateButton, 2);

            absoluteLayout.Add(gridInactiveFee);
            absoluteLayout.SetLayoutBounds(gridInactiveFee, new Rect(10 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 200 * App.screenHeightAdapter));

        }


        public void createActiveFeeLayout()
        {
            gridActiveFee = new Microsoft.Maui.Controls.Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, RowSpacing = 10 * App.screenHeightAdapter };
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = 100 });
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridActiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
            gridActiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            Label feeYearLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = DateTime.Now.ToString("yyyy"),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            Image akslLogoFee = new Image
            {
                Source = "company_logo.png",
                WidthRequest = 100 * App.screenWidthAdapter
            };

            Image fnkpLogoFee = new Image
            {
                Source = "logo_fnkp.png",
                WidthRequest = 100 * App.screenWidthAdapter

            };

            Label feeActiveLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Quotas Ativas",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.topColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            Label feeActiveDueDateLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Válida até 31-12-" + DateTime.Now.ToString("yyyy"),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = 35
            };


            gridActiveFee.Add(feeYearLabel, 0, 0);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeYearLabel, 2);

            gridActiveFee.Add(fnkpLogoFee, 0, 1);
            gridActiveFee.Add(akslLogoFee, 1, 1);

            gridActiveFee.Add(feeActiveLabel, 0, 2);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeActiveLabel, 2);

            gridActiveFee.Add(feeActiveDueDateLabel, 0, 3);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(feeActiveDueDateLabel, 2);


            Label historicoQuotasLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
            historicoQuotasLabel.Text = "HISTÓRICO QUOTAS";

            gridActiveFee.Add(historicoQuotasLabel, 0, 4);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(historicoQuotasLabel, 2);

            gridActiveFee.Add(collectionViewPastQuotas, 0, 5);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(collectionViewPastQuotas, 2);

            absoluteLayout.Add(gridActiveFee);
            absoluteLayout.SetLayoutBounds(gridActiveFee, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 110 * App.screenHeightAdapter));

        }

        public async Task<int> CreatePastQuotas()
		{			
			var result = await GetPastFees(App.member);
            
			//COLLECTION GRADUACOES
			collectionViewPastQuotas = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = App.member.pastFees,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem quotas anteriores.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewPastQuotas.SelectionChanged += OncollectionViewFeeSelectionChangedAsync;

			collectionViewPastQuotas.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest= 40 * App.screenHeightAdapter,
                    WidthRequest = App.screenWidth
                };

				Border itemFrame = new Border
                {
                    BackgroundColor = Colors.Transparent,

                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
					Padding = new Thickness(2, 2, 2, 2),
					HeightRequest = 40 * App.screenHeightAdapter,
					VerticalOptions = LayoutOptions.Center,
				};

				//itemFrame.Content = itemabsoluteLayout;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.screenWidth-40*App.screenWidthAdapter, 40 * App.screenHeightAdapter));

                Label periodLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formLabelFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				periodLabel.SetBinding(Label.TextProperty, "periodo");

				itemabsoluteLayout.Add(periodLabel);
				itemabsoluteLayout.SetLayoutBounds(periodLabel, new Rect(5 * App.screenWidthAdapter, 0, 30 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formLabelFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "tipo_desc");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(35 * App.screenWidthAdapter, 0, App.screenWidth - 40 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

			return 0;
			/*quotasabsoluteLayout.Add(collectionViewPastQuotas);
            quotasabsoluteLayout.SetLayoutBounds(collectionViewPastQuotas, new Rect(0, 200 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 360 * App.screenHeightAdapter));*/
		}


		public QuotasListPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async Task<int> GetCurrentFees(Member member)
		{
			Debug.WriteLine("GetCurrentFees");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetCurrentFees(member);
			if (result == -1)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return -1;
			}
			return result;
		}

		async Task<int> GetPastFees(Member member)
		{
			Debug.WriteLine("GetPastFees");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetPastFees(member);
			if (result == -1)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return -1;
			}
			return result;
		}

		void OncollectionViewFeeSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
		{
			Debug.Print("OncollectionViewFeeSelectionChangedAsync");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Fee selectedFee = (sender as CollectionView).SelectedItem as Fee;

				InvoiceDocument(selectedFee);
				
				(sender as CollectionView).SelectedItem = null;
			}
			else
			{
				Debug.WriteLine("OncollectionViewMonthFeesSelectionChanged selected item = nulll");
			}
		}

		public async void InvoiceDocument(Fee fee)
		{
			Payment payment = await GetFeePaymentAsync(fee);
			if (payment.invoiceid != null)
			{
				await Navigation.PushAsync(new InvoiceDocumentPageCS(payment));
			}
		}
		

		public async Task<Payment> GetFeePaymentAsync(Fee fee)
		{
			Debug.WriteLine("GetFeePayment");
			MemberManager memberManager = new MemberManager();

			List<Payment> result = await memberManager.GetFeePayment(fee.id);
			if (result == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return result[0];
			}
			return result[0];
		}

        async void OnActivateButtonClicked(object sender, EventArgs e)
        {

            Debug.Print("App.member.member_type = " + App.member.member_type);
            showActivityIndicator();
            activateButton.IsEnabled = false;

            MemberManager memberManager = new MemberManager();


            if (App.member.currentFee is null)
            {

                var result_create = "0";

                result_create = await memberManager.CreateFee(App.member.id, App.member.member_type, DateTime.Now.ToString("yyyy"));
                if (result_create == "-1")
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };
                }

                var result_get = await GetCurrentFees(App.member);
                if (result_create == "-1")
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };
                }
            }

            await Navigation.PushAsync(new QuotasPaymentPageCS(App.member));
            hideActivityIndicator();
        }

    }
}
