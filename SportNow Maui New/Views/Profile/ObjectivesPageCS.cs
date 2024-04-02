using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views.Profile
{
    public class ObjectivesPageCS : DefaultPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AdaptScreen();
            initSpecificLayout();
        }

        protected override void OnDisappearing()
        {
            this.CleanScreen();
        }

        FormValueEditLongText objetivosEntry, disponibilidadeEntry;

        public void initLayout()
        {
            Title = "EXPECTATIVAS ÉPOCA";
        }


        public void CleanScreen()
        {
            Debug.Print("CleanScreen");
        }

        public async void initSpecificLayout()
        {
            showActivityIndicator();

            //LogManager logManager = new LogManager();
            //await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH CONFIRM", "Visit Personal Coach Confirm Page");
            string textWelcome = "Olá " + App.member.nickname;

            //USERNAME LABEL
            Label usernameLabel = new Label
            {
                Text = textWelcome,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 18 * App.screenWidthAdapter,
                FontFamily = "futuracondensedmedium",
            };
            absoluteLayout.Add(usernameLabel);
            absoluteLayout.SetLayoutBounds(usernameLabel, new Rect((App.screenWidth) - (320 * App.screenWidthAdapter), 2 * App.screenHeightAdapter, 300 * App.screenHeightAdapter, 30 * App.screenHeightAdapter));



            Label objetivosLabel = new Label
            {
                Text = "EXPECTATIVAS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };
            absoluteLayout.Add(objetivosLabel);
            absoluteLayout.SetLayoutBounds(objetivosLabel, new Rect(10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

            Label objetivosExplicacaoLabel = new Label
            {
                Text = "Agora que vamos começar a época gostávamos de obter informação relativamente às expectativas que tens para este ano. " +
                "Diz-nos o que te levou a inscrever no Karate este ano, o que gostavas de alcançar ou quais as experiências que gostavas de ter." +
                "\nNa Ippon queremos que todos se sintam especiais, e só assim o conseguimos!",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.formLabelFontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                FontFamily = "futuracondensedmedium",
            };
            absoluteLayout.Add(objetivosExplicacaoLabel);
            absoluteLayout.SetLayoutBounds(objetivosExplicacaoLabel, new Rect(10 * App.screenWidthAdapter, 70 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 130 * App.screenHeightAdapter));

            if ((App.member.objectives != null) & (App.member.objectives.Count > 0))
            {
                objetivosEntry = new FormValueEditLongText(App.member.objectives[0].objectivos, Keyboard.Chat, Convert.ToInt16(400 * App.screenHeightAdapter));
            }
            else
            {
                objetivosEntry = new FormValueEditLongText("", Keyboard.Chat, Convert.ToInt16(400 * App.screenHeightAdapter));
            }

            absoluteLayout.Add(objetivosEntry);
            absoluteLayout.SetLayoutBounds(objetivosEntry, new Rect(10 * App.screenWidthAdapter, 200 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenHeightAdapter), 400 * App.screenHeightAdapter));
           
            RegisterButton confirmButton = new RegisterButton("CONFIRMAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            //personalClassesButton.button.BackgroundColor = App.topColor;
            confirmButton.button.Clicked += OnConfirmButtonClicked;

            absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenWidthAdapter, App.screenHeight - (160 * App.screenHeightAdapter), App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

            hideActivityIndicator();
        }



        public ObjectivesPageCS()
        {
            this.initLayout();
        }


        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("ObjectivesPageCS.OnConfirmButtonClicked");

            if (objetivosEntry.entry.Text != "")
            {
                showActivityIndicator();
                MemberManager memberManager = new MemberManager();
                await memberManager.CreateObjective(App.member.id, "Objetivos - " + App.member.nickname + " - " + App.getSeasonString(), App.getSeason(), objetivosEntry.entry.Text);
                hideActivityIndicator();

                await DisplayAlert("OBRIGADO", "Obrigado por partilhares connosco as tuas expectativas para esta época.", "OK");
                App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };
            }
            else
            {
                bool res = await DisplayAlert("Informação em falta", "Tens a certeza que não queres dizer-nos as tuas expectativas para esta época?", "Agora Não", "Quero");
                if (res == true)
                {
                    showActivityIndicator();
                    MemberManager memberManager = new MemberManager();
                    await memberManager.CreateObjective(App.member.id, "Objetivos - " + App.member.nickname + " - " + App.getSeasonString(), App.getSeason(), "");
                    hideActivityIndicator();
                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                    };
                }
            }



        }

        
    }
}
