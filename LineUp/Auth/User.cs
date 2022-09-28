using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Auth
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User(int id,string login,string password)
        {
            ID = id;
            Login = login;
            Password = password;
        }
    }
}
