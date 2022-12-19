using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDD
{
    class Sparepart : Item
	{
		String id_category;
		int stok;
		String description;

        public Sparepart(string id, string nama, string id_category, int stok, string description) : base(id, nama)
        {
            this.id_category = id_category;
            this.stok = stok;
            this.description = description;
        }

    }
}
