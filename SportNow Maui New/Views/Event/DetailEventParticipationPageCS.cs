using SportNow.Model;
using SportNow.CustomViews;


namespace SportNow.Views
{
	public class DetailEventParticipationPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			gridEvent = null;
			//App.competition_participation = competition_participation;

		}


		FormValue estadoValue = new FormValue("");

		private Event_Participation event_participation;

		private List<Payment> payments;

		Button registerButton = new Button();

		private Microsoft.Maui.Controls.Grid gridEvent;

		public void initLayout()
		{
			Title = event_participation.evento_name;
		}


		public async void initSpecificLayout()
		{
            this.BackgroundColor = App.backgroundOppositeColor;

            gridEvent = new Microsoft.Maui.Controls.Grid { Padding = 0, ColumnSpacing = 2 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 }); //GridLength.Auto
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth / 5 * 4 }); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
            dateLabel.TextColor = App.oppositeTextColor;
            FormValue dateValue = new FormValue(event_participation.evento_detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
            placeLabel.TextColor = App.oppositeTextColor;
            FormValue placeValue = new FormValue(event_participation.evento_local);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
            typeLabel.TextColor = App.oppositeTextColor;
            FormValue typeValue = new FormValue(Constants.event_type[event_participation.evento_tipo]);

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
            websiteLabel.TextColor = App.oppositeTextColor;
            FormValue websiteValue = new FormValue(event_participation.evento_website);


			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(event_participation.evento_website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});


			Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 };
			eventoImage.Source = event_participation.imagemSource;

			absoluteLayout.Add(eventoImage);
            absoluteLayout.SetLayoutBounds(eventoImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

			gridEvent.Add(dateLabel, 0, 0);
			gridEvent.Add(dateValue, 1, 0);

			gridEvent.Add(placeLabel, 0, 1);
			gridEvent.Add(placeValue, 1, 1);

			gridEvent.Add(typeLabel, 0, 2);
			gridEvent.Add(typeValue, 1, 2);

			gridEvent.Add(websiteLabel, 0, 3);
			gridEvent.Add(websiteValue, 1, 3);

			absoluteLayout.Add(gridEvent);
            absoluteLayout.SetLayoutBounds(gridEvent, new Rect(0, 0, App.screenWidth - 10 * App.screenWidthAdapter, App.screenHeight));
		}



		public DetailEventParticipationPageCS(Event_Participation event_participation)
		{
			this.event_participation = event_participation;
			//App.event_participation = event_participation;

			this.initLayout();
			//this.initSpecificLayout();
		}

	}
}

