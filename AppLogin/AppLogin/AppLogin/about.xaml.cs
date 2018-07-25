using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace AppLogin
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class about : ContentPage
    {
        //INotifyPropertyChanged
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                //RaisePropertyChanged("IsLoading");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

       
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                    return ip.ToString();

                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        
        string ipv4;
        public about ()
		{
            ipv4 = GetLocalIPAddress();
            InitializeComponent ();

            IsLoading = false;
            BindingContext = this;

            

        }

        

        string comment1;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            //base.OnAppearing();
            comment1 = comment.Text;
            IsLoading = true;
            Console.WriteLine(comment1);
            WebClient wb = new WebClient();
            string result;
            base.OnAppearing();
            try
            {
                
                result = wb.UploadString("http://10.80.119.250:8082/About/Comment", comment1);
                IsLoading = false;
            } catch
            {
                throw;
            }

             await DisplayAlert("Comment", result, "Ok");
            
            
        }
        

        private void Image_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}