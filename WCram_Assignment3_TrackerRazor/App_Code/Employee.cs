using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for Employee
/// </summary>
public class Employee
{
    public string TutorID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public int SecurityId { get; set; }

} // End Employee

public class EmployeeManager
{
    // Calls a procedure that checks their username, password and security level
    public bool AdminLogin(string ConStr, string id, string password)
    {

        using(SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AdminLogin";

                cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 10).Value = id;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 30).Value = password;


                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                // Executes stored procedure
                cmd.ExecuteNonQuery();

                return (Convert.ToInt16(returnValue.Value) > 0) ? true : false;
            }


        }

    }

    // Calls a procedure that just checks their user name and password
    public bool TutorLogin(string ConStr, string id, string password)
    {

        using (SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "TutorLogin";

                cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 10).Value = id;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 30).Value = password;


                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                // Executes stored procedure
                cmd.ExecuteNonQuery();

                return (Convert.ToInt16(returnValue.Value) > 0) ? true : false;
            }


        }

    }

    // Returns all employees
    public List<Employee> DisplayEmployees(string ConStr)
    {

            List<Employee> list = new List<Employee>();

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "AllEmployeesList";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            list.Add(new Employee()
                            {
                                TutorID = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                                FirstName = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                                LastName = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                                Password = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                                SecurityId = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[4])

                            });
                        }

                    }

                }


            }
            return list;
    } // End DisplayEmployees

    // Adds a new employee
    public void AddEmployee(string ConStr, string id, string first, string last, string pass, int secId)
    {

        using(SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddEmployee";

                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 10).Value = id;
                cmd.Parameters.Add("@first", SqlDbType.NVarChar, 20).Value = first;
                cmd.Parameters.Add("@last", SqlDbType.NVarChar, 20).Value = last;
                cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 30).Value = pass;
                cmd.Parameters.Add("@securityId", SqlDbType.Int).Value = secId;

                cmd.ExecuteNonQuery();
            }

        }

    } // End AddEmployee

} // End EmployeeManager