using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using X.ParkingSigns.App.Models;

namespace X.ParkingSigns.App
{
    public class JsonAdo
    {
        public JsonAdo()
        {
        }

        public List<Sign> GetSigns()
        {
            var signs = new List<Sign>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://com-parkingsigns-app.firebaseio.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("signs.json").Result;
                if (response.IsSuccessStatusCode)
                {
                    var signsString = response.Content.ReadAsStringAsync().Result;
                    signs = JsonConvert.DeserializeObject<List<Sign>>(signsString);
                }
            }

            return signs;
        }

        //public List<Meter> GetMeters()
        //{

        //}
    }
}

