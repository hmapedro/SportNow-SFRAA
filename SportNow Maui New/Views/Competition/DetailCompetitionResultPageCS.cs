using SportNow.Model;
using SportNow.CustomViews;

using System.Diagnostics;
//using static Android.OS.VibrationEffect;

namespace SportNow.Views
{
	public class DetailCompetitionResultPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			gridCompetiton = null;
			//App.competition_participation = competition_participation;

		}


		FormValue estadoValue = new FormValue("");

		private Competition_Participation competition_participation;

		private List<Payment> payments;

		Button registerButton = new Button();

		private Microsoft.Maui.Controls.Grid gridCompetiton;

		public void initLayout()
		{
			Title = competition_participation.competicao_name;
		}


		public async void initSpecificLayout()
		{

            this.BackgroundColor = App.backgroundOppositeColor;
            Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
            eventoImage.Source = competition_participation.imagemSource;

            absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

            gridCompetiton = new Microsoft.Maui.Controls.Grid { Padding = 0, ColumnSpacing = 2 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 }); //GridLength.Auto
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 * 4 }); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
            dateLabel.TextColor = App.oppositeTextColor;
            FormValue dateValue = new FormValue(competition_participation.competicao_detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
            placeLabel.TextColor = App.oppositeTextColor;
            FormValue placeValue = new FormValue(competition_participation.competicao_local);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
            typeLabel.TextColor = App.oppositeTextColor;
            FormValue typeValue = new FormValue(Constants.competition_type[competition_participation.competicao_tipo]);

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
            websiteLabel.TextColor = App.oppositeTextColor;
            FormValue websiteValue = new FormValue(competition_participation.competicao_website);


			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(competition_participation.competicao_website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						Debug.Print(ex.Message);
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});

			FormLabel provaLabel = new FormLabel { Text = "PROVA" };
            provaLabel.TextColor = App.oppositeTextColor;
            FormValue provaValue = new FormValue(competition_participation.categoria);

			FormLabel classificacaoLabel = new FormLabel { Text = "RESULTADO" };
            classificacaoLabel.TextColor = App.oppositeTextColor;
            FormValue classificacaoValue = new FormValue(competition_participation.classificacao);
			classificacaoValue.Padding = new Thickness(1, 1, 1, 1);
			//classificacaoValue.BackgroundColor = competition_participation.classificacaoColor;
			classificacaoValue.label.BackgroundColor = competition_participation.classificacaoColor;

			gridCompetiton.Add(dateLabel, 0, 0);
			gridCompetiton.Add(dateValue, 1, 0);

			gridCompetiton.Add(placeLabel, 0, 1);
			gridCompetiton.Add(placeValue, 1, 1);

			gridCompetiton.Add(typeLabel, 0, 2);
			gridCompetiton.Add(typeValue, 1, 2);

			gridCompetiton.Add(websiteLabel, 0, 3);
			gridCompetiton.Add(websiteValue, 1, 3);

			gridCompetiton.Add(provaLabel, 0, 4);
			gridCompetiton.Add(provaValue, 1, 4);

			gridCompetiton.Add(classificacaoLabel, 0, 5);
			gridCompetiton.Add(classificacaoValue, 1, 5);

			absoluteLayout.Add(gridCompetiton);
            absoluteLayout.SetLayoutBounds(gridCompetiton, new Rect(0, 0, App.screenWidth - 10 * App.screenWidthAdapter, App.screenHeight));
		}



		public DetailCompetitionResultPageCS(Competition_Participation competition_participation)
		{
			this.competition_participation = competition_participation;
			App.competition_participation = competition_participation;

			this.initLayout();
			//this.initSpecificLayout();
		}


		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{

			/*registerButton.IsEnabled = false;

			if (competition_participation != null)
			{
				Debug.WriteLine("OnRegisterButtonClicked competition_participation.name " + competition_participation.name);

                await Navigation.PushAsync(new CompetitionMBPageCS(competition_participation));

			}*/

		}




	}
}

