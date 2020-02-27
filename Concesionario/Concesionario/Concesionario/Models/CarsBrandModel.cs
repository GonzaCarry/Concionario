using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace Concesionario.Models
{
    public partial class CarsBrandModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string id_Cars;

        [PrimaryKey]
        public string Id_Cars
        {
            get { return id_Cars; }
            set
            {
                id_Cars = value;
                OnPropertyChanged();
            }
        }

        private string brand;

        public string Brand
        {
            get { return brand; }
            set
            {
                brand = value;
                OnPropertyChanged();
            }
        }

        private string headquarters;

        public string Headquarters
        {
            get { return headquarters; }
            set
            {
                headquarters = value;
                OnPropertyChanged();
            }
        }

        private string founder;

        public string Founder
        {
            get { return founder; }
            set
            {
                founder = value;
                OnPropertyChanged();
            }
        }

        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy = false; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

    }
}
