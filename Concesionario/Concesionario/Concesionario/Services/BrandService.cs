﻿using Concesionario.Services;
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
    public class BrandService
    {
        public ObservableCollection<Brand> Brands { get; set; }
        private string apiUrl;

        public BrandService()
        {
            using (var data = new DataAccess())
            {
                apiUrl = "http://192.168.43.236:40089/Api/Cars";
            }
            if (Brands == null)
            {
                Brands = new ObservableCollection<Brand>();
            }
        }

        public async System.Threading.Tasks.Task<ObservableCollection<Brand>> Consult()
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
                        Brands = JsonConvert.DeserializeObject<ObservableCollection<Brand>>(result);
                    }
                }
                return Brands;
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

        public ObservableCollection<Brand> ConsultLocal()
        {
            using (var data = new DataAccess())
            {
                var list = data.GetBrands();
                foreach (var item in list)
                    Brands.Add(item);
            }
            return Brands;
        }

        public async void Save(Brand model)
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

        public void SaveLocal(Brand model)
        {
            using (var data = new DataAccess())
            {
                data.InsertBrand(model);
            }
        }

        public async void Modify(Brand model)
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

        public void ModifyLocal(Brand model)
        {
            using (var data = new DataAccess())
            {
                data.ModifyBrand(model);
            }
        }

        public async void Delete(string idBrand)
        {
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client = CreateClient();
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + idBrand);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLocal(Brand model)
        {
            using (var data = new DataAccess())
            {
                data.DeleteBrand(model);
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