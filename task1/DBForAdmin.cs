using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace task1
{
    class DBForAdmin
    {
        public string connectDB { get; } = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        public List<TourInfo> tours { get; set; }
        public List<User> users { get; set; }

        public DBForAdmin()
        {
            tours = new List<TourInfo>();
            users = new List<User>();
        }
        public void SelectAdminAll()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {

                    string zapros = "select T.TourCountry,T.TourTown,Tim.TourBegin,Tim.TourEnd,T.MaxSeats,Tim.CurrentSeats,S.DiscountId,T.Price,T.id from dbo.Tour T join dbo.TourTime Tim on T.TourTimeId = Tim.Id join dbo.Seasons S on S.Id = T.SeasonId";

                    string zapros1 = "select u.LastName,u.FirstName,u.BirthDate,u.UserLogin,u.UserPassword,u.Id from dbo.UserPeson u where u.RoleName='User'";
                    sqlConnection.Open();

                    SqlCommand sqlComand = new SqlCommand
                    {
                        CommandText = zapros,
                        Connection = sqlConnection
                    };

                    SqlCommand sqlComand1 = new SqlCommand
                    {
                        CommandText = zapros1,
                        Connection = sqlConnection
                    };

                    using (SqlDataReader sqlDataReader1 = sqlComand1.ExecuteReader())
                    {
                        while (sqlDataReader1.Read())
                        {
                            User user = new User();
                            user.LastName = sqlDataReader1[0].ToString();
                            user.FirstName = sqlDataReader1[1].ToString();
                            user.BirthDate = Convert.ToDateTime(sqlDataReader1[2]);
                            user.Userlogin = sqlDataReader1[3].ToString();
                            user.UserPassword = sqlDataReader1[4].ToString();
                            user.Id = (int)sqlDataReader1[5];
                            users.Add(user);
                        }
                    }


                    using (SqlDataReader sqlDataReader = sqlComand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            TourInfo tour = new TourInfo();
                            tour.TourCountry = sqlDataReader[0].ToString();
                            tour.TourCity = sqlDataReader[1].ToString();
                            tour.TourBegin = Convert.ToDateTime(sqlDataReader[2]);
                            tour.TourEnd = Convert.ToDateTime(sqlDataReader[3]);
                            tour.MaxSeats = (int)sqlDataReader[4];
                            tour.CurrentSeats = (int)sqlDataReader[5];
                            tour.DiscountId = (int)sqlDataReader[6];
                            tour.Price = Convert.ToDouble(sqlDataReader[7]);
                            tour.Id = (int)sqlDataReader[8];
                            tours.Add(tour);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void AddTour(TourInfo tour)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_addtour",
                        CommandType = CommandType.StoredProcedure,
                        Connection = sqlConnection
                    };

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourCountry",
                        Value = tour.TourCountry,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourCity",
                        Value = tour.TourCity,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourBegin",
                        Value = tour.TourBegin,
                        SqlDbType = SqlDbType.Date
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourEnd",
                        Value = tour.TourEnd,
                        SqlDbType = SqlDbType.Date,
                    };

                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@MaxSeats",
                        Value = tour.MaxSeats,
                        SqlDbType = SqlDbType.Int,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@CurrentSeats",
                        Value = tour.CurrentSeats,
                        SqlDbType = SqlDbType.Int,

                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Price",
                        Value = tour.Price,
                        SqlDbType = SqlDbType.Money,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Task completed succesfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteTour(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_deltour",
                        CommandType = CommandType.StoredProcedure,
                        Connection = sqlConnection
                    };
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id,
                        SqlDbType = SqlDbType.Int
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Console.WriteLine("Command completed succesfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateTour(TourInfo tour)
        {
            try

            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_updatetour",
                        CommandType = CommandType.StoredProcedure,
                        Connection = sqlConnection
                    };

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourCountry",
                        Value = tour.TourCountry,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourCity",
                        Value = tour.TourCity,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourBegin",
                        Value = tour.TourBegin,
                        SqlDbType = SqlDbType.Date
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourEnd",
                        Value = tour.TourEnd,
                        SqlDbType = SqlDbType.Date,
                    };

                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@MaxSeats",
                        Value = tour.MaxSeats,
                        SqlDbType = SqlDbType.Int,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        Value = tour.Id,
                        SqlDbType = SqlDbType.Int,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);



                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@CurrentSeats",
                        Value = tour.CurrentSeats,
                        SqlDbType = SqlDbType.Int,

                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Price",
                        Value = tour.Price,
                        SqlDbType = SqlDbType.Money,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Task completed succesfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddUser(User user)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "insert into dbo.UserPeson values (@UserLogin,@UserPassword,@LastName,@FirstName,@BirthDate,'User')",
                        Connection = sqlConnection
                    };

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@LastName",
                        Value = user.LastName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        Value = user.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@BirthDate",
                        Value = user.BirthDate,
                        SqlDbType = SqlDbType.Date,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@UserLogin",
                        Value = user.Userlogin,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@UserPassword",
                        Value = user.UserPassword,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);


                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Task completed succesfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = $"delete from UserPeson where id=@id",
                        Connection = sqlConnection
                    };
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id,
                        SqlDbType = SqlDbType.Int
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Console.WriteLine("Command completed succesfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "update UserPeson set UserLogin= @UserLogin, UserPassword=@UserPassword,LastName=@LastName,FirstName=@FirstName,BirthDate=@BirthDate where id=@id",
                        Connection = sqlConnection
                    };

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@UserLogin",
                        Value = user.Userlogin,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@UserPassword",
                        Value = user.UserPassword,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);


                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@LastName",
                        Value = user.LastName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        Value = user.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@BirthDate",
                        Value = user.BirthDate,
                        SqlDbType = SqlDbType.Date,
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = user.Id,
                        SqlDbType = SqlDbType.Int
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Task completed succesfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
