using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Xml;

namespace SportNow.Views.Profile
{
    public class ApproveRegistrationPageCS : DefaultPage
    {

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{	
		}

        private CollectionView collectionViewMembers;
		List<Member> members_To_Approve;
		Label titleLabel;

		Member new_member;
		List<Class_Detail> allClasses;

		Frame frameClassPicker;
		Label classPickerTitleLabel;
		Picker classPicker;
		CollectionView classesCollectionView;

        CancelButton cancelButton;

		AbsoluteLayout absoluteLayout_Classes;


        public void initLayout()
		{
			Title = "APROVAR INSCRIÇÕES";
		}

		public async void initSpecificLayout()
		{
			App.AdaptScreen();
			showActivityIndicator();

            members_To_Approve = await GetMembers_To_Approve();

			Debug.Print("SelectMemberPageCS.initSpecificLayout App.titleFontSize = " + App.titleFontSize);

			titleLabel = new Label { FontFamily = "futuracondensedmedium", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			if (members_To_Approve.Count == 0)
			{
				titleLabel.Text = "Não tem novos sócios para aprovar.";
			}
			else
			{
				titleLabel.Text = "Tem os seguintes novos sócios para aprovar:";
			}

			absoluteLayout.Add(titleLabel);
            absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, 60 * App.screenHeightAdapter));

			CreateMembersColletion();
			hideActivityIndicator();
        }

		public void CreateMembersColletion()
		{

			Debug.Print("SelectMemberPageCS.CreateMembersColletion");
			//COLLECTION GRADUACOES
			collectionViewMembers = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = members_To_Approve,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter},
				
			};

			collectionViewMembers.SelectionChanged += OnCollectionViewMembersSelectionChanged;

			collectionViewMembers.ItemTemplate = new DataTemplate(() =>
			{

				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = 45 * App.screenHeightAdapter,
					WidthRequest = App.screenWidth// - 20 * App.screenWidthAdapter
                };

				FormValue numberLabel = new FormValue("");
				numberLabel.label.SetBinding(Label.TextProperty, "number_member");


				itemabsoluteLayout.Add(numberLabel);
				itemabsoluteLayout.SetLayoutBounds(numberLabel, new Rect(0, 0, 50 * App.screenWidthAdapter, 45 * App.screenHeightAdapter));
				
				FormValue nicknameLabel = new FormValue("");
				nicknameLabel.label.SetBinding(Label.TextProperty, "nickname");
				
				itemabsoluteLayout.Add(nicknameLabel);
				itemabsoluteLayout.SetLayoutBounds(nicknameLabel, new Rect(55 * App.screenWidthAdapter, 0, (App.screenWidth - (185 * App.screenWidthAdapter)), 45 * App.screenHeightAdapter));

				FormValue birthdateLabel = new FormValue("");
                birthdateLabel.label.SetBinding(Label.TextProperty, "birthdate");

				itemabsoluteLayout.Add(birthdateLabel);
				itemabsoluteLayout.SetLayoutBounds(birthdateLabel, new Rect(App.screenWidth - (125 * App.screenWidthAdapter), 0, 100 * App.screenWidthAdapter, 45 * App.screenHeightAdapter));
				
				return itemabsoluteLayout;
			});

            absoluteLayout.Add(collectionViewMembers);
            absoluteLayout.SetLayoutBounds(collectionViewMembers, new Rect(10 * App.screenWidthAdapter, 90 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenWidthAdapter), App.screenHeight - 90 * App.screenHeightAdapter));
		}

		public async void createClassPicker(string membername, string dojoid)
		{
            absoluteLayout_Classes = new AbsoluteLayout()
            {
                BackgroundColor = App.backgroundColor,
			};

			absoluteLayout.Add(absoluteLayout_Classes);
            absoluteLayout.SetLayoutBounds(absoluteLayout_Classes, new Rect(0, 0, App.screenWidth, App.screenHeight));

            ClassManager classManager = new ClassManager();
			showActivityIndicator();
            allClasses = await classManager.GetAllClasses(dojoid);
			CompleteClass_Detail();
			hideActivityIndicator();

			classPickerTitleLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Escolhe a Classe a que o novo Sócio " + membername +" vai pertencer",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				LineBreakMode = LineBreakMode.WordWrap
			};

			absoluteLayout_Classes.Add(classPickerTitleLabel);
            absoluteLayout_Classes.SetLayoutBounds(classPickerTitleLabel, new Rect(30 * App.screenHeightAdapter, 10 * App.screenHeightAdapter, App.screenWidth - (60 * App.screenWidthAdapter), 60 * App.screenHeightAdapter));
            

			CreateClassesColletion();

			cancelButton = new CancelButton("CANCELAR", 100, 50);
            cancelButton.button.Clicked += OnCancelButtonClicked;

            absoluteLayout_Classes.Add(cancelButton);
            absoluteLayout_Classes.SetLayoutBounds(cancelButton, new Rect(0, App.screenHeight - 60 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));
            
        }

		public void CompleteClass_Detail()
		{
			foreach (Class_Detail class_detail in allClasses)
			{
				if (class_detail.imagesource == null)
				{
					class_detail.imagesourceObject = "logo_login" + ".png";
                    Debug.Print("image = " + "logo_login" + ".png");
                }
				else
				{
                    Debug.Print("image = " + Constants.images_URL + class_detail.id + "_imagem_c");
                    class_detail.imagesourceObject = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + class_detail.id + "_imagem_c"),
						CachingEnabled = false,
						CacheValidity = new TimeSpan(0, 0, 0, 0)
					};
				}
			}
		}

		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = allClasses,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não existem Classes.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			//classesCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;
			classesCollectionView.SelectionChanged += confirmClassSelectionChanged;

			classesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
                {
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float)App.screenWidthAdapter,
					IsClippedToBounds = true,
					BorderColor = Colors.Transparent,
					BackgroundColor = Colors.Black,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

				itemabsoluteLayout.Add(itemFrame);

				itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth - 5 * App.screenWidthAdapter, App.ItemHeight));

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20 * App.screenWidthAdapter, TextColor = App.normalTextColor };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));


                

                return itemabsoluteLayout;
			});
			absoluteLayout_Classes.Add(classesCollectionView);
            absoluteLayout_Classes.SetLayoutBounds(classesCollectionView, new Rect(0, 80 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - (180 * App.screenWidthAdapter)));

		}

		public ApproveRegistrationPageCS()
		{
			this.initLayout();
			this.initSpecificLayout();
		}


		public async Task<List<Member>> GetMembers_To_Approve()
		{
			Debug.WriteLine("GetMembers_To_Approve");

			MemberManager memberManager = new MemberManager();

			List<Member> members;

			members = await memberManager.GetMembers_To_Approve();

			return members;

		}

		async void OnCollectionViewMembersSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("ApproveRegistrationPageCS.OnCollectionViewMembersSelectionChanged");

			this.new_member = (sender as CollectionView).SelectedItem as Member;
			//Debug.Print("this.new_member nickname AQUI!!! = " + new_member.nickname);

            if (collectionViewMembers.SelectedItem != null)
            {
				var actionSheet = await DisplayActionSheet("Aprovar novo Sócio " + new_member.nickname, "Cancel", null, "Aprovar", "Rejeitar");

				string result = "";
				switch (actionSheet)
				{
					case "Cancel":
						break;
					case "Aprovar":
                        //_ = await Update_Member_Status(this.new_member.id, this.new_member.nickname, this.new_member.email, "aprovado", null);
                        if (new_member.member_type == "praticante")
						{
							createClassPicker(new_member.nickname, new_member.dojoid);
						}
						else
						{
							_ = await Update_Member_Status(this.new_member.id, this.new_member.nickname, this.new_member.email, "activo", null);
                        }
						break;
					case "Rejeitar":
						showActivityIndicator();

                        MemberManager memberManager = new MemberManager();
						result = await memberManager.Update_Member_Approved_Status(new_member.id, App.member.nickname, new_member.email, "rejeitado", null);
						collectionViewMembers.SelectedItem = null;
						collectionViewMembers.ItemsSource = null;

						members_To_Approve = await GetMembers_To_Approve();

						if (members_To_Approve.Count == 0)
						{
							titleLabel.Text = "Não tem novos sócios para aprovar.";
						}
						else
						{
							titleLabel.Text = "Tem os seguintes novos sócios para aprovar:";
						}

						collectionViewMembers.ItemsSource = members_To_Approve;

                        collectionViewMembers.SelectedItem = null;

						hideActivityIndicator();
                        break;
				}
            }
        }

		async void confirmClassSelectionChanged(object sender, EventArgs e)
		{
			if ((sender as CollectionView).SelectedItem != null)
			{
				
				Class_Detail class_detail = (sender as CollectionView).SelectedItem as Class_Detail;
				classesCollectionView.SelectedItem = null;
				//string classId = getClassID(allClasses, class_detail.id);
				Debug.Print("classId = " + class_detail.id);
                //Debug.Print("new_member nickname = " + new_member.nickname);
                //Debug.Print("new_member = " + new_member.id);
                _ = await Update_Member_Status(new_member.id, new_member.nickname, new_member.email, "activo", class_detail.id);

				
                collectionViewMembers.SelectedItem = null;

                await Navigation.PopAsync();
            }

			
		}

		public async Task<string> Update_Member_Status(string new_member_id, string new_member_name, string new_member_email, string status, string classId)
		{
			showActivityIndicator();

			MemberManager memberManager = new MemberManager();
			string result = await memberManager.Update_Member_Approved_Status(new_member_id, new_member_name, new_member_email, status, classId);

			await DisplayAlert("Sócio Aprovado", "O Sócio " + this.new_member.nickname + " foi colocado como "+status+".", "OK");

			collectionViewMembers.SelectedItem = null;
			collectionViewMembers.ItemsSource = null;

            members_To_Approve = await GetMembers_To_Approve();
            if (members_To_Approve.Count == 0)
			{
				titleLabel.Text = "Não tem novos sócios para aprovar.";
			}
			else
			{
				titleLabel.Text = "Tem os seguintes novos sócios para aprovar:";
			}

            collectionViewMembers.ItemsSource = members_To_Approve;
			
			if (frameClassPicker != null)
            {
                absoluteLayout.Remove(frameClassPicker);
            }
            if (classPickerTitleLabel != null)
            {
                absoluteLayout.Remove(classPickerTitleLabel);
            }
            if (classesCollectionView != null)
            {
                absoluteLayout.Remove(classesCollectionView);
            }

			hideActivityIndicator();
            return result;
		}

		public string getClassID(List<Class_Detail> class_Details, string classId)
		{
			Debug.Print("getClassID begin "+ classId);
			foreach (Class_Detail class_detail in class_Details)
			{
				Debug.Print("getClassID class_detail.id = "+ class_detail.id);
				if (class_detail.id == classId)
				{
					return class_detail.id;
				}
			}
			return null;			
		}


        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            cancelButton.IsEnabled = false;
			absoluteLayout.Remove(absoluteLayout_Classes);
        }

    }

}