using System;
using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;

using Microsoft.Maui.Controls;

namespace SportNow.Views
{
    public class SelectMemberPageCS : DefaultPage
    {

        protected override void OnAppearing()
        {
            base.OnAppearing();
            initSpecificLayout();
        }

        protected override void OnDisappearing()
        {
            this.CleanScreen();
        }

        MenuButton proximosEventosButton;
        MenuButton participacoesEventosButton;

        private Microsoft.Maui.Controls.StackLayout stackButtons;

        private CollectionView collectionViewMembers, collectionViewStudents;

        //private List<Member> members;

        public void initLayout()
        {
            Debug.Print("SelectMemberPageCS.initLayout");
            Title = "ESCOLHER UTILIZADOR";
        }


        public void CleanScreen()
        {
            Debug.Print("SelectMemberPageCS.CleanScreen");
            //valida se os objetos já foram criados antes de os remover
            if (stackButtons != null)
            {
                absoluteLayout.Remove(stackButtons);
                absoluteLayout.Remove(collectionViewMembers);

                stackButtons = null;
                collectionViewMembers = null;
            }

        }

        public async void initSpecificLayout()
        {
            Debug.Print("SelectMemberPageCS.initSpecificLayout");

            Label titleLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            titleLabel.Text = "O seu email tem vários sócios associados.\n Escolhe o sócio que pretende utilizar:";


            absoluteLayout.Add(titleLabel);
            absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

            CreateMembersColletion();
        }

        public void CreateMembersColletion()
        {

            Debug.Print("SelectMemberPageCS.CreateMembersColletion");
            //COLLECTION GRADUACOES
            collectionViewMembers = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = App.members,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5, },
                EmptyView = new ContentView
                {
                    Content = new Microsoft.Maui.Controls.StackLayout
                    {
                        Children =
                            {
                                new Label { FontFamily = "futuracondensedmedium", Text = "Não tem membros associados.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            collectionViewMembers.SelectionChanged += OnCollectionViewMembersSelectionChanged;

            collectionViewMembers.ItemTemplate = new DataTemplate(() =>
            {

                AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
                    HeightRequest = 30 * App.screenHeightAdapter
                };

                FormValue numberLabel = new FormValue("", 30 * App.screenHeightAdapter);
                numberLabel.label.SetBinding(Label.TextProperty, "number_member");


                itemabsoluteLayout.Add(numberLabel);
                itemabsoluteLayout.SetLayoutBounds(numberLabel, new Rect(0, 0, 50 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                FormValue nicknameLabel = new FormValue("", 30 * App.screenHeightAdapter);
                nicknameLabel.label.SetBinding(Label.TextProperty, "nickname");


                itemabsoluteLayout.Add(nicknameLabel);
                itemabsoluteLayout.SetLayoutBounds(nicknameLabel, new Rect(55 * App.screenWidthAdapter, 0, App.screenWidth - 60 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

                return itemabsoluteLayout;
            });

            absoluteLayout.Add(collectionViewMembers);
            absoluteLayout.SetLayoutBounds(collectionViewMembers, new Rect(0, 60 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 60 * App.screenHeightAdapter));
        }


        public SelectMemberPageCS()
        {
            Debug.WriteLine("SelectMemberPageCS");
            this.initLayout();
            //this.initSpecificLayout();

        }

        async void OnCollectionViewMembersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged");

            if ((sender as CollectionView).SelectedItem != null)
            {

                Member member = (sender as CollectionView).SelectedItem as Member;

                App.member = member;
                App.original_member = member;

                saveSelectedUser(member.id);

                /*Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
				await Navigation.PopToRootAsync();*/


                App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };
                //await Navigation.PopAsync();
            }
        }

        protected void saveSelectedUser(string memberid)
        {
            Preferences.Default.Remove("SELECTEDUSER");
            Preferences.Default.Set("SELECTEDUSER", memberid);
            //Application.Current.SavePropertiesAsync();
        }
    }
}
