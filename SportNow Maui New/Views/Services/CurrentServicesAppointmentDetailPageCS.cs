using SportNow.Model;
using System.Diagnostics;
using Microsoft.Maui.Controls.Shapes;
using SportNow.Services.Data.JSON;
using System.Collections.ObjectModel;
using SportNow.ViewModel;
using SportNow.CustomViews;
//using static System.Net.Mime.MediaTypeNames;
//using Microsoft.Maui.Controls;

namespace SportNow.Views.Services
{
	public class CurrentServicesAppointmentDetailPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
            
            showActivityIndicator();

			//initSpecificLayout();

			hideActivityIndicator();
		}

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

        Appointment appointment;

        Grid gridAppointment;

        public void CleanScreen()
		{
            if (gridAppointment != null)
            {
                //absoluteLayout.Clear();

                absoluteLayout.Children.Remove(gridAppointment);
            }
        }

        public void initLayout()
		{
            Title = "MARCAÇÃO";
		}


		public async void initSpecificLayout()
		{
            gridAppointment = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter, ColumnSpacing = 10 * App.screenWidthAdapter };
            gridAppointment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridAppointment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridAppointment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridAppointment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridAppointment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridAppointment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            Image appointmentImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.20 };

            Debug.Print("appointment.imagemSource.GetType().ToString() = " + appointment.imagemSource.GetType().ToString());

            if (appointment.imagemSource.GetType().ToString() == "Microsoft.Maui.Controls.UriImageSource")
            {
                appointmentImage.Source = (ImageSource)appointment.imagemSource;
            }
            else
            {
                appointmentImage.Source = (string) appointment.imagemSource;
            }

            

            absoluteLayout.Add(appointmentImage);
            absoluteLayout.SetLayoutBounds(appointmentImage, new Rect(0, 0, App.screenWidth, App.screenHeight));

            Label nameLabel = new FormLabel { Text = "MARCAÇÃO" };
            nameLabel.TextColor = App.topColor;
            FormValue nameValue = new FormValue(appointment.name);

            Label serviceLabel = new FormLabel { Text = "SERVIÇO" };
            serviceLabel.TextColor = App.topColor;
            FormValue serviceValue = new FormValue(appointment.servicename);

            Label dateLabel = new FormLabel { Text = "DATA" };
            dateLabel.TextColor = App.topColor;
            FormValue dateValue = new FormValue(appointment.date_string);

            Label estadoLabel = new FormLabel { Text = "ESTADO" };
            estadoLabel.TextColor = App.topColor;
            FormValue estadoValue = new FormValue(Constants.ToTitleCase(appointment.estado));


            gridAppointment.Add(nameLabel, 0, 0);
            gridAppointment.Add(nameValue, 1, 0);
            gridAppointment.Add(serviceLabel, 0, 1);
            gridAppointment.Add(serviceValue, 1, 1);
            gridAppointment.Add(dateLabel, 0, 2);
            gridAppointment.Add(dateValue, 1, 2);
            gridAppointment.Add(estadoLabel, 0, 3);
            gridAppointment.Add(estadoValue, 1, 3);

            absoluteLayout.Add(gridAppointment);
            absoluteLayout.SetLayoutBounds(gridAppointment, new Rect(0 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100));

        }

        public CurrentServicesAppointmentDetailPageCS(Appointment appointment)
		{
            this.appointment = appointment;
            this.initLayout();
			this.initSpecificLayout();

		}
    }
}
