using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace AppLogin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewVaccine : ContentPage
    {
        VaccineViewModel vaccinemodel = new VaccineViewModel();
       

        public ViewVaccine()
        {
            InitializeComponent();
            BindingContext = vaccinemodel as VaccineViewModel;
        
        }
 

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (vaccinemodel.Items.Count == 0)
                vaccinemodel.LoadItemsCommand.Execute(null);
        }

       
    }
}