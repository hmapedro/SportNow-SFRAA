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
	public class CurrentServicesAppointmentPageCS : DefaultPage
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

        Service service;

        ObservableCollection<Appointment> appointments, pastAppointments, futureAppointments;

        Label pastAttendanceLabel, futureAttendanceLabel;

        private CollectionView pastAppointmentsCollectionView;
        private CollectionView futureAppointmentsCollectionView;

        ScheduleCollection scheduleCollection;

        Grid gridService, gridPresencasButtons;

        private OptionButton marcarAulaButton, estatisticasButton, presencasButton, mensalidadesButton, mensalidadesStudentButton;

        int service_students_count;

        RoundButton solicitarMarcacaoButton;

        public void CleanScreen()
		{
            if (gridService != null)
            {
                //absoluteLayout.Clear();

                absoluteLayout.Children.Remove(gridService);
            }
        }

        public void initLayout()
		{
			Title = service.nome.ToUpper();
		}


		public async void initSpecificLayout()
		{
            gridService = new Grid { BackgroundColor = Colors.Transparent, Padding = 0, RowSpacing = 10 * App.screenHeightAdapter, ColumnSpacing = 10 * App.screenWidthAdapter };
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.RowDefinitions.Add(new RowDefinition { Height = App.ItemHeight + 20 * App.screenHeightAdapter });
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridService.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

            ServicesManager servicesManager = new ServicesManager();
            appointments = await servicesManager.GetServiceAppointments(App.member.id, service.id);
            completeAppointments();

            
            _ = await createFutureAppointments();
            _ = await createPastAppointments();

            CreateSolicitarMarcacaoButton();

            gridService.Add(futureAttendanceLabel, 0, 0);
            gridService.Add(futureAppointmentsCollectionView, 0, 1);
            gridService.Add(pastAttendanceLabel, 0, 2);
            gridService.Add(pastAppointmentsCollectionView, 0, 3);

            absoluteLayout.Add(gridService);
            absoluteLayout.SetLayoutBounds(gridService, new Rect(0 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 - 60 * App.screenHeightAdapter));

        }

        public void completeAppointments()
        {

            futureAppointments = new ObservableCollection<Appointment>();
            pastAppointments = new ObservableCollection<Appointment>();

            foreach (Appointment appointment in appointments)
            {

                DateTime appointment_date = DateTime.Parse(appointment.date_start).Date;
                DateTime dateNow = DateTime.Now;
                Debug.Print("appointment_date = " + appointment_date.ToString() + " dateNow = " + dateNow.ToString());

                appointment.imagem = service.imagem;
                appointment.imagemSource = service.imagemSource;
                appointment.servicename = service.nome;

                Debug.Print("imagem = " + appointment.imagem + " imagemSource = " + appointment.imagemSource);

                if ((appointment.estado == "pedida") | (appointment.estado == "cancelada"))
                {
                    appointment.participationimage = "iconinativo.png";
                }
                else 
                {
                    appointment.participationimage = "iconcheck.png";
                }

                if (appointment.estado == "pedida")
                {
                    appointment.date_string = "A definir";
                }
                else
                {
                    appointment.date_string = appointment.date_start + " às " + appointment.date_end;
                }
                

                if (DateTime.Compare(appointment_date, dateNow) < 0)
                {
                    Debug.Print("É passado");
                    pastAppointments.Add(appointment);

                }
                else
                {
                    Debug.Print("É futuro");
                    futureAppointments.Add(appointment);
                }
            }
        }


        public async Task<int> createFutureAppointments()
        {
            futureAttendanceLabel = new Label
            {
                Text = "MARCAÇÕES AGENDADAS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };
            createFutureAppointmentsCollection();
            int result = 1;
            return result;
        }

        public async Task<int> createPastAppointments()
        {
            pastAttendanceLabel = new Label
            {
                Text = "MARCAÇÕES JÁ EFETUADAS",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            createPastAppointmentsCollection();

            int result = 1;
            return result;
        }

        public void createFutureAppointmentsCollection()
        {
            futureAppointmentsCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = futureAppointments,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem marcações futuras.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium",FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            futureAppointmentsCollectionView.SelectionChanged += OnFutureAppointmentsCollectionViewSelectionChanged;

            futureAppointmentsCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                };

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundColor,
                    Padding = new Thickness(0, 0, 0, 0),
                    HeightRequest = App.ItemHeight,// -(10 * App.screenHeightAdapter),
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image
                {
                    Aspect = Aspect.AspectFill,
                    Opacity = 0.40,
                };
                eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "date_string");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));


                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

                return itemabsoluteLayout;
            });
        }



        public void createPastAppointmentsCollection()
        {
            pastAppointmentsCollectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = pastAppointments,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem marcações passadas.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium",FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            pastAppointmentsCollectionView.SelectionChanged += OnPastAppointmentsCollectionViewSelectionChanged;

            pastAppointmentsCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                };

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundColor,
                    Padding = new Thickness(0, 0, 0, 0),
                    HeightRequest = App.ItemHeight,// -(10 * App.screenHeightAdapter),
                    VerticalOptions = LayoutOptions.Center,
                };

                Image eventoImage = new Image
                {
                    Aspect = Aspect.AspectFill,
                    Opacity = 0.40,
                };
                eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

                itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.inactiveTitleTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "date_sting");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));


                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

                return itemabsoluteLayout;
            });
        }

        public void CreateSolicitarMarcacaoButton()
        {

            solicitarMarcacaoButton = new RoundButton("SOLICITAR MARCAÇÃO", App.screenWidth - 20 * App.screenWidthAdapter, 50);
            solicitarMarcacaoButton.button.Clicked += OnSolicitarMarcacaoButtonClicked;

            absoluteLayout.Add(solicitarMarcacaoButton);
            absoluteLayout.SetLayoutBounds(solicitarMarcacaoButton, new Rect(0, App.screenHeight - 100 - 60 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
        }


        public CurrentServicesAppointmentPageCS(Service service)
		{
            this.service = service;
			this.initLayout();
			this.initSpecificLayout();

		}

        async void OnSolicitarMarcacaoButtonClicked(object sender, EventArgs e)
        {
            showActivityIndicator();

            Debug.WriteLine("OnSolicitarMarcacaoButtonClicked");

            string input = await DisplayPromptAsync("Disponibilidade", "Indique-nos a sua disponibilidade para esta marcação.", "Enviar", "Cancelar", "", keyboard: Keyboard.Text);

            if (input != null)
            {
                Debug.Print("A disponibilidade é " + input);

                ServicesManager servicesManager = new ServicesManager();

                string res = await servicesManager.createServiceAppointment(App.member.id, this.service.id, this.service.nome, "pedida", input, App.member.nickname, App.member.phone, App.member.email);

                Debug.Print("App.member.gender = " + App.member.gender);

                if (App.member.gender == "male")
                {
                    await DisplayAlert("Pedido Marcação enviado", "O seu pedido de Marcação foi enviado para os serviços da SFRAA. Será contactado brevemente para confirmar a sua Marcação.", "Ok");
                }
                else
                {
                    await DisplayAlert("Pedido Marcação enviado", "O seu pedido de Marcação foi enviado para os serviços da SFRAA. Será contactada brevemente para confirmar a sua Marcação", "Sim");
                }
                futureAppointments = await servicesManager.GetServiceAppointments(App.member.id, service.id);
                completeAppointments();
                /*futureAppointmentsCollectionView.ItemsSource = null;
                futureAppointmentsCollectionView.ItemsSource = futureAppointments;*/
            }

            hideActivityIndicator();

        }

        async void OnFutureAppointmentsCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("CurrentServicesAppointmentPageCS.OnFutureAppointmentsCollectionViewSelectionChanged");

            if ((sender as CollectionView).SelectedItem != null)
            {
                Appointment appointment_i = (sender as CollectionView).SelectedItem as Appointment;
                (sender as CollectionView).SelectedItem = null;
                await Navigation.PushAsync(new CurrentServicesAppointmentDetailPageCS(appointment_i));
            }
        }

        async void OnPastAppointmentsCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("CurrentServicesAppointmentPageCS.OnPastAppointmentsCollectionViewSelectionChanged");

            if ((sender as CollectionView).SelectedItem != null)
            {
                Appointment appointment = (sender as CollectionView).SelectedItem as Appointment;
                (sender as CollectionView).SelectedItem = null;
                await Navigation.PushAsync(new CurrentServicesAppointmentDetailPageCS(appointment));
            }
        }

    }
}
