using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Road
    {
        private long id;
        private long src;
        private long dest;
        private List<string> characteristics;

        Road() { }

        Road(long id_, long src_, long dest_, string characteristics_)
        {
            id = id_;
            src = src_;
            dest = dest_;
            characteristics = characteristics_.Split(',').ToList<string>();
        }
        Road(long id_, long src_, long dest_, List<string> characteristics_)
        {
            id = id_;
            src = src_;
            dest = dest_;
            characteristics = characteristics_;
        }
        Road(long src_, long dest_, string characteristics_)
        {
            src = src_;
            dest = dest_;
            characteristics = characteristics_.Split(',').ToList<string>();
        }
        Road(long src_, long dest_, List<string> characteristics_)
        {
            src = src_;
            dest = dest_;
            characteristics = characteristics_;
        }
        public Road(SQLiteDataReader reader)
        {
            id = (long)reader["id"];
            src = (long)reader["src"];
            dest = (long)reader["dest"];
            characteristics = ((string)reader["characteristics"]).Split(',').ToList<string>();
        }

        public void insert()
        {
            string sql = "INSERT INTO Node (src, dest, characteristics) VALUES(" +
                src.ToString() + "," + dest.ToString() + "," + String.Join(",", characteristics) + ");";

            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        static public List<Road> select_star()
        {
            string sql = "SELECT * FROM Road;";
            var db = Database.GetInstance();
            return db.ExecuteQuery<Road>(sql);
        }
        public override string ToString()
        {
            return id.ToString() + "," + src.ToString() + "," + dest.ToString() + "," + String.Join(" ", characteristics);
        }
    }
}
