using System;
using System.Collections.Generic;
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

        public long Id
        {
            get;set;
        }
        public long Src
        {
            get;set;
        }
        public long Dest
        {
            get;set;
        }
        public List<string> Characteristics
        {
            get;set;
        }
        public long Distance()
        {
            //TODO: izracunaj udaljenost.
            return 1;
        }
    }
}
