using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDD
{
    class Tools : Item
    {
        public String id_categori { get; set; }
        public int status { get; set; }

        public Tools(string id, string nama, string id_categori, int status) : base(id, nama)
        {
            this.id_categori = id_categori;
            this.status = status;
        }
    }
}
