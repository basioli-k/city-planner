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
    public partial class CityPlan : UserControl
    {
        readonly double vertex_radius = 5;
        readonly double road_width = 3;
        public CityPlan()
        {
            InitializeComponent();
        }

        private void CityPlan_Click(object sender, EventArgs e)
        {
            var x = MousePosition.X - Location.X;
            var y = MousePosition.Y - Location.Y;
            var database = Database.GetInstance();
            // if checked dodaj_cvor/add_vertex ...
                // u bazu dodaj cvor
                // draw_plan(filter) 
            // 
        }
    }
}
