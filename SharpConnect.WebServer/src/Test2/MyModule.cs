//2015, MIT, EngineKit
using System.Collections.Generic;
using System;
using System.Reflection;
using SharpConnect.WebServers;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;

namespace SharpConnect
{
    
    enum ConvPlan
    {
        Unknown,
        ToString,
        ToDouble,
        ToInt32,
        ToFloat,
        ToShort
    }

    class ParameterAdapter
    {
        public string ParName;
        public Type ParType;
        public ConvPlan convPlan;

        public ParameterAdapter(string parName, Type parType)
        {
            ParName = parName;
            ParType = parType;
            //convert from string to target type
            switch (ParType.FullName)
            {
                default:
                    break;
                case "System.Int32":
                    convPlan = ConvPlan.ToInt32;
                    break;
                case "System.String":
                    convPlan = ConvPlan.ToString;
                    break;
                case "System.Double":
                    convPlan = ConvPlan.ToDouble;
                    break;
            }
        }
        public object GetActualValue(string strValue)
        {
            try
            {
                switch (convPlan)
                {
                    default:
                        return null;
                    case ConvPlan.ToDouble:
                        return double.Parse(strValue);
                    case ConvPlan.ToFloat:
                        return float.Parse(strValue);
                    case ConvPlan.ToInt32:
                        return int.Parse(strValue);
                    case ConvPlan.ToString:
                        return strValue;
                }
            }
            catch (Exception ex) //cannot manage problem
            {
                Console.WriteLine("Error : {0}", ex.GetType());
                return null;
            }
        }
        public override string ToString()
        {
            return ParType + " " + ParName;
        }
    }
    class MetAdapter
    {
        public Object moduleInstance;
        public MethodInfo metInfo;
        bool _conservativeForm;

        ParameterAdapter[] parAdapters;
        public MetAdapter(Object moduleInstance, MethodInfo metInfo)
        {
            this.moduleInstance = moduleInstance;
            this.metInfo = metInfo;

            //conservative form
            ParameterInfo[] pars = metInfo.GetParameters();
            if (pars.Length == 2 &&
                pars[0].ParameterType == typeof(HttpRequest) &&
                pars[1].ParameterType == typeof(HttpResponse))
            {
                _conservativeForm = true;
            }
            else
            {
                parAdapters = new ParameterAdapter[pars.Length];

                int i = 0;
                foreach (ParameterInfo p in pars)
                {
                    ParameterAdapter parAdapter = new ParameterAdapter(p.Name, p.ParameterType);
                    parAdapters[i] = parAdapter;
                    i++;
                }
                _conservativeForm = false;
            }
        }
        object[] PrepareInputArgs(HttpRequest req)
        {
            
            int j = parAdapters.Length;
            object[] pars = new object[j];
            
            
            for (int i = 0; i < j; ++i)
            {
                ParameterAdapter a = parAdapters[i];
                string inputValue = req.GetReqParameterValue(a.ParName);
                pars[i] = a.GetActualValue(inputValue);
                if (pars[i] == null)
                {
                    return null;
                }
            }
            return pars;
        }

        public void Invoke(HttpRequest req, HttpResponse resp)
        {
            //check parameter before invoke
            if (parAdapters != null)
            {
                int j = parAdapters.Length;
                object[] check = new object[j];
                if (j != req.ReqParameters.Length) //parameters
                {
                    resp.End("Please input only " + j + " parameters");
                    return;
                }
                for (int i = 0; i < j; ++i)
                {
                    ParameterAdapter a = parAdapters[i];
                    string inputValue = req.GetReqParameterValue(a.ParName); //value
                    check[i] = a.GetActualValue(inputValue); //type
                    if (inputValue == "") //wrong parameter
                    {
                        resp.End("Wrong parameter name or invalid input.");
                        return;

                    }
                    if (check[i] == null) //worng type
                    {
                        resp.End("Wrong input type.");
                        return;
                    }

                }
            }
            
            //object check = PrepareInputArgs(req);
            if (_conservativeForm)
            {
                this.metInfo.Invoke(
                    moduleInstance, new object[] { req, resp }
                );
            }
            else
            {
                
                
                
                //....prepare input 

                object result = metInfo.Invoke(moduleInstance, PrepareInputArgs(req));

                //prepare result
                resp.End(result.ToString());
            }
        }
    }
    //3.
    class MyModule
    {
        //4.
        [HttpMethod]
        public void Go(HttpRequest req, HttpResponse resp)
        {
            resp.End("go!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Say(HttpRequest req, HttpResponse resp)
        {
            resp.End("say!12345");
        }
    }
    class MyModule2
    {
        //4.
        [HttpMethod]
        public void Walk(HttpRequest req, HttpResponse resp)
        {
            resp.End("walk!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Fly(HttpRequest req, HttpResponse resp)
        {
            resp.End("fly");
        }
    }

    class MyModule3
    {
        //4.
        [HttpMethod]
        public void Go1(HttpRequest req, HttpResponse resp)
        {
            resp.End("go1!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Say1(HttpRequest req, HttpResponse resp)
        {
            resp.End("say1!7890");
        }
    }
    //5. 
    class MyAdvanceMathModule
    {
        [HttpMethod]
        public void CalculateSomething1(HttpRequest req, HttpResponse resp)
        {
            string s1_s = req.GetReqParameterValue("s1");
            string s2_s = req.GetReqParameterValue("s2");
            //..... 
            double result = CalculateSomething(double.Parse(s1_s), double.Parse(s2_s));


            //.....
            resp.End(result.ToString());
        }

        [HttpMethod]
        public double CalculateSomething(double s1, double s2)
        {
            return s1 + s2;
        }

        [HttpMethod]
        public double CalculateX(double s1, double s2)
        {
            return s1 * s2;
        }
    }
    
    class MMath1
    {

        [HttpMethod]
        public double CalculateSomething(double s1, double s2)
        {
            return s1 + s2;
        }

        [HttpMethod]
        public double CalculateX(double s1, double s2)
        {
            return s1 * s2;
        }
        [HttpMethod]
        public string RegisterNewUser(string username)
        {
            return "Hello" + username;
        }
    }
    
    class UserUnHxListEventArgs : EventArgs
    {
        //...
        public object undohxlist;
        
        
    }
    
    //public List<UserUnHxListEventArgs> undo = new List<UserUnHxListEventArgs>();
    class Undo_Data
    {
        
        public event EventHandler<UserUnHxListEventArgs> DataArrived;
        public event EventHandler<UserUnHxListEventArgs> LoadData;
        //4.
        [HttpMethod]
        public void Panel_Data(HttpRequest req, HttpResponse resp)
        {
            string content = req.GetBodyContentAsString();
            Console.WriteLine(req.HttpMethod);
            //UserUnHxListEventArgs evArgs = new UserUnHxListEventArgs();
            UserUnHxListEventArgs evArgs = new UserUnHxListEventArgs();
            if (DataArrived != null && req.HttpMethod.ToString() == "Post")
            {
                //UserUnHxListEventArgs evArgs = new UserUnHxListEventArgs();
                evArgs.undohxlist = content;//TODO: change to history list
                //evArgs.undohxlist = evArgs;
                DataArrived(this, evArgs);
                resp.End(content);
                return;

            } else if (req.HttpMethod.ToString() == "Get")
            {
                evArgs.undohxlist = null;
                LoadData(this, evArgs);
                if (evArgs.undohxlist == null)
                {
                    resp.End("No Data");
                    return;
                }
                resp.End((string)evArgs.undohxlist);
                //resp.End("?");
                return;
            }
            resp.End("?");

        }
    }
    //mobile
    class Kidshistory : EventArgs{
        public object about;
        public object vaccine;
        public object health;
        public object users;
        public object notification;
    }
    class About
    {
        public event EventHandler<Kidshistory> Comment_Arrived;
        public event EventHandler<Kidshistory> Load_comment;

        [HttpMethod]
        public void Comment(HttpRequest req, HttpResponse resp)
        {
            
            string content = req.GetBodyContentAsString();
            //Console.WriteLine(req.HttpMethod); 
            Kidshistory evArgs = new Kidshistory();
            //Comment_Arrived.DynamicInvoke(evArgs);
            if (Comment_Arrived != null && req.HttpMethod.ToString() == "Post")
            {
                evArgs.about = content; //comment
                Comment_Arrived(this, evArgs);
                resp.End("Success");
                
                return;

            }
            else if (req.HttpMethod.ToString() == "Get")
            {
                evArgs.about = null;
                Load_comment(this, evArgs);
                if (evArgs.about == null)
                {
                    resp.End("No Data");
                    return;
                }
                resp.Write((string)evArgs.about);
                resp.End();
                
                return;
            }
            resp.End("?");

        }
    }
    class Vaccine
    {
        
        public event EventHandler<Kidshistory> Load_vaccine;

        [HttpMethod]
        public void Load_list_vaccine(HttpRequest req, HttpResponse resp)
        {
            string content = req.GetBodyContentAsString();
            //Console.WriteLine(req.HttpMethod); 
            Kidshistory evArgs = new Kidshistory();
            if (req.HttpMethod.ToString() == "Post")
            {
                evArgs.vaccine = content; //comment
                Load_vaccine(this, evArgs);
                if ((string)evArgs.vaccine == "Fail")
                {
                    resp.Write("No data");
                } else
                {
                    resp.Write((string)evArgs.vaccine);
                }
                
                resp.End();
                return;

            }
            else if (req.HttpMethod.ToString() == "Get")
            {
               
                resp.End("Post only");
                return;
            }
            resp.End("?");
        }
    }
    class Users
    {
        public event EventHandler<Kidshistory> Login;
        [HttpMethod]
        public void Check(HttpRequest req, HttpResponse resp)
        {
            string content = req.GetBodyContentAsString();
            //Console.WriteLine(req.HttpMethod); 
            Kidshistory evArgs = new Kidshistory();
            if (req.HttpMethod.ToString() == "Post") //check
            {
                evArgs.users = content; //comment
                
                Login(this, evArgs);
                if (evArgs.users == null)
                {
                    resp.End("No Data");
                    return;
                }
                string contain = (string)evArgs.users; //success or fail
                
                if (contain.Contains("Success"))
                {
                    resp.Write((string)evArgs.users);
                    
                    resp.End();
                    //req.Dispose();

                    return;
                }
                resp.Write((string)evArgs.users);
                resp.End();
                return;

            }
            else if (req.HttpMethod.ToString() == "Get")
            {
                resp.End("?");


            }
            resp.End("?");
        }
    }
    class Notification
    {
        public event EventHandler<Kidshistory> Load_Appointment; //
        

        [HttpMethod]
        public void Get_Appoinement_Data(HttpRequest req, HttpResponse resp)
        {
            string content = req.GetBodyContentAsString();
            //Console.WriteLine(req.HttpMethod); 
            Kidshistory evArgs = new Kidshistory();
            if (Load_Appointment != null && req.HttpMethod.ToString() == "Post")
            {
                evArgs.notification = content; //comment
                Load_Appointment(this, evArgs);
                if (evArgs.notification == null)
                {
                    resp.End("Error");
                    return;
                }
                resp.Write((string)evArgs.notification);
                resp.End();
                return;

            }
            else if (req.HttpMethod.ToString() == "Get")
            {
                resp.End("Post only");
                return;
            }
            resp.End("?");
        }

    }
    class Health
    {
        public event EventHandler<Kidshistory> Load_age_gender;
        

        [HttpMethod]
        public void Age_Gender(HttpRequest req, HttpResponse resp)
        {
            string content = req.GetBodyContentAsString();
            //Console.WriteLine(req.HttpMethod); 
            Kidshistory evArgs = new Kidshistory();
            if (req.HttpMethod.ToString() == "Post") //check
            {
                evArgs.health = content; //health query

                Load_age_gender(this, evArgs);
                if (evArgs.health == null)
                {
                    resp.End("Error");
                    return;
                }
                resp.Write((string)evArgs.health);
                resp.End();       //return data
                return;

            }
            else if (req.HttpMethod.ToString() == "Get")
            {  
                resp.End("Post only");
                return;
            }
            resp.End("?");
        }
    }
}