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
        public List<Appointment> appointments { get; private set; }


        public ServicesManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

        public async Task<int> GetStudents_Service_Count(string memberid, string serviceid)
        {
            Debug.WriteLine("ServicesManager.GetStudents_Service_Count  " + Constants.RestUrl_Get_Member_Students_Service_Count + "?userid=" + memberid + "&serviceid=" + serviceid);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Member_Students_Service_Count + "?userid=" + memberid + "&serviceid=" + serviceid));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("ServicesManager.GetStudents_Service_Count - content = " + content);
                    List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

                    return Convert.ToInt32(createResultList[0].result);
                }
                else
                {
                    Debug.WriteLine("ServicesManager.GetStudents_Service_Count - login not ok");
                    return -1;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ServicesManager.GetStudents_Service_Count - http request error");
                Debug.Print(e.StackTrace);
                return -2;
            }
        }

        public async Task<List<Service>> GetServices()
		{
            Debug.Print("GetServices - " + Constants.RestUrl_Get_Services);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Services, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
                    Debug.Print("GetServices content=" + content);
                    services = JsonConvert.DeserializeObject<List<Service>>(content);
				}
				return services;
			}
			catch
			{
				Debug.WriteLine("GetServices - http request error");
				return null;
			}

		}

        public async Task<List<Service>> GetCurrentServices(string memberid)
        {
            Debug.Print("GetCurrentServices - " + Constants.RestUrl_Get_Current_Services+"?userid="+ memberid);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Current_Services + "?userid=" + memberid, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.Print("GetCurrentServices content=" + content);
                    services = JsonConvert.DeserializeObject<List<Service>>(content);
                }
                return services;
            }
            catch
            {
                Debug.WriteLine("GetCurrentServices - http request error");
                return null;
            }
        }

        public async Task<List<Appointment>> GetServiceAppointments(string memberid, string serviceid)
        {
            Debug.Print("GetServiceAppointments - " + Constants.RestUrl_Get_Service_Appointments + "?userid=" + memberid + "&serviceid=" + serviceid);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Service_Appointments + "?userid=" + memberid + "&serviceid=" + serviceid, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.Print("GetServiceAppointments content=" + content);
                    appointments = JsonConvert.DeserializeObject<List<Appointment>>(content);
                }
                return appointments;
            }
            catch
            {
                Debug.WriteLine("GetServiceAppointments - http request error");
                return null;
            }
        }


        public async Task<string> createServiceAppointment(string memberid, string serviceid, string status, string description)
        {
            Debug.Print("createAppointment " + Constants.RestUrl_Create_Service_Appointment + "?userid=" + memberid + "&serviceid=" + serviceid + "&status=" + status + "&description=" + description);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Create_Service_Appointment + "?userid=" + memberid + "&serviceid=" + serviceid + "&status=" + status + "&description=" + description, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var result = "0";

                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("content=" + content);
                    List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

                    return createResultList[0].result;
                }
                else
                {
                    Debug.WriteLine("error creating event participation");
                    result = "-1";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("http request error");
                return "-1";
            }
        }

        public async Task<string> SendMailService(string serviceid, string servicename, string responsavel, string responsavel_email, string userid, string username, string useremail, string userphone)
        {
            Debug.WriteLine("SendMailService begin " + Constants.RestUrl_SendMail_Service + "?serviceid=" + serviceid + "&servicename=" + servicename + "&responsavel=" + responsavel + "&responsavel_email=" + responsavel_email
                + "&userid=" + userid + "&username=" + username + "&useremail=" + useremail + "&userphone=" + userphone);
            Uri uri = new Uri(string.Format(Constants.RestUrl_SendMail_Service + "?serviceid=" + serviceid + "&servicename=" + servicename + "&responsavel=" + responsavel + "&responsavel_email=" + responsavel_email
                + "&userid=" + userid + "&username=" + username + "&useremail=" + useremail + "&userphone=" + userphone, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var result = "0";
                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("content=" + content);
                    List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

                    return createResultList[0].result;
                }
                else
                {
                    Debug.WriteLine("SendMailService - error sending email");
                    result = "-1";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("SendMailService - http request error");
                return "-2";
            }
        }

    }
}