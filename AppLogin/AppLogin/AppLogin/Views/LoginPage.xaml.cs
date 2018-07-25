using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using Plugin.LocalNotifications;


namespace AppLogin
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {

        public static string myUser_id;
        string myStatus;

        public LoginPage ()
		{
			InitializeComponent ();

        }

       
        public async void OnButtonClicked(object sender, EventArgs args)
        {
            //await label.RelRotateTo(360, 1000);
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            string username = Entry_Username.Text;
            string password = Entry_Password.Text;
            if (user.CheckConnection())
            {

                if (user.CheckInformation())
                {

                    WebClient wb = new WebClient();
                    string data_login = username + "|" + password;

                    string result = wb.UploadString("http://10.80.119.250:8082/users/check", data_login);
                    //string result_from_server = wb.DownloadString("http://10.80.118.132:8082/users/check");

                    string[] word = result.Split('|');
                    myStatus = word[0].ToString();
                    myUser_id = word[1].ToString();


                    if (myStatus == "Success")
                    {
                        //await DisplayAlert("Login", "Login Success", "Ok");

                        await Navigation.PushAsync(new MenuPage());
                       

                        //Notification
                        string re = wb.UploadString("http://10.80.119.250:8082/notification/get_appoinement_data", myUser_id);
                        string[] detail = re.Split('|');
                        CrossLocalNotifications.Current.Show(detail[0], detail[1], 100, DateTime.Now.AddSeconds(5));


                                            

                    }
                    else
                    {
                        await DisplayAlert("Login", "Not Correct, empty username or password", "Ok");
                    }


                }
                else
                {

                    await DisplayAlert("Login", "Not Correct, empty username or password", "Ok");

                }
            }
            else
            {
                await DisplayAlert("Login", "No Inetrnet Connection", "Ok");
            }
           

        }

    }
}