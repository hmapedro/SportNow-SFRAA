using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.ViewModel;
using Microsoft.Maui;


namespace SportNow.Views
{
	public class GradeProgramDetailPageCS : DefaultPage
	{

		protected override void OnDisappearing() {
			//collectionViewExaminations.SelectedItem = null;
		}

		Examination_Program examination_Program;


		public void initLayout()
		{
			Title = "PROGRAMA EXAME";
		}

		public void CleanProgramasExameCollectionView()
		{
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateProgramasExameDetail();

            hideActivityIndicator();
		}

		public async void CreateProgramasExameDetail()
		{

			int techniqueTypeIndex = 0;
            int techniqueTypeIndexHeader = 0;

            Microsoft.Maui.Controls.Grid grid = new Microsoft.Maui.Controls.Grid { Padding = 10 };
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 80 * App.screenWidthAdapter });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 200 * App.screenWidthAdapter });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			Image image = new Image { Source = examination_Program.image, Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 80 * App.screenWidthAdapter };

			Label gradeLabel = new Label { Text = examination_Program.examinationTo_string, FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, TextDecorations = TextDecorations.Underline, LineBreakMode = LineBreakMode.NoWrap, FontSize = App.titleFontSize };


            /*Image youtubeImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60, Source = "youtube.png" };
            youtubeImage.SetBinding(Image.AutomationIdProperty, "video");

            var youtubeImage_tap = new TapGestureRecognizer();
            youtubeImage_tap.Tapped += async (s, e) =>
            {
                try
                {
					Debug.Print("Open Youtube video "+ ((Image)s).AutomationId);
                    //await Browser.OpenAsync(((Image)s).AutomationId, BrowserLaunchMode.SystemPreferred);
                    await Browser.OpenAsync("https://www.ippon.pt", BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                }
            };
            youtubeImage.GestureRecognizers.Add(youtubeImage_tap);*/

            grid.Add(image, 0, 0);
            grid.Add(gradeLabel, 1, 0);
            //grid.Add(youtubeImage, 2, 0);

            Microsoft.Maui.Controls.Grid gridDetail = new Microsoft.Maui.Controls.Grid { Padding = 10 };
            gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridDetail.ColumnDefinitions.Add(new ColumnDefinition { Width = WidthRequest = App.screenWidth });

            techniqueTypeIndex = createTechniqueLabels(gridDetail, "kihon", "KIHON", techniqueTypeIndex);
            techniqueTypeIndex = createTechniqueLabels(gridDetail, "kata", "KATA", techniqueTypeIndex);
            techniqueTypeIndex = createTechniqueLabels(gridDetail, "kumite", "KUMITE", techniqueTypeIndex);
            techniqueTypeIndex = createTechniqueLabels(gridDetail, "atletico", "ATLÉTICO", techniqueTypeIndex);
            techniqueTypeIndex = createTechniqueLabels(gridDetail, "flexibilidade", "FLEXIBILIDADE", techniqueTypeIndex);

            /*Label kihonHeaderLabel, kihonLabel, kataHeaderLabel, kataLabel, kumiteHeaderLabel, kumiteLabel, atleticoHeaderLabel, atleticoLabel, flexibilidadeHeaderLabel, flexibilidadeLabel, youtubeLabel;//, flexibilidadeLabel;

            bool existsKihon = false;
            techniqueTypeIndexHeader = techniqueTypeIndex;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == "kihon")
                {
                    if (existsKihon == false)
                    {
                        existsKihon = true;
                        techniqueTypeIndex++;
                    }
                    
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    kihonLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    kihonLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(kihonLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(kihonLabel, 2);
                    techniqueTypeIndex++;
                }
            }

            if (existsKihon == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                kihonHeaderLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                kihonHeaderLabel.Text = "KIHON";
                gridDetail.Add(kihonHeaderLabel, 0, techniqueTypeIndexHeader);
            }

            techniqueTypeIndexHeader = techniqueTypeIndex;
            bool existsKata = false;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == "kata")
                {
                    if (existsKata == false)
                    {
                        existsKata = true;
                        techniqueTypeIndex++;
                    }
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    kataLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    kataLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(kataLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(kataLabel, 2);
                    techniqueTypeIndex++;
                }
            }

            if (existsKata == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                kataHeaderLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                kataHeaderLabel.Text = "KATA";
                gridDetail.Add(kataHeaderLabel, 0, techniqueTypeIndexHeader);
            } 

            bool existsKumite = false;
            techniqueTypeIndexHeader = techniqueTypeIndex;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == "kumite")
                {
                    if (existsKumite == false)
                    {
                        existsKumite = true;
                        techniqueTypeIndex++;
                    };
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    kumiteLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    kumiteLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(kumiteLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(kumiteLabel, 2);
                    techniqueTypeIndex++;
                }
            }

            if (existsKumite == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                kumiteHeaderLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                kumiteHeaderLabel.Text = "KUMITE";
                gridDetail.Add(kumiteHeaderLabel, 0, techniqueTypeIndexHeader);
            }

            bool existsAtletico= false;
            techniqueTypeIndexHeader = techniqueTypeIndex;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == "atletico")
                {
                    if (existsAtletico == false)
                    {
                        existsAtletico = true;
                        techniqueTypeIndex++;
                    };
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    atleticoLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    atleticoLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(atleticoLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(atleticoLabel, 2);
                    techniqueTypeIndex++;
                }
            }
            if (existsAtletico == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                atleticoHeaderLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                atleticoHeaderLabel.Text = "ATLETICO";
                gridDetail.Add(atleticoHeaderLabel, 0, techniqueTypeIndexHeader);
            }

            bool existsFlexibilidade = false;
            techniqueTypeIndexHeader = techniqueTypeIndex;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == "flexibilidade")
                {
                    if (existsFlexibilidade == false)
                    {
                        existsFlexibilidade = true;
                        techniqueTypeIndex++;
                    };
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    flexibilidadeLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    flexibilidadeLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(flexibilidadeLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(flexibilidadeLabel, 2);

                    TapGestureRecognizer flexibilidadeLabel_tap = new TapGestureRecognizer();
                    flexibilidadeLabel_tap.Tapped += async (s, e) =>
                    {
                        try
                        {
                            Debug.Print("Open Youtube video " + examination_Technique.video);
                            await Browser.OpenAsync(examination_Technique.video, BrowserLaunchMode.SystemPreferred);
                            //await Browser.OpenAsync("https://www.ippon.pt", BrowserLaunchMode.SystemPreferred);
                        }
                        catch (Exception ex)
                        {
                        }
                    };
                    flexibilidadeLabel.GestureRecognizers.Add(flexibilidadeLabel_tap);

                    techniqueTypeIndex++;
                }
            }

            if (existsFlexibilidade == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                flexibilidadeHeaderLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                flexibilidadeHeaderLabel.Text = "FLEXIBILIDADE";
                gridDetail.Add(flexibilidadeHeaderLabel, 0, techniqueTypeIndexHeader);
            }*/


            /*Image youtubeImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60, Source = "youtube.png" };

                   var youtubeImage_tap = new TapGestureRecognizer();
                   youtubeImage_tap.Tapped += async (s, e) =>
                   {
                       try
                       {
                           Debug.Print("Open Youtube video " + examination_Technique.video);
                           await Browser.OpenAsync(examination_Technique.video, BrowserLaunchMode.SystemPreferred);
                           //await Browser.OpenAsync("https://www.ippon.pt", BrowserLaunchMode.SystemPreferred);
                       }
                       catch (Exception ex)
                       {
                       }
                   };
                   flexibilidadeLabel.GestureRecognizers.Add(youtubeImage_tap);

                   gridDetail.Add(youtubeImage, 2, i);*/
            /*flexibilidadeLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
            flexibilidadeLabel.Text = examination_Program.flexibilidadeText;
            gridDetail.Add(flexibilidadeLabel, 0, 9);
            Microsoft.Maui.Controls.Grid.SetColumnSpan(flexibilidadeLabel, 3);*/

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.Add(gridDetail, 0, 1);
			grid.SetColumnSpan(gridDetail, 2);

			absoluteLayout.Add(grid);
            absoluteLayout.SetLayoutBounds(grid, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 160 * App.screenHeightAdapter));
        }


        public int createTechniqueLabels(Grid gridDetail, string techniqueType, string techniqueTypeText, int techniqueTypeIndex)
        {

            Label headerLabel, techniqueLabel;

            bool existsFlexibilidade = false;
            int techniqueTypeIndexHeader = techniqueTypeIndex;
            foreach (Examination_Technique examination_Technique in examination_Program.examination_techniques)
            {
                if (examination_Technique.type == techniqueType)
                {
                    if (existsFlexibilidade == false)
                    {
                        existsFlexibilidade = true;
                        techniqueTypeIndex++;
                    };
                    gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    techniqueLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                    techniqueLabel.Text = examination_Technique.order + " - " + examination_Technique.name;
                    gridDetail.Add(techniqueLabel, 0, techniqueTypeIndex);
                    Microsoft.Maui.Controls.Grid.SetColumnSpan(techniqueLabel, 2);

                    TapGestureRecognizer techniqueLabel_tap = new TapGestureRecognizer();
                    techniqueLabel_tap.Tapped += async (s, e) =>
                    {
                        try
                        {
                            Debug.Print("Open Youtube video " + examination_Technique.video);
                            await Browser.OpenAsync(examination_Technique.video, BrowserLaunchMode.SystemPreferred);
                            //await Browser.OpenAsync("https://www.ippon.pt", BrowserLaunchMode.SystemPreferred);
                        }
                        catch (Exception ex)
                        {
                        }
                    };
                    techniqueLabel.GestureRecognizers.Add(techniqueLabel_tap);

                    techniqueTypeIndex++;
                }
            }

            if (existsFlexibilidade == true)
            {
                gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                headerLabel = new Label { FontFamily = "futuracondensedmedium", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
                headerLabel.Text = techniqueTypeText;
                gridDetail.Add(headerLabel, 0, techniqueTypeIndexHeader);
            }
            return techniqueTypeIndex;
        }
			
		public GradeProgramDetailPageCS(Examination_Program examination_Program)
		{
			this.examination_Program = examination_Program;

            this.initLayout();
			this.initSpecificLayout();

			//Parent.

		}
	}
}

