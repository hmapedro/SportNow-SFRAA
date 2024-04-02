
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    public class ServiceBox : AbsoluteLayout
    {
        public Label label;
		public Border frame;


		public ServiceBox(string text, double width, double height)
        {
			this.WidthRequest = width;
			this.HeightRequest = height;
			frame = new Border
			{
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5 * (float)App.screenHeightAdapter,
                },
				Stroke = App.topColor,
				BackgroundColor = Colors.Transparent,
				Padding = new Thickness(2, 2, 2, 2),
				HeightRequest = height,
                WidthRequest = width,
                VerticalOptions = LayoutOptions.Center,
			};

			this.Children.Add(frame);
            this.SetLayoutBounds(frame, new Rect(0, 0, width - 5 * App.screenHeightAdapter, height));

			label = new Label { LineBreakMode = LineBreakMode.WordWrap, BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.bottomColor };
			label.Text = text;

			this.Children.Add(label);
            this.SetLayoutBounds(label, new Rect(5 * App.screenWidthAdapter, 5 * App.screenHeightAdapter, width - 10 * App.screenWidthAdapter, height - 10 * App.screenHeightAdapter));

			//return itemabsoluteLayout;
		}
    }
}
