using Concesionario.Models;
using Concesionario.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Concesionario.ViewModels
{
    class CarsBrandViewModel : CarsBrandModel
    {
        private ObservableCollection<CarsBrandModel> BrandsLocal { get; set; }
        private Task<ObservableCollection<CarsBrandModel>> BrandsTask { get; set; }
        private ObservableCollection<CarsBrandModel> BrandsAux { get; set; }
        public ObservableCollection<CarsBrandModel> Brands { get; set; }

        public string Url { get; private set; }

        CarsBrandModel model;

        BrandService service = new BrandService();

        public CarsBrandViewModel()
        {
            ListView();
            ListViewAsync();
            SaveCommand = new Command(async () => await Save(), () => !IsBusy);
            ModifyCommand = new Command(async () => await Modify(), () => !IsBusy);
            DeleteCommand = new Command(async () => await Delete(), () => !IsBusy);
            CleanCommand = new Command(Clean, () => !IsBusy);

        }

        private void ListView()
        {
            Brands = new ObservableCollection<CarsBrandModel>();
            BrandsLocal = new ObservableCollection<CarsBrandModel>();
            BrandsAux = service.ConsultLocal();
            for (int i = 0; i < BrandsAux.Count; i++)
            {
                Brands.Add(BrandsAux[i]);
                BrandsLocal.Add(BrandsAux[i]);
            }
        }

        private async Task ListViewAsync()
        {
            BrandsTask = service.Consult();
            BrandsAux = await BrandsTask;
            for (int i = 0; i < BrandsAux.Count; i++)
            {
                Brands.Add(BrandsAux[i]);
            }
            //SyncroLocalBrand();
            //SyncroBrand();
        }

        private void SyncroBrand()
        {
            for (int i = 0; i < Brands.Count; i++)
            {
                service.Save(Brands[i]);
            }
        }

        private void SyncroLocalBrand()
        {
            for (int i = 0; i < Brands.Count; i++)
            {
                service.SaveLocal(Brands[i]);
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
            model = new CarsBrandModel()
            {
                Brand = Brand,
                Headquarters = Headquarters,
                Founder = Founder,
                Id_Cars = idBrand.ToString()
            };
            if (string.IsNullOrEmpty(model.Brand))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La marca no puede ser nula", "Aceptar");
            }
            else
            {
                service.SaveLocal(model);
                Brands.Add(model);
                Clean();
                if (await service.CheckConnection())
                {
                    service.Save(model);
                }
            }
            await Task.Delay(2000);
            IsBusy = false;
        }

        private async Task Modify()
        {
            IsBusy = true;
            model = new CarsBrandModel()
            {
                Brand = Brand,
                Headquarters = Headquarters,
                Founder = Founder,
                Id_Cars = Id_Cars
            };
            service.ModifyLocal(model);
            var item = Brands.FirstOrDefault(i => i.Id_Cars == model.Id_Cars);
            if (item != null)
            {
                item.Brand = model.Brand;
                item.Headquarters = model.Headquarters;
                item.Founder = model.Founder;
            }
            Clean();
            if (await service.CheckConnection())
            {
                service.Modify(model);
            }
            await Task.Delay(2000);
            IsBusy = false;
        }

        private async Task Delete()
        {
            IsBusy = true;
            model = new CarsBrandModel()
            {
                Brand = Brand,
                Headquarters = Headquarters,
                Founder = Founder,
                Id_Cars = Id_Cars
            };
            service.DeleteLocal(model);
            var item = Brands.FirstOrDefault(i => i.Id_Cars == model.Id_Cars);
            Brands.Remove(item);
            Clean();
            if (await service.CheckConnection())
            {
                service.Delete(model.Id_Cars);
            }
            await Task.Delay(2000);
            IsBusy = false;
        }

        private void Clean()
        {
            Brand = "";
            Headquarters = "";
            Founder = "";
            Id_Cars = "";
        }
    }
}

