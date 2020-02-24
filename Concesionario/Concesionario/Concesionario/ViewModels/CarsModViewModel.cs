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
    class CarsModViewModel : CarsModel
    {
        private ObservableCollection<CarsModel> CarsModelsLocal { get; set; }
        private Task<ObservableCollection<CarsModel>> ModelsTask { get; set; }
        private ObservableCollection<CarsModel> ModelsAux { get; set; }
        public ObservableCollection<CarsModel> CarsModels { get; set; }

        public string Url { get; private set; }

        CarsModel model;

        ModelService service = new ModelService();

        public CarsModViewModel()
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
            CarsModels = new ObservableCollection<CarsModel>();
            ModelsAux = service.ConsultLocal();
            for (int i = 0; i < ModelsAux.Count; i++)
            {
                CarsModels.Add(ModelsAux[i]);
            }
        }

        private async Task ListViewAsync()
        {
            ModelsTask = service.Consult();
            ModelsAux = await ModelsTask;
            SyncroCarsModels();
            for (int i = 0; i < ModelsAux.Count; i++)
            {
                CarsModels.Add(ModelsAux[i]);
            }
            SyncroLocalCarsModels();
        }

        private void SyncroCarsModels()
        {
            for (int i = 0; i < CarsModels.Count; i++)
            {
                if (CarsModels[i] != ModelsAux[i])
                {
                    service.Save(CarsModels[i]);
                }
            }
        }

        private void SyncroLocalCarsModels()
        {
            for (int i = 0; i < CarsModels.Count; i++)
            {
                if (CarsModels[i] != CarsModelsLocal[i])
                {
                    service.SaveLocal(CarsModels[i]);
                }
            }
        }

        public Command SaveCommand { get; set; }

        public Command ModifyCommand { get; set; }

        public Command DeleteCommand { get; set; }

        public Command CleanCommand { get; set; }

        public Command GoCommand { get; set; }


        private async Task Save()
        {
            IsBusy = true;
            Guid idBrand = Guid.NewGuid();
            model = new CarsModel()
            {
                ModelName = ModelName,
                Power = Power,
                Color = Color,
                DoorsNumber = DoorsNumber,
                Id = idBrand.ToString()
            };
            if (string.IsNullOrEmpty(model.ModelName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El modelo no puede ser nulo", "Aceptar");
            }
            else
            {
                service.SaveLocal(model);
                CarsModels.Add(model);
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
            model = new CarsModel()
            {
                ModelName = ModelName,
                Power = Power,
                Color = Color,
                DoorsNumber = DoorsNumber,
                Id = Id
            };
            service.ModifyLocal(model);
            var item = CarsModels.FirstOrDefault(i => i.Id == model.Id);
            if (item != null)
            {
                item.ModelName = model.ModelName;
                item.Power = model.Power;
                item.Color = model.Color;
                item.DoorsNumber = model.DoorsNumber;
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
            model = new CarsModel()
            {
                ModelName = ModelName,
                Power = Power,
                Color = Color,
                DoorsNumber = DoorsNumber,
                Id = Id
            };
            service.DeleteLocal(model);
            var item = CarsModels.FirstOrDefault(i => i.Id == model.Id);
            CarsModels.Remove(item);
            Clean();
            if (await service.CheckConnection())
            {
                service.Delete(model.Id);
            }
            await Task.Delay(2000);
            IsBusy = false;
        }

        private void Clean()
        {
            ModelName = "";
            Power = "";
            Color = "";
            DoorsNumber = "";
            Id = "";
        }
    }
}
