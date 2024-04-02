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
	public class TechnicalManager
    {
		//IRestService restService;

		HttpClient client;

		public List<Cycle> cycles { get; private set; }
		

		public TechnicalManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Cycle>> GetCycles(string classname, string type)
		{
            Debug.Print("GetCycles - " + Constants.RestUrl_Get_Cycles + "?classname="+ classname+"&type=" + type);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Cycles + "?classname="+ classname+"&type=" + type, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
                    Debug.Print("GetCycles content=" + content);
                    cycles = JsonConvert.DeserializeObject<List<Cycle>>(content);
				}
				return cycles;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

		}

        public async Task<string> Update_Cycle(string cycleid, string objectives)
        {
            Debug.Print("Update_Cycle");
            Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Cycle + "?cycleid=" + cycleid + "&objectives=" + objectives, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    List<Result> updateResultList = JsonConvert.DeserializeObject<List<Result>>(content);

                    return updateResultList[0].result;
                }
                else
                {
                    Debug.WriteLine("Update_Cycle not ok");
                    return "-2";
                }
            }
            catch
            {
                Debug.WriteLine("http request error");
                return "-2";
            }
        }

    }
}