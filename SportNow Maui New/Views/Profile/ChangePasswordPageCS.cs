using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;


namespace SportNow.Views.Profile
{
    public class ChangePasswordPageCS : DefaultPage
	{
		private Member member;


		private Microsoft.Maui.Controls.Grid grid;
		FormEntryPassword currentPasswordEntry;
		FormEntryPassword newPasswordEntry;
		FormEntryPassword newPasswordConfirmEntry;
		RoundButton changePasswordButton;
        CancelButton deleteMemberButton;
		Label messageLabel;

		public void initLayout()
		{
			Title = "SEGURANÇA";

			/*var toolbarItem = new ToolbarItem
			{
				Text = "Logout"
			};
			toolbarItem.Clicked += OnLogoutButtonClicked;
			ToolbarItems.Add(toolbarItem);*/

		}


		public void initSpecificLayout()
		{

			//member = App.members[0];


			grid = new Microsoft.Maui.Controls.Grid { RowSpacing = 5, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth/2 - 10 * App.screenWidthAdapter }); //GridLength.Auto
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth/2 - 10 * App.screenWidthAdapter }); //GridLength.Auto 

			FormLabel currentPasswordLabel = new FormLabel { Text = "PASSWORD ATUAL" };
			currentPasswordEntry = new FormEntryPassword("", "", App.screenWidth / 2 - 5 * App.screenWidthAdapter);

			FormLabel newPasswordLabel = new FormLabel { Text = "NOVA PASSWORD" };
			newPasswordEntry = new FormEntryPassword("", "", App.screenWidth / 2 - 5 * App.screenWidthAdapter);

			FormLabel newPasswordConfirmLabel = new FormLabel { Text = "NOVA PASSWORD CONFIRMAÇÃO" };
			newPasswordConfirmEntry = new FormEntryPassword("", "", App.screenWidth / 2 - 5 * App.screenWidthAdapter);

			changePasswordButton = new RoundButton("Alterar Password", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
			changePasswordButton.button.Clicked += OnChangePasswordButtonClicked;

			messageLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                HorizontalOptions = LayoutOptions.Center,
				TextColor = Colors.Red,
				FontSize = App.itemTitleFontSize
			};

			grid.Add(currentPasswordLabel, 0, 0);
			grid.Add(currentPasswordEntry, 1, 0);

			grid.Add(newPasswordLabel, 0, 1);
			grid.Add(newPasswordEntry, 1, 1);

			grid.Add(newPasswordConfirmLabel, 0, 2);
			grid.Add(newPasswordConfirmEntry, 1, 2);

			grid.Add(changePasswordButton, 0, 3);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(changePasswordButton, 2);

			grid.Add(messageLabel, 0, 4);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(messageLabel, 2);


            deleteMemberButton = new CancelButton("Apagar Sócio", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            deleteMemberButton.VerticalOptions = LayoutOptions.End;
            //absoluteLayout.SetLayoutBounds(deleteMemberButton, new Rect(10 * App.screenWidthAdapter, 0 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

            deleteMemberButton.button.Clicked += OndeleteMemberButtonClicked;

            //grid.Add(deleteMemberButton, 0, 5);
            //Microsoft.Maui.Controls.Grid.SetColumnSpan(deleteMemberButton, 2);

			absoluteLayout.Add(grid);
            absoluteLayout.SetLayoutBounds(grid, new Rect(10 * App.screenWidthAdapter, 0 * App.screenHeightAdapter, App.screenWidth - 10 * App.screenWidthAdapter, App.screenHeight - 110 * App.screenHeightAdapter));

            //OnChangePasswordButtonClicked
        }

        public ChangePasswordPageCS(Member member)
		{
			this.member = member;
			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnChangePasswordButtonClicked(object sender, EventArgs e)
		{
			changePasswordButton.IsEnabled = false;
			messageLabel.TextColor = Colors.Red;
			if (newPasswordEntry.entry.Text.Length < 6) {
				messageLabel.Text = "A nova password tem de ter pelo menos 6 caracteres.";
			}
			else if (newPasswordEntry.entry.Text != newPasswordConfirmEntry.entry.Text)
			{
				messageLabel.Text = "A nova password não coincide com a password de confirmação.";
			}
			else 
			{
				MemberManager memberManager = new MemberManager();
				var user = new User
				{
					Username = member.email,
					Password = currentPasswordEntry.entry.Text
				};

				var loginResult = await memberManager.Login(user);

				if (loginResult == "1")
				{
					Debug.WriteLine("password ok");
					int changePasswordResult = await ChangePassword(member.email, newPasswordEntry.entry.Text);
					if (changePasswordResult == 1)
					{
						messageLabel.TextColor = Colors.Green;
						messageLabel.Text = "A password foi alterada com sucesso.";
						
					}
					else
                    {
						messageLabel.Text = "A alteração de Password falhou. Tente novamente mais tarde ou contacte o seu instrutor.";
					}
				}
				else
				{
					Debug.WriteLine("password nok");
					changePasswordButton.IsEnabled = true;
					currentPasswordEntry.entry.Text = string.Empty;
					newPasswordEntry.entry.Text = string.Empty;
					newPasswordConfirmEntry.entry.Text = string.Empty;

					if (loginResult == "0")
					{
						messageLabel.Text = "A alteração de Password falhou. O Utilizador não existe."; //isto não pode acontecer...
					}
					else if (loginResult == "-1")
					{
						messageLabel.Text = "A alteração de Password falhou. A Password atual está incorreta.";
					}
					else
					{
						messageLabel.Text = "Ocorreu um erro. Volte a tentar mais tarde.";
					}
				}
			}
			changePasswordButton.IsEnabled = true;

			/*Application.Current.MainPage = new NavigationPage(new LoginPageCS())
			{
				BarBackgroundColor = App.backgroundColor,
				BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
			};
			//_ = Navigation.PushModalAsync(new LoginPageCS());*/

		}

        async void OndeleteMemberButtonClicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Apagar Membro?", "Tem a certeza que pretende apagar a sua conta e todos os dados associados? \nATENÇÃO: Esta acçao é irreversível!", "Sim", "Não");

            if (res == true)
            {
                MemberManager memberManager = new MemberManager();
				string result = await memberManager.Update_Member_Approved_Status(App.member.id, "", "", "inactivo", "");
                App.Current.MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
            }
        }

        async Task<int> ChangePassword(string email, string newpassword)
		{
			Debug.WriteLine("ChangePassword");
			MemberManager memberManager = new MemberManager();

			int result = await memberManager.ChangePassword(email, newpassword);

            LogManager logManager = new LogManager();
            await logManager.writeLog(App.original_member.id, App.member.id, "CHANGE PASSWORD", "Change Password");

            return result;
			
		}

	}
}