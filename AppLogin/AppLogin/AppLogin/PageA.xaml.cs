using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AppLogin;
using System.Net;
using System.Net.Sockets;
using AppLogin.Views;

//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using SharpConnect.WebServers;

namespace AppLogin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageA : ContentPage 
	{
		public PageA()
		{
			InitializeComponent();
		}
        


        public double w, h, s, a;
		public string gender;
        public string userid;
		private void Btn_Onclick(object sender, EventArgs e)
		{
			double.TryParse(weight.Text, out w);
			double.TryParse(height.Text, out h);
			double.TryParse(surface.Text, out s);
			SendData();
			//double age = 0.0;
		}
		private void Calculate(double weight, double height, double surface, double age, string sex)
		{
			//string sex = "Male";
			double h = height / 100;
			double bmi = weight / (h * h);
			if (age >= 0.0 && age <= 0.04)
			{
				if (surface >= 33 && surface <= 37)
				{
					sresult.Text = "มาตรฐาน";
					sresult.TextColor = Color.Green;

				}
				else if (surface < 33)
				{
					sresult.Text = "ต่ำกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;
				}
				else
				{
					sresult.Text = "สูงกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
			}
			else if (age >= 0.0 && age <= 0.04)
			{
				if (surface >= 33 && surface <= 37)
				{
					sresult.Text = "มาตรฐาน";
					sresult.TextColor = Color.Green;

				}
				else if (surface < 33)
				{
					sresult.Text = "ต่ำกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
				else
				{
					sresult.Text = "สูงกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
			}
			else if (age > 0.04 && age <= 0.12)
			{
				if (surface == 40)
				{
					sresult.Text = "มาตรฐาน";
					sresult.TextColor = Color.Green;

				}
				else if (surface < 40)
				{
					sresult.Text = "ต่ำกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
				else
				{
					sresult.Text = "สูงกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;
				}
			}
			else if (age > 0.12 && age <= 3)
			{
				if (surface >= 45 && surface <= 47)
				{
					sresult.Text = "มาตรฐาน";
					sresult.TextColor = Color.Green;

				}
				else if (surface < 45)
				{
					sresult.Text = "ต่ำกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
				else
				{
					sresult.Text = "สูงกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
			}
			else if (age > 3 && age <= 10)
			{
				{
					if (surface == 50)
					{
						sresult.Text = "มาตรฐาน";
						sresult.TextColor = Color.Green;

					}
					else if (surface < 50)
					{
						sresult.Text = "ต่ำกว่ามาตรฐาน";
						sresult.TextColor = Color.Red;

					}
					else
					{
						sresult.Text = "สูงกว่ามาตรฐาน";
						sresult.TextColor = Color.Red;

					}
				}
			}
			else
			{
				if (surface == 55)
				{
					sresult.Text = "มาตรฐาน";
					sresult.TextColor = Color.Green;

				}
				else if (surface < 55)
				{
					sresult.Text = "ต่ำกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
				else
				{
					sresult.Text = "สูงกว่ามาตรฐาน";
					sresult.TextColor = Color.Red;

				}
			}


			if (sex == "male")
			{
				if (age == 0.0)
				{
					if (weight >= 2.8 && weight <= 3.9)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 2.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 47.6 && height <= 53.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;


					}
					else if (height < 47.6)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.01)
				{
					if (weight >= 3.4 && weight <= 4.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;


					}
					else if (weight < 3.4)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 50.4 && height <= 56.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;


					}
					else if (height < 50.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.02)
				{
					if (weight >= 4.2 && weight <= 5.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 4.2)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 53.2 && height <= 59.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 53.2)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.03)
				{
					if (weight >= 4.8 && weight <= 6.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 4.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 55.7 && height <= 61.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 55.7)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.04)
				{
					if (weight >= 5.3 && weight <= 7.1)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 5.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 58.1 && height <= 64.6)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 58.1)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.05)
				{
					if (weight >= 5.8 && weight <= 7.8)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 5.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 60.4 && height <= 67.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 60.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.06)
				{
					if (weight >= 6.3 && weight <= 8.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 6.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 62.4 && height <= 69.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 62.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.07)
				{
					if (weight >= 6.8 && weight <= 9.0)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 6.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 64.2 && height <= 71.3)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 64.2)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.08)
				{
					if (weight >= 7.2 && weight <= 9.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 7.2)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 65.9 && height <= 73.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 65.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.09)
				{
					if (weight >= 7.6 && weight <= 9.9)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 9.9)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 67.4 && height <= 75.0)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 67.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.10)
				{
					if (weight >= 7.9 && weight <= 10.3)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 7.9)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 68.9 && height <= 76.7)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 68.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.11)
				{
					if (weight >= 8.1 && weight <= 10.6)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 8.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 70.2 && height <= 78.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 70.2)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 1)
				{
					if (weight >= 8.3 && weight <= 11.0)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 8.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 71.5 && height <= 79.7)
					{

						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 71.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 2)
				{
					if (weight >= 10.5 && weight <= 14.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 10.5)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 82.5 && height <= 91.5)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 82.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 3)
				{
					if (weight >= 12.1 && weight <= 17.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 12.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 89.4 && height <= 100.8)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 89.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 4)
				{
					if (weight >= 13.6 && weight <= 19.9)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 13.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 95.5 && height <= 108.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 95.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 5)
				{
					if (weight >= 15.0 && weight <= 22.6)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 15.0)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 102.0 && height <= 115.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 102.0)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 6)
				{
					if (weight >= 16.6 && weight <= 25.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 16.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 107.7 && height <= 121.3)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 107.7)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 7)
				{
					if (weight >= 18.3 && weight <= 28.8)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 18.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 112.8 && height <= 127.4)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 112.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 8)
				{
					if (weight >= 20.0 && weight <= 32.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 20.0)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 117.4 && height <= 133.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 117.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 9)
				{
					if (weight >= 21.5 && weight <= 36.6)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 21.5)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 121.8 && height <= 138.3)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 121.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 10)
				{
					if (weight >= 23.6 && weight <= 40.8)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 23.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 126.2 && height <= 143.4)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 126.2)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 11)
				{
					if (weight >= 25.6 && weight <= 45.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 25.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 130.5 && height <= 149.4)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 130.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 12)
				{
					if (weight >= 28.1 && weight <= 50.0)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 28.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 135.1 && height <= 156.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 135.1)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 13)
				{
					if (weight >= 31.6 && weight <= 51.6)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 31.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 140.9 && height <= 164.4)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 140.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 14)
				{
					if (weight >= 35.6 && weight <= 58.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 35.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 147.3 && height <= 170.0)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 147.3)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 15)
				{
					if (weight >= 40.1 && weight <= 61.9)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 40.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 153.5 && height <= 173.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 153.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
			}
			else if (sex == "female")
			{
				if (age == 0.00)
				{
					if (weight >= 2.7 && weight <= 3.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 2.7)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 46.8 && height <= 52.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 46.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.01)
				{
					if (weight >= 3.3 && weight <= 4.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 3.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 49.4 && height <= 56.0)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 49.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.02)
				{
					if (weight >= 3.8 && weight <= 5.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 3.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 52.0 && height <= 59.0)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 52.0)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.03)
				{
					if (weight >= 4.4 && weight <= 6.0)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 4.4)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 54.4 && height <= 61.8)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 54.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.04)
				{
					if (weight >= 4.9 && weight <= 6.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 4.9)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 56.8 && height <= 64.5)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 56.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.05)
				{
					if (weight >= 5.3 && weight <= 7.3)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 5.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 58.9 && height <= 66.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 58.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.06)
				{
					if (weight >= 5.8 && weight <= 7.9)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 5.8)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 60.9 && height <= 69.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 60.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.07)
				{
					if (weight >= 6.2 && weight <= 8.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 6.2)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 62.6 && height <= 71.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 62.6)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;
					}
				}
				else if (age == 0.08)
				{
					if (weight >= 6.6 && weight <= 9.0)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 6.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 64.2 && height <= 72.8)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 64.2)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.09)
				{
					if (weight >= 6.9 && weight <= 9.3)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 6.9)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 65.5 && height <= 74.5)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 65.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.10)
				{
					if (weight >= 7.2 && weight <= 9.8)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 7.2)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 66.7 && height <= 76.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 66.7)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 0.11)
				{
					if (weight >= 7.5 && weight <= 10.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 7.5)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 67.7 && height <= 77.6)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 67.7)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 1)
				{
					if (weight >= 7.7 && weight <= 10.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 7.7)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 68.8 && height <= 78.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 68.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 2)
				{
					if (weight >= 9.7 && weight <= 13.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 9.7)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 70.8 && height <= 89.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 70.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 3)
				{
					if (weight >= 11.5 && weight <= 16.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 11.5)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 95.0 && height <= 106.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 95.0)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 4)
				{
					if (weight >= 13.0 && weight <= 19.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 13.0)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 95.5 && height <= 106.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 95.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 5)
				{
					if (weight >= 14.4 && weight <= 21.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 14.4)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 101.1 && height <= 113.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 101.1)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 6)
				{
					if (weight >= 16.1 && weight <= 24.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 16.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 107.4 && height <= 120.8)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 107.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 7)
				{
					if (weight >= 17.7 && weight <= 28.7)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 17.7)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 112.4 && height <= 126.8)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 112.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 8)
				{
					if (weight >= 19.3 && weight <= 32.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 19.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 117.0 && height <= 132.4)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 117.0)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 9)
				{
					if (weight >= 21.2 && weight <= 37.4)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 21.2)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 121.9 && height <= 139.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 121.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 10)
				{
					if (weight >= 23.4 && weight <= 42.1)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 23.4)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 127.1 && height <= 146.1)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 127.1)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 11)
				{
					if (weight >= 26.1 && weight <= 46.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 26.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 132.9 && height <= 152.6)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 132.9)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 12)
				{
					if (weight >= 26.1 && weight <= 46.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 26.1)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 138.8 && height <= 156.9)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 138.8)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 13)
				{
					if (weight >= 33.0 && weight <= 53.1)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 33.0)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 143.5 && height <= 160.2)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 143.5)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 14)
				{
					if (weight >= 36.3 && weight <= 55.2)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 36.3)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 147.0 && height <= 162.3)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 147.0)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
				else if (age == 15)
				{
					if (weight >= 38.6 && weight <= 56.5)
					{
						wresult.Text = "มาตรฐาน";
						wresult.TextColor = Color.Green;

					}
					else if (weight < 38.6)
					{
						wresult.Text = "ต่ำกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					else
					{
						wresult.Text = "สูงกว่ามาตรฐาน";
						wresult.TextColor = Color.Red;

					}
					if (height >= 148.4 && height <= 163.5)
					{
						hresult.Text = "มาตรฐาน";
						hresult.TextColor = Color.Green;

					}
					else if (height < 148.4)
					{
						hresult.Text = "ต่ำกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
					else
					{
						hresult.Text = "สูงกว่ามาตรฐาน";
						hresult.TextColor = Color.Red;

					}
				}
			}
		}

        
		public void SendData()
		{
			string data;
			string myQuery;


            string user_id = LoginPage.myUser_id;
            WebClient wb = new WebClient();
			myQuery = "SELECT gender,age FROM health WHERE username_id=" + user_id;
			data = wb.UploadString("http://10.80.119.250:8082/health/age_gender", myQuery);

			string[] user_data = data.Split('|');
			double.TryParse(user_data[1],out a);
			gender = user_data[0];
			Calculate(w, h, s, a, gender);

		}

      
	}
}
