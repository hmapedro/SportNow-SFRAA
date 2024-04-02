using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    public class FormValue : Border
    {

        public Label label;
        //public string Text {get; set; }

        public FormValue(string text, double height)
        {
            createFormValue(text, height, App.formValueFontSize, Colors.Transparent, App.normalTextColor, TextAlignment.Start);
        }

        public FormValue(string text)
        {
            createFormValue(text, 45 * App.screenHeightAdapter, App.formValueFontSize, Colors.Transparent, App.normalTextColor, TextAlignment.Start);
        }

        public FormValue(string text, int fontSize, Color backgroundColor, Color textColor, TextAlignment textHorizontalAlignment)
        {
            createFormValue(text, 35 * App.screenHeightAdapter, App.formValueFontSize, backgroundColor, textColor, textHorizontalAlignment);
        }

        public void createFormValue(string text, double height, int fontSize, Color backgroundColorColor, Color textColor, TextAlignment textHorizontalAlignment)
        {
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            BackgroundColor = backgroundColorColor;
            Padding = new Thickness(2, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = height;
            this.VerticalOptions = LayoutOptions.Center;

            label = new Label
            {
                Padding = new Thickness(5, 0, 5, 0),
                Text = text,
                HorizontalTextAlignment = textHorizontalAlignment,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = textColor,
                BackgroundColor = App.backgroundColor,
                FontSize = fontSize,
                FontFamily = "futuracondensedmedium",
            };

            this.Content = label; // relativeLayout_Button;
        }
    }
}
