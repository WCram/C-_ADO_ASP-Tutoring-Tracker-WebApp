using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


public class Appointment
{
    public int Id { get; set; }
    public string TutorId { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string Subject { get; set; }
    public string Location { get; set; }
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
    public int Active { get; set; }

}

public class AppointmentManager
{

    // Enum to Choose which appointments to display
    public enum ShowAppointments { ALL, ACTIVE, INACTIVE }

    // Returns a list depending on ShowAppointments
    public List<Appointment> DisplayAppointments(string ConStr, string tutorID, ShowAppointments show = 0)
    {
        List<Appointment> list = new List<Appointment>();

        using (SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                switch (show)
                {
                    case ShowAppointments.ALL:
                        cmd.CommandText = "AppointmentsByTutor";
                        break;
                    case ShowAppointments.ACTIVE:
                        cmd.CommandText = "AppointmentsByTutorActive";

                        break;
                    case ShowAppointments.INACTIVE:
                        cmd.CommandText = "AppointmentsByTutorInactive";

                        break;
                }

                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 10).Value = tutorID;

                // Populate the mapping class to return and display list
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        list.Add(new Appointment()
                        {
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[0]),
                            TutorId = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            StudentId = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            StudentName = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            Subject = ds.Tables[0].Rows[i].ItemArray[4].ToString(),
                            Location = ds.Tables[0].Rows[i].ItemArray[5].ToString(),
                            Start = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            Finish = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[7].ToString()),
                            Active = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[8])

                        });
                    }

                }

            }

        }

        return list;
    }

    // Display Appoints by date, filter
    public List<Appointment> DisplayAppointmentsByDate(string ConStr, string tutorID, DateTime start, DateTime end, ShowAppointments show = 0)
    {
        List<Appointment> list = new List<Appointment>();

        using (SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                switch (show)
                {
                    case ShowAppointments.ALL:
                        cmd.CommandText = "DisplayAllByDate";
                        break;
                    case ShowAppointments.ACTIVE:
                        cmd.CommandText = "DisplayAllByDate";
                        break;
                    case ShowAppointments.INACTIVE:
                        cmd.CommandText = "DisplayInactiveByDate";

                        break;
                }

                // Passing Parameters
                cmd.Parameters.Add("@tutorId", SqlDbType.NVarChar, 10).Value = tutorID;
                cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = start;
                cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = end;


                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        list.Add(new Appointment()
                        {
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[0]),
                            TutorId = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            StudentId = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            StudentName = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            Subject = ds.Tables[0].Rows[i].ItemArray[4].ToString(),
                            Location = ds.Tables[0].Rows[i].ItemArray[5].ToString(),
                            Start = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            Finish = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[7].ToString()),
                            Active = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[8])

                        });
                    }

                }

            }

        }

        return list;
    }

    // Starts a new appointment under current logged in user
    public void StartAppointment(string ConStr, string tutorId, string studentId, string studentName, string subject, string location)
    {

        using(SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "StartAppointment";

                cmd.Parameters.Add("@tutorId", SqlDbType.NVarChar, 10).Value = tutorId;
                cmd.Parameters.Add("@studentId", SqlDbType.NVarChar, 10).Value = studentId;
                cmd.Parameters.Add("@studentName", SqlDbType.NVarChar, 40).Value = studentName;
                cmd.Parameters.Add("@subject", SqlDbType.NVarChar, 20).Value = subject;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar, 20).Value = location;

                cmd.ExecuteNonQuery();
            }


        }

    } // End StartAppointment()

    // Ends the appointment and updates the table
    public void EndAppointment(string ConStr, int id, string tutorId)
    {

        using(SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "FinishAppointment";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@tutorId", SqlDbType.NVarChar, 10).Value = tutorId;

                cmd.ExecuteNonQuery();


            }
        }

    }

    // Deletes specific appointment
    public void DeleteAppointment(string ConStr, int id, string tutorId)
    {
        using (SqlConnection con = new SqlConnection(ConStr))
        {
            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteAppointment";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@tutorId", SqlDbType.NVarChar, 10).Value = tutorId;

                cmd.ExecuteNonQuery();


            }
        }
    }


} // End AppointmentManager