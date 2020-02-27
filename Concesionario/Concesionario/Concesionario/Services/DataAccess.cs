using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.IO;
using SQLite;
using Concesionario.Models;
using Concesionario.Services;

namespace Concesionario.Services
{
    class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(Path.Combine(config.DirDB, "DBConcesionario.db3"), false);
            //connection.CreateTable<Function>();
            connection.CreateTable<Connection>();
            connection.CreateTable<CarsModel>();
            connection.CreateTable<CarsBrandModel>();
            connection.CreateTable<User>();
        }

        public void InsertCarsModels(CarsModel car)
        {
            connection.Insert(car);
        }

        public void ModifyCarsModels(CarsModel car)
        {
            connection.Update(car);
        }

        public void DeleteCarsModels(CarsModel car)
        {
            connection.Delete(car);
        }

        public CarsModel GetCarsModels(int IDCar)
        {
            return connection.Table<CarsModel>().FirstOrDefault(c => c.Id.Equals(IDCar));
        }

        public List<CarsModel> GetCarsModels()
        {
            return connection.Table<CarsModel>().OrderBy(c => c.ModelName).ToList();
        }

        public void InsertBrand(CarsBrandModel brand)
        {
            connection.Insert(brand);
        }

        public void ModifyBrand(CarsBrandModel brand)
        {
            connection.Update(brand);
        }

        public void DeleteBrand(CarsBrandModel brand)
        {
            connection.Delete(brand);
        }

        public CarsBrandModel GetBrand(int IDBrand)
        {
            return connection.Table<CarsBrandModel>().FirstOrDefault(c => c.Id_Cars.Equals(IDBrand));
        }

        public List<CarsBrandModel> GetBrands()
        {
            return connection.Table<CarsBrandModel>().OrderBy(c => c.Brand).ToList();
        }

        public Connection GetConnection()
        {
            if (connection.Table<Connection>().ToList().Count > 0)
            {
                return connection.Table<Connection>().FirstOrDefault(c => c.Id.Equals(0));
            }
            return new Connection();
        }

        public void InsertConnection(Connection con)
        {
            if (connection.Table<Connection>().ToList().Count > 0)
            {
                connection.Delete(connection.Table<Connection>().FirstOrDefault(c => c.Id.Equals(0)));
            }
            connection.Insert(con);
        }

        public void InsertUser(User user)
        {
            if (connection.Table<User>().ToList().Count > 0)
            {
                connection.Delete(connection.Table<User>().FirstOrDefault(c => c.Id.Equals(0)));
            }
            connection.Insert(user);
        }

        public void ModifyUser(User user)
        {
            connection.Update(user);
        }

        public void DeleteUser(User user)
        {
            connection.Delete(user);
        }

        public User GetUser()
        {
            return connection.Table<User>().FirstOrDefault(c => c.Id.Equals(0));
        }

        public List<User> GetUsers()
        {
            return connection.Table<User>().OrderBy(c => c.Name).ToList();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
