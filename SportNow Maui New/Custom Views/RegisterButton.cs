using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    public class RegisterButton: Border
    {

        /*public double width { get; set; }
        public string text { get; set; }*/

        //public Frame frame;
        public Button button;

        public RegisterButton(string text, double width, double height)
        {

            createRegisterButton(text, width, height, 1);
        }

        public RegisterButton(string text, double width, double height, double screenAdaptor)
        {
            createRegisterButton(text, width, height, screenAdaptor);
        }

        public void createRegisterButton(string text, double width, double height, double screenAdaptor)
        {

            /*GradientBrush gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
            };

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));*/

            //BUTTON
            button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromRgb(96, 182, 89), //gradient,
                TextColor = Colors.White,
                FontSize = App.itemTitleFontSize, //* App.screenWidthAdapter,
                WidthRequest = width,
                HeightRequest = height,
                FontFamily = "futuracondensedmedium",
            };
            //geralButton.Clicked += OnGeralButtonClicked;

            //frame = new Frame { BackgroundColor = App.backgroundColor, BorderColor = Colors.LightGray, CornerRadius = 20, IsClippedToBounds = true, Padding = 0 };
            this.BackgroundColor = Color.FromRgb(96, 182, 89);//Color.FromRgb(25, 25, 25);
                                                              //this.BorderColor = Colors.LightGray;
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            this.Padding = 0;
            this.WidthRequest = width;
            this.HeightRequest = height;
            this.Content = button; // relativeLayout_Button;
        }
    }
}
