using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
//using static Android.Icu.Text.ListFormatter;

namespace SportNow.CustomViews
{
    public class FormEntry: Border
    {

        public Entry entry;

        //public string Text {get; set; }


        public FormEntry(string text, string placeholder, Keyboard keyboard)
        {
            createFormEntry(text, placeholder, keyboard, 0);
            this.WidthRequest = App.screenWidth - 40 * App.screenWidthAdapter;
        }

        public FormEntry(string text, string placeholder, Keyboard keyboard, double width)
        {
            createFormEntry(text, placeholder, keyboard, width);
            this.WidthRequest = width;
        }


        public FormEntry(string text, string placeholder, double width)
        {
            createFormEntry(text, placeholder, Keyboard.Text, width);
            this.WidthRequest = width;
        }

        public FormEntry(string text, string placeholder, double width, Keyboard keyboard)
        {
            createFormEntry(text, placeholder, keyboard, 0);
            this.WidthRequest = width;
        }

        public void createFormEntry(string text, string placeholder, Keyboard keyboard, double width)
        {

            this.BackgroundColor = App.backgroundColor;
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            this.Padding = new Thickness(2,2,2,2);
            
            this.HeightRequest = 45 * App.screenHeightAdapter;

            //USERNAME ENTRY
            entry = new Entry
            {
                //Text = "tete@hotmail.com",
                Text = text,
                TextColor = App.normalTextColor,
                BackgroundColor = App.backgroundColor,
                Placeholder = placeholder,
                PlaceholderColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = App.formValueFontSize,
                Keyboard = keyboard,
                FontFamily = "futuracondensedmedium",
                
            };

            if (width != 0)
            {
                entry.WidthRequest = width-4;
            }
            this.Content = entry;

        }
    }
}
