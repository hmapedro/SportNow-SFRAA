using System;
using Microsoft.Maui;

namespace SportNow.CustomViews
{
    public class FormLabel: Label
    {


        public FormLabel()
        {
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalTextAlignment = TextAlignment.Start;
            TextColor = App.normalTextColor;
            LineBreakMode = LineBreakMode.NoWrap;
            Padding = 0;
            FontFamily = "futuracondensedmedium";
            FontSize = App.formLabelFontSize;
        }
    }
}
