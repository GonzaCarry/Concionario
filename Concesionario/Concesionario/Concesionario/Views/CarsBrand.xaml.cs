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
    public partial class CarsBrand : ContentPage
    {
        CarsBrandViewModel Context = new CarsBrandViewModel();
        public CarsBrand()
        {
            Console.WriteLine("pasa");
            InitializeComponent();

            BindingContext = Context;
            LvBrands.ItemSelected += LvBrands_ItemSelected;
        }

        private void LvBrands_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Models.CarsBrandModel model = (Models.CarsBrandModel)e.SelectedItem;
                Context.Brand = model.Brand;
                Context.Headquarters = model.Headquarters;
                Context.Founder = model.Founder;
                Context.Id_Cars = model.Id_Cars;
            }
        }
    }
}

