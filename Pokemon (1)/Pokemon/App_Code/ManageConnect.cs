using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;


public class ManageConnect
{
    /// <summary>
    /// Mở kết nối đến cơ sở dữ liệu
    /// </summary>
    /// <returns>Opened Connection if success. Otherwise, return null</returns>
    public static SqlConnection OpenConnection()
    {
        try
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["ConnectionStringPublic"];
            SqlConnection connection = new SqlConnection(settings.ConnectionString);
            connection.Open();
            return connection;
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception.ToString());
            return null;
        }
    }

    /// <summary>
    /// Đóng kết nối đến cơ sở dữ liệu
    /// </summary>
    /// <param name="connection"></param>
    public static void CloseConnection(SqlConnection connection)
    {
        if (connection != null)
        {
            connection.Close();
        }
    }
}
