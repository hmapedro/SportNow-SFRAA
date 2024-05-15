using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views.CompleteRegistration
{
	public class BeginPageCS : DefaultPage
	{
		bool dialogShowing;

		CheckBox checkboxConfirm;


        protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}


		public void initLayout()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS - initLayout");
			Title = "REGISTO";

            //NavigationPage.SetHasNavigationBar(this, false);

            App.AdaptScreen();

			//NavigationPage.SetHasBackButton(this, false);
		}


		public void initSpecificLayout()
		{

			Debug.Print("CompleteRegistration_Begin_PageCS - initSpecificLayout");

            Grid gridBegin = new Microsoft.Maui.Controls.Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter };
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = 200 * App.screenHeightAdapter });
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = 100 * App.screenHeightAdapter });
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star  });
            gridBegin.RowDefinitions.Add(new RowDefinition { Height = 40 * App.screenHeightAdapter });
            //gridBegin.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
            gridBegin.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 2 }); //GridLength.Auto
            gridBegin.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 2 }); //GridLength.Auto 


            Label welcomeLabel = new Label
			{
				Text = "BEM-VINDO",
				TextColor = App.topColor,
				FontSize = App.bigTitleFontSize,
				HorizontalOptions = LayoutOptions.Center,
                FontFamily = "futuracondensedmedium",
            };

            Image company_logo = new Image
            {
                Source = "company_logo.png",
                HorizontalOptions = LayoutOptions.Center,
                Opacity = 0.8,
            };

            Label labelMember = new Label { FontFamily = "futuracondensedmedium",  BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
            labelMember.Text = "PODE USAR A APP DA SFRAA COMO SÓCIO OU APENAS COMO UTILIZADOR REGISTADO";

            RoundButton memberButton = new RoundButton("QUERO SER SÓCIO", App.screenWidth / 2 - 20 * App.screenWidthAdapter, 80 * App.screenHeightAdapter);
            memberButton.button.BackgroundColor = App.topColor;

            memberButton.button.Clicked += confirmMemberTypeButtonClicked;

            RoundButton notMemberButton = new RoundButton("NÃO QUERO SER SÓCIO", App.screenWidth / 2 - 20 * App.screenWidthAdapter, 80 * App.screenHeightAdapter);
            notMemberButton.button.BackgroundColor = App.bottomColor;
            notMemberButton.button.Clicked += confirmNotMemberTypeButtonClicked;

            checkboxConfirm = new CheckBox { Color = App.topColor };


            Label labelConsentimentos = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelConsentimentos.Text = "PARA CONTINUAR, TEM DE CONSENTIR COM OS TERMOS E CONDIÇÕES DISPONÍVEIS AQUI";

            TapGestureRecognizer labelConsentimentos_tap = new TapGestureRecognizer();
            labelConsentimentos_tap.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new ConsentPageCS());
            };
            labelConsentimentos.GestureRecognizers.Add(labelConsentimentos_tap);


            Grid gridConsent = new Microsoft.Maui.Controls.Grid { Padding = 0, RowSpacing = 5 * App.screenHeightAdapter };
            gridConsent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridConsent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridConsent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto

            RoundButton confirmButton = new RoundButton("CONTINUAR", App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			confirmButton.button.Clicked += OnConfirmButtonClicked;


            gridBegin.Add(welcomeLabel, 0, 0);
            Grid.SetColumnSpan(welcomeLabel, 2);
            gridBegin.Add(company_logo, 0, 1);
            Grid.SetColumnSpan(company_logo, 2);
            gridBegin.Add(labelMember, 0, 2);
            Grid.SetColumnSpan(labelMember, 2);
            gridBegin.Add(memberButton, 0, 3);
            gridBegin.Add(notMemberButton, 1, 3);
            gridConsent.Add(checkboxConfirm, 0, 0);
            gridConsent.Add(labelConsentimentos, 1, 0);
            gridBegin.Add(gridConsent, 0, 5);
            Grid.SetColumnSpan(gridConsent, 2);
            //gridBegin.Add(confirmButton, 0, 6);
            //Grid.SetColumnSpan(confirmButton, 2);

            absoluteLayout.Add(gridBegin);
            absoluteLayout.SetLayoutBounds(gridBegin, new Rect(0 * App.screenWidthAdapter, 0, App.screenWidth - 0 * App.screenWidthAdapter, App.screenHeight - 100));


        }

        public BeginPageCS()
		{
            this.initLayout();
			this.initSpecificLayout();
		}

		async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new NewMemberPageCS());
			//await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
		}

        async void confirmMemberTypeButtonClicked(object sender, EventArgs e)
        {
            if (checkboxConfirm.IsChecked == false)
            {
                await DisplayAlert("Confirmação necessária", "Para prosseguir é necessário confirmar que aceitas as condições expostas.", "OK");
                return;
            }
            createNewMember();
            App.member.estado = "ativo";
            App.member.member_type = "socio";
            await Navigation.PushAsync(new NewMemberPageCS());
        }

        async void confirmNotMemberTypeButtonClicked(object sender, EventArgs e)
        {
            if (checkboxConfirm.IsChecked == false)
            {
                await DisplayAlert("Confirmação necessária", "Para prosseguir é necessário confirmar que aceitas as condições expostas.", "OK");
                return;
            }
            createNewMember();
            App.member.estado = "ativo";
            App.member.member_type = "nao_socio";
            await Navigation.PushAsync(new NewMemberPageCS());
        }

        public void createNewMember()
        {
            App.member = new Member();
            App.member.id = "";
            App.member.name = "";
            App.member.cc_number = "";
            App.member.gender = "female";
            //App.member.country = "PORTUGAL";
            
            App.member.name = "SFRAA Sócio";
            App.member.cc_number = "111111111";
            App.member.nif = "222222222";
            App.member.birthdate = "2000-01-01";
            App.member.email = "hmap@hotmail.com";
            App.member.phone = "911111111";
            App.member.address = "Rua 1111";
            App.member.city = "Amadora";
            App.member.postalcode = "1000-000";
            App.original_member = App.member;
        }
    }

}