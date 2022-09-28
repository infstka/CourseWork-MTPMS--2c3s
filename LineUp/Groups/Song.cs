using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Groups
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int BandID { get; set; }
        public Song(int id,string name,int bandid)
        {
            ID = id;
            Name = name;
            BandID = bandid;
        }
    }
}
