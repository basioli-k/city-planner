using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class InvalidSrcDestPair : Exception
    {
        public InvalidSrcDestPair(string message)
        {
        }
    }
    // Road class which represents crossroads/squares
    // Road class takes care of its interaction with the database
    public class Road : PlannerObject
    {
        private long id;
        private long src;
        private long dest;
        private bool biDir;
        
        public long Id
        {
            get { return id; } set { id = value; }
        }
        public long Src
        {
            get { return src; } set { src = value; }
        }
        public long Dest
        {
            get { return dest; } set { dest = value; }
        }
        public bool BiDir
        {
            get { return biDir; }
            set { biDir = value; }
        }
        public double Distance()
        {
            var srcDestPair = GetSourceAndDest();

            return Math.Sqrt((double)((srcDestPair.Item1.X - srcDestPair.Item2.X) * (srcDestPair.Item1.X - srcDestPair.Item2.X) +
                (srcDestPair.Item1.Y - srcDestPair.Item2.Y) * (srcDestPair.Item1.Y - srcDestPair.Item2.Y)));
        }
        public Tuple<Node, Node> GetSourceAndDest()
        {
            string sql = "SELECT * FROM Node WHERE id=" + src.ToString() + " OR id=" + dest.ToString() + ";";
            var db = Database.GetInstance();
            var res = db.ExecuteQuery<Node>(sql);
            if (res.Count != 2)
                throw new InvalidSrcDestPair("(" + src.ToString() + "," + dest.ToString() + ") node pairs not in database.");
            return new Tuple<Node, Node>(res[0].Id == src ? res[0] : res[1], res[1].Id == dest ? res[1] : res[0]);
        }
        private List<Road> GetRoadFromDb(long src_, long dest_)
        {
            var db = Database.GetInstance();
            return db.ExecuteQuery<Road>("SELECT * FROM Road WHERE src=" + src_.ToString() + " AND dest=" + dest_.ToString() + ";");
        }
        Road() { }

        public Road(long src_, long dest_, bool biDir_ = false)
        {
            biDir = biDir_;
            var temp = GetRoadFromDb(src_, dest_);
            if (temp.Count() == 0)
            {
                src = src_;
                dest = dest_;
                characteristics = new List<string>();
                insert();
                temp = GetRoadFromDb(src_, dest_);
                id = temp[0].id;
            }
            else
            {
                id = temp[0].id;
                src = temp[0].src;
                dest = temp[0].dest;
                characteristics = temp[0].characteristics;
                biDir = temp[0].biDir;
            }
        }

        public Road(long id_, long src_, long dest_, string characteristics_, bool biDir_ = false)
        {
            biDir = biDir_;
            var temp = GetRoadFromDb(src_, dest_);
            if (temp.Count() == 0)
            {
                src = src_;
                dest = dest_;
                characteristics = characteristics_.Split(',').ToList<string>();
                insert();
                temp = GetRoadFromDb(src_, dest_);
                id = temp[0].id;
            }
            else
            {
                id = temp[0].id;
                src = temp[0].src;
                dest = temp[0].dest;
                characteristics = characteristics_.Split(',').ToList<string>();
                biDir = temp[0].biDir;
                update();
            }
            
        }
        public Road(long id_, long src_, long dest_, List<string> characteristics_, bool biDir_ = false)
        {
            biDir = biDir_;
            var temp = GetRoadFromDb(src_, dest_);
            if (temp.Count() == 0)
            {
                src = src_;
                dest = dest_;
                characteristics = characteristics_;
                insert();
                temp = GetRoadFromDb(src_, dest_);
                id = temp[0].id;
            }
            else
            {
                id = temp[0].id;
                src = temp[0].src;
                dest = temp[0].dest;
                characteristics = characteristics_;
                biDir = temp[0].biDir;
                update();
            }
        }
        public Road(long src_, long dest_, string characteristics_, bool biDir_)
        {
            biDir = biDir_;
            var temp = GetRoadFromDb(src_, dest_);
            if (temp.Count() == 0)
            {
                src = src_;
                dest = dest_;
                characteristics = characteristics_.Split(',').ToList<string>();
                insert();
                temp = GetRoadFromDb(src_, dest_);
                id = temp[0].id;
            }
            else
            {
                id = temp[0].id;
                src = temp[0].src;
                dest = temp[0].dest;
                characteristics = characteristics_.Split(',').ToList<string>();
                biDir = temp[0].biDir;
                update();
            }
        }
        public Road(long src_, long dest_, List<string> characteristics_, bool biDir_ = false)
        {
            biDir = biDir_;
            var temp = GetRoadFromDb(src_, dest_);
            if (temp.Count() == 0)
            {
                src = src_;
                dest = dest_;
                characteristics = characteristics_;
                insert();
                temp = GetRoadFromDb(src_, dest_);
                id = temp[0].id;
            }
            else
            {
                id = temp[0].id;
                src = temp[0].src;
                dest = temp[0].dest;
                characteristics = characteristics_;
                biDir = temp[0].biDir;
                update();
            }
        }
        public Road(SQLiteDataReader reader)
        {
            biDir = (long)reader["biDir"] == 1;
            id = (long)reader["id"];
            src = (long)reader["src"];
            dest = (long)reader["dest"];
            characteristics = ((string)reader["characteristics"]).Split(',').ToList<string>();
            if (characteristics.Count == 1 && characteristics[0] == "")
                characteristics = new List<string>();
        }

        private void insert()
        {
            string sql = "INSERT INTO Road (src, dest, characteristics, biDir) VALUES(" +
                src.ToString() + "," + dest.ToString() + ",\"" + String.Join(",", characteristics) + "\"," + (biDir ? "1" : "0") + ");";

            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        // updates characteristics
        private void update()
        {
            string sql = "UPDATE Road SET characteristics=\"" + String.Join(",", characteristics) + "\" WHERE id=" + id.ToString() + ";";
            var db = Database.GetInstance();
            db.ExecuteNonQuery(sql);
        }

        public void delete()
        {
            string sql = "DELETE FROM Road WHERE id=" + id.ToString() + ";";
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
