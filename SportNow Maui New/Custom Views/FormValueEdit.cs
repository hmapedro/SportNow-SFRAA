using System;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    /*public class FormValueEdit : Frame
    {

        public Entry entry;
        //public string Text {get; set; }

        public FormValueEdit(string text)
        {

            //this.BackgroundColor = Color.FromRgb(25, 25, 25);
            this.BackgroundColor = Color.FromRgb(25, 25, 25);
            this.BorderColor = Colors.LightGray;

            this.CornerRadius = 10;
            this.IsClippedToBounds = true;
            this.Padding = new Thickness(10, 2, 10, 2);
            //this.WidthRequest = width;
            this.HeightRequest = 30;

            //USERNAME ENTRY
            entry = new Entry
            {
                //Text = "tete@hotmail.com",
                Text = text,
                TextColor = App.normalTextColor,
                BackgroundColor = App.backgroundColor,
                //Placeholder = placeholder,
                HorizontalOptions = LayoutOptions.Start,
                //WidthRequest = width,
                FontSize = 18,
            };
            this.Content = entry;

        }
    }*/

    public class FormValueEdit : Border
    {

        public Entry entry;
        //public string Text {get; set; }

        public FormValueEdit(string Text, Keyboard keyboard, int height)
        {

            create_FormValueEdit(Text, keyboard, height);
        }

        public FormValueEdit(string Text, Keyboard keyboard)
        {

            create_FormValueEdit(Text, keyboard, 45);
        }

        public FormValueEdit(string Text)
        {
            create_FormValueEdit(Text, Keyboard.Default, 45);
        }

        public void create_FormValueEdit(string Text, Keyboard keyboard, int height)
        {

            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            BackgroundColor = Colors.Transparent;
            this.Padding = new Thickness(1, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = height * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            
            entry = new Entry
            {

                //Padding = new Thickness(5,0,5,0),
                Text = Text,
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                BackgroundColor = App.backgroundColor,
                FontSize = App.formValueFontSize,
                Keyboard = keyboard,
                FontFamily = "futuracondensedmedium",
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                //HeightRequest = 30
            };

            this.Content = entry; // relativeLayout_Button;

        } 
    }
}
