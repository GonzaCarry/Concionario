using Concesionario.ViewModels;
using Concesionario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Concesionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarsModels : ContentPage
    {
        CarsModViewModel Context = new CarsModViewModel();
        public CarsModels()
        {
            Console.WriteLine("pasa");
            InitializeComponent();

            BindingContext = Context;
            LvCarsModels.ItemSelected += LvCarsModels_ItemSelected;
        }

        private void LvCarsModels_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                CarsModel model = (CarsModel)e.SelectedItem;
                Context.ModelName = model.ModelName;
                Context.Power = model.Power;
                Context.Color = model.Color;
                Context.DoorsNumber = model.DoorsNumber;
                Context.Id = model.Id;
            }
        }
    }
}

