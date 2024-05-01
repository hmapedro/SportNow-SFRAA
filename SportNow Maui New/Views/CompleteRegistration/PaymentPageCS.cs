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


		FormValue valueQuota, valueTotal;
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


            Grid gridPayment = new Grid { Padding = 10, RowSpacing = 5 * App.screenHeightAdapter };
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            //TENHO DE METER AQUI VALIDAÇÕES!!!
            //
            /*string feeID = await memberManager.CreateFee(App.original_member.id, App.member.member_type, season);

            List<Payment> payments = await memberManager.GetFeePayment(feeID);
			Payment payment = payments[0];
            paymentID = payment.id;

            PaymentManager paymentManager = new PaymentManager();
            _ = await paymentManager.Update_Payment(paymentID, App.member.id, App.member.dojoid, "Inscrição - "+App.member.name);


            string year = DateTime.Now.Year.ToString();
			string month = "";
			if (DateTime.Now.Month == 8)
			{
				month = "9";
			}
			else
			{
				month = DateTime.Now.Month.ToString();
			}

			List<Fee> allFees = await memberManager.GetFees(App.member.id, season);
			Fee fee = allFees[0];
			valorQuotaNKS = fee.valor;
			Debug.Print("fee.valor = " + fee.valor);

            monthFeeValor = 0;

            if (App.member.member_type == "praticante")
            {
                MonthFeeManager monthFeeManager = new MonthFeeManager();
                string monthFeeID = "";

                monthFeeID = await monthFeeManager.CreateMonthFee(App.original_member.id, App.member.id, App.member.name, year, month, "emitida", paymentID, "0");
                //valor_mensalidade = calculateMensalidade(0).ToString("0.00").Replace(",", ".");
                //await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, valor_mensalidade);
                MonthFee monthFee = await monthFeeManager.GetMonthFeebyId(monthFeeID);

                monthFeeValor = double.Parse(monthFee.value, System.Globalization.CultureInfo.InvariantCulture);

                Debug.Print("monthFee.value = " + monthFee.value);
                Debug.Print("monthFeeValor = " + monthFeeValor);
            }

            */
			
			Label labelQuota = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelQuota.Text = "Quota Sócio";

            valueQuota = new FormValue(valorQuota.ToString("0.00") + "€", App.titleFontSize, Colors.White, App.normalTextColor, TextAlignment.End);
            //valueQuotaADCPN.Text = calculateQuotaADCPN();

            hideActivityIndicator();
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
				FontSize = App.titleFontSize,
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

            gridPayment.Add(labelQuota, 0, 0);
            gridPayment.Add(valueQuota, 1, 0);

            gridPayment.Add(labelTotal, 0, 1);
            gridPayment.Add(valueTotal, 1, 1);

            gridPayment.Add(selectPaymentModeLabel, 0, 2);
            Grid.SetColumnSpan(selectPaymentModeLabel, 2);

            gridPayment.Add(MBLogoImage, 0, 3);
            gridPayment.Add(MBWayLogoImage, 1, 3);

			this.Content = gridPayment;
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

		public double calculateMensalidade(double desconto)
		{
			//Debug.Print("calculateMensalidade App.member.aulavalor = " + App.member.aulavalor);
			Debug.Print("calculateMensalidade desconto = " + desconto);
			Debug.Print("calculateMensalidade desconto = " + desconto);

			//Debug.Print("App.member.aulavalor = " + String.Format("{0:0}", App.member.aulavalor) + "€");
			//return String.Format("{0:0}", App.member.aulavalor) + ";
			double aulavalor = 0;// double.Parse(App.member.aulavalor, CultureInfo.InvariantCulture);
			return aulavalor * (1 - desconto);

		}

		public double calculateTotal(double desconto)
		{
			return valorQuota;
			//return calculateQuotaADCPN() + calculateFiliacaoFPG() + calculateSeguroFPG() + calculateMensalidade();
		}

		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new PaymentMBPageCS(paymentID));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new PaymentMBWayPageCS(paymentID));
		}

	}

}