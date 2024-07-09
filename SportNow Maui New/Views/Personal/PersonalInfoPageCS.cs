using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views.Personal
{
	public class PersonalInfoPageCS : DefaultPage
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

		public void initLayout()
		{
			Title = "TREINOS PESSOAIS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "PERSONAL INFO", "Visit Personal Info Page");

            Label titleLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "AGORA JÁ PODES MARCAR AULAS PESSOAIS COM O TEU TREINADOR FAVORITO",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize
            };
            absoluteLayout.Add(titleLabel);
            absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, 90 * App.screenHeightAdapter));

            Label descricaoLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Caso queiras treinar mais horas ou ter aulas dedicadas especificamente às suas necessidades.",
                TextColor = App.bottomColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize,
                LineBreakMode = LineBreakMode.WordWrap 

            };

            absoluteLayout.Add(descricaoLabel);
            absoluteLayout.SetLayoutBounds(descricaoLabel, new Rect(0 , 100 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

            Label tipoLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Escolhe um dos seguintes tipos de aulas:",
                TextColor = App.bottomColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize
            };

            absoluteLayout.Add(tipoLabel);
            absoluteLayout.SetLayoutBounds(tipoLabel, new Rect(0, 160 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

            ServiceBox exameServiceBox = new ServiceBox("PREPARAÇÃO EXAME", 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter);
            absoluteLayout.Add(exameServiceBox);
            absoluteLayout.SetLayoutBounds(exameServiceBox, new Rect(10 * App.screenWidthAdapter, 230 * App.screenHeightAdapter, 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter));

            var exameServiceBox_tap = new TapGestureRecognizer();
            exameServiceBox_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new PersonalCoachPageCS("Preparação para Exame"));
            };
            exameServiceBox.GestureRecognizers.Add(exameServiceBox_tap);

            ServiceBox tecnicoServiceBox = new ServiceBox("TÉCNICO OU FÍSICO", 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter);
            absoluteLayout.Add(tecnicoServiceBox);
            absoluteLayout.SetLayoutBounds(tecnicoServiceBox, new Rect(App.screenWidth - 160 * App.screenWidthAdapter, 230 * App.screenHeightAdapter, 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter));

            var tecnicoServiceBox_tap = new TapGestureRecognizer();
            tecnicoServiceBox_tap.Tapped += (s, e) =>
            {

                Navigation.PushAsync(new PersonalCoachPageCS("Técnico e Físico"));
            };
            tecnicoServiceBox.GestureRecognizers.Add(tecnicoServiceBox_tap);

            ServiceBox kataServiceBox = new ServiceBox("COMPETIÇAO KATA", 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter);
            absoluteLayout.Add(kataServiceBox);
            absoluteLayout.SetLayoutBounds(kataServiceBox, new Rect(10 * App.screenWidthAdapter, 340 * App.screenHeightAdapter, 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter));

            var kataServiceBox_tap = new TapGestureRecognizer();
            kataServiceBox_tap.Tapped += (s, e) =>
            {

                Navigation.PushAsync(new PersonalCoachPageCS("Competição Kata"));
            };
            kataServiceBox.GestureRecognizers.Add(kataServiceBox_tap);


            ServiceBox kumiteServiceBox = new ServiceBox("COMPETIÇAO KUMITE", 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter);
            absoluteLayout.Add(kumiteServiceBox);
            absoluteLayout.SetLayoutBounds(kumiteServiceBox, new Rect(App.screenWidth - 160 * App.screenWidthAdapter, 340 * App.screenHeightAdapter, 150 * App.screenWidthAdapter, 100 * App.screenHeightAdapter));



            var kumiteServiceBox_tap = new TapGestureRecognizer();
            kumiteServiceBox_tap.Tapped += (s, e) =>
            {

                Navigation.PushAsync(new PersonalCoachPageCS("Competição Kumite"));
            };
            kumiteServiceBox.GestureRecognizers.Add(kumiteServiceBox_tap);

            Label textLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Escolhe o treinador que pretendes, o tipo de aula e o horário mais conveniente para as suas aulas pessoais.",
                TextColor = App.bottomColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize,
                LineBreakMode = LineBreakMode.WordWrap
            };

            absoluteLayout.Add(textLabel);
            absoluteLayout.SetLayoutBounds(textLabel, new Rect(0, 460 * App.screenHeightAdapter, App.screenWidth, 60 * App.screenHeightAdapter));


            hideActivityIndicator();
        }

		public PersonalInfoPageCS()
		{
			this.initLayout();
		}

        async void OnPersonalClassesButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MainPageCS.OnPersonalClassesButtonClicked");
            //await Navigation.PushAsync(new PersonalCoachPageCS());
        }

    }
}
