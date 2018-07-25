using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfCalendar.XForms;


namespace AppLogin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GettingStarted : ContentPage
	{
		public GettingStarted ()
		{
			//InitializeComponent ();

            SfCalendar calendar = new SfCalendar();
            //calendar.SelectionMode = SelectionMode.MultiSelection; //เลือกวันMuti

            calendar.ShowInlineEvents = true;

            CalendarInlineEvent events = new CalendarInlineEvent();

            WebClient wb = new WebClient();

            string userID = LoginPage.myUser_id;
            string myQuery = userID.ToString();
            string re = wb.UploadString("http://10.80.119.250:8082/notification/get_appoinement_data", myQuery);
            //string re = "2018-07-24 11:00:00|ฉีดยาไข้หวัดนก";
            string[] detail = re.Split('|');

            DateTime time;
            DateTime.TryParse(detail[0],out time);

            events.StartTime = time;
            events.EndTime = time.AddHours(5);
            events.Subject = detail[1];


            events.Color = Color.Fuchsia;
            CalendarEventCollection collection = new CalendarEventCollection();
            collection.Add(events);
            calendar.DataSource = collection;

            this.Content = calendar;
        }
    }
}