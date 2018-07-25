using System;
using System.Collections.Generic;
using System.Text;

namespace AppLogin.Models
{
    public class Token
    {
        public int ID { get; set; }
        public String Access_token { get; set; }
        public String Error_description { get; set; }
        public DateTime Expire_date { get; set; }

        public Token() { }

    }
}
