using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Services.Data.JSON;
//Ausing Acr.UserDialogs;
using System.Globalization;

namespace SportNow.Views.CompleteRegistration
{
	public class PaymentPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}


		private ScrollView scrollView;

		//private Member member;


		FormValue valueJoia, valueCartao, valueQuota, valueTotal;
        Fee fee_joia, fee_cartao, fee_quota;
        FormValueEditPicker quotaPeriodPickerValue;
        public double valorQuota;

        string paymentID;
		Payment payment;

		bool paymentDetected;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{
			showActivityIndicator();
			MemberManager memberManager = new MemberManager();
			string season = DateTime.Now.ToString("yyyy");

            Grid gridPayment = new Grid { Padding = 10, RowSpacing = 5 * App.screenHeightAdapter, ColumnSpacing = 5 * App.screenWidthAdapter };

            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 * App.screenWidthAdapter });


            Grid gridPaymentOptions = new Grid { Padding = 10, RowSpacing = 50 * App.screenHeightAdapter };
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            //TENHO DE METER AQUI VALIDAÇÕES!!!
            //
            string paymentID = await memberManager.CreateAllFees(App.original_member.id, App.member.id, App.member.name, "3");

            
            PaymentManager paymentManager = new PaymentManager();
            payment = await paymentManager.GetPayment(paymentID);
            List<Fee> fees = await paymentManager.GetPaymentFees(paymentID);
            hideActivityIndicator();

            fee_joia = null;
			fee_cartao = null;
			fee_quota = null;

            foreach (Fee fee in fees)
			{
				if (fee.tipo == "joia")
				{
					fee_joia = fee;

                }
                else if (fee.tipo == "cartao")
				{
					fee_cartao = fee;
				}
                else if (fee.tipo == "quota")
                {
					fee_quota = fee;
                }
            }

            paymentID = payment.id;

            Label labelJoia = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelJoia.Text = "Jóia";
			
            valueJoia = new FormValue(String.Format("{0:0.00}", fee_joia.valor) + "€", App.titleFontSize, Colors.White, App.normalTextColor, TextAlignment.End);;

            Label labelCartao = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelCartao.Text = "Cartão de Sócio";

            valueCartao = new FormValue(String.Format("{0:0.00}", fee_cartao.valor) + "€", App.titleFontSize, Colors.White, App.normalTextColor, TextAlignment.End);

            Label labelQuotas = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelQuotas.Text = "Quotas de Sócio";

            string quotaPeriodtring = "";
            List<string> quotaPeriodList = new List<string>();
            foreach (KeyValuePair<string, string> entry in Constants.quotaPeriod)
            {
                quotaPeriodList.Add(entry.Value);
                /*if ( == entry.Key)
                {
                    quotaPeriodtring = entry.Value;
                }*/
            }
            quotaPeriodPickerValue = new FormValueEditPicker(quotaPeriodtring, quotaPeriodList);

            quotaPeriodPickerValue.picker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
				quotaPeriodPickerValue.picker.Unfocus();
                showActivityIndicator();
				string durationText = quotaPeriodPickerValue.picker.SelectedItem.ToString();
				string[] durationArray = durationText.Split();
                string durationNumber = durationArray[0];

				string res = await memberManager.UpdateFeeDuration(fee_quota.id, paymentID, durationNumber);
				fee_quota = await memberManager.GetFee(fee_quota.id);
                payment = await paymentManager.GetPayment(paymentID);

				Debug.Print("Aqui payment.value = "+payment.value);

                valueQuota.label.Text = String.Format("{0:0.00}", fee_quota.valor) + "€";

                valueTotal.label.Text = calculateTotal(0).ToString("0.00") + "€";
                hideActivityIndicator();

            };

            valueQuota = new FormValue(String.Format("{0:0.00}", fee_quota.valor) + "€", App.titleFontSize, Colors.White, App.normalTextColor, TextAlignment.End);
            //valueQuota.label.Text = "fee_cartao.valor;

            Label labelTotal = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			labelTotal.Text = "TOTAL";

			valueTotal = new FormValue(calculateTotal(0).ToString("0.00") + "€", App.titleFontSize, App.backgroundColor, App.topColor, TextAlignment.End);

            Label selectPaymentModeLabel = new Label
			{
				Text = "Escolha o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				MinimumHeightRequest = 120 * App.screenHeightAdapter,
				HeightRequest = 120 * App.screenHeightAdapter,
			};

			var tapGestureRecognizerMB = new TapGestureRecognizer();
			tapGestureRecognizerMB.Tapped += OnMBButtonClicked;
			MBLogoImage.GestureRecognizers.Add(tapGestureRecognizerMB);

            Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				MinimumHeightRequest = 120 * App.screenHeightAdapter,
				HeightRequest = 120 * App.screenHeightAdapter
			};


			var tapGestureRecognizerMBWay = new TapGestureRecognizer();
			tapGestureRecognizerMBWay.Tapped += OnMBWayButtonClicked;
			MBWayLogoImage.GestureRecognizers.Add(tapGestureRecognizerMBWay);

            gridPayment.Add(labelJoia, 0, 0);
            gridPayment.Add(valueJoia, 2, 0);
            Grid.SetColumnSpan(labelJoia, 2);

            gridPayment.Add(labelCartao, 0, 1);
            gridPayment.Add(valueCartao, 2, 1);
            Grid.SetColumnSpan(labelCartao, 2);

            gridPayment.Add(labelQuotas, 0, 2);
            gridPayment.Add(quotaPeriodPickerValue, 1, 2);
            gridPayment.Add(valueQuota, 2, 2);

            gridPayment.Add(labelTotal, 0, 3);
            gridPayment.Add(valueTotal, 2, 3);
            Grid.SetColumnSpan(labelCartao, 2);

            gridPaymentOptions.Add(selectPaymentModeLabel, 0, 0);
            Grid.SetColumnSpan(selectPaymentModeLabel, 2);

            gridPaymentOptions.Add(MBLogoImage, 0, 1);
            gridPaymentOptions.Add(MBWayLogoImage, 1, 1);

            gridPayment.Add(gridPaymentOptions, 0, 4);
            Grid.SetColumnSpan(gridPaymentOptions, 3);

            absoluteLayout.Add(gridPayment);
            absoluteLayout.SetLayoutBounds(gridPayment, new Rect(0 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 -10 * App.screenHeightAdapter));

        }



        public double getValorQuota(List<Fee> allFees, string tipoQuota)
		{
			foreach (Fee fee in allFees)
			{
				if (fee.tipo == tipoQuota)
				{
					return Convert.ToDouble(fee.valor);
				}
			}
			return 0;
		}

		public int CreateHeader()
		{
			int y_index = 10;

			Label nameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = App.member.nickname,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Colors.Gray,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

            absoluteLayout.Add(nameLabel);
            absoluteLayout.SetLayoutBounds(nameLabel, new Rect(10 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

			return y_index + 30;
		}

		public PaymentPageCS()
		{

			this.initLayout();
			this.initSpecificLayout();

			paymentDetected = false;

			int sleepTime = 5;
			Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
			{
				if ((paymentID != null) & (paymentID != ""))
				{
					this.checkPaymentStatus(paymentID);
					if (paymentDetected == false)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			});
		}

		async void checkPaymentStatus(string paymentID)
		{
			Debug.Print("checkPaymentStatus");
			this.payment = await GetPayment(paymentID);
			if ((payment.status == "confirmado") | (payment.status == "fechado") | (payment.status == "recebido"))
			{
				App.member.estado = "activo";
				App.original_member.estado = "activo";

				if (paymentDetected == false)
				{
					paymentDetected = true;

					await DisplayAlert("Pagamento Confirmado", "O seu pagamento foi recebido com sucesso. Já pode aceder à nossa App!", "Ok");
					App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };

                }
			}
		}

		async Task<Payment> GetPayment(string paymentID)
		{
			Debug.WriteLine("GetPayment");
			PaymentManager paymentManager = new PaymentManager();

			Payment payment = await paymentManager.GetPayment(this.paymentID);

			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };
                return null;
			}
			return payment;
		}


		public double calculateTotal(double desconto)
		{
			//return valorQuota;
			return fee_joia.valor + fee_cartao.valor + fee_quota.valor;
		}

		async void OnMBButtonClicked(object sender, EventArgs e)
		{
            Debug.Print("AQUI 1 paymentID = " + payment.id);
            await Navigation.PushAsync(new PaymentMBPageCS(payment));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
            Debug.Print("AQUI 2 paymentID = " + payment.id);
            await Navigation.PushAsync(new PaymentMBWayPageCS(payment));
		}

	}

}