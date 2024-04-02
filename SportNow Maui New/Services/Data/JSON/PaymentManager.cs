using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;
using System.Collections.ObjectModel;

namespace SportNow.Services.Data.JSON
{
	public class PaymentManager
	{
		//IRestService restService;

		HttpClient client;
		

		public PaymentManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		/*public async Task<List<Class_Attendance>> Get_Invoice_byID(string invoiceid)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Invoice_byID + "?invoiceid=" + invoiceid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_attendances = JsonConvert.DeserializeObject<List<Class_Attendance>>(content);
				}
				return class_attendances;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}*/

		public async Task<string> CreateMbWayPayment(string memberid, string paymentid, string orderid, string phonenumber, string value, string email)
		{
			Debug.WriteLine("CreateMbWayPayment begin "+ Constants.RestUrl_Create_MbWay_Payment + "?userid=" + memberid + "&paymentid=" + paymentid + "&phonenumber=" + phonenumber + "&value=" + value + "&email=" + email + "&orderid=" + orderid);
			Debug.WriteLine("paymentid = "+ paymentid);
			Debug.WriteLine("phonenumber = " + phonenumber);
			Debug.WriteLine("value = " + value);
			Debug.WriteLine("email = " + email);
			Debug.WriteLine("orderid = " + orderid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_MbWay_Payment + "?userid=" + memberid + "&paymentid=" + paymentid + "&phonenumber=" + phonenumber + "&value=" + value + "&email=" + email + "&orderid=" + orderid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);
				string content = await response.Content.ReadAsStringAsync();
				Debug.WriteLine("content=" + content);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					//string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return createResultList[0].result;

				}
				else
				{
					Debug.WriteLine("error creating payment MBWay");
					return "-2";
				}

			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-3";
			}
		}

        public async Task<Payment> GetPayment(string paymentid)
        {
            Debug.WriteLine("GetPayment " + Constants.RestUrl_Get_Payment + "?pagamentoid=" + paymentid);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Payment + "?pagamentoid=" + paymentid, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    List<Payment> paymentTemp = JsonConvert.DeserializeObject<List<Payment>>(content);
                    return paymentTemp[0];
                }
                else
                {
                    Debug.WriteLine("error getting fees");
                    return null;
                }

                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("PaymentManager.GetPayment http request error " + e.ToString());
                return null;
            }
        }


        public async Task<string> Update_Payment_Status(string paymentid, string status)
        {
            Debug.WriteLine("Update_Payment_Status begin " + Constants.RestUrl_Update_Payment_Status + "?paymentid=" + paymentid + "&status=" + status);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Payment_Status + "?paymentid=" + paymentid + "&status=" + status, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var result = "0";
                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("content=" + content);
                    result = "1";
                }
                else
                {
                    Debug.WriteLine("error updating payment status");
                    result = "-1";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("http request error");
                return "-2";
            }
        }

        public async Task<string> Update_Payment_Mode(string paymentid, string modo_pagamento)
        {
            Debug.WriteLine("Update_Payment_Status begin " + Constants.RestUrl_Update_Payment_Mode + "?paymentid=" + paymentid + "&modo_pagamento=" + modo_pagamento);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Payment_Mode + "?paymentid=" + paymentid + "&modo_pagamento=" + modo_pagamento, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var result = "0";
                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("content=" + content);
                    result = "1";
                }
                else
                {
                    Debug.WriteLine("error updating payment status");
                    result = "-1";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("http request error");
                return "-2";
            }
        }


        public async Task<string> Update_Payment(string paymentid, string memberid, string dojoid, string name)
        {
            Debug.WriteLine("Update_Payment_Name begin " + Constants.RestUrl_Update_Payment + "?paymentid=" + paymentid + "&memberid=" + memberid + "&dojoid=" + dojoid + "&name=" + name);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Payment + "?paymentid=" + paymentid + "&memberid=" + memberid + "&dojoid=" + dojoid + "&name=" + name, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var result = "0";
                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Update_Payment_Name content=" + content);
                    result = "1";
                }
                else
                {
                    Debug.WriteLine("Update_Payment_Name - error updating payment name");
                    result = "-1";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("Update_Payment_Name - http request error");
                return "-2";
            }
        }


    }
}