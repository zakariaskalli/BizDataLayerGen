using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizDataLayerGen.DataAccessLayer
{
    public class clsGeneralData
    {

        public static string[] GetAllApplications()
        {
            // Initialize a list to store the database names
            List<string> databaseNames = new List<string>();

            // Connection string to your database
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // SQL query to retrieve database names except 'master'
            string query = "SELECT name FROM sys.databases WHERE name != 'master'";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                // Open the connection
                connection.Open();

                // Execute the query and get the SqlDataReader
                SqlDataReader reader = command.ExecuteReader();

                // Check if the reader has rows
                if (reader.HasRows)
                {
                    // Read each row and add the name to the list
                    while (reader.Read())
                    {
                        databaseNames.Add(reader["name"].ToString());
                    }
                }

                // Close the reader
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle any errors
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection
                connection.Close();
            }

            // Convert the list to an array and return it
            return databaseNames.ToArray();
        }

        public static bool AddDataBaseToSQL(string backupFilePath, string databaseName)
        {
            bool IsAdd = false;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = $@"RESTORE Database @databaseName 
                                FROM DISK = @backupFilePath";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@databaseName", databaseName);
            Command.Parameters.AddWithValue("@backupFilePath", backupFilePath);



            try
            {
                connection.Open();

                int result = Command.ExecuteNonQuery();


                if (result > 0)
                    IsAdd = true;
                else
                    IsAdd = false;
            }
            finally
            {
                connection.Close();
            }

            return IsAdd;

        }

    }
}
