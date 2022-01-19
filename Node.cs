using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    // Node class which represents crossroads/squares
    // Node class takes care of its interaction with the database
    class Node
    {
        private long id;
        private long x;
        private long y;
        private List<string> characteristics;

        public long Id { get { return id; } set { id = value; } }
        public long X { get { return x; } set { x = value; } }
        public long Y { get { return y; } set { y = value; } }
        public List<string> Characteristics { get { return characteristics; } set { characteristics= value; } }

        private List<Node> GetNodeFromDb(long x_, long y_)
        {
            var db = Database.GetInstance();
            return db.ExecuteQuery<Node>("SELECT * FROM Node WHERE x=" + x_.ToString() + " AND y=" + y_.ToString() + ";");
        }
        public Node() { }
        // returns a node that doesn't end up in the database, use carefully
        static public Node GetNonDbNode(long x_, long y_)
        {
            var node = new Node();
            node.x = x_;
            node.y = y_;
            return node;
        }
        public Node(long x_, long y_)
        {
            var temp = GetNodeFromDb(x_, y_);
            if (temp.Count() == 0)
            {
                x = x_;
                y = y_;
                characteristics = new List<string>();
                insert();
                temp = GetNodeFromDb(x_, y_);
                id = temp[0].id;
            }
            else
            {
                x = temp[0].x;
                y = temp[0].y;
                characteristics = temp[0].characteristics;
                id = temp[0].id;
            }
        }

        public Node(long id_, long x_, long y_, string characteristics_)
        {
            var temp = GetNodeFromDb(x_, y_);
            if (temp.Count() == 0)
            {
                x = x_;
                y = y_;
                characteristics = characteristics_.Split(',').ToList<string>();
                insert();
                temp = GetNodeFromDb(x_, y_);
                id = temp[0].id;
            }
            else
            {
                x = temp[0].x;
                y = temp[0].y;
                characteristics = characteristics_.Split(',').ToList<string>();
                id = temp[0].id;
                update();
            }
            
        }
        public Node(long id_, long x_, long y_, List<string> characteristics_)
        {
            var temp = GetNodeFromDb(x_, y_);
            if (temp.Count() == 0)
            {
                x = x_;
                y = y_;
                characteristics = characteristics_;
                insert();
                temp = GetNodeFromDb(x_, y_);
                id = temp[0].id;
            }
            else
            {
                x = temp[0].x;
                y = temp[0].y;
                characteristics = characteristics_;
                id = temp[0].id;
                update();
            }
        }
        public Node(long x_, long y_, string characteristics_)
        {
            var temp = GetNodeFromDb(x_, y_);
            if (temp.Count() == 0)
            {
                x = x_;
                y = y_;
                characteristics = characteristics_.Split(',').ToList<string>();
                insert();
                temp = GetNodeFromDb(x_, y_);
                id = temp[0].id;
            }
            else
            {
                x = temp[0].x;
                y = temp[0].y;
                characteristics = characteristics_.Split(',').ToList<string>();
                id = temp[0].id;
                update();
            }
        }
        public Node(long x_, long y_, List<string> characteristics_)
        {
            var temp = GetNodeFromDb(x_, y_);
            if (temp.Count() == 0)
            {
                x = x_;
                y = y_;
                characteristics = characteristics_;
                insert();
                temp = GetNodeFromDb(x_, y_);
                id = temp[0].id;
            }
            else
            {
                x = temp[0].x;
                y = temp[0].y;
                characteristics = characteristics_;
                id = temp[0].id;
                update();
            }
        }
        public Node(SQLiteDataReader reader)
        {
            var temp = reader["id"];
            id = (long)reader["id"];
            x = (long)reader["x"];
            y = (long)reader["y"];
            characteristics = ((string)reader["characteristics"]).Split(',').ToList<string>();
        }

        private void insert()
        {
            string sql = "INSERT INTO Node (x, y, characteristics) VALUES(" +
                x.ToString() + "," + y.ToString() + ",\"" + String.Join(",", characteristics) + "\");";

            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }
        private void update()
        {
            string sql = "UPDATE Node SET characteristics=\"" + String.Join(",", characteristics) + "\" WHERE id=" + id.ToString() + ";";
            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        static public List<Node> select_star()
        {
            string sql = "SELECT * FROM Node;";
            var db = Database.GetInstance();
            return db.ExecuteQuery<Node>(sql);
        }
        public void delete()
        {
            string sql = "DELETE FROM Node WHERE id=" + id.ToString() + ";";
            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        static public double Distance(Node n1, Node n2)
        {
            return Math.Sqrt((double)((n1.X - n2.X) * (n1.X - n2.X) +
                (n1.Y - n2.Y) * (n1.Y - n2.Y)));
        }
        public override string ToString()
        {
            return id.ToString() + "," + x.ToString() + "," + y.ToString() + "," + String.Join(" ", characteristics);
        }

        public override bool Equals(object o)
        {
            return ((Node)o).id == id;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public static bool operator ==(Node n1, Node n2)
        {
            return n1.Equals(n2);
        }
        public static bool operator !=(Node n1, Node n2)
        {
            return !n1.Equals(n2);
        }
    }
}
