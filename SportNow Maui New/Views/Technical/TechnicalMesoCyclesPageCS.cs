using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views.Technical
{
	public class TechnicalMesoCyclesPageCS : DefaultPage
	{
		protected override void OnAppearing()
		{

            base.OnAppearing();
        }

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}


        DateTime firstDayWeek_datetime;
        Label currentMacroCycleLabel;
        Cycle currentMacroCycle;
        int currentMacroCycle_Index;
        FormValueEditLongText objetivosEntry;
        RoundButton confirmButton;
        private List<Class_Detail> dojoClasses;
        private List<Cycle> macroCycles;

        Picker dojoPicker, classesPicker;
        List<Dojo> dojos;

        string personalClass_type;

        private Microsoft.Maui.Controls.StackLayout stackButtons, stackMacroCycleSelector;

        public void initLayout()
		{
			Title = "MESOCICLOS";
		}


		public void CleanScreen()
		{
			Debug.Print("TechnicalMesoCyclesPageCS.CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            DojoManager dojoManager = new DojoManager();
            dojos = await dojoManager.GetAllDojos();

            ClassManager classManager = new ClassManager();
            dojoClasses = await classManager.GetAllClasses(App.member.dojoid);

            TechnicalManager technicalManager = new TechnicalManager();
			macroCycles = await technicalManager.GetCycles(dojoClasses[0].name, "macrociclo");

            hideActivityIndicator();

            _ = await CreateDojoPicker();
            _ = await CreateClassesPicker();
            CreateMacroCycleSelector();
            CreateMacroCycleDescription();
            CreateConfirmButton();

            hideActivityIndicator();
        }

        public string getDojoIDbyName(string dojoName)
        {
            foreach (Dojo dojo in dojos)
            {
                if (dojo.name == dojoName)
                {
                    return dojo.id;
                }
            }
            return "";
        }
         

        public async Task<int> CreateDojoPicker()
        {
            Debug.Print("TechnicalManageCycles.CreateDojoPicker");
            List<string> dojoList = new List<string>();
            int selectedIndex = 0;
            int selectedIndex_temp = 0;

            foreach (Dojo dojo in dojos)
            {
                dojoList.Add(dojo.name);
                if (dojo.name == App.member.dojo)
                {
                    selectedIndex = selectedIndex_temp;
                }
                selectedIndex_temp++;
            }

            dojoPicker = new Picker
            {
                Title = "",
                TitleColor = Colors.White,
                BackgroundColor = Colors.Transparent,
                TextColor = Color.FromRgb(246, 220, 178),
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize

            };
            dojoPicker.ItemsSource = dojoList;
            dojoPicker.SelectedIndex = selectedIndex;

            dojoPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                showActivityIndicator();
                Debug.Print("SelectedIndexChanged.DojoPicker selectedItem = " + dojoPicker.SelectedItem.ToString());
                ClassManager classManager = new ClassManager();
                dojoClasses = await classManager.GetAllClasses(getDojoIDbyName(dojoPicker.SelectedItem.ToString()));

                if (dojoClasses == null)
                {
                    dojoClasses = new List<Class_Detail>();
                }

                List<string> classesList = new List<string>();
                foreach (Class_Detail classDetail in dojoClasses)
                {
                    classesList.Add(classDetail.name);
                }

                classesPicker.ItemsSource = classesList;
                classesPicker.SelectedIndex = 0;

                hideActivityIndicator();          
            };

            absoluteLayout.Add(dojoPicker);
            absoluteLayout.SetLayoutBounds(dojoPicker, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));

            return 1;
        }

        public async Task<int> CreateClassesPicker()
        {
            Debug.Print("TechnicalManageCycles.CreateClassesPicker");
            List<string> classesList = new List<string>();
            int selectedIndex = 0;

            foreach (Class_Detail classDetail in dojoClasses)
            {
                classesList.Add(classDetail.name);
            }

            classesPicker = new Picker
            {
                Title = "",
                TitleColor = Colors.White,
                BackgroundColor = Colors.Transparent,
                TextColor = Color.FromRgb(246, 220, 178),
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize

            };
            classesPicker.ItemsSource = classesList;
            classesPicker.SelectedIndex = selectedIndex;

            classesPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {

                showActivityIndicator();

                if (dojoClasses.Count > 0)
                {
                    Debug.Print("classesPicker selectedItem = " + classesPicker.SelectedItem.ToString());
                    TechnicalManager technicalManager = new TechnicalManager();
                    macroCycles = await technicalManager.GetCycles(dojoClasses[selectedIndex].name, "macrociclo");
                }

                

                hideActivityIndicator();

            };

            absoluteLayout.Add(classesPicker);
            absoluteLayout.SetLayoutBounds(classesPicker, new Rect(0, 45 * App.screenHeightAdapter, App.screenWidth, 40 * App.screenHeightAdapter));

            return 1;
        }

        public void CreateMacroCycleSelector()
        {
            Debug.Print("TechnicalManageCycles.CreateMacroCycleSelector");
            var width = Constants.ScreenWidth;
            var buttonWidth = (width - 50) / 3;

            Button previousWeekButton = new Button();
            Button nextWeekButton = new Button();

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                previousWeekButton = new Button()
                {
                    Text = "<",
                    FontSize = App.titleFontSize,
                    TextColor = Color.FromRgb(246, 220, 178),
                    VerticalOptions = LayoutOptions.Center
                };
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                previousWeekButton = new Button()
                {
                    Text = "<",
                    FontSize = App.titleFontSize,
                    TextColor = Color.FromRgb(246, 220, 178),
                    BackgroundColor = App.backgroundColor,
                    VerticalOptions = LayoutOptions.Center
                };
            }

            previousWeekButton.Clicked += OnPreviousButtonClicked;

            currentMacroCycle = new Cycle();
            Debug.Print("macroCycles.count = " + macroCycles.Count);
            bool found = false;
            int i = 0;
            foreach (Cycle currentMacroCycle_i in macroCycles)
            {
                if ((DateTime.Now >= DateTime.Parse(currentMacroCycle_i.data_inicio)) & (DateTime.Now <= DateTime.Parse(currentMacroCycle_i.data_fim)))
                {
                    currentMacroCycle = currentMacroCycle_i;
                    currentMacroCycle_Index = i;
                    found = true;
                    break;
                }
                i++;
            }
            if ((found == false) & (macroCycles.Count() > 0))
            {
                currentMacroCycle = macroCycles[macroCycles.Count()-1];
            }

            currentMacroCycleLabel = new Label()
            {
                Text = currentMacroCycle.name + "\n"+ currentMacroCycle.data_inicio + " - "+ currentMacroCycle.data_fim,
                FontSize = App.titleFontSize,
                TextColor = Color.FromRgb(246, 220, 178),
                WidthRequest = 250 * App.screenWidthAdapter,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                nextWeekButton = new Button()
                {
                    Text = ">",
                    FontSize = App.titleFontSize,
                    TextColor = Color.FromRgb(246, 220, 178),
                    VerticalOptions = LayoutOptions.Center
                };
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                nextWeekButton = new Button()
                {
                    Text = ">",
                    FontSize = App.titleFontSize,
                    TextColor = Color.FromRgb(246, 220, 178),
                    BackgroundColor = App.backgroundColor,
                    VerticalOptions = LayoutOptions.Center
                };
            }

            nextWeekButton.Clicked += OnNextButtonClicked;

            stackMacroCycleSelector = new Microsoft.Maui.Controls.StackLayout
            {
                //WidthRequest = 370,
                Margin = new Thickness(0),
                Spacing = 5 * App.screenHeightAdapter,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 60 * App.screenHeightAdapter,
                Children =
                {
                    previousWeekButton,
                    currentMacroCycleLabel,
                    nextWeekButton
                }
            };

            absoluteLayout.Add(stackMacroCycleSelector);
            absoluteLayout.SetLayoutBounds(stackMacroCycleSelector, new Rect(0, 90 * App.screenHeightAdapter, App.screenWidth, 60 * App.screenHeightAdapter));


        }

        public void CreateMacroCycleDescription()
        {
            Debug.Print("TechnicalManageCycles.CreateMacroCycleDescription");

            objetivosEntry = new FormValueEditLongText(currentMacroCycle.objetivos, Keyboard.Chat, Convert.ToInt16(400 * App.screenHeightAdapter));

            absoluteLayout.Add(objetivosEntry);
            absoluteLayout.SetLayoutBounds(objetivosEntry, new Rect(5 * App.screenWidthAdapter, 160 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 400 * App.screenHeightAdapter));

        }

        public void CreateConfirmButton()
        {

            confirmButton = new RoundButton("GRAVAR", 100, 50);
            confirmButton.button.Clicked += OnConfirmButtonClicked;

            absoluteLayout.Add(confirmButton);
            absoluteLayout.SetLayoutBounds(confirmButton, new Rect(0, App.screenHeight - 40 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

        }

        public TechnicalMesoCyclesPageCS()
		{
            this.initLayout();
            this.initSpecificLayout();
        }

        async void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            showActivityIndicator();
            if (currentMacroCycle_Index == 0)
            {
                await DisplayAlert("NÃO HÁ MAIS CICLOS", "Não existem macrociclos anteriores a este.", "OK");
            }
            else
            {
                currentMacroCycle_Index--;
                currentMacroCycle = macroCycles[currentMacroCycle_Index];
                currentMacroCycleLabel.Text = currentMacroCycle.name + "\n" + currentMacroCycle.data_inicio + " - " + currentMacroCycle.data_fim;
                objetivosEntry.entry.Text = currentMacroCycle.objetivos;
            }

            TechnicalManager technicalManager = new TechnicalManager();
            string res = await technicalManager.Update_Cycle(currentMacroCycle.id, objetivosEntry.entry.Text);
            hideActivityIndicator();
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            showActivityIndicator();
            if (currentMacroCycle_Index == (macroCycles.Count-1))
            {
                await DisplayAlert("NÃO HÁ MAIS CICLOS", "Não existem macrociclos posteriores a este.", "OK");
            }
            else
            {
                currentMacroCycle_Index++;
                currentMacroCycle = macroCycles[currentMacroCycle_Index];
                currentMacroCycleLabel.Text = currentMacroCycle.name + "\n" + currentMacroCycle.data_inicio + " - " + currentMacroCycle.data_fim;
                objetivosEntry.entry.Text = currentMacroCycle.objetivos;
            }

            TechnicalManager technicalManager = new TechnicalManager();
            string res = await technicalManager.Update_Cycle(currentMacroCycle.id, objetivosEntry.entry.Text);

            hideActivityIndicator();
        }

        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            showActivityIndicator();

            Debug.WriteLine("TechnicalManageCycles.OnConfirmButtonClicked");

            TechnicalManager technicalManager = new TechnicalManager();
            string res = await technicalManager.Update_Cycle(currentMacroCycle.id, objetivosEntry.entry.Text);

            hideActivityIndicator();

        }

    }
}
