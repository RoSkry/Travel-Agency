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
    class DBForUsers
    {
        public string connectDB { get; } = ConfigurationManager.ConnectionStrings["connectDB"].ConnectionString;
        public List<TouristInfo> tourists { get; set; }
        public List<TourInfo> tours { get; set; }
        public List<Discount> discounts { get; set; }
        public DBForUsers()
        {
            tourists = new List<TouristInfo>();
            tours = new List<TourInfo>();
            discounts = new List<Discount>();
        }

        public void SelectAll()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    string zapros = "select T.FirstName,T.LastName, doc.Serial,doc.Number,ph.PhoneNumber,pay.TourId[TourId],pay.DiscountId, pay.TotalSum,T.Id from dbo.Tourists T join dbo.Documents Doc on T.Id = Doc.TouristId join dbo.TouristsPhones ph on t.Id = ph.TouristId Join dbo.Payment pay on pay.TouristId = t.Id";
                    string zapros1 = "select T.TourCountry,T.TourTown,Tim.TourBegin,Tim.TourEnd,T.MaxSeats,Tim.CurrentSeats,S.DiscountId,T.Price,T.id from dbo.Tour T join dbo.TourTime Tim on T.TourTimeId = Tim.Id join dbo.Seasons S on S.Id = T.SeasonId";
                    string zapros2 = "select d.DiscountType,d.Number,d.Id from dbo.Discounts d ";

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
                    SqlCommand sqlComand2 = new SqlCommand
                    {
                        CommandText = zapros2,
                        Connection = sqlConnection
                    };

                    using (SqlDataReader sqlDataReader = sqlComand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            TouristInfo tourist = new TouristInfo();
                            tourist.FirstName = sqlDataReader[0].ToString();
                            tourist.LastName = sqlDataReader[1].ToString();
                            tourist.Serial = sqlDataReader[2].ToString();
                            if (string.IsNullOrWhiteSpace(tourist.Serial)) tourist.Serial = "-";
                            tourist.Number = sqlDataReader[3].ToString();
                            tourist.PhoneNumber = sqlDataReader[4].ToString();
                            tourist.TourId = (int)sqlDataReader[5];
                            tourist.DiscountId = (int)sqlDataReader[6];
                            tourist.TotalSum = Convert.ToDouble(sqlDataReader[7]);
                            tourist.Id = (int)sqlDataReader[8];
                            tourists.Add(tourist);
                        }


                    }
                    using (SqlDataReader sqlDataReader = sqlComand1.ExecuteReader())
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
                    using (SqlDataReader sqlDataReader = sqlComand2.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Discount discount = new Discount();
                            discount.Type = sqlDataReader[0].ToString();
                            discount.Number = (int)sqlDataReader[1];
                            discount.Id = (int)sqlDataReader[2];
                            discounts.Add(discount);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public double TourPrice(int TourId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = $"Select Price from Tour where id={TourId}",
                        Connection = sqlConnection
                    };
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            return Convert.ToDouble(sqlDataReader[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        public int SelectDiscount(int DiscountId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = $"Select Number from Discounts where id={DiscountId}",
                        Connection = sqlConnection
                    };
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            return (int)sqlDataReader[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }


        public void AddTourist(TouristInfo tourist)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_addTourist",
                        CommandType = CommandType.StoredProcedure,
                        Connection = sqlConnection
                    };

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Lastname",
                        Value = tourist.LastName,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        Value = tourist.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourId",
                        Value = tourist.TourId,
                        SqlDbType = SqlDbType.Int
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@DiscountId",
                        Value = tourist.DiscountId,
                        SqlDbType = SqlDbType.Int,

                    };

                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TotalSum",
                        Value = tourist.TotalSum,
                        SqlDbType = SqlDbType.Float,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Serial",
                        Value = tourist.Serial,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Number",
                        Value = tourist.Number,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@PhoneNumber",
                        Value = tourist.PhoneNumber,
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

        public void DeleteTourist(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_delrourist",
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

        public void UpdateTourist(TouristInfo tourist)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectDB))
                {

                    SqlCommand sqlCommand = new SqlCommand
                    {
                        CommandText = "sp_UpdateTourist",
                        CommandType = CommandType.StoredProcedure,
                        Connection = sqlConnection
                    };



                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Lastname",
                        Value = tourist.LastName,
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        Value = tourist.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TourId",
                        Value = tourist.TourId,
                        SqlDbType = SqlDbType.Int
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@DiscountId",
                        Value = tourist.DiscountId,
                        SqlDbType = SqlDbType.Int,

                    };

                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        Value = tourist.Id,
                        SqlDbType = SqlDbType.Int,

                    };

                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@TotalSum",
                        Value = tourist.TotalSum,
                        SqlDbType = SqlDbType.Float,

                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Serial",
                        Value = tourist.Serial,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@Number",
                        Value = tourist.Number,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = "@PhoneNumber",
                        Value = tourist.PhoneNumber,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50
                    };
                    sqlCommand.Parameters.Add(sqlParameter);
                    sqlConnection.Open();
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
