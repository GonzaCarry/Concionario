using Concesionario.Services;
using Concesionario.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace Concesionario.Resources
{
    public class ModelService
    {
        public ObservableCollection<CarsModel> CarsModels { get; set; }
        private string apiUrl;

        public ModelService()
        {
            using (var data = new DataAccess())
            {
                apiUrl = data.GetConnection().Url + "/api/Models";
            }
            if (CarsModels == null)
            {
                CarsModels = new ObservableCollection<CarsModel>();
            }
        }

        public async System.Threading.Tasks.Task<ObservableCollection<CarsModel>> Consult()
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        CarsModels = JsonConvert.DeserializeObject<ObservableCollection<CarsModel>>(result);
                    }
                }
                return CarsModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task<bool> CheckConnection()
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ObservableCollection<CarsModel> ConsultLocal()
        {
            using (var data = new DataAccess())
            {
                var list = data.GetCarsModels();
                foreach (var item in list)
                    CarsModels.Add(item);
            }
            return CarsModels;
        }

        public async void Save(CarsModel model)
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    var send = Newtonsoft.Json.JsonConvert.SerializeObject(model,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                    request.Content = new StringContent(send, Encoding.UTF8, "application/json");//CONTENT-TYPE header
                    HttpResponseMessage response = await client.SendAsync(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveLocal(CarsModel model)
        {
            using (var data = new DataAccess())
            {
                data.InsertCarsModels(model);
            }
        }

        public async void Modify(CarsModel model)
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    Uri apiUrl2 = new Uri(string.Format(apiUrl + "/{0}", model.Id));
                    HttpResponseMessage response = await client.PutAsync(apiUrl2, content);
                    Console.WriteLine(response.IsSuccessStatusCode);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ModifyLocal(CarsModel model)
        {
            using (var data = new DataAccess())
            {
                data.ModifyCarsModels(model);
            }
        }

        public async void Delete(string idModel)
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + idModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLocal(CarsModel model)
        {
            using (var data = new DataAccess())
            {
                data.DeleteCarsModels(model);
            }
        }

        private HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["token"].ToString());
            client.Timeout = TimeSpan.FromMinutes(10);
            client.Timeout = new TimeSpan(0, 0, 0, 0, -1);
            return client;
        }

    }
}
