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
	public class ServicesManager
    {
		//IRestService restService;

		HttpClient client;

		public List<Service> services { get; private set; }
		

		public ServicesManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Service>> GetServices()
		{
            Debug.Print("GetCycles - " + Constants.RestUrl_Get_Services);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Services, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
                    Debug.Print("GetCycles content=" + content);
                    services = JsonConvert.DeserializeObject<List<Service>>(content);
				}
				return services;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

		}

    }
}