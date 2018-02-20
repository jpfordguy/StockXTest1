using System;
using Npgsql;


// Here is the main database access module.  There are a few caveats to this that I will highlight here.
//
// 1) Normally, I'd put the updates in a stored procedute, mainly for performance as well as  commits and rollbacks.
//
// 2) According to the Postgres documentation, the uuid does not have an auto-generation feature.  So, I create a GUID here, turn it into a string, and use that.
//
// 3) I probably could have made the size table without a primary key, but I'm not a Postgres expert (by any stretch of the imagination), so I went with what I 
//    knew would work.
//
// 4) On an update to the database, I look for the name of the shoe, and if I can't find it, add it to the name master table.  Then I add the size.
// 
// 5) When I get the sizes, I probably could have done this with either a join or a foreign key, but, I don't want to be here all night reseraching that for a 
//    simple test like this.

namespace StockXTest1
{
    class PostgresInterface : IDisposable
    {

        private NpgsqlConnection _conn = null;
        private string _lastError = "";
        private bool   _disposed = false;
        private const string _dbName = "stockxtest";


        public PostgresInterface(string server,  int port,  string userid, string password)
        {
            if (server == null)
            {
                throw new Exception("Bad / invalid value for server.");
            }
            if (server.Trim() == "")
            {
                throw new Exception("Bad / invalid value for server.");
            }
            if (port < 1)
            {
                throw new Exception("Bad / invalid value for port.");
            }
            if (userid == null)
            {
                throw new Exception("Bad / invalid value for userid.");
            }
            if (userid.Trim()== "")
            {
                throw new Exception("Bad / invalid value for userid.");
            }
            if (password == null)
            {
                throw new Exception("Bad / invalid value for password.");
            }
            if (password.Trim() == "")
            {
                throw new Exception("Bad / invalid value for password.");
            }

            string tmpstr = "Server=" + server.Trim() + "; Port=" + port.ToString() + "; User Id=" + userid.Trim() + "; Password=" + password.Trim() + "; Database=" + _dbName + ";";

            _conn = new NpgsqlConnection(tmpstr);
            try
            {
                _conn.Open();
            }
            catch
            {
                throw new Exception("Unable to open database");
            }
        }



        public string LastError
        {
            get
            {
                return _lastError;
            }
            private set
            {
                _lastError = value;
            }

        }

        public bool Update(string name, int size)
        {
            ClearError();
            string shoeid = GetId(name);
            if(shoeid == "")
            {
                return false;
            }
            string id = Guid.NewGuid().ToString("N");
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = "INSERT INTO ShoeSizes(SizeId,Id,Size) VALUES ('" + id + "', '" + shoeid + "', " + size.ToString() + ")";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                LastError = "Error updating table: " + ex.Message;
                return false;
            }
            return true;
        }

        public double GetSize(string name)
        {
            double ret = -1;
            ClearError();
            string shoeid = GetId(name);
            if (shoeid == "")
            {
                return -1;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader rdr = null;
            cmd.Connection = _conn;
            cmd.CommandText = "SELECT AVG(Size) FROM ShoeSizes WHERE Id = '" + shoeid + "'";
            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                LastError = "Error getting average of shoe sizes: " + ex.Message;
                return ret;
            }
            if (rdr == null)
            {
                LastError = "Error getting average of shoe sizes - no data returned";
                return ret;
            }
            if(!rdr.Read())
            {
                LastError = "Error getting average of shoe sizes - error getting return value.";
                rdr.Close();
                rdr = null;
                return ret;
            }
            ret = rdr.GetDouble(0);
            rdr.Close();
            rdr = null;
            return ret;
        }


        private string GetId(string name)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader rdr = null;
            string ret = "";
            cmd.Connection = _conn;
            cmd.CommandText = "SELECT Id FROM ShoeNames WHERE Name='" + name + "'";
            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch
            {
                LastError = "Unable to get namer id from database.";
                cmd.Dispose();
                return "";
            }
            if(rdr != null)
            {
                if(rdr.Read())
                {
                    ret = rdr.GetString(0);
                }
                rdr.Close();
                rdr = null;
                if(ret != null)
                {
                    if(ret != "")
                    {
                        return ret;
                    }
                }
            }
            ret = Guid.NewGuid().ToString("N");
            cmd = new NpgsqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = "INSERT INTO ShoeNames (Name, Id) VALUES ('" + name + "', '" + ret + "')";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                LastError = "Unable to add new shoe name to the database: " + ex.Message;
                cmd.Dispose();
                return "";
            }
            cmd.Dispose();
            return ret;
        }


        private void ClearError()
        {
            LastError = "";
        }

        public void Dispose()
        {
            if(_disposed)
            {
                return;
            }
            _disposed = true;
            try
            {
                _conn.Close();
            }
            catch
            {                
            }
        }
    }
}
