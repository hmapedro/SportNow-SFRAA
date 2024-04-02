using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    public class OptionButton: AbsoluteLayout
    {

        /*public double width { get; set; }
        public string text { get; set; }*/

        //public Frame frame;
        public Label label;
        public Image image;
		public Border frame;


		public OptionButton(string text, string imageSource, double width, double height)
        {
            this.WidthRequest = App.screenWidth / 2;
            this.HeightRequest = height;//App.screenWidth / 4;
			frame = new Border
			{
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5 * (float)App.screenHeightAdapter,
                },
                Stroke = App.topColor,
                BackgroundColor = App.backgroundOppositeColor,
				Padding = new Thickness(2, 2, 2, 2),
                WidthRequest = width,
                HeightRequest = height,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

			image = new Image { Source = imageSource, Aspect = Aspect.AspectFill, Opacity = 0.5 }; //, HeightRequest = 60, WidthRequest = 60
			frame.Content = image;

			this.Add(frame);
			this.SetLayoutBounds(frame, new Rect(0, 0, width, height));


			label = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Colors.White, LineBreakMode = LineBreakMode.WordWrap, FontFamily = "futuracondensedmedium" };
			label.Text = text;

			this.Add(label);
            this.SetLayoutBounds(label, new Rect(5 * App.screenWidthAdapter, (height / 2) - (20 * App.screenHeightAdapter), width - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			//return itemabsoluteLayout;
		}
    }
}
