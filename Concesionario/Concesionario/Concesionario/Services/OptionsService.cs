using Concesionario.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Concesionario.Services
{
    class OptionsService
    {
        private Connection connection { get; set; }
        private User user { get; set; }

        public OptionsService()
        {
            if (connection == null)
            {
                connection = new Connection();
            }
        }

        public Connection ConsultLocal()
        {
            using (var data = new DataAccess())
            {
                connection = data.GetConnection();
            }
            return connection;
        }

        public User ConsultUser()
        {
            using (var data = new DataAccess())
            {
                user = data.GetUser();
            }
            return user;
        }

        public async System.Threading.Tasks.Task<bool> SaveLocalAsync(Connection con)
        {
            using (var data = new DataAccess())
            {
                data.InsertConnection(con);
            }
            try
            {
                HttpClient client;
                using (client = new HttpClient())
                {
                    client.GetAsync(con.Url).Result.EnsureSuccessStatusCode();
                    HttpResponseMessage response = await client.GetAsync(con.Url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveUser(User user)
        {
            using (var data = new DataAccess())
            {
                data.InsertUser(user);
            }
        }
    }
}
