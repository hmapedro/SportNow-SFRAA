using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class LogManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Event> events { get; private set; }

		public List<Event_Participation> event_participations { get; private set; }

		public List<Payment> payments { get; private set; }
		

		public LogManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Event>> writeLog(string originalmemberid, string memberid, string title, string message)
		{
			Debug.Print("writeLog " + Constants.RestUrl_Get_WriteLog + "?originalmemberid=" + originalmemberid + "&memberid=" + memberid + "&title=" + title + "&message=" + message);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_WriteLog + "?originalmemberid=" + originalmemberid + "&memberid=" + memberid + "&title=" + title + "&message=" + message, string.Empty));

            try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					events = JsonConvert.DeserializeObject<List<Event>>(content);
				}
				return events;
			}
			catch
			{
				Debug.WriteLine("writeLog - http request error");
				return null;
			}
		}
	}
}