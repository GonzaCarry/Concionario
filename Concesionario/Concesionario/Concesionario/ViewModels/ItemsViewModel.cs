using Concesionario.Models;
using Concesionario.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Concesionario.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command CarsCommand { get; set; }
        public Command ModelsCommand { get; set; }
        public Command ExitCommand { get; set; }
        INavigation Navigation;

        public ItemsViewModel(INavigation Nav)
        {
            Navigation = Nav;
            CarsCommand = new Command(async () => await CarsPage(), () => !IsBusy);
            ModelsCommand = new Command(async () => await ModelsPage(), () => !IsBusy);
            ExitCommand = new Command(Exit);
            Title = "Home";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CarsPage()
        {
            await Navigation.PushAsync(new CarsBrand());
        }

        private async Task ModelsPage()
        {
            await Navigation.PushAsync(new CarsModels());
        }

        private void Exit()
        {
            //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());

        }

    }
}