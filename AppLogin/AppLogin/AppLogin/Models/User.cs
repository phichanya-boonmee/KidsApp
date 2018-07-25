using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Plugin.Connectivity;
//using SharpConnect.MySql;
//using SharpConnect.MySql.SyncPatt;

namespace AppLogin.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public String Password { get; set; }

        public User() { }

        public User(String Username, String Password)
        {
            this.Username = Username;
            this.Password = Password;

        }
        //protected static MySqlConnectionString GetMySqlConnString()
        //{
        //    string h = "127.0.0.1";
        //    string u = "root";
        //    string p = "";
        //    string d = "ip";

        //    return new MySqlConnectionString(h, u, p, d);
        //}
        public bool CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                
                return true;

            }
            else
            {
                return false;
            }

        }
        public bool CheckInformation()
        {
            
            //Check();
            //if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password) && status == 1)
            //{
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                return true;
            }

            else
            {
                return false;
            }
                

        }
        //int status = 0;
        public void Check()
        {

            //string myname;
            //string mypassword;
            //string[] words = get_data.Split('|');
            //string username = words[0].ToString();
            //string password = words[1].ToString();

            //string sql = "SELECT username,password FROM users WHERE username='" + Username + "' AND password='" + Password + "'";


            //MySqlConnectionString connStr = GetMySqlConnString();
            //var conn = new MySqlConnection(connStr);
            ////conn.UseConnectionPool = true;
            //conn.Open();
            ////Data.Items.Clear(); ////Data = listbox
            //try
            //{

            //    MySqlCommand cmd = new MySqlCommand(sql, conn);
            //    MySqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        myname = reader.GetString("username");
            //        mypassword = reader.GetString("password");
            //        if (myname == Username && mypassword == Password)
            //        {
            //            myname = reader.GetString("username");
            //            mypassword = reader.GetString("password");
            //            MessageBox.Show("Username: " + myname + "\n" + "Password: " + mypassword);
            //        }
            //        else
            //        {

            //        }


            //    }


            //}
            //catch (Exception h)
            //{
            //    MessageBox.Show("{error}" + h.Message);

            //}
        }
    }
}
