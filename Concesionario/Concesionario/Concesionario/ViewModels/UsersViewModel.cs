using Concesionario.Models;
using Concesionario.Resources;
using Concesionario.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Concesionario.ViewModels
{
    class UsersViewModel : User
    {
        private Task<ObservableCollection<User>> UsersTask { get; set; }
        private ObservableCollection<User> UsersAux { get; set; }
        public ObservableCollection<User> Users { get; set; }

        User model;

        UserService service = new UserService();

        public UsersViewModel()
        {
            //ListView();
            //ListViewAsync();
            SaveCommand = new Command(async () => await Save(), () => !IsBusy);
            ModifyCommand = new Command(async () => await Modify(), () => !IsBusy);
            DeleteCommand = new Command(async () => await Delete(), () => !IsBusy);
            CleanCommand = new Command(Clean, () => !IsBusy);
            GoCommand = new Command(Go, () => !IsBusy);
            BackCommand = new Command(Back, () => !IsBusy);

        }

        private void ListView()
        {
            Users = new ObservableCollection<User>();
            //FunctionsAux = service.ConsultLocal();
            for (int i = 0; i < UsersAux.Count; i++)
            {
                Users.Add(UsersAux[i]);
            }
        }

        private async Task ListViewAsync()
        {
            //FunctionsTask = service.Consult();
            UsersAux = await UsersTask;
            for (int i = 0; i < UsersAux.Count; i++)
            {
                Users.Add(UsersAux[i]);
            }
        }

        public bool GoBool { get; set; }

        public Command SaveCommand { get; set; }

        public Command ModifyCommand { get; set; }

        public Command DeleteCommand { get; set; }

        public Command CleanCommand { get; set; }

        public Command GoCommand { get; set; }

        public Command BackCommand { get; set; }

        private async Task Save()
        {
            IsBusy = true;
            Guid idBrand = Guid.NewGuid();
            model = new User()
            {
                Username = Username,
                Name = Name,
                Surname = Surname,
                Phonenumber = Phonenumber,
                Id = idBrand.ToString()
            };
            if (string.IsNullOrEmpty(model.Username))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El usuario no puede ser nulo", "Aceptar");
            }
            else
            {
                //service.Save(model);
                //service.SaveLocal(model);
                Users.Add(model);
                Clean();
            }
            await Task.Delay(2000);
            IsBusy = false;
        }

        private async Task Modify()
        {
            IsBusy = true;
            model = new User()
            {
                Username = Username,
                Name = Name,
                Surname = Surname,
                Phonenumber = Phonenumber,
                Id = Id
            };
            //service.Modify(model);
            //service.ModifyLocal(model);
            var item = Users.FirstOrDefault(i => i.Id == model.Id);
            if (item != null)
            {
                item.Username = model.Username;
                item.Name = model.Name;
                item.Surname = model.Surname;
                item.Phonenumber = model.Phonenumber;
            }
            Clean();
            await Task.Delay(2000);
            IsBusy = false;
        }

        private async Task Delete()
        {
            IsBusy = true;
            model = new User()
            {
                Username = Username,
                Name = Name,
                Surname = Surname,
                Phonenumber = Phonenumber,
                Id = Id
            };
            //service.DeleteLocal(model);
            //service.Delete(model.Id);
            var item = Users.FirstOrDefault(i => i.Id == model.Id);
            Users.Remove(item);
            Clean();
            await Task.Delay(2000);
            IsBusy = false;
        }

        private void Clean()
        {
            Username = "";
            Name = "";
            Surname = "";
            Phonenumber = "";
            Id = "";
        }

        private void Go()
        {
            if (GoBool == false)
            {
                GoBool = true;
            }
            else
            {
                GoBool = false;
            }
        }

        private void Back()
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}

