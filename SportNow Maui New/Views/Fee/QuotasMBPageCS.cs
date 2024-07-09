using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.Views.Profile;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class QuotasMBPageCS : DefaultPage
	{
		private Member member;

		private Microsoft.Maui.Controls.Grid gridMBPayment;

        private List<Payment> payments;

        double quotaOriginalValue = 0;

        public void initLayout()
		{
			Title = "QUOTA - PAGAMENTO MB";
			
		}


		public async void initSpecificLayout()
		{

			member = App.member;

			showActivityIndicator();
            payments = await GetFeePayment(this.member);

            PaymentManager paymentManager = new PaymentManager();
            await paymentManager.Update_Payment_Mode(payments[0].id, "dinheiro");

            payments = await GetFeePayment(this.member);
            quotaOriginalValue = payments[0].value;

            await paymentManager.Update_Payment_Mode(payments[0].id, "mb");

            payments = await GetFeePayment(this.member);
            hideActivityIndicator();


            createMBPaymentLayout();
		}

		public void createMBPaymentLayout() {
            gridMBPayment = new Microsoft.Maui.Controls.Grid { Padding = 10, ColumnSpacing = 20 * App.screenHeightAdapter, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 160 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label feeYearLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Para ativares a sua Quota \n " + member.currentFee.name + "\n efetua o pagamento MB com os dados apresentados em baixo:",
                VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
                WidthRequest = 164 * App.screenHeightAdapter,
                HeightRequest = 142 * App.screenHeightAdapter
            };

			Label referenciaMBLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Pagamento por\n Multibanco",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
                //LineBreakMode = LineBreakMode.NoWrap,
                HeightRequest = 142 * App.screenHeightAdapter,
                FontSize = App.bigTitleFontSize
            };

			Microsoft.Maui.Controls.Grid gridMBDataPayment = new Microsoft.Maui.Controls.Grid { Padding = 10 * App.screenWidthAdapter, ColumnSpacing = 5 * App.screenHeightAdapter,  HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

			Label entityLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Entidade:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
			};
			Label referenceLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label valueLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };

			Label entityValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = App.member.currentFee.entidade,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label referenceValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = App.member.currentFee.referencia,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };
			Label valueValue = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = String.Format("{0:0.00}", payments[0].value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = App.titleFontSize
            };

            Border MBDataFrame = new Border
            {
                BackgroundColor = App.backgroundColor,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5 * (float)App.screenHeightAdapter,
                },
                Stroke = App.topColor,
            };
            MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Add(entityLabel, 0, 0);
			gridMBDataPayment.Add(entityValue, 1, 0);
			gridMBDataPayment.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Add(referenceValue, 1, 1);
			gridMBDataPayment.Add(valueLabel, 0, 2);
			gridMBDataPayment.Add(valueValue, 1, 2);

			gridMBPayment.Add(feeYearLabel, 0, 0);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(feeYearLabel, 2);

			gridMBPayment.Add(MBLogoImage, 0, 2);
			gridMBPayment.Add(referenciaMBLabel, 1, 2);

			gridMBPayment.Add(MBDataFrame, 0, 4);
			Microsoft.Maui.Controls.Grid.SetColumnSpan(MBDataFrame, 2);

            Label labelTax = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "O valor total desta transação incluiu uma taxa de " + String.Format("{0:0.00}", payments[0].value - quotaOriginalValue) + "€",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                FontSize = App.itemTextFontSize
            };

            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 * App.screenHeightAdapter });
            gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridMBPayment.Add(labelTax, 0, 5);
            Grid.SetColumnSpan(labelTax, 2);

            absoluteLayout.Add(gridMBPayment);
            absoluteLayout.SetLayoutBounds(gridMBPayment, new Rect(0, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 10 * App.screenHeightAdapter));
        
		}

		public QuotasMBPageCS(Member member)
		{

			this.member = member;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}


        async Task<List<Payment>> GetFeePayment(Member member)
        {
            Debug.WriteLine("GetFeePayment");
            MemberManager memberManager = new MemberManager();

            payments = await memberManager.GetFeePayment(member.currentFee.id);
            Debug.Print("OLA");
            if (payments == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
                return null;
            }
            return payments;
        }

    }
}

