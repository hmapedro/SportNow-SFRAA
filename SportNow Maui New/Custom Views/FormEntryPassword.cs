using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.CustomViews
{
    public class FormEntryPassword: Border
    {

        public Entry entry;
        //public string Text {get; set; }


        public FormEntryPassword(string Text, string placeholder, double width)
        {
            createFormEntry(Text, placeholder, width);
            this.WidthRequest = width;

        }

        public FormEntryPassword(string Text, string placeholder)
        {
            createFormEntry(Text, placeholder, 0);

        }

        public void createFormEntry(string Text, string placeholder, double width)
        {

            this.BackgroundColor = App.backgroundColor;
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            this.Padding = new Thickness(2, 2, 2, 2);
            //this.WidthRequest = 300;
            this.HeightRequest = 45 * App.screenHeightAdapter;


            //USERNAME ENTRY
            entry = new Entry
            {
                //Text = "tete@hotmail.com",
                Text = Text,
                IsPassword = true,
                TextColor = App.normalTextColor,
                BackgroundColor = App.backgroundColor,
                Placeholder = placeholder,
                PlaceholderColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Start,
                //WidthRequest = 300 * App.screenWidthAdapter,
                FontSize = App.formValueFontSize,
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
