using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RESTAPIConnector
{
    public interface IRESTAPI
    {
        string BaseUrl { get; set; }
        string Password { get; set; }
        string UploadUrl { get; set; }
        string UserName { get; set; }

        void Login();
        Task UploadData(string Data);
    }

    public class RESTAPI : IRESTAPI
    {
        public string BaseUrl { get; set; }
        public string UploadUrl { get; set; }
        private DateTime TokenExpirationDate { get; set; }
        private string Token { get; set; }
        private string Token_type { get; set; }
        private string expires_in { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public void Login()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var response = client.UploadString(BaseUrl + "/Token", "POST", "grant_type=password&username=" + UserName + "&password=" + Password);
                var jObject = JObject.Parse(response);
                Token = jObject.GetValue("access_token").ToString();
                Token_type = jObject.GetValue("token_type").ToString();
                expires_in = jObject.GetValue("expires_in").ToString();
                //check for response
                TokenExpirationDate = DateTime.Now.AddSeconds(double.Parse(expires_in)); // check the unit
                Console.WriteLine("Client has logged in successfully ");
            }
        }

        public async Task UploadData(string Data)
        {
            if (TokenExpirationDate == null || DateTime.Now > TokenExpirationDate)
            {
                //Login
                Login();

            }
            else
            {
                //Upload Data
                using (WebClient client = new WebClient())
                {
                    client.Headers.Clear();
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Authorization", Token_type + " " + Token);
                    Uri uri = new Uri(BaseUrl + "/" + UploadUrl);
                    client.UploadDataAsync(uri, "POST", Encoding.UTF8.GetBytes(Data));

                }
            }




        }
    }
}
