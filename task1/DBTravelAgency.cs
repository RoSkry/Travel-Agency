using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace task1
{
    class DBTravelAgency
    {
        public string connectMaster { get; } = ConfigurationManager.ConnectionStrings["connectMaster"].ConnectionString;
        public string connectDB { get; } = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        //public List<User> users { get; set; }

        public DBTravelAgency()
        {
          
            //users = new List<User>();
        }


        public string CreateDB { get; } = "USE master;" +
"IF (SELECT name FROM master.sys.databases WHERE name LIKE 'TravelAgency') IS NULL " +
"BEGIN " +
"CREATE DATABASE TravelAgency;" +
"END " +
"ELSE " +
"ROLLBACK";
        public void CreateDataBase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectMaster))
                {

                    using (SqlCommand command = new SqlCommand(CreateDB, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }

                using (SqlConnection connection = new SqlConnection(connectDB))
                {
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = FileRead("1.txt"),
                        Connection = connection
                    };
                    connection.Open();
                    if (sqlCommand.ExecuteNonQuery() <= 0)
                    {
                        throw new Exception();
                    }



                    SqlCommand sqlCommand1 = new SqlCommand
                    {
                        CommandText = FileRead("2.txt"),
                        Connection = connection
                    };

                    sqlCommand1.ExecuteNonQuery();


                    SqlCommand sqlCommand2 = new SqlCommand
                    {
                        CommandText = FileRead("3.txt"),
                        Connection = connection
                    };

                    sqlCommand2.ExecuteNonQuery();

                    SqlCommand sqlCommand3 = new SqlCommand
                    {
                        CommandText = FileRead("4.txt"),
                        Connection = connection
                    };

                    sqlCommand3.ExecuteNonQuery();

                    SqlCommand sqlCommand4 = new SqlCommand
                    {
                        CommandText = FileRead("SP_AddTour.txt"),
                        Connection = connection
                    };

                    sqlCommand4.ExecuteNonQuery();

                    SqlCommand sqlCommand5 = new SqlCommand
                    {
                        CommandText = FileRead("SP_DellTour.txt"),
                        Connection = connection
                    };

                    sqlCommand5.ExecuteNonQuery();

                    SqlCommand sqlCommand6 = new SqlCommand
                    {
                        CommandText = FileRead("SP_UpdateTour.txt"),
                        Connection = connection
                    };

                    sqlCommand6.ExecuteNonQuery();

                }
            }

            catch (SqlException ex)
            {
                if (!ex.Message.Contains("ROLLBACK"))
                {
                    throw;
                }
            }

        }


        public User SelectRole(string UserLogin, string UserPassword)
        {
            User user = new User();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = $"select * from UserPeson where UserLogin='{UserLogin}' and userPassword='{UserPassword}'",
                        Connection = sqlConnection
                    };
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            user.Id = (int)sqlDataReader[0];
                            user.LastName = sqlDataReader[3].ToString();
                            user.FirstName = sqlDataReader[4].ToString();
                            user.BirthDate = Convert.ToDateTime(sqlDataReader[5]);
                            user.Userlogin = sqlDataReader[1].ToString();
                            user.UserPassword = sqlDataReader[2].ToString();
                            user.Role= sqlDataReader[6].ToString();
                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public string FileRead(string fileName)
        {
            return File.ReadAllText(fileName, Encoding.Default);
        }

       
    }
}
