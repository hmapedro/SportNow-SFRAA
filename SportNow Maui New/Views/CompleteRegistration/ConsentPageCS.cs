using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Runtime.CompilerServices;


namespace SportNow.Views.CompleteRegistration
{
    public class ConsentPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			initLayout();
			initSpecificLayout();
		}


		protected async override void OnDisappearing()
		{
			if (absoluteLayout != null)
			{
				absoluteLayout = null;
				this.Content = null;
			}

		}

		//Image estadoQuotaImage;

		CheckBox checkboxConfirm;

        private ScrollView scrollView;

        public void initLayout()
		{
			Title = "POLÍTICA DADOS";

		}


		public async void initSpecificLayout()
		{
			if (absoluteLayout == null)
			{
				initBaseLayout();
            }

            scrollView = new ScrollView { Orientation = ScrollOrientation.Vertical };

            absoluteLayout.Add(scrollView);
            absoluteLayout.SetLayoutBounds(scrollView, new Rect(10 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 120 * App.screenHeightAdapter));


			Microsoft.Maui.Controls.Grid gridConsent = new Microsoft.Maui.Controls.Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
            scrollView.Content = gridConsent;
            gridConsent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gridConsent.RowDefinitions.Add(new RowDefinition { Height = 100 * App.screenHeightAdapter });
            gridConsent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridConsent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridConsent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 
            gridConsent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label labelRegulamentoInterno = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			labelRegulamentoInterno.Text = "O NKS na execução da atividade laboral, acede e lida, quando necessário, com os dados pessoais dos nossos atletas, sócios, parceiros e demais atores que intervém para o desenrolar na nossa atividade associativa. Esta comunicação pretende informar da forma como o NKS processa esses dados pessoais e qual a finalidade do tratamento das mesmas, assim como informar dos direitos que assistem aos titulares dos dados e da forma como os mesmos poderão exercer esses mesmos direitos.\n\nEntidade responsável pelo tratamento:\nNKS - Núcleo Karate de Sangalhos – NIF: 509 923 194\nCom morada na Rua Ivo Neves, n.º405, 3780-524 Sangalhos\nE-mail: nks@karatesangalhos.com\n\nTratamos os dados pessoais para as seguintes finalidades:\nPrestação de serviços relacionados com a nossa atividade associativa;\nCumprimento de obrigações legais;\nEfeitos de marketing e redes sociais.\n\nO NKS poderá ter de comunicar os dados pessoais recolhidos a terceiros que estejam envolvidos na prestação dos serviços por si contratados, assim como a autoridades judiciais, fiscais e regulatórias, com a finalidade do cumprimento de obrigações legais. Os dados pessoais recolhidos serão exclusivamente tratados para as finalidades acima indicadas pelo NKS e serão conservados somente durante o período necessário à concretização das finalidades que motivaram a sua recolha.";

            Label rgpdLabel = new Label { FontFamily = "futuracondensedmedium", Text = "Consulta aqui a Política de Tratamento de Dados do NK Sangalhos", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 13 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

            var rgpdLabel_tap = new TapGestureRecognizer();
            rgpdLabel_tap.Tapped += async (s, e) =>
            {
                try
                {
                    await Browser.OpenAsync("https://karatesangalhos.pt/rgpd", BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    // An unexpected error occured. No browser may be installed on the device.
                }
            };
            rgpdLabel.GestureRecognizers.Add(rgpdLabel_tap);


            Label labelConfirm = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelConfirm.Text = "CONFIRMO QUE ACEITO A POLÍTICA DE TRATAMENTO DE DADOS.";


            checkboxConfirm = new CheckBox { Color = App.topColor, HorizontalOptions = LayoutOptions.Start};

            gridConsent.Add(labelRegulamentoInterno, 0, 0);
            Grid.SetColumnSpan(labelRegulamentoInterno, 2);

            gridConsent.Add(rgpdLabel, 0, 1);
            Grid.SetColumnSpan(rgpdLabel, 2);

            gridConsent.Add(checkboxConfirm, 0, 2);
			gridConsent.Add(labelConfirm, 1, 2);

            RoundButton confirmButton = new RoundButton("CONFIRMAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			confirmButton.button.Clicked += confirmConsentButtonClicked;

			gridConsent.Add(confirmButton, 0, 3);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(confirmButton, 2);

            /*absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenWidthAdapter, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));
			*/

        }

        public ConsentPageCS()
		{
            this.initLayout();
            this.initSpecificLayout();
        }

		async void confirmConsentButtonClicked(object sender, EventArgs e)
		{

            if (checkboxConfirm.IsChecked == false)
			{
                await DisplayAlert("Confirmação necessária", "Para prosseguir é necessário confirmar que aceitas as condições expostas.", "OK");
				return;
            }
            //SAVE CONSENTIMENTOS!!!!!
            showActivityIndicator();
			MemberManager memberManager = new MemberManager();
			//App.member = new Member();
			App.member.consentimento_regulamento = "1";// Convert.ToInt32(checkBoxRegulamentoInterno.IsChecked).ToString();				
            //var result = await memberManager.Update_Member_Authorizations(App.member.id, App.member.consentimento_regulamento);
            hideActivityIndicator();
            await Navigation.PushAsync(new DocumentsPageCS());
        }
	}

}