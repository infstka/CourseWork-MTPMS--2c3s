using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Groups
{
    public class Member
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int BandID { get; set; }
        public Member(int id,string name,string image,int bandid)
        {
            ID = id;
            Name = name;
            Image = image;
            BandID = bandid;
        }
    }
}
