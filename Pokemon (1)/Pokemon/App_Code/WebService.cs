using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for WebService
/// </summary>
/// [WebService]  
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    private const string DBNAME = "DB_9C4DA8_pokemon";
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string getAllApps()
    {
        SqlConnection con = ManageConnect.OpenConnection();
        List<Apps> listApps = new List<Apps>();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.APPS", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Apps app = new Apps();
                    int pkID; ;
                    app.PkID = Int32.TryParse(row[AppsColumn.pkID].ToString(), out pkID) ? pkID.ToString() : "0";
                    app.NAMEAPP = row[AppsColumn.NAMEAPP].ToString();
                    app.DESCRIPTION = row[AppsColumn.DESCRIPTION].ToString();
                    listApps.Add(app);
                }
            }
        }
        catch
        {
            return "";
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
        return new JavaScriptSerializer().Serialize(listApps);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string getAllAssetsByMacID(String macid)
    {
        SqlConnection con = ManageConnect.OpenConnection();
        List<Apps> listApps = new List<Apps>();
        try
        {
            String command = "SELECT " + DBNAME + ".dbo.ASSETS.pkID," + DBNAME + ".dbo.APPS.NAMEAPP" + " FROM " + DBNAME + ".dbo.ASSETS" +
            " INNER JOIN " + DBNAME + ".dbo.PHONES ON " + DBNAME + ".dbo.ASSETS.PhoneID =" + DBNAME + ".dbo.PHONES.pkID INNER JOIN " +
            DBNAME + ".dbo.APPS ON " + DBNAME + ".dbo.ASSETS.APPSID = " + DBNAME + ".dbo.APPS.pkID WHERE "
            + DBNAME + ".dbo.PHONES.MACID ='" + macid + "'";
            SqlCommand myCommand = new SqlCommand(command, con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Apps app = new Apps();
                    int pkID; ;
                    app.PkID = Int32.TryParse(row[AppsColumn.pkID].ToString(), out pkID) ? pkID.ToString() : "0";
                    app.NAMEAPP = row[AppsColumn.NAMEAPP].ToString();
                    listApps.Add(app);
                }
            }
        }
        catch
        {
            return "";
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
        return new JavaScriptSerializer().Serialize(listApps);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string getAllPhones()
    {
        SqlConnection con = ManageConnect.OpenConnection();
        List<Phones> listPhones = new List<Phones>();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.PHONES", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Phones phone = new Phones();
                    int pkID; ;
                    phone.PkID = Int32.TryParse(row[PhonesColumn.pkID].ToString(), out pkID) ? pkID.ToString() : "0";
                    phone.MACID = row[PhonesColumn.MACID].ToString();
                    phone.PHONENR = row[PhonesColumn.PHONENR].ToString();
                    listPhones.Add(phone);
                }
            }
        }
        catch
        {
            return "";
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
        return new JavaScriptSerializer().Serialize(listPhones);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string getAllGlobals()
    {
        SqlConnection con = ManageConnect.OpenConnection();
        List<Global> listGlobals = new List<Global>();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.GLOBAL", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Global global = new Global();
                    int pkID; ;
                    global.AdminID = Int32.TryParse(row[GlobalColumn.adminID].ToString(), out pkID) ? pkID.ToString() : "0";
                    global.NAME = row[GlobalColumn.NAME].ToString();
                    global.USERNAME = row[GlobalColumn.USERNAME].ToString();
                    global.PASSCLAVE = row[GlobalColumn.PASSCLAVE].ToString();
                    listGlobals.Add(global);
                }
            }
        }
        catch
        {
            return "";
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
        return new JavaScriptSerializer().Serialize(listGlobals);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public String addNewOrUpdatePhone(String MACID, String PHONENR)
    {
        SqlConnection con = ManageConnect.OpenConnection();
        Result result = new Result();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.PHONES WHERE mACID = '" + MACID + "'", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count == 0)
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = con;
                    comm.CommandType = CommandType.Text;
                    string cmdInsert = "INSERT INTO " + DBNAME + ".dbo.PHONES" + " (MACID,PHONENR) VALUES (@MACID, @PHONENR)";
                    comm.CommandText = cmdInsert;

                    comm.Parameters.AddWithValue("@MACID", MACID);
                    comm.Parameters.AddWithValue("@PHONENR", PHONENR);
                    try
                    {
                        comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        comm.CommandText = "SELECT @@IDENTITY";

                        int identity = Convert.ToInt32(comm.ExecuteScalar());
                        if (identity > 0)
                        {
                            result.Succes = true;
                            result.Message = "Insert new success : " + (identity + 1).ToString();
                        }
                    }
                    catch (SqlException e)
                    {
                        result.Succes = false;
                        result.Message = e.ToString();
                    }
                }
            }
            else if (table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                int pkID;
                pkID = Int32.TryParse(row[PhonesColumn.pkID].ToString(), out pkID) ? pkID : 0;
                string cmdUpdate = "UPDATE " + DBNAME + ".dbo.PHONES" + " SET PHONENR = @PHONENR WHERE mACID = '" + MACID + "'";

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = con;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = cmdUpdate;
                    comm.Parameters.AddWithValue("@PHONENR", PHONENR);
                    try
                    {
                        comm.ExecuteNonQuery();
                        result.Succes = true;
                        result.Message = "Update success : " + pkID;
                    }
                    catch (SqlException e)
                    {
                        result.Succes = false;
                        result.Message = e.ToString();
                    }
                }
            }
            else
            {
                result.Succes = false;
                result.Message = "Can not insert or update";
            }
        }
        catch
        {
            result.Succes = false;
            result.Message = "Can not insert or update";
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
        return new JavaScriptSerializer().Serialize(result);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public String updateAssetByAssetID(string pkID, string DATEPROSPECT, string GPSLAT, string GPSLON,
        string DATEINSTALL, string MACID, string NAMEAPP)
    {
        Result result = new Result();
        int phonePKID = getIDPhoneByMacid(MACID);
        if (phonePKID > 0)
        {
        }
        else
        {
            result.Succes = false;
            result.Message = "Not exits macid"; ;
        }
        int appID = getAppIDByAppName(NAMEAPP);
        if (appID > 0)
        {
        }
        else
        {
            result.Succes = false;
            result.Message = "Not exits appname"; ;
        }
        if (appID > 0 && phonePKID > 0)
        {
            SqlConnection con = ManageConnect.OpenConnection();
            int pkIDTemp;
            pkIDTemp = Int32.TryParse(pkID, out pkIDTemp) ? pkIDTemp : 0;
            string cmdUpdate = "UPDATE " + DBNAME + ".dbo.ASSETS " + " SET DATEPROSPECT = @DATEPROSPECT,GPSLAT=@GPSLAT,"
            + "GPSLON=@GPSLON,DATEINSTALL=@DATEINSTALL,PhoneID=@PhoneID,APPSID=@APPSID WHERE pkID = " + pkIDTemp;

            using (SqlCommand comm = new SqlCommand())
            {
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = cmdUpdate;
                comm.Parameters.AddWithValue("@DATEPROSPECT", "2000-01-01 00:00:00.00");
                comm.Parameters.AddWithValue("@GPSLAT", GPSLAT);
                comm.Parameters.AddWithValue("@GPSLON", GPSLON);
                if (DATEINSTALL.Equals("") || DATEINSTALL.Equals("-1") || DATEINSTALL.Equals("0"))
                {
                    comm.Parameters.AddWithValue("@DATEINSTALL", "2000-01-01 00:00:00.00");
                }
                else
                {
                    comm.Parameters.AddWithValue("@DATEINSTALL", DATEINSTALL);
                }
                comm.Parameters.AddWithValue("@PhoneID", phonePKID);
                comm.Parameters.AddWithValue("@APPSID", appID);

                try
                {
                    comm.ExecuteNonQuery();
                    result.Succes = true;
                    result.Message = "Update success : " + pkID;
                }
                catch (SqlException e)
                {
                    result.Succes = false;
                    result.Message = e.ToString();
                }
                finally
                {
                    ManageConnect.CloseConnection(con);
                }
            }
        }
        return new JavaScriptSerializer().Serialize(result);
    }

    public int getIDPhoneByMacid(string macid)
    {
        SqlConnection con = ManageConnect.OpenConnection();
        Result result = new Result();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.PHONES WHERE mACID = '" + macid + "'", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                int pkID;
                pkID = Int32.TryParse(row[PhonesColumn.pkID].ToString(), out pkID) ? pkID : 0;
                if (pkID > 0)
                    return pkID;
                else return 0;
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
    }

    public int getAppIDByAppName(string appName)
    {
        SqlConnection con = ManageConnect.OpenConnection();
        Result result = new Result();
        try
        {
            SqlCommand myCommand = new SqlCommand("Select * from " + DBNAME + ".dbo.APPS WHERE NAMEAPP = '" + appName + "'", con);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                int pkID;
                pkID = Int32.TryParse(row[AppsColumn.pkID].ToString(), out pkID) ? pkID : 0;
                if (pkID > 0)
                    return pkID;
                else return 0;
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }
        finally
        {
            ManageConnect.CloseConnection(con);
        }
    }
}

