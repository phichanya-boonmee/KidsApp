using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using SharpConnect.MySql.SyncPatt;

using SharpConnect.WebServers;
using SharpConnect;
using Newtonsoft.Json;
using SharpConnect.MySql;

namespace Test2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SharpConnect.AppHost testApp;
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
        static MySqlConnectionString GetMySqlConnString()
        {
            string h = "127.0.0.1";
            string u = "root";
            string p = "";
            string d = "kidshistory";
            return new MySqlConnectionString(h, u, p, d);
        }
        
        //public class Blue
        //{
        //    public int X { get; set; }
        //    public int Y
        //    {
        //        get; set;
        //    }
        //    public int T { get; set; }
        //}
        //List<Blue> panel = new List<Blue>();
        //List<Blue> Undo_history = new List<Blue>();


        private void Form1_Load(object sender, EventArgs e)
        {
            //this.button1.Visible = false;
            //this.textBox1.Visible = false;
            testApp = new SharpConnect.AppHost();
            Undo_Data undo = new Undo_Data();
            undo.DataArrived += DataArrived;
            undo.LoadData += LoadData;
            //about
            About about = new About();
            about.Comment_Arrived += About_Comment_Arrived;
            //vaccine
            Vaccine vaccine = new Vaccine();
            
            vaccine.Load_vaccine += Load_vaccine;
            //user login
            Users login = new Users();
            login.Login += Login;
            //health
            Health health = new Health();
            health.Load_age_gender += Health_Load_age_gender;
            //appointment
            Notification appointment = new Notification();
            appointment.Load_Appointment += Load_Appoinement;

            testApp.RegisterModule(health);
            testApp.RegisterModule(vaccine);
            testApp.RegisterModule(about);
            testApp.RegisterModule(appointment);
            testApp.RegisterModule(login);

            string name = "[1,2,3,4,5]";
            JArray con = JArray.Parse(name);
            
            
            Console.WriteLine(con);

            //testApp.RegisterModule(new MyModule());
            //testApp.RegisterModule(new MyModule2());
            //testApp.RegisterModule(new MyModule3());
            //testApp.RegisterModule(new MyAdvanceMathModule());
            //testApp.RegisterModule(new MMath1());
            //testApp.RegisterModule(new Undo_Data());
            //1. create  
            WebServer webServer = new WebServer(8082, false, testApp.HandleRequest);
            //test websocket 
            var webSocketServer = new WebSocketServer();
            webSocketServer.SetOnNewConnectionContext(ctx =>
            {
                ctx.SetMessageHandler(testApp.HandleWebSocket);
            });
            webServer.WebSocketServer = webSocketServer;
            webServer.Start();
        }

        private void Load_Appoinement(object sender, Kidshistory e)
        {
            string id= (string)e.notification;
            MySqlConnectionString connStr = GetMySqlConnString();
            var conn = new MySqlConnection(connStr);
            conn.UseConnectionPool = true;
            conn.Open();
            string result = "";
            try
            {
                var cmd = new MySqlCommand("SELECT detail, appointment FROM notification " +
                          "WHERE username_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result += reader.GetString("appointment");
                    result += "|" + reader.GetString("detail");
                    e.notification = result;
                }
                if (result == "")
                {
                    e.notification = "No data";
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
            }
            conn.Close();
        }

        private void Health_Load_age_gender(object sender, Kidshistory e) //age gender
        {
            string query = (string)e.health;
            MySqlConnectionString connStr = GetMySqlConnString();
            var conn = new MySqlConnection(connStr);
            conn.UseConnectionPool = true;
            conn.Open();
            string result = "";
            try
            {
                var cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString("gender");
                    result = result + "|" + reader.GetString("age");
                    e.health = result;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
            
        }

        private void Login(object sender, Kidshistory e)
        {
            bool status = false;
            string myname = "";
            string mypassword;
            string user_id = "";
            string word = (string)e.users;
            string[] words = word.Split('|');
            string username = words[0].ToString();
            string password = words[1].ToString();
            MySqlConnectionString connStr = GetMySqlConnString();
            var conn = new MySqlConnection(connStr);
            conn.UseConnectionPool = true;
            conn.Open();
            try
            {
                var cmd = new MySqlCommand("SELECT username,password,username_id FROM users WHERE " +
                          "username='" + username + "' AND password='" + password + "'", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    myname = reader.GetString("username");
                    mypassword = reader.GetString("password");
                    if (myname == username && mypassword == password)
                    {
                        myname = reader.GetString("username");
                        mypassword = reader.GetString("password");
                        user_id = reader.GetString("username_id");
                        Console.WriteLine("Username: " + myname + "\n" + "Password: " + mypassword);
                        status = true;
                        
                        break;
                    }
                    
                }
                reader.Close();
                if (status) //true
                {
                    e.users = "Success" + "|" + user_id; //response data
                    var cmd2 = new MySqlCommand("UPDATE users SET State=@S WHERE username=@user", conn);
                    cmd2.Parameters.AddWithValue("@S", 1);
                    cmd2.Parameters.AddWithValue("@user", myname);
                    cmd2.ExecuteNonQuery();
                } else
                {
                    e.users = "Fail" + "|" + 0;
                    Console.WriteLine("Login failed");
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            conn.Close();
            
        }

        private void Load_vaccine(object sender, Kidshistory e)
        {
            //throw new NotImplementedException();
            string id = (string)e.vaccine;
            MySqlConnectionString connStr = GetMySqlConnString();
            var conn = new MySqlConnection(connStr);
            conn.UseConnectionPool = true;
            conn.Open();
            string result = "";
            try
            {
                var cmd = new MySqlCommand("SELECT injected_vaccine FROM vaccine WHERE username_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString("injected_vaccine");
                    e.vaccine = result;
                    Console.WriteLine("List: {0} by id:{1}", result, id);
                }
                
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (result == "")
            {
                e.vaccine = "Fail";
            }
            conn.Close();
        }

        

        private void About_Comment_Arrived(object sender, Kidshistory e)
        {
            MySqlConnectionString connStr = GetMySqlConnString();
            var conn = new MySqlConnection(connStr);
            conn.UseConnectionPool = true;
            conn.Open();
            try
            {
                var cmd = new MySqlCommand("INSERT INTO about(comment)VALUES(@comment)", conn);
                cmd.Parameters.AddWithValue("@comment", (string)e.about);
                cmd.ExecuteNonQuery();
            }
            catch (Exception) { }
            conn.Close();
        }
        //Panel
        string Panel_Data;
        private void LoadData(object sender, UserUnHxListEventArgs e)
        {
            e.undohxlist = Panel_Data;
        }

        private void DataArrived(object sender, UserUnHxListEventArgs e)
        {
            Panel_Data = (string)e.undohxlist; //string
   
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //testApp.CurrentServerMsg = this.textBox1.Text;
            this.textBox1.Text = "URL Requested = " + testApp.CurrentServerMsg + " \r\nSuccess = " + testApp.success + " \r\nFailed = " + testApp.failed;
    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
//loadData
//Blue Panel

//using (StreamReader r = new StreamReader("SavePanel.json"))
//{
//    string panel = r.ReadToEnd();
//}
////History
//using (StreamReader r = new StreamReader("History.json"))
//{
//    string history = r.ReadToEnd();
//}
////listcount
//using (StreamReader r = new StreamReader("ListCountHistory.json"))
//{
//    string count = r.ReadToEnd();
//}

//e.undohxlist = panel + "|" + history + "|" + count;
//DataArrive
//panel.Clear();

//Undo_history.Clear();

//Undo_Data data = (Undo_Data)sender;
//data.DataArrived1 = (EventHandler<UserUnHxListEventArgs>)e.undohxlist;
//string[] Data = ((string)e.undohxlist).Split("|".ToCharArray());


//    e.panel = Data[0];
//    e.history = Data[1];
//    e.count = Data[2];
//    JArray Panel = JArray.Parse(Data[0]);
//    JArray History = JArray.Parse(Data[1]);
//    JArray ListCount = JArray.Parse(Data[2]);
//    //Panel
//    using (FileStream fs = new FileStream("SavePanel.json", FileMode.Create))
//    using (StreamWriter file = new StreamWriter(fs))
//    {
//        JsonSerializer json = new JsonSerializer();

//        foreach (JObject o in Panel.Children<JObject>())
//        {
//            Blue Blue_Panel = new Blue();
//            foreach (JProperty p in o.Properties())
//            {
//                if (p.Name == "X")
//                {
//                    Blue_Panel.X = (int)p.Value;
//                }
//                if (p.Name == "Y")
//                {
//                    Blue_Panel.Y = (int)p.Value;
//                }
//                if (p.Name == "T")
//                {
//                    Blue_Panel.T = (int)p.Value;
//                }
//                //Console.WriteLine(p.Name);
//            }
//            Undo_history.Add(Blue_Panel);
//        }

//        json.Serialize(file, Undo_history); //Panel
//    }

//    //History
//    using (FileStream fs = new FileStream("History.json", FileMode.Create))
//    using (StreamWriter file = new StreamWriter(fs))
//    {
//        JsonSerializer json = new JsonSerializer();

//        foreach (JObject o in History.Children<JObject>())
//        {
//            Blue Undo = new Blue();
//            foreach (JProperty p in o.Properties())
//            {
//                if (p.Name == "X")
//                {
//                    Undo.X = (int)p.Value;
//                }
//                if (p.Name == "Y")
//                {
//                    Undo.Y = (int)p.Value;
//                }
//                if (p.Name == "T")
//                {
//                    Undo.T = (int)p.Value;
//                }
//                //Console.WriteLine(p.Name);
//            }
//            panel.Add(Undo);
//        }

//        json.Serialize(file, panel); //Panel
//    }
//    //Count Undo
//    using (FileStream fs = new FileStream("ListCountHistory.json", FileMode.Create))
//    using (StreamWriter file = new StreamWriter(fs))
//    {
//        JsonSerializer json = new JsonSerializer();
//        json.Serialize(file, ListCount);
//    }
