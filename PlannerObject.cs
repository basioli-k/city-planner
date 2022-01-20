using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    public class PlannerObject
    {
        protected List<string> characteristics;
        public List<string> Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }
        public PlannerObject() { }
        public bool hasAllCharacteristics(List<string> chars)
        {
            foreach (var c in chars)
            {
                if (!characteristics.Contains(c)) return false;
            }

            return true;
        }

        public bool hasAnyCharacteristics(List<string> chars)
        {
            foreach (var c in chars)
            {
                if (characteristics.Contains(c)) return true;
            }

            return false;
        }
    }
}
