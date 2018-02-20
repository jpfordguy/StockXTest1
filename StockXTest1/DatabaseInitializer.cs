using System;
using Npgsql;

namespace StockXTest1
{
    class DatabaseInitializer
    {
        private const string _dbName = "stockxtest";


        public static bool CreateDatabase(string server, int port, string userid, string password)
        {
            if (server == null)
            {
                throw new Exception("Bad / invalid value for server.");
            }
            if(server.Trim() == "")
            {
                throw new Exception("Bad / invalid value for server.");
            }
            if(port < 1)
            {
                throw new Exception("Bad / invalid value for port.");
            }
            if (userid == null)
            {
                throw new Exception("Bad / invalid value for userid.");
            }
            if (userid.Trim() == "")
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

            string tmpstr = "Server=" + server.Trim() + "; Port=" + port.ToString() + "; User Id=" + userid.Trim() + "; Password=" + password.Trim() + ";";
            NpgsqlConnection conn = new NpgsqlConnection(tmpstr);
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            object exists = null;

            cmd.CommandText = "SELECT 1 FROM pg_database WHERE datname='" + _dbName + "'";
            try
            {
                exists = cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                cmd.Clone();
                cmd.Dispose();
                conn.Close();
                return false;
            }
            cmd.Clone();
            cmd.Dispose();

            bool create = false;
            if (exists == null)
            {
                create = true;
            }
            else
            {
                if ((int)exists != 1)
                {
                    create = true;
                }
            }
            if (create)
            {
                cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CREATE DATABASE " + _dbName + " WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    cmd.Clone();
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                    return false;
                }
                cmd.Clone();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();

            tmpstr = "Server=" + server.Trim() + "; Port=" + port.ToString() + "; User Id=" + userid.Trim() + "; Password=" + password.Trim() + "; Database=" + _dbName + ";";
            conn = new NpgsqlConnection(tmpstr);
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS ShoeNames(Name varchar CONSTRAINT Name PRIMARY KEY, Id varchar)";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                cmd.Clone();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                return false;
            }
            cmd.Clone();
            cmd.Dispose();

            cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS ShoeSizes(SizeId varchar CONSTRAINT Sizeid PRIMARY KEY, Id varchar, Size int4)";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                cmd.Clone();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                return false;
            }
            cmd.Clone();
            cmd.Dispose();


            conn.Close();
            conn.Dispose();

            return true;
        }

    }
}
