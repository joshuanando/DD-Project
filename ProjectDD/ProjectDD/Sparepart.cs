using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDD
{
    class Sparepart : Item
	{
		public String id_category { get; set; }
        public int stok { get; set; }
        public String description { get; set; }

        public Sparepart(string id, string nama, string id_category, int stok, string description) : base(id, nama)
        {
            this.id_category = id_category;
            this.stok = stok;
            this.description = description;
        }

    }
}
