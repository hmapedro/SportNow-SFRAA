using System;
using System.Diagnostics;
//using Xamarin.CommunityToolkit.Behaviors;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
/*--using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Markup;*/

namespace SportNow.CustomViews
{
   
     public class FormValueEditCodPostal : Border
     {

         public Entry entry;
         //public string Text {get; set; }

         public FormValueEditCodPostal(string Text) {

            StrokeShape = new RoundRectangle
            {
                CornerRadius = 5 * (float)App.screenHeightAdapter,
            };
            Stroke = App.topColor;
            BackgroundColor = Color.FromRgb(0,0,0);
            this.Padding = new Thickness(1, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;

             entry = new Entry
             {
                 //Padding = new Thickness(5,0,5,0),
                 Placeholder = "XXXX-XXX",
                 Keyboard = Keyboard.Numeric,
                 Text = Text,
                 HorizontalTextAlignment = TextAlignment.Start,
                 //VerticalTextAlignment = TextAlignment.Center,
                 TextColor = App.normalTextColor,
                 BackgroundColor = Color.FromRgb(255,255,255),
                 FontSize = App.formValueFontSize,
                 FontFamily = "futuracondensedmedium",
                 //HeightRequest = 30
             };
            
             this.Content = entry; // relativeLayout_Button;

            this.Content = entry;
            /*--entry.Behaviors.Add(new MaskedBehavior() { Mask = "XXXX-XXX" });*/
        }
     }
}