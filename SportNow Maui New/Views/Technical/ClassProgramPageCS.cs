using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;


namespace SportNow.Views.Technical
{
	public class ClassProgramPageCS : DefaultPage
	{
		protected override void OnAppearing()
		{
            base.OnAppearing();
        }

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

        public Class_Schedule class_schedule;

        public string classProgramID;
        Class_Program classProgram;

        public void initLayout()
		{
		}


		public void CleanScreen()
		{
			Debug.Print("classProgramPageCS.CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            ClassManager classManager = new ClassManager();
            classProgramID = await classManager.CreateClass_Program(App.member.id, class_schedule.classid, class_schedule.date, "");
            classProgram = await classManager.GetClass_Program_byID(classProgramID);

            CreateTitle();



            hideActivityIndicator();
        }

        public void CreateTitle()
        {

            Label className = new Label()
            {
                Text = this.class_schedule.name + "\n" + class_schedule.dojo + "\n" + class_schedule.date,
                FontSize = 20,
                TextColor = Color.FromRgb(246, 220, 178),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            absoluteLayout.Add(className);
            absoluteLayout.SetLayoutBounds(className, new Rect(0, 0, App.screenWidth, 80 * App.screenHeightAdapter));

        }

        public ClassProgramPageCS(Class_Schedule class_schedule)
		{
            this.class_schedule = class_schedule;
            this.initLayout();
            this.initSpecificLayout();
        }



        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
        }

    }
}
