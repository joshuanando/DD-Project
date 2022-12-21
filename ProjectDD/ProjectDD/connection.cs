using System.Data.OracleClient;

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
            conn.Close();
            conn.Open();
        }

        public static void closeConn()
        {
            conn.Close();
        }
    }
}
