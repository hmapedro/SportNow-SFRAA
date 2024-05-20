using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Text.RegularExpressions;

using SkiaSharp;
using System.Net;

namespace SportNow.Views.CompleteRegistration
{
    public class NewMemberPageCS : DefaultPage
    {

        protected async override void OnAppearing()
        {
        }


        protected async override void OnDisappearing()
        {
            //await UpdateMember();
        }

        Stream stream_aux; 
        private ScrollView scrollView;

        MenuButton geralButton;
        MenuButton identificacaoButton;
        MenuButton moradaButton;
        MenuButton contactosButton;


        StackLayout stackButtons;
        private Grid gridGeral;
        private Grid gridIdentificacao;
        private Grid gridMorada;
        private Grid gridContactos;

        FormValueEdit nameValue;
        FormValueEdit emailValue;
        FormValueEditPicker dojoValue;
        FormValueEditPicker genderValue;
        FormValueEditPicker tipoIDDocValue;
        //FormValueEditPicker countryValue;
        FormValueEditPicker memberTypeValue;
        FormValueEditDate birthdateValue;
        FormValueEdit phoneValue;
        FormValueEdit addressValue;
        FormValueEdit cityValue;
        FormValueEdit postalcodeValue;
        FormValueEdit EncEducacao1NomeValue;
        FormValueEdit EncEducacao1PhoneValue;
        FormValueEdit EncEducacao1MailValue;
        FormValueEdit EncEducacao2NomeValue;
        FormValueEdit EncEducacao2PhoneValue;
        FormValueEdit EncEducacao2MailValue;
        FormValueEdit cc_numberValue, nifValue, fnkpValue;
        FormValueEdit jobValue;

        RoundImage memberPhotoImage;
        Stream stream;

        RoundButton confirmButton;

        List<Dojo> listAllDojos;
        List<string> dojoList;

        bool imageloaded;

        public void initLayout()
        {
            Title = "NOVO REGISTO";
            Content = absoluteLayout;
        }


        public async void initSpecificLayout()
        {
            scrollView = new ScrollView { Orientation = ScrollOrientation.Vertical };

            absoluteLayout.Add(scrollView);
            absoluteLayout.SetLayoutBounds(scrollView, new Rect(10 * App.screenWidthAdapter, 260 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 320 * App.screenHeightAdapter));

            listAllDojos = await GetAllDojos();

            dojoList = new List<string>();
            dojoList.Add("-");
            foreach (Dojo entry in listAllDojos)
            {
                dojoList.Add(entry.name);
                Debug.Print("Dojo Name = " + entry.name);
            }

            CreatePhoto();
            //CreateQuota();
            //CreateGraduacao();
            CreateStackButtons();
            CreateGridGeral();
            CreateGridMorada();
            CreateGridContactos();
            CreateConfirmButton();

            /*gridIdentificacao.IsVisible = false;
			gridMorada.IsVisible = false;
			gridEncEducacao.IsVisible = false;*/
            OnGeralButtonClicked(null, null);


        }

        public void CreatePhoto()
        {
            imageloaded = false;

            memberPhotoImage = new RoundImage();

            WebResponse response;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Constants.images_URL + App.member.id + "_photo");
            Debug.Print(Constants.images_URL + App.member.id + "_photo");
            request.Method = "HEAD";
            bool exists;
            try
            {
                response = request.GetResponse();
                Debug.Print("response.Headers.GetType()= " + response.Headers.GetType());
                exists = true;
            }
            catch (Exception ex)
            {
                exists = false;
            }

            Debug.Print("Photo exists? = " + exists);

            if (exists)
            {
                imageloaded = true;
                memberPhotoImage.Source = new UriImageSource
                {
                    Uri = new Uri(Constants.images_URL + App.member.id + "_photo"),
                    CachingEnabled = false,
                    CacheValidity = new TimeSpan(0, 0, 0, 1)
                };
            }
            else
            {
                memberPhotoImage.Source = "iconadicionarfoto.png";
            }

            var memberPhotoImage_tap = new TapGestureRecognizer();
            memberPhotoImage_tap.Tapped += memberPhotoImageTappedAsync;
            memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);


            absoluteLayout.Add(memberPhotoImage);
            absoluteLayout.SetLayoutBounds(memberPhotoImage, new Rect((App.screenWidth / 2) - (90 * App.screenHeightAdapter), 0, 180 * App.screenHeightAdapter, 180 * App.screenHeightAdapter));

        }

        public void CreateStackButtons()
        {
            var buttonWidth = (App.screenWidth - 10 * App.screenWidthAdapter) / 3;

            geralButton = new MenuButton("GERAL", buttonWidth, 60);
            geralButton.button.Clicked += OnGeralButtonClicked;
            moradaButton = new MenuButton("MORADA", buttonWidth, 60);
            moradaButton.button.Clicked += OnMoradaButtonClicked;
            contactosButton = new MenuButton("CONTACTOS", buttonWidth, 60);
            contactosButton.button.Clicked += OnContactosButtonClicked;

            stackButtons = new StackLayout
            {
                Spacing = 5,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    geralButton,
                    moradaButton,
                    contactosButton
                }
            };

            absoluteLayout.Add(stackButtons);
            absoluteLayout.SetLayoutBounds(stackButtons, new Rect(0, 185 * App.screenHeightAdapter, App.screenWidth, 60 * App.screenHeightAdapter));

            geralButton.activate();
            moradaButton.deactivate();
            contactosButton.deactivate();
        }

        public void CreateGridGeral()
        {

            gridGeral = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand, RowSpacing = 5 * App.screenHeightAdapter};
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
            gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            FormLabel nameLabel = new FormLabel { Text = "NOME *", HorizontalTextAlignment = TextAlignment.Start };
            nameValue = new FormValueEdit(App.member.name);

            string genderString = "";
            List<string> gendersList = new List<string>();
            foreach (KeyValuePair<string, string> entry in Constants.genders)
            {
                gendersList.Add(entry.Value);
                if (App.member.gender == entry.Key)
                {
                    genderString = entry.Value;
                }
            }
            FormLabel genderLabel = new FormLabel { Text = "GÉNERO *" };
            genderValue = new FormValueEditPicker(genderString, gendersList);

            string memberTypeString = "";
            List<string> memberTypeList = new List<string>();
            foreach (KeyValuePair<string, string> entry in Constants.memberTypes)
            {
                memberTypeList.Add(entry.Value);
                if (App.member.member_type == entry.Key)
                {
                    memberTypeString = entry.Value;
                }
            }

            FormLabel birthdateLabel = new FormLabel { Text = "NASCIMENTO *" };
            birthdateValue = new FormValueEditDate(App.member.birthdate);

            FormLabel cc_numberLabel = new FormLabel { Text = "N. ID *" };

            string tipoIDDocString = "";
            List<string> tipoIDDocStringList = new List<string>();
            foreach (KeyValuePair<string, string> entry in Constants.tipoDocumentoIdentificao)
            {
                tipoIDDocStringList.Add(entry.Value);
                /*                if (App.member.gender == entry.Key)
                                {
                                    tipoIDDocString = entry.Value;
                                }*/
            }

            tipoIDDocValue = new FormValueEditPicker(tipoIDDocString, tipoIDDocStringList);

            cc_numberValue = new FormValueEdit(App.member.cc_number);

            FormLabel nifLabel = new FormLabel { Text = "NIF *" };
            nifValue = new FormValueEdit(App.member.nif);

            FormLabel emailLabel = new FormLabel { Text = "EMAIL *" };
            emailValue = new FormValueEdit(App.member.email, Keyboard.Email);

            FormLabel phoneLabel = new FormLabel { Text = "TELEFONE *" };
            phoneValue = new FormValueEdit(App.member.phone, Keyboard.Telephone);

            gridGeral.Add(nameLabel, 0, 0);
            gridGeral.Add(nameValue, 1, 0);
            Grid.SetColumnSpan(nameValue, 2);

            gridGeral.Add(genderLabel, 0, 1);
            gridGeral.Add(genderValue, 1, 1);
            Grid.SetColumnSpan(genderValue, 2);

            gridGeral.Add(birthdateLabel, 0,2);
            gridGeral.Add(birthdateValue, 1, 2);
            Grid.SetColumnSpan(birthdateValue, 2);

            gridGeral.Add(cc_numberLabel, 0, 3);
            gridGeral.Add(tipoIDDocValue, 1, 3);
            gridGeral.Add(cc_numberValue, 2, 3);

            gridGeral.Add(nifLabel, 0, 4);
            gridGeral.Add(nifValue, 1, 4);
            Grid.SetColumnSpan(nifValue, 2);

            gridGeral.Add(emailLabel, 0, 5);
            gridGeral.Add(emailValue, 1, 5);
            Grid.SetColumnSpan(emailValue, 2);

            gridGeral.Add(phoneLabel, 0, 6);
            gridGeral.Add(phoneValue, 1, 6);
            Grid.SetColumnSpan(phoneValue, 2);
        }

        public void CreateGridMorada()
        {

            gridMorada = new Grid { Padding = 10, RowSpacing = 5 * App.screenHeightAdapter };
            gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
            gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            FormLabel addressLabel = new FormLabel { Text = "MORADA *" };
            addressValue = new FormValueEdit(App.member.address);

            FormLabel cityLabel = new FormLabel { Text = "CIDADE *" };
            cityValue = new FormValueEdit(App.member.city);

            FormLabel postalcodeLabel = new FormLabel { Text = "CÓDIGO POSTAL *" };
            postalcodeValue = new FormValueEdit(App.member.postalcode);

            gridMorada.Add(addressLabel, 0, 0);
            gridMorada.Add(addressValue, 1, 0);

            gridMorada.Add(cityLabel, 0, 1);
            gridMorada.Add(cityValue, 1, 1);

            gridMorada.Add(postalcodeLabel, 0, 2);
            gridMorada.Add(postalcodeValue, 1, 2);
        }

        public void CreateGridContactos()
        {
            gridContactos = new Grid { Padding = 10, RowSpacing = 5 * App.screenHeightAdapter };
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridContactos.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridContactos.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });


            FormLabel EncEducacao1Label = new FormLabel { Text = "ENCARREGADO DE EDUCACAO 1", FontSize = App.itemTitleFontSize };

            FormLabel EncEducacao1NomeLabel = new FormLabel { Text = "NOME" };
            EncEducacao1NomeValue = new FormValueEdit("");

            FormLabel EncEducacao1PhoneLabel = new FormLabel { Text = "TELEFONE" };
            EncEducacao1PhoneValue = new FormValueEdit("");

            FormLabel EncEducacao1MailLabel = new FormLabel { Text = "MAIL" };
            EncEducacao1MailValue = new FormValueEdit("");


            FormLabel EncEducacao2Label = new FormLabel { Text = "ENCARREGADO DE EDUCACAO 2", FontSize = App.itemTitleFontSize };

            FormLabel EncEducacao2NomeLabel = new FormLabel { Text = "NOME" };
            EncEducacao2NomeValue = new FormValueEdit("");

            FormLabel EncEducacao2PhoneLabel = new FormLabel { Text = "TELEFONE" };
            EncEducacao2PhoneValue = new FormValueEdit("");

            FormLabel EncEducacao2MailLabel = new FormLabel { Text = "MAIL" };
            EncEducacao2MailValue = new FormValueEdit("");


            gridContactos.Add(EncEducacao1Label, 0, 0);
            Grid.SetColumnSpan(EncEducacao1Label, 2);

            gridContactos.Add(EncEducacao1NomeLabel, 0, 1);
            gridContactos.Add(EncEducacao1NomeValue, 1, 1);

            gridContactos.Add(EncEducacao1PhoneLabel, 0, 2);
            gridContactos.Add(EncEducacao1PhoneValue, 1, 2);

            gridContactos.Add(EncEducacao1MailLabel, 0, 3);
            gridContactos.Add(EncEducacao1MailValue, 1, 3);

            gridContactos.Add(EncEducacao2Label, 0, 4);
            Grid.SetColumnSpan(EncEducacao2Label, 2);

            gridContactos.Add(EncEducacao2NomeLabel, 0, 5);
            gridContactos.Add(EncEducacao2NomeValue, 1, 5);

            gridContactos.Add(EncEducacao2PhoneLabel, 0, 6);
            gridContactos.Add(EncEducacao2PhoneValue, 1, 6);

            gridContactos.Add(EncEducacao2MailLabel, 0, 7);
            gridContactos.Add(EncEducacao2MailValue, 1, 7);

        }


        public NewMemberPageCS()
        {
            Debug.WriteLine("ProfileCS");
            NavigationPage.SetBackButtonTitle(this, "");
            this.initLayout();
            this.initSpecificLayout();

        }

        async Task<List<Dojo>> GetAllDojos()
        {
            DojoManager dojoManager = new DojoManager();
            List<Dojo> dojos = await dojoManager.GetAllDojos();

            return dojos;
        }

        async void OnGeralButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnLoginButtonClicked");
            geralButton.activate();
            moradaButton.deactivate();
            contactosButton.deactivate();

            scrollView.Content = gridGeral;
        }

        async void OnIdentificacaoButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnIdentificacaoButtonClicked");
            geralButton.deactivate();
            moradaButton.deactivate();
            contactosButton.deactivate();

            scrollView.Content = gridIdentificacao;

        }


        async void OnMoradaButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnMoradaButtonClicked");

            geralButton.deactivate();
            moradaButton.activate();
            contactosButton.deactivate();

            scrollView.Content = gridMorada;

        }

        async void OnContactosButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnContactosButtonClicked");

            geralButton.deactivate();
            moradaButton.deactivate();
            contactosButton.activate();

            scrollView.Content = gridContactos;

        }

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnLogoutButtonClicked");

            Preferences.Default.Remove("EMAIL");
            Preferences.Default.Remove("PASSWORD");
            Preferences.Default.Remove("SELECTEDUSER");
            App.member = null;
            App.members = null;

            Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }


        async Task<string> UpdateMember()
        {
            Debug.Print("AQUIIII UpdateMember");

            if (imageloaded == false)
            {
                await DisplayAlert("FOTO EM FALTA", "Para criar o novo utilizador tem de introduzir uma fotografia.", "Ok" );
                return "-1";
            }

            if (!Regex.IsMatch(emailValue.entry.Text, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O email introduzido não é válido.", "Ok" );
                return "-1";
            }

            if (string.IsNullOrEmpty(postalcodeValue.entry.Text))
            {
                postalcodeValue.entry.Text = "";
            }

            if ((nameValue.entry.Text == "") | (CountWords(nameValue.entry.Text) < 2))
            {
                
                await DisplayAlert("DADOS INVÁLIDOS", "O nome introduzido não é válido.", "OK");
                return "-1";
            }
            else if (genderValue.picker.SelectedItem.ToString() == "-")
            {
                await DisplayAlert("DADOS INVÁLIDOS", "Tem de escolher um género.", "OK");
                return "-1";
            }

            if (!Regex.IsMatch(birthdateValue.entry.Text, @"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$"))
            {
                await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok" );
                return "-1";
            }
            else
            {
                if ((DateTime.Parse(birthdateValue.entry.Text) - DateTime.Now).Days > 0)
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok" );
                    return "-1";
                }
            }

            if (cc_numberValue.entry.Text == "")
            {
                Debug.WriteLine("UpdateMemberInfo Aqui 1");
                OnIdentificacaoButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O número de identificação introduzido não é válido.", "Ok" );
                return "-1";
            }
            else if (nifValue.entry.Text == "")
            {
                OnIdentificacaoButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O número de identificação fiscal introduzido não é válido.", "Ok" );
                return "-1";
            }

            if (!Regex.IsMatch(emailValue.entry.Text, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")) 
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O email introduzido não é válido.", "Ok");
                return "-1";
            }
            else if (!Regex.IsMatch(phoneValue.entry.Text, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"))
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido não é válido.", "Ok");
                return "-1";
            }
            else if (addressValue.entry.Text == "")
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS",  "A morada introduzida não é válida.", "Ok" );
                return "-1";
            }
            else if (cityValue.entry.Text == "")
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "A cidade introduzida não é válida.", "Ok" );
                return "-1";
            }
            else if (!Regex.IsMatch((postalcodeValue.entry.Text), @"^\d{4}-\d{3}$"))
            {
                OnMoradaButtonClicked(null, null);
                await DisplayAlert("DADOS INVÁLIDOS", "O código postal introduzido não é válido.", "Ok" );
                return "-1";
            }

            if ((DateTime.Now.Year - DateTime.Parse(birthdateValue.entry.Text).Year) < 18)
            {
                if ((EncEducacao1NomeValue.entry.Text == "") | (EncEducacao1MailValue.entry.Text == "") | (EncEducacao1PhoneValue.entry.Text == ""))
                {
                    OnContactosButtonClicked(null, null);
                    await DisplayAlert("DADOS INVÁLIDOS", "O dados do encarregado de educação 1 introduzidos não são válida.", "Ok" );
                    return "-1";
                }
            }

            App.member.name = nameValue.entry.Text;

            App.member.gender = Constants.KeyByValue(Constants.genders, genderValue.picker.SelectedItem.ToString());

            App.member.documento_identificacao = Constants.KeyByValue(Constants.tipoDocumentoIdentificao, tipoIDDocValue.picker.SelectedItem.ToString());

            App.member.birthdate = birthdateValue.entry.Text;

            App.member.cc_number = cc_numberValue.entry.Text;
            App.member.nif = nifValue.entry.Text;

            App.member.email = emailValue.entry.Text;
            App.member.phone = phoneValue.entry.Text;

            App.member.address = addressValue.entry.Text;
            App.member.city = cityValue.entry.Text;
            App.member.postalcode = postalcodeValue.entry.Text;
            App.member.name_enc1 = EncEducacao1NomeValue.entry.Text;
            App.member.phone_enc1 = EncEducacao1PhoneValue.entry.Text;
            App.member.mail_enc1 = EncEducacao1MailValue.entry.Text;

            App.member.name_enc2 = EncEducacao2NomeValue.entry.Text;
            App.member.phone_enc2 = EncEducacao2PhoneValue.entry.Text;
            App.member.mail_enc2 = EncEducacao2MailValue.entry.Text;

            MemberManager memberManager = new MemberManager();

            showActivityIndicator();
            string result;

            if (App.member.wasCreated == true)
            {
                result = await memberManager.UpdateMemberInfo(App.member);
            }
            else
            {
                result  = await memberManager.createNewMember(App.member);
                App.member.wasCreated = true;
            }

            


            hideActivityIndicator();

            if (result == "-1")
            {
                await DisplayAlert("DADOS INVÁLIDOS", "Tem de preencher todos os dados obrigatórios", "OK");

            }
            else if (result == "-2")
            {
                await DisplayAlert("SÓCIO JÁ EXISTE", "Já existe um sócio no nosso sistema com este Número de Identificação.", "OK");

            }
            else if (result == "-3")
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
                return "-1";
            }
            else
            {
                App.member.id = result;
                showActivityIndicator();
                await memberManager.Upload_Member_Photo(stream);
                hideActivityIndicator();
                //await memberManager.Upload_Member_Photo(stream);

                if (App.member.member_type == "socio")
                {
                    await Navigation.PushAsync(new PaymentPageCS());
                }
                else
                {
                    await Navigation.PushAsync(new NewMemberSuccessPageCS());
                }
                
            }


            
            return result;
        }

        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnConfirmButtonClicked");

            

             confirmButton.button.IsEnabled = false;
            await UpdateMember();
            confirmButton.button.IsEnabled = true;
        }

        public void CreateConfirmButton()
        {

            confirmButton = new RoundButton("CONTINUAR", (App.screenWidth - 20 * App.screenHeightAdapter), 50 * App.screenHeightAdapter);
            confirmButton.button.Clicked += OnConfirmButtonClicked;

            absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenWidthAdapter, (App.screenHeight) - (160 * App.screenHeightAdapter), (App.screenWidth - 20 * App.screenHeightAdapter), 50 * App.screenHeightAdapter));

        }

        void memberPhotoImageTappedAsync(object sender, System.EventArgs e)
        {
            displayMemberPhotoImageActionSheet();
        }

        async Task<string> displayMemberPhotoImageActionSheet()
        {
            var actionSheet = await DisplayActionSheet("Fotografia Sócio", "Cancel", null, "Tirar Foto", "Galeria de Imagens");
            MemberManager memberManager = new MemberManager();
            string result = "";
            switch (actionSheet)
            {
                case "Cancel":
                    break;
                case "Tirar Foto":
                    TakeAPhotoTapped();
                    break;
                case "Galeria de Imagens":
                    OpenGalleryTapped();
                    break;
            }

            return "";
        }

        async void OpenGalleryTapped()
        {
            showActivityIndicator();

            ImageService imageService = new ImageService();
            var result = await imageService.PickPhotoAsync();


            if (result != null)
            {
                stream = await Constants.ResizePhotoStream(result); //result.OpenReadAsync();
                Stream localstream = await Constants.ResizePhotoStream(result);  //await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                imageloaded = true;
                /*MemberManager memberManager = new MemberManager();
                await memberManager.Upload_Member_Photo(stream);*/

            }
            hideActivityIndicator();
        }


        async void TakeAPhotoTapped()
        {
            showActivityIndicator();
            ImageService imageService = new ImageService();
            var result = await imageService.CapturePhotoAsync();


            if (result != null)
            {
                stream = await Constants.ResizePhotoStream(result); //result.OpenReadAsync();
                Stream localstream = await Constants.ResizePhotoStream(result);  //await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                imageloaded = true;
                /*MemberManager memberManager = new MemberManager();
                await memberManager.Upload_Member_Photo(stream);*/
            }

            hideActivityIndicator();

        }

        public static int CountWords(string test)
        {
            int count = 0;
            bool wasInWord = false;
            bool inWord = false;

            for (int i = 0; i < test.Length; i++)
            {
                if (inWord)
                {
                    wasInWord = true;
                }

                if (Char.IsWhiteSpace(test[i]))
                {
                    if (wasInWord)
                    {
                        count++;
                        wasInWord = false;
                    }
                    inWord = false;
                }
                else
                {
                    inWord = true;
                }
            }

            // Check to see if we got out with seeing a word
            if (wasInWord)
            {
                count++;
            }

            return count;
        }
    }
}