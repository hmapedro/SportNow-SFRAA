using SportNow.Model;
using SportNow.ViewModel;
using System.Collections.ObjectModel;


namespace SportNow.Views
{
	public class ExaminationEvaluationConfirmPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
		}

		private CollectionView collectionViewExaminationSessionCall;

		ObservableCollection<Examination> examinations;
		private ExaminationCollection examinationCollection;

		Examination_Session examination_session;


        int currentExaminationIndex = 0;

		public void initLayout()
		{
			Title = "CONFIRMAÇÃO EXAMES";
		}


		public async void initSpecificLayout()
		{
			cleanExaminations();

			examinationCollection = new ExaminationCollection();
			examinationCollection.Items = examinations;

			createExaminationsEvaluation();
		}

		public void cleanExaminations()
		{

			ObservableCollection<Examination> examinations_new = new ObservableCollection<Examination>();
			foreach (Examination examination_i in examinations)
			{
				if (examination_i.selected == true)
				{
					examinations_new.Add(examination_i);
				}
			}
			this.examinations = examinations_new;
		}

		public async void createExaminationsEvaluation()
		{

			collectionViewExaminationSessionCall = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = examination_sessionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não foi criada convocatória para esta Sessão de Exames.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Colors.Red, FontSize = 20 },
			}
					}
				}
			};

			this.BindingContext = examinationCollection;
			collectionViewExaminationSessionCall.SetBinding(ItemsView.ItemsSourceProperty, "Items");

			//collectionViewExaminationSessionCall.SelectionChanged += OncollectionViewExaminationSessionCallSelectionChanged;

			collectionViewExaminationSessionCall.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3),
					HeightRequest = 80
				};

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membername");
				//nameLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame nameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Colors.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemabsoluteLayout.Add(nameFrame);
				itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 0, App.screenWidth, 40 * App.screenHeightAdapter));

				Label gradeTypeLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				gradeTypeLabel.SetBinding(Label.TextProperty, "typeLabel");
				//gradeTypeLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame typeFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Colors.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				typeFrame.Content = gradeTypeLabel;

				itemabsoluteLayout.Add(typeFrame);
				itemabsoluteLayout.SetLayoutBounds(typeFrame, new Rect(0, 45 * App.screenHeightAdapter, App.screenWidth/2, 40 * App.screenHeightAdapter));

				Label gradeLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				gradeTypeLabel.SetBinding(Label.TextProperty, "gradeLabel");
				//gradeTypeLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame gradeFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Colors.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				gradeFrame.Content = gradeLabel;

				itemabsoluteLayout.Add(gradeFrame);
				itemabsoluteLayout.SetLayoutBounds(gradeFrame, new Rect(0, 45 * App.screenHeightAdapter, App.screenWidth / 2, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

			absoluteLayout.Add(collectionViewExaminationSessionCall);
            absoluteLayout.SetLayoutBounds(collectionViewExaminationSessionCall, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 170 * App.screenHeightAdapter));

			/*

			examinationMemberNameLabel = new Label
			{
				Text = examinations[currentExaminationIndex].membername,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.normalTextColor
			};

			absoluteLayout.Add(examinationMemberNameLabel,
				xConstraint: )0),
				yConstraint: )0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);


			var grade_typePicker = new Picker
			{
				Title = "",
				TitleColor = Colors.White,
				BackgroundColor = Colors.Transparent,
				TextColor = Color.FromRgb(246, 220, 178),
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 20

			};

			var grade_typeList = new List<string>();


			foreach (var data in Constants.grade_type)
			{
				grade_typeList.Add(data.Value);
			}

			grade_typePicker.ItemsSource = grade_typeList;
			if (examinations[currentExaminationIndex].memberage <= 6)
            {
				grade_typePicker.SelectedIndex = 0;
			}
			else if (examinations[currentExaminationIndex].memberage <= 12)
			{
				grade_typePicker.SelectedIndex = 1;
			}
			else
			{
				grade_typePicker.SelectedIndex = 2;
			}

			grade_typePicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("grade_typePicker.SelectedItem.ToString() = " + grade_typePicker.SelectedItem.ToString());

			};


			/*examinationGradeLabel = new Label
			{
				Text = examinations[currentExaminationIndex].gradeLabel,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.normalTextColor
			};*/

			/*absoluteLayout.Add(grade_typePicker,
				xConstraint: )0),
				yConstraint: )60),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width/2); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);

			examinationTypeLabel = new Label
			{
				Text = examinations[currentExaminationIndex].grade,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.normalTextColor
			};

			absoluteLayout.Add(examinationTypeLabel,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2); // center of image (which is 40 wide)
				}),
				yConstraint: )60),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);*/

		}
		

		public ExaminationEvaluationConfirmPageCS(Examination_Session examination_session, ObservableCollection<Examination> examinations)
		{
			this.examinations = examinations;
			this.examination_session = examination_session;
			this.initLayout();
			//this.initSpecificLayout();

		}

	}
}
