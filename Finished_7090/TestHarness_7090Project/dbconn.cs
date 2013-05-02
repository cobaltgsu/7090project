using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

	using System.Reflection;
	using System.Linq.Expressions;

namespace TestHarness_7090Project
{
    public class dbconn
    {

            public static void Conndb()
            {
                // 1. Instantiate the connection
                SqlConnection conn = new SqlConnection(
                    "Data Source=(local);Initial Catalog=2008GIS;Integrated Security=SSPI");

                SqlDataReader rdr = null;

                try
                {
                    // 2. Open the connection
                    conn.Open();

                    // 3. Pass the connection to a command object
                    SqlCommand cmd = new SqlCommand("Select * from projectoutput where Field3 BETWEEN -87 AND -83 AND Field4 BETWEEN 30 AND 31;", conn);

                    //
                    // 4. Use the connection
                    //

                    // get query results
                    rdr = cmd.ExecuteReader();


                    using (FileStream fs = new FileStream("C://test//queryoutput.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {


                            // print the CustomerID of each record
                            while (rdr.Read())
                            {
                                // get the results of each column
                                string spid = rdr["spid"].ToString();
                                string id = rdr["Field1"].ToString();
                                string month = rdr["Field3"].ToString();
                                string day = rdr["Field4"].ToString();
                                string x = rdr["Field5"].ToString();
                                
                                Console.Write("\n");
                                Console.WriteLine(rdr[1] + " -- " + rdr[6]);
                                sw.WriteLine(rdr[1] + " -- " + rdr[6]);

                            } sw.Close();
                        } 
                    }
                }
                finally
                {
                    // close the reader
                    if (rdr != null)
                    {
                        rdr.Close();
                    }

                    // 5. Close the connection
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }


    
}

   