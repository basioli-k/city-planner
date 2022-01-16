using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace city_planner
{
    public partial class CityPlanner : Form
    {
        public CityPlanner()
        {
            InitializeComponent();
            var db = Database.GetInstance();
        }
    }
}
