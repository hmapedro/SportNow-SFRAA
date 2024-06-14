using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class EquipamentsOrderPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            //initSpecificLayout();
        }

		Equipment equipment;

		public void initLayout()
		{
			Title = "ENCOMENDA EQUIPAMENTO";
		}


		public void CleanScreen()
		{

		}

		public async void initSpecificLayout()
		{
			CreateEquipmentView();
		}

		

		public void CreateEquipmentView()
		{
			

			Label subtypeLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			subtypeLabel.Text = App.ToTitleCase(equipment.type) + " - " + equipment.subtype;

			absoluteLayout.Add(subtypeLabel);
            absoluteLayout.SetLayoutBounds(subtypeLabel, new Rect(0, 0, (App.screenWidth / 5 * 4) - (10 * App.screenHeightAdapter), 30 * App.screenHeightAdapter));

			Label valueTitleLabel = new Label { FontFamily = "futuracondensedmedium", Text = "VALOR", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };

			absoluteLayout.Add(valueTitleLabel);
            absoluteLayout.SetLayoutBounds(valueTitleLabel, new Rect((App.screenWidth / 5 * 4), 0, (App.screenWidth / 5) - (10 * App.screenHeightAdapter), 30 * App.screenHeightAdapter));

			Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			nameLabel.Text = equipment.name;

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

			absoluteLayout.Add(nameFrame);
            absoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 40 * App.screenHeightAdapter, (App.screenWidth / 5 * 4) - (10 * App.screenHeightAdapter), 30 * App.screenHeightAdapter));

			Label valueLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			valueLabel.Text = equipment.valueFormatted;

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

			absoluteLayout.Add(valueFrame);
            absoluteLayout.SetLayoutBounds(valueFrame, new Rect((App.screenWidth / 5 * 4), 40 * App.screenHeightAdapter, (App.screenWidth / 5) - (10 * App.screenHeightAdapter), 30 * App.screenHeightAdapter));


			RoundButton orderButton = new RoundButton("SOLICITAR EQUIPAMENTO", App.screenWidth - 10 * App.screenWidthAdapter, 50);
			//Button orderButton = new Button { BackgroundColor = Colors.Transparent, VerticalOptions = LayoutOptions.Center, HorizontalOptions= LayoutOptions.Center, FontSize = 20, TextColor = App.normalTextColor};
			orderButton.button.Clicked += OnOrderButtonClicked;

			absoluteLayout.Add(orderButton);
            absoluteLayout.SetLayoutBounds(orderButton, new Rect(5 * App.screenWidthAdapter, 140 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

			Label orderdescLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			orderdescLabel.Text = "Ao solicitar este equipamento iremos efetuar uma validação do Stock disponível e o responsável do teu dojo levar-lhe a sua encomenda com a maior brevidade possível.";

			absoluteLayout.Add(orderdescLabel);
            absoluteLayout.SetLayoutBounds(orderdescLabel, new Rect(0, 200 * App.screenHeightAdapter, App.screenWidth, 80 * App.screenHeightAdapter));

		}


		
		public EquipamentsOrderPageCS(Equipment equipment)
		{
			this.equipment = equipment;
			this.initLayout();
			initSpecificLayout();
		}

		async void OnOrderButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("OnOrderButtonClicked");
			EquipmentManager equipmentManager = new EquipmentManager();

			var result = await equipmentManager.CreateEquipmentOrder(App.member.id, App.member.name, equipment.id, equipment.type + " - " + equipment.subtype + " - " + equipment.name);
			if ((result == "-1") | (result == "-2"))
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
			}
			hideActivityIndicator();

            await DisplayAlert("EQUIPAMENTO SOLICITADO", "A tua encomenda foi realizada com sucesso. Fala com o teu instrutor para saber quando te conseguirá entregar a mesma.", "Ok");



//			showActivityIndicator();
//			hideActivityIndicator();   //Hide loader

		}
	}
}
