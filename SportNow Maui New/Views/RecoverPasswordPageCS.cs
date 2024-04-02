
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Collections.Generic;
using System.Net.Mail;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class RecoverPasswordPageCS : DefaultPage
	{

        RoundButton recoverPasswordButton;
        FormEntry usernameEntry;
		Label messageLabel;

		public void initBaseLayout() {
			base.initBaseLayout();
			Title = "RECUPERAR PASSWORD";			

			/*var toolbarItem = new ToolbarItem {
				Text = "Sign Up",
			};
			toolbarItem.Clicked += OnSignUpButtonClicked;
			ToolbarItems.Add (toolbarItem);

			messageLabel = new Label ();*/

		}

		public void initSpecificLayout()
		{

			Microsoft.Maui.Controls.Grid gridLogin = new Microsoft.Maui.Controls.Grid { Padding = 0 };
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = 50 * App.screenHeightAdapter });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = 50 * App.screenHeightAdapter });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth}); //GridLength.Auto 
			gridLogin.RowSpacing = 5;

            Label recoverPasswordLabel = new Label
			{
				Text = "Introduza o email para o qual pretende recuperar a Password.",
				HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = App.normalTextColor,
				FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

			string username = "";

			if (Preferences.Default.Get("EMAIL", "") != "")
			{
				username = Preferences.Default.Get("EMAIL", "");
			}

            usernameEntry = new FormEntry(username, "EMAIL", Keyboard.Email, App.screenWidth - 20 * App.screenWidthAdapter);

			//LOGIN BUTTON
			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

            gradient.GradientStops.Add(new GradientStop(App.topColor, Convert.ToSingle(0)));
            gradient.GradientStops.Add(new GradientStop(App.bottomColor, Convert.ToSingle(0.5)));


            recoverPasswordButton = new RoundButton("ENVIAR EMAIL", App.screenWidth - 20 * App.screenWidthAdapter, 50);
            recoverPasswordButton.button.Clicked += OnrecoverPasswordButtonClicked;

            /*            recoverPasswordButton = new Button
			{
				Text = "ENVIAR EMAIL",
				Background = gradient,
                TextColor = App.oppositeTextColor,
				HorizontalOptions = LayoutOptions.Center,
				FontSize = App.itemTitleFontSize,
				WidthRequest = App.screenWidth - 20 * App.screenWidthAdapter,
                FontFamily = "futuracondensedmedium"
            };
			recoverPasswordButton.Clicked += OnrecoverPasswordButtonClicked;


			Frame frame_recoverPasswordButton = new Frame {
                BackgroundColor = App.backgroundColor,
                BorderColor = Colors.LightGray,
                CornerRadius = 10 * (float) App.screenWidthAdapter,
                IsClippedToBounds = true,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = App.screenWidth - 20 * App.screenWidthAdapter,
                HeightRequest = 45 * App.screenWidthAdapter
            };

            frame_recoverPasswordButton.Content = recoverPasswordButton;
			*/
			messageLabel = new Label {
                FontFamily = "futuracondensedmedium",
                HorizontalOptions = LayoutOptions.Center,
				TextColor = Colors.Green,
				FontSize = App.itemTitleFontSize,
				LineBreakMode = LineBreakMode.WordWrap,
			};

			//RECOVER PASSWORD LABEL

			gridLogin.Add(recoverPasswordLabel, 0, 0);
			gridLogin.Add(usernameEntry, 0, 1);
			gridLogin.Add(recoverPasswordButton, 0, 2);
			gridLogin.Add(messageLabel, 0, 3);

			absoluteLayout.Add(gridLogin);
            absoluteLayout.SetLayoutBounds(gridLogin, new Rect(0, 0, App.screenWidth, App.screenHeight));

        }


		public RecoverPasswordPageCS()
		{

			this.initBaseLayout();
			this.initSpecificLayout();

		}


		async void OnrecoverPasswordButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnrecoverPasswordButtonClicked");

			recoverPasswordButton.IsEnabled = false;
			messageLabel.Text = "";
			messageLabel.TextColor = Colors.Green;

			MemberManager memberManager = new MemberManager();


			if (IsValidEmail(usernameEntry.entry.Text))
			{
				int result = await memberManager.RecoverPassword(usernameEntry.entry.Text);

				if (result == 1)
				{
					messageLabel.TextColor = Colors.Green;
					messageLabel.Text = "Enviámos um email para o endereço indicado em cima com os dados para recuperar a tua password.";
				}
				else if (result == -1)
				{
					messageLabel.TextColor = Colors.Red;
					messageLabel.Text = "Houve um erro. O email introduzido não é válido.";
				}
				else 
				{
					messageLabel.TextColor = Colors.Red;
					messageLabel.Text = "Houve um erro. Verifica a tua ligação à Internet ou tenta novamente mais tarde.";
				}
			}
			else
			{
				messageLabel.TextColor = Colors.Red;
				messageLabel.Text = "Houve um erro. O email introduzido não é válido.";
			}
			recoverPasswordButton.IsEnabled = true;
		}

		async Task <string> AreCredentialsCorrect (User user)
		{
			Debug.WriteLine("AreCredentialsCorrect");
			MemberManager memberManager = new MemberManager();

			string loginOk = await memberManager.Login(user);

			return loginOk;
		}

		async Task<List<Member>> GetMembers(User user)
		{
			Debug.WriteLine("AreCredentialsCorrect");
			UserManager userManager = new UserManager();

			List<Member> members;

			members = await userManager.GetMembers(user);

			return members;
			
		}

		public bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

	}
}


