using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Game.Handlers
{
    public class DatabaseHandler
    {
        private static DatabaseHandler instance = null;

        private readonly Dictionary<string, object> _connectionStringValues = new Dictionary<string, object>();
        private MySqlConnection MySqlConnection;

        public static DatabaseHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseHandler();
            }

            return instance;
        }

        public bool Initialize()
        {
            this.DefaultConnectionValues();
            this.MySqlConnection = new MySqlConnection(this.ConstructConnectionString());

            if (!new DatabaseTest("SELECT * FROM areas").ExecuteTest())
            {
                return false;
            }

            if (!new DatabaseTest("SELECT * FROM servers").ExecuteTest())
            {
                return false;
            }

            if (!new DatabaseTest("SELECT * FROM settings").ExecuteTest())
            {
                return false;
            }

            if (!new DatabaseTest("SELECT * FROM users").ExecuteTest())
            {
                return false;
            }
            
            

            return true;
        }

        private void DefaultConnectionValues()
        {
            this.AddConnectionStringValue(
                "server",
                ConfigHandler.GetInstance().GetString("mysqlHost")
            );

            this.AddConnectionStringValue(
                "database",
                ConfigHandler.GetInstance().GetString("mysqlDb")
            );

            this.AddConnectionStringValue(
                "uid",
                ConfigHandler.GetInstance().GetString("mysqlUser")
            );

            this.AddConnectionStringValue(
                "password",
                ConfigHandler.GetInstance().GetString("mysqlPass")
            );

            this.AddConnectionStringValue(
                "port",
                ConfigHandler.GetInstance().GetInt("mysqlPort")
            );
        }

        private void AddConnectionStringValue(string key, object value)
        {
            LogHandler.GetInstance().Log("Key -> " + key + "; Value -> " + value + ";", LogType.DEBUG);
            this._connectionStringValues[key.ToUpper()] = value;
        }

        private string ConstructConnectionString()
        {
            var value = "";
            foreach (KeyValuePair<string, object> f in this._connectionStringValues)
            {
                value += f.Key + "=" + f.Value + ";";
            }

            return value;
        }

        private bool OpenConnection()
        {
            try
            {
                this.MySqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Core.GetInstance()
                            .Error("Database Connection failed. Please contact the system administrator.");
                        break;
                    case 1045:
                        Core.GetInstance().Error(
                            "Database Connection failed. The Username and Password combination supplied is incorrect. Please try again later.");
                        break;
                    default:
                        Core.GetInstance().Error(ex.Message);
                        break;
                }

                return false;
            }
        }

        private void CloseConnection()
        {
            try
            {
                this.MySqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                Core.GetInstance().Error(ex.Message);
            }
        }

        public void Shutdown()
        {
            LogHandler.GetInstance().Log("Shutting down DatabaseHandler gracefully", LogType.INFO);
            if (this.MySqlConnection != null)
            {
                if (this.MySqlConnection.State != ConnectionState.Closed)
                {
                    this.CloseConnection();
                }
            }

            LogHandler.GetInstance().Log("DatabaseHandler has been shutdown", LogType.SUCCESS);
        }


        public DataTable Fetch(string query)
        {
            var dt = new DataTable();

            if (this.OpenConnection())
            {
                var cmd = new MySqlCommand(query, this.MySqlConnection);
                dt.Load(cmd.ExecuteReader());
                this.CloseConnection();
            }

            return dt;
        }

        public void Execute(string query)
        {
            if (this.OpenConnection())
            {
                var command = new MySqlCommand(query, this.MySqlConnection);
                command.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void Execute(string query, params MySqlParameter[] parameters)
        {
            if (this.OpenConnection())
            {
                var command = new MySqlCommand(query, this.MySqlConnection);
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }

                command.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public int GetNumberOfRows(string query)
        {
            return this.Fetch(query).Rows.Count;
        }
    }

    class DatabaseTest
    {
        private readonly string _test;

        public DatabaseTest(string query)
        {
            this._test = query;
        }

        public bool ExecuteTest()
        {
            try
            {
                DatabaseHandler.GetInstance().Fetch(this._test);
                LogHandler.GetInstance().Log("[DatabaseTest] " + this._test + " passed.", LogType.DEBUG);
                return true;
            }
            catch (MySqlException e)
            {
                Core.GetInstance().Error("[DatabaseTest] There was an error when executing " + this._test + ". Error:" + e.Message);
                return false;
            }
        }
    }
}