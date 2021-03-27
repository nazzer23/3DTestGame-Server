using System;
using System.Collections.Generic;
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

        public void Initialize()
        {
        }
    }
}