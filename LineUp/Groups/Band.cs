using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Groups
{
    public class Band
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Ganre { get; set; }
        public int Rate { get; set; }
        public string Image { get; set; }
        public Band(int id, string name, string ganre, int rate,string image)
        {
            ID = id;
            Name = name;
            Ganre = ganre;
            Rate = rate;
            Image = image;
        }
    }
}
