using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDD
{
    class Item
    {
        public String id { get; set; }
        public String nama { get; set; }
        public String status;

        public Item(string id, string nama)
        {
            this.id = id;
            this.nama = nama;
        }

        public int getQty()
        {
            if (status.ToLower() == "available")
            {
                return 1;
            }
            else if (status.ToLower() == "not available")
            {
                return 0;
            }
            else
            {
                try
                {
                    return Int32.Parse(status.Substring(1).Trim());
                }
                catch (Exception)
                {
                }
                return -1;
            }
        }

        public bool isAvailable()
        {
            if (status.ToLower() == "available")
            {
                return true;
            }
            else if (status.ToLower() == "not available")
            {
                return false;
            }
            try
            {
                int qty = Int32.Parse(status.Substring(1).Trim());
                if (qty>0)
                {
                    return true;
                }
            } catch (Exception)
            {
            }
            return false;
        }
    }
}
