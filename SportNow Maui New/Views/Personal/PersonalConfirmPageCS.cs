using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Globalization;



namespace SportNow.Views.Personal
{
	public class PersonalConfirmPageCS : DefaultPage
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


        Member selectedCoach;
        string personalClass_type;
        FormValueEditLongText objetivosEntry, disponibilidadeEntry;
        Picker numeroAulasPicker;

        Label valoresLabel;

        public void initLayout()
		{
			Title = "ENVIAR PEDIDO";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH CONFIRM", "Visit Personal Coach Confirm Page");

            Label objetivosLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "OBJETIVOS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize
            };
            absoluteLayout.Add(objetivosLabel);
            absoluteLayout.SetLayoutBounds(objetivosLabel, new Rect(10 * App.screenWidthAdapter, 0, App.screenWidth, 40 * App.screenHeightAdapter));

            Label objetivosExplicacaoLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O que gostarias de alcançar com estas aulas:",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize
            };
            absoluteLayout.Add(objetivosExplicacaoLabel);
            absoluteLayout.SetLayoutBounds(objetivosExplicacaoLabel, new Rect(10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter, App.screenWidth, 20 * App.screenHeightAdapter));


            objetivosEntry = new FormValueEditLongText("", Keyboard.Chat, 100);

            absoluteLayout.Add(objetivosEntry);
            absoluteLayout.SetLayoutBounds(objetivosEntry, new Rect(10 * App.screenWidthAdapter, 65 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenWidthAdapter), 100 * App.screenHeightAdapter));


            Label numeroAulasLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "NÚMERO AULAS SEMANAIS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize
            };
            absoluteLayout.Add(numeroAulasLabel);
            absoluteLayout.SetLayoutBounds(numeroAulasLabel, new Rect(10 * App.screenWidthAdapter, 180 * App.screenHeightAdapter, 170 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

            valoresLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.itemTitleFontSize
            };
            absoluteLayout.Add(valoresLabel);
            absoluteLayout.SetLayoutBounds(valoresLabel, new Rect(240 * App.screenWidthAdapter, 180 * App.screenHeightAdapter, App.screenWidth - (240 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));

            List<string> numeroAulasList = new List<string>();
            numeroAulasList.Add("1");
            numeroAulasList.Add("2");
            numeroAulasList.Add("3");
            numeroAulasList.Add("4");
            numeroAulasList.Add("5");


            numeroAulasPicker = new Picker
            {
                FontFamily = "futuracondensedmedium",
                Title = "",
                TitleColor = Colors.White,
                BackgroundColor = Colors.Transparent,
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize

            };
            numeroAulasPicker.ItemsSource = numeroAulasList;
            

            numeroAulasPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                int numeroAulas_Int = int.Parse(numeroAulasPicker.SelectedItem.ToString());
                Debug.Print("selectedCoach.valor_hora_minino = " + selectedCoach.valor_hora_minino);
                double valorMinimo = double.Parse(selectedCoach.valor_hora_minino, CultureInfo.InvariantCulture);
                double valorMaximo = double.Parse(selectedCoach.valor_hora_maximo, CultureInfo.InvariantCulture);

                //double valorMinimo = Convert.ToDouble(selectedCoach.valor_hora_minino);// double.Parse(selectedCoach.valor_hora_minino.Replace(".", ","));
                //double valorMaximo = Convert.ToDouble(selectedCoach.valor_hora_maximo); //double.Parse(selectedCoach.valor_hora_maximo.Replace(".", ","));
                Debug.Print("valorMaximo = " + valorMaximo);
                valoresLabel.Text = "Valor Mensal Estimado\n" + Convert.ToInt32(4 * numeroAulas_Int * valorMinimo).ToString("0.00") + "€ - " + Convert.ToInt32(4 * numeroAulas_Int * valorMaximo).ToString("0.00") + "€";
            };

            numeroAulasPicker.SelectedIndex = 0;

            absoluteLayout.Add(numeroAulasPicker);
            absoluteLayout.SetLayoutBounds(numeroAulasPicker, new Rect(185 * App.screenWidthAdapter, 180 * App.screenHeightAdapter, 30 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

            Label disponibilidadeLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "DISPONIBILIDADE",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize
            };
            absoluteLayout.Add(disponibilidadeLabel);
            absoluteLayout.SetLayoutBounds(disponibilidadeLabel, new Rect(10 * App.screenWidthAdapter, 240 * App.screenHeightAdapter,App.screenWidth, 40 * App.screenHeightAdapter));

            Label disponibilidadeExplicacaoLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Quando poderias fazer estas aulas:",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.itemTitleFontSize
            };
            absoluteLayout.Add(disponibilidadeExplicacaoLabel);
            absoluteLayout.SetLayoutBounds(disponibilidadeExplicacaoLabel, new Rect(10 * App.screenWidthAdapter, 280 * App.screenHeightAdapter, App.screenWidth, 20 * App.screenHeightAdapter));



            disponibilidadeEntry = new FormValueEditLongText("", Keyboard.Chat, 100);

            absoluteLayout.Add(disponibilidadeEntry);
            absoluteLayout.SetLayoutBounds(disponibilidadeEntry, new Rect(10 * App.screenWidthAdapter, 305 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenWidthAdapter), 100 * App.screenHeightAdapter));

            RegisterButton personalClassesButton = new RegisterButton("ENVIAR PEDIDO", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            //personalClassesButton.button.BackgroundColor = App.topColor;
            personalClassesButton.button.Clicked += OnPersonalClassesButtonClicked;

            absoluteLayout.Add(personalClassesButton);
            absoluteLayout.SetLayoutBounds(personalClassesButton, new Rect(0, App.screenHeight - 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

            hideActivityIndicator();
        }



		public PersonalConfirmPageCS(Member selectedCoach, string personalClass_type)
		{
            this.selectedCoach = selectedCoach;
            this.personalClass_type = personalClass_type;
            this.initLayout();
		}


        async void OnPersonalClassesButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("PersonalConfirmPageCS.OnPersonalClassesButtonClicked");
            showActivityIndicator();
            if (objetivosEntry.entry.Text == "")
            {
                await DisplayAlert("Indica por favor os Objetivos", "Para prosseguir é necessário indicar os objetivos que tem para estas aulas.", "OK");
                hideActivityIndicator();
                return;
            }
            if (disponibilidadeEntry.entry.Text == "")
            {
                await DisplayAlert("Indica por favor a Disponibilidade", "Para prosseguir é necessário indicar a sua disponibilidade para estas aulas.", "OK");
                hideActivityIndicator();
                return;
            }
            MemberManager memberManager = new MemberManager();
            await memberManager.Request_Personal(selectedCoach.nickname, selectedCoach.email, App.member.nickname, App.member.phone, this.personalClass_type, numeroAulasPicker.SelectedItem.ToString(), objetivosEntry.entry.Text , disponibilidadeEntry.entry.Text);


            string message = "Olá " + selectedCoach.nickname + "!\n" +
                "O meu nome é " + App.member.nickname + " e gostava de ter Aulas Pessoais contigo.\n" +
                "Gostava de fazer " + numeroAulasPicker.SelectedItem + " aula(s) por semana consigo de " + this.personalClass_type + " com o objetivo de:\n" + objetivosEntry.entry.Text + "\n" +
                "Tenho a seguinte disponibilidade:\n" + disponibilidadeEntry.entry.Text;

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH CONFIRMED", message);


            string result = await App.SendWhatsApp(selectedCoach.phone, message);
            if (result != "1")
            {
                try
                {
                    SmsMessage smsMessage = new SmsMessage(message, new[] { selectedCoach.phone });
                    await Sms.ComposeAsync(smsMessage);
                }
                catch (FeatureNotSupportedException ex)
                {
                    Debug.Print("FeatureNotSupportedException");
                }
                catch (Exception ex)
                {
                    Debug.Print("Other Exception");
                }
            }
            hideActivityIndicator();

            await DisplayAlert("OBRIGADO PELA CONFIANÇA", "Obrigado por quereres melhorar o teu Karate, com os melhores treinadores do MUNDO, os nossos!", "OK");
            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
            
        }
    }
}
