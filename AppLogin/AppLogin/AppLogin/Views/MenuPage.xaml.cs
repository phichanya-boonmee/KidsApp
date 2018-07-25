using AppLogin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppLogin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        
		public MenuPage ()
		{
          
            InitializeComponent ();
           
        }

       
        public async void GoPage1Clicked(object sender, EventArgs args)
        {
            //await DisplayAlert("Message", "Go Page 1.", "Ok");
            await Navigation.PushAsync(new PageA());

        }
        public async void GoPage2Clicked(object sender, EventArgs args)
        {
            //DisplayAlert("Message", "Go Page 2.", "Ok");
            await Navigation.PushAsync(new ViewVaccine());
        }
        public async void GoPage3Clicked(object sender, EventArgs args)
        {
            //await DisplayAlert("Message", "Go Page 3.", "Ok");
            await Navigation.PushAsync(new about());

        }
        public async void GoPage4Clicked(object sender, EventArgs args)
        {
            //await DisplayAlert("Message", "Go Page 4.", "Ok");
            await Navigation.PushAsync(new GettingStarted());
        }

    }
}