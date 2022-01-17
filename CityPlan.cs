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

        public event EventHandler klik;

        private void CityPlan_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;
            var database = Database.GetInstance();

            if(klik != null)
            {
                klik(this, EventArgs.Empty);
                crtaj_tocku(x, y);
            }

            // if checked dodaj_cvor/add_vertex ...
            // u bazu dodaj cvor
            // draw_plan(filter) 
            // 
        }


        void crtaj_tocku(int x, int y)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawEllipse(Pens.Black, x - (int)vertex_radius / 2, y - (int)vertex_radius / 2, (float)vertex_radius, (float)vertex_radius);
        }

    }
}
