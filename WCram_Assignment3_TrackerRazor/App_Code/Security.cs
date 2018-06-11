using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Security
/// </summary>
public class Security
{
    public int Id { get; set; }
    public string Title { get; set; }

}

public class SecurityManager
{
    // Returns a list of security levels
    public List<Security> SecurityList(string ConStr)
    {
        List<Security> list = new List<Security>();

        using(SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SecurityLevelList";
                
                using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        list.Add(new Security()
                        {
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[0]),
                            Title = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                        });
                    }

                }

            }


        }
        return list;
    } // End SecurityList

}