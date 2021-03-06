﻿using Concesionario.Models;
using Concesionario.ViewModels;
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
    public partial class ItemsPage : ContentPage
    {
        public ItemsViewModel Context { get; set; }
        public ItemsPage()
        {
            InitializeComponent();
            Context = new ItemsViewModel(Navigation);
            BindingContext = Context;
        }
    }
}