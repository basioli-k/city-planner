using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace city_planner
{
    // singleton database class
    class Database
    {
        private Database()
        {
            // create sql file if it doesn't exist
            if (!File.Exists("database.db"))
            {
                SQLiteConnection.CreateFile("database.db");
            }
            dbConnection = new SQLiteConnection("Data Source=database.db;Version=3;");
            dbConnection.Open();
            CreateTables();
        }
        // Executes query without returning the results (for inserts, deletes, updates, creates)
        public void ExecuteNonQuery(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }
        // executes query and returns list of results (for selects)
        public List<T> ExecuteQuery<T>(string sql)
        {
            List<T> queryResults = new List<T>();
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                queryResults.Add((T)Activator.CreateInstance(typeof(T), reader));
            }

            return queryResults;
        }
        private void CreateTables()
        {
            string create_node_table = "CREATE TABLE IF NOT EXISTS Node(\n" +
                "id INTEGER PRIMARY KEY,\n" +
                "x INTEGER,\n" +
                "y INTEGER,\n" +
                "characteristics TEXT\n," +
                "UNIQUE(x, y) ON CONFLICT REPLACE" +
                ")";

            string create_road_table = "CREATE TABLE IF NOT EXISTS Road(\n" +
                "id INTEGER PRIMARY KEY,\n" +
                "src INTEGER,\n" +
                "dest INTEGER,\n" +
                "characteristics TEXT\n," +
                "biDir INTEGER\n," +
                "UNIQUE(src, dest) ON CONFLICT REPLACE\n" +
                "FOREIGN KEY (src)\n" +  
                "REFERENCES Node(id)\n" +
                "ON DELETE CASCADE\n" +
                "ON UPDATE NO ACTION\n" +
                "FOREIGN KEY (dest)\n" +
                "REFERENCES Node(id)\n" +
                "ON DELETE CASCADE\n" +
                "ON UPDATE NO ACTION\n" +
                ")";

            ExecuteNonQuery(create_node_table);
            ExecuteNonQuery(create_road_table);
        }

        private static Database _instance;
        private SQLiteConnection dbConnection;

        private static readonly object _lock = new object();

        public static Database GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Database();
                    }
                }
            }
            return _instance;
        }
    }

}
