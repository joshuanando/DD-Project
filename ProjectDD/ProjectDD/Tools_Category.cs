using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDD
{
    class Tools_Category
    {
        public string id { get; set; }
        public string nama { get; set; }

        public Tools_Category(string id, string namaKategori)
        {
            this.id = id;
            this.nama = namaKategori;
        }

        public override string ToString()
        {
            return nama;
        }
    }
}
