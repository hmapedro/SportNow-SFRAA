using SportNow.Model;


namespace SportNow.Views
{
	public class IntroPageCS : DefaultPage
	{

		public List<MainMenuItem> MainMenuItems { get; set; }

		protected override void OnAppearing()
		{
			Constants.ScreenWidth = Application.Current.MainPage.Width;//DeviceDisplay.MainDisplayInfo.Width;
			Constants.ScreenHeight = Application.Current.MainPage.Height; //DeviceDisplay.MainDisplayInfo.Height;
			//Debug.Print("ScreenWidth = "+ Constants.ScreenWidth + " ScreenHeight = " + Constants.ScreenHeight);
		}

		public void initLayout()
		{
			Title = "Home";
		}



		public IntroPageCS ()
		{
			this.initLayout();
			
		}
	}
}
