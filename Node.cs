using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Node
    {
        private long id;
        private long x;
        private long y;
        private List<string> characteristics;

        Node() { }

        Node(long id_, long x_, long y_, string characteristics_)
        {
            id = id_;
            x = x_;
            y = y_;
            characteristics = characteristics_.Split(',').ToList<string>();
        }
        Node(long id_, long x_, long y_, List<string> characteristics_)
        {
            id = id_;
            x = x_;
            y = y_;
            characteristics = characteristics_;
        }
        Node(long x_, long y_, string characteristics_)
        {
            x = x_;
            y = y_;
            characteristics = characteristics_.Split(',').ToList<string>();
        }
        Node(long x_, long y_, List<string> characteristics_)
        {
            x = x_;
            y = y_;
            characteristics = characteristics_;
        }
        public Node(SQLiteDataReader reader)
        {
            var temp = reader["id"];
            id = (long)reader["id"];
            x = (long)reader["x"];
            y = (long)reader["y"];
            characteristics = ((string)reader["characteristics"]).Split(',').ToList<string>();
        }

        public void insert()
        {
            string sql = "INSERT INTO Node (x, y, characteristics) VALUES(" +
                x.ToString() + "," + y.ToString() + "," + String.Join(",", characteristics) + ");";

            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        static public List<Node> select_star()
        {
            string sql = "SELECT * FROM Node;";
            var db = Database.GetInstance();
            return db.ExecuteQuery<Node>(sql);
        }

        public override string ToString()
        {
            return id.ToString() + "," + x.ToString() + "," + y.ToString() + "," + String.Join(" ", characteristics);
        }

        // insert, konstruktori odgovarajuci
    }
}
