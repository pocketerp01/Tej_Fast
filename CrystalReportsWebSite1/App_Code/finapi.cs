using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


    public class myProp
    {
        public string col1 { get; set; }
        public string col2 { get; set; }
        public string col3 { get; set; }
        public string col4 { get; set; }
        public string col5 { get; set; }
    }

    public static class finapi
    {
        public static string apiURL = "http://192.168.0.108/finapi/FinWebApisv.svc/seek_iname";
        public static string DATA = @"{""col1"":""dddd""}";

        public static void RunAsync()
        {
            //System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            //client.BaseAddress = new System.Uri(apiURL);
            //byte[] cred = UTF8Encoding.UTF8.GetBytes("username:password");
            ////client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
            //HttpResponseMessage messge = client.PostAsync(apiURL, content).Result;
            //string description = string.Empty;
            //if (messge.IsSuccessStatusCode)
            //{
            //    string result = messge.Content.ReadAsStringAsync().Result;
            //    description = result;
            //}
        }
    }
