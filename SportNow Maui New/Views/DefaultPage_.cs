using System;
using System.Diagnostics;
using Microsoft.Maui;
//

namespace SportNow.Views
{
    public class DefaultPage_ : ContentPage
	{
        Microsoft.Maui.Controls.StackLayout stack;
        Image loading;

        public DefaultPage_()
        {
            this.initBaseLayout();
		}

		public AbsoluteLayout absoluteLayout;

		public void initBaseLayout()
		{

            this.BackgroundColor = App.backgroundColor;

            absoluteLayout = new AbsoluteLayout
            {
                Margin = new Thickness(10),
                //BackgroundColor = Color.FromRgb(0, 0, 255)

			};
			Content = absoluteLayout;

			NavigationPage.SetBackButtonTitle(this, "");


            if (Application.Current.MainPage != null)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Colors.Black;
                navigationPage.BarTextColor = App.normalTextColor;
            }

            stack = new StackLayout() { BackgroundColor = Color.FromRgb(255, 25, 25), Opacity = 0.6 };
            loading = new Image() { BackgroundColor = Color.FromRgb(255,0,0), Source = "loading_ippon.gif", IsAnimationPlaying = true }; 

            //indicator = new ActivityIndicator() { Color = App.topColor, HeightRequest = 100, WidthRequest = 100, MinimumHeightRequest = 100, MinimumWidthRequest = 100};
        }

        public void showActivityIndicator()
        {
            //indicator.IsRunning = true;

            /*if (absoluteLayout == null)
            {
                initBaseLayout();
            }*/

            absoluteLayout.Add(stack);
            absoluteLayout.SetLayoutBounds(stack, new Rect(0, 0, App.screenWidth, App.screenHeight));
            //, new Rect(0, 0, 400, 400));
            /*xConstraint: )0),
            yConstraint: )0),
            widthConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width); }),
            heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height); })); ;*/
            /*absoluteLayout.Add(indicator);
            absoluteLayout.SetLayoutBounds(, new Rect(, , , ));Constraint.RelativeToParent((parent) => { return ((parent.Width / 2) - 50); }),
                yConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Height / 2) - 50); }),
                widthConstraint: )100),
                heightConstraint: )100));*/
            absoluteLayout.Add(loading);

            Debug.Print("AQUIIIII App.screenWidth = " + App.screenWidth);
            Debug.Print("AQUIIIII Window.Width = " + Window.Width);

            Debug.Print("AQUIIIII (DeviceDisplay.MainDisplayInfo.Width = " + (DeviceDisplay.MainDisplayInfo.Width));
            Debug.Print("AQUIIIII DeviceDisplay.MainDisplayInfo.Density) = " + DeviceDisplay.MainDisplayInfo.Density);
            Debug.Print("AQUIIIII (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) = " + (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density));
            Debug.Print("AQUIIIII (App.screenWidth/2) - 50 = " + ((App.screenWidth / 2) - 50));

            Debug.Print("AQUIIIII Window.Height = " + Window.Height);
            Debug.Print("AQUIIIII App.screenHeight= " + App.screenHeight);
            Debug.Print("AQUIIIII (DeviceDisplay.MainDisplayInfo.Height = " + (DeviceDisplay.MainDisplayInfo.Height));
            Debug.Print("AQUIIIII DeviceDisplay.MainDisplayInfo.Density) = " + DeviceDisplay.MainDisplayInfo.Density);
            Debug.Print("AQUIIIII (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) = " + (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density));
            Debug.Print("AQUIIIII (App.Height/2) - 50 = " + ((App.screenHeight / 2) - 50));

            absoluteLayout.SetLayoutBounds(loading, new Rect((App.screenWidth/2) - 50, (App.screenHeight) - 300, 100 , 100 ));
            /*);
            absoluteLayout.SetLayoutBounds(, new Rect(, , , ));Constraint.RelativeToParent((parent) => { return ((parent.Width / 2) - 50); }),
                yConstraint: Constraint.RelativeToParent((parent) => { return ((parent.Height / 2) - 50); }),
                widthConstraint: )100),
                heightConstraint: )100));*/

        }

        public void hideActivityIndicator()
        {
            absoluteLayout.Remove(stack);
            absoluteLayout.Remove(loading);
            //indicator.IsRunning = false;
        }
    }
}
