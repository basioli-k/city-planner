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
            cityPlan1.TabIndex = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = (s, ee) => { };

            if (radioButton1.Checked)
            {
                cityPlan1.addPoint += handler;
            }
            else
            {
                cityPlan1.addPoint -= handler;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = (s, ee) => { };

            if (radioButton2.Checked)
            {
                cityPlan1.addLine += handler;
            }
            else
            {
                cityPlan1.addLine -= handler;
                cityPlan1.FirstX = -1;
                cityPlan1.FirstY = -1;
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1) 
            {
                cityPlan1.IndexTab = 1;
                cityPlan1.Start = -1;
                cityPlan1.End = -1;
                EventHandler handler = (s, ee) => { };
                if (radioButton1.Checked)
                {
                    radioButton1.Checked = false;
                }
                else if (radioButton2.Checked)
                {
                    radioButton2.Checked = false;
                    cityPlan1.FirstX = -1;
                    cityPlan1.FirstY = -1;
                }
            }
            else
            {
                cityPlan1.IndexTab = 0;
                cityPlan1.repaintNodes(Brushes.Black);
                cityPlan1.repaintRoads(Brushes.Black);
            }
        }
    }
}
