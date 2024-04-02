using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;


//Ausing Acr.UserDialogs;

namespace SportNow.Views.Profile
{
    public class AttendanteEvaluationPageCS : DefaultPage
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

        Class_Attendance class_Attendance;
        string presencaid;
        string evaluationname;

        public void initLayout()
        {
            Title = "AVALIAÇÃO AULA";
        }


        public void CleanScreen()
        {
            Debug.Print("CleanScreen");
        }

        public async void initSpecificLayout()
        {
            showActivityIndicator();

            ClassManager classManager = new ClassManager();
            class_Attendance = await classManager.GetClass_Attendances_byID(presencaid);
            //class_Attendance = await classManager.GetClass_Attendances_byID("eb945229-2662-4b6c-0ea6-6434875cf50f");

            evaluationname = "Avaliação Aula - " + App.member.nickname + " - " + class_Attendance.classname + " - " + class_Attendance.date;
            Debug.Print("evaluationname = " + evaluationname);
            //LogManager logManager = new LogManager();
            //await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL COACH CONFIRM", "Visit Personal Coach Confirm Page");
            string textWelcome = "Olá " + App.member.nickname;

            //USERNAME LABEL
            Label usernameLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = textWelcome,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 18 * App.screenWidthAdapter
            };
            absoluteLayout.Add(usernameLabel);
            absoluteLayout.SetLayoutBounds(usernameLabel, new Rect(App.screenWidth - 300 * App.screenWidthAdapter, 2 * App.screenHeightAdapter, 300 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

            Label objetivosLabel = new Label
            {
                Text = "AVALIAÇÃO AULA\n"+ class_Attendance.classname+"\n"+class_Attendance.date,
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize
            };
            absoluteLayout.Add(objetivosLabel);
            absoluteLayout.SetLayoutBounds(objetivosLabel, new Rect(10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter, App.screenWidth, 120 * App.screenHeightAdapter ));

            Label objetivosExplicacaoLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Diz-nos como avalias a aula "+"...",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize,
                LineBreakMode = LineBreakMode.WordWrap 
            };
            /*absoluteLayout.Add(objetivosExplicacaoLabel);
            absoluteLayout.SetLayoutBounds(, new Rect(, , , ));)1070 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width); // center of image (which is 40 wide)
                }),
                heightConstraint: )100 * App.screenHeightAdapter));
            */

            Image negativeImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "iconinsatisfeito.png"
            };

            TapGestureRecognizer negativeImage_tapEvent = new TapGestureRecognizer();
            negativeImage_tapEvent.Tapped += OnnegativeImageClicked;
            negativeImage.GestureRecognizers.Add(negativeImage_tapEvent);

            absoluteLayout.Add(negativeImage);
            absoluteLayout.SetLayoutBounds(negativeImage, new Rect(10 * App.screenHeightAdapter, (App.screenHeight / 2) - 40 * App.screenHeightAdapter, 80 * App.screenHeightAdapter, 80 * App.screenHeightAdapter));

            Image neutralImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "iconmedio.png"
            };

            TapGestureRecognizer neutralImage_tapEvent = new TapGestureRecognizer();
            neutralImage_tapEvent.Tapped += OnneutralImageClicked;
            neutralImage.GestureRecognizers.Add(neutralImage_tapEvent);

            absoluteLayout.Add(neutralImage);
            absoluteLayout.SetLayoutBounds(neutralImage, new Rect((App.screenWidth / 2) - 40 * App.screenHeightAdapter, (App.screenHeight / 2) - 40 * App.screenHeightAdapter, 80 * App.screenHeightAdapter, 80 * App.screenHeightAdapter));

            Image positiveImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "iconsatisfeito.png"
            };

            TapGestureRecognizer positiveImage_tapEvent = new TapGestureRecognizer();
            positiveImage_tapEvent.Tapped += OnpositiveImageClicked;
            positiveImage.GestureRecognizers.Add(positiveImage_tapEvent);

            absoluteLayout.Add(positiveImage);
            absoluteLayout.SetLayoutBounds(positiveImage, new Rect((App.screenWidth) - 90 * App.screenHeightAdapter, (App.screenHeight / 2) - 40 * App.screenHeightAdapter, 80 * App.screenHeightAdapter, 80 * App.screenHeightAdapter));


            hideActivityIndicator();
        }



        public AttendanteEvaluationPageCS(string presencaid)
        {
            this.presencaid = presencaid;
            this.initLayout();
        }

        async void OnnegativeImageClicked(object sender, EventArgs e)
        {
            showActivityIndicator();

            
            var result = await DisplayPromptAsync("OBRIGADO PELO FEEDBACK", "Se quiseres partilhar connosco o que correu menos bem podes faze-lo agora.", "ENVIAR", "NÃO ENVIAR", "Comentários", -1, Keyboard.Chat);

            ClassManager classManager = new ClassManager();
            string res = await classManager.CreateClass_Evaluation(evaluationname, class_Attendance.classattendanceid, "insatisfeito", result);

            
            //await DisplayAlert("OBRIGADO PELO FEEDBACK", "Obrigado por partilhares connosco a tua avaliação deste treino.", "OK");
            hideActivityIndicator();

            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }

        async void OnneutralImageClicked(object sender, EventArgs e)
        {
            showActivityIndicator();
            ClassManager classManager = new ClassManager();
            string res = await classManager.CreateClass_Evaluation(evaluationname, class_Attendance.classattendanceid, "neutro", "");
            await DisplayAlert("OBRIGADO PELO FEEDBACK", "Obrigado por partilhares connosco a tua avaliação deste treino.", "OK");
            hideActivityIndicator();

            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }

        async void OnpositiveImageClicked(object sender, EventArgs e)
        {
            showActivityIndicator();
            ClassManager classManager = new ClassManager();
            string res = await classManager.CreateClass_Evaluation(evaluationname, class_Attendance.classattendanceid, "satisfeito", "");
            await DisplayAlert("OBRIGADO PELO FEEDBACK", "Obrigado por partilhares connosco a tua avaliação deste treino.", "OK");
            hideActivityIndicator();

            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }

    }
}
