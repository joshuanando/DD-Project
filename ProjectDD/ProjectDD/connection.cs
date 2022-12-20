using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace ProjectDD
{
    class connection
    {
        public static OracleConnection conn = null;
        public static string cabangnow = "";

        public static void addConn(OracleConnection con)
        {
            conn = con;
        }

        public static void openConn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public static void closeConn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
