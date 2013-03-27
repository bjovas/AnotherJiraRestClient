using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AspnetSampleApp
{
    public class DataBaseStuff
    {
        public int GetUserIdFromUsername(string username)
        {
            // In this method we would read to from our database. Let us fake a dead connection
 
            SqlConnection conn = new SqlConnection("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");
            conn.Open();

            // do some query

            conn.Close();

            return 11443; // this will of course never happen.
        }
    }
}