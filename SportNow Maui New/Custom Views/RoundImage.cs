using Microsoft.Maui.Controls.Shapes;
//using Syncfusion.Maui.Core;

namespace SportNow.CustomViews
{
    public class RoundImage : Image
    {
        public RoundImage()
        {
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Aspect = Aspect.AspectFill;
            this.WidthRequest = (int)160 * App.screenHeightAdapter;
            this.HeightRequest = (int)160 * App.screenHeightAdapter;

            this.Clip = new EllipseGeometry()
			{
				Center = new Point(80 * App.screenHeightAdapter, 80 * App.screenHeightAdapter),
				RadiusX = 80 * App.screenHeightAdapter,
				RadiusY = 80 * App.screenHeightAdapter
			};

        }
    }

   /*public class RoundImage : SfAvatarView
    {
       public RoundImage()
       {

            this.VerticalOptions = LayoutOptions.Center;
            this.HorizontalOptions = LayoutOptions.Center;
            this.BackgroundColor = Color.FromRgba("#252525");
            this.ContentType = ContentType.Custom;
            this.WidthRequest = 180 * App.screenHeightAdapter;
            this.HeightRequest = 180 * App.screenHeightAdapter;
            this.CornerRadius = 90 * App.screenHeightAdapter;
        }
   }*/
}
