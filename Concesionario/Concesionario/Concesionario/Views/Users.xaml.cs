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
    public partial class Users : ContentPage
    {
        UsersViewModel Context = new UsersViewModel();
        public Users()
        {
            Console.WriteLine("pasa");
            InitializeComponent();

            BindingContext = Context;
            //LvUsers.ItemSelected += LvUsers_ItemSelected;
        }

        //private void LvUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    if (e.SelectedItem != null)
        //    {
        //        User model = (User)e.SelectedItem;
        //        if (Context.GoBool)
        //        {
        //            ((ListView)sender).SelectedItem = null;
        //            //Navigation.PushAsync(new DetallePage(model));
        //        }
        //        Context.Username = model.Username;
        //        Context.Name = model.Name;
        //        Context.Surname = model.Surname;
        //        Context.Phonenumber = model.Phonenumber;
        //        Context.Id = model.Id;
        //    }
        //}
    }
}