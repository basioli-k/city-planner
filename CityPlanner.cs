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
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = (s, ee) => { };

            if (radioButton3.Checked)
            {
                cityPlan1.deleteObject += handler;
            }
            else
            {
                cityPlan1.deleteObject-= handler;
            }
        }

        private void uncheckRadioButtons()
        {
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cityPlan1.IndexTab = tabControl1.SelectedIndex;
            if (tabControl1.SelectedIndex == 0) 
            {
                
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                cityPlan1.Start = -1;
                cityPlan1.End = -1;
                textBox2.Text = "0";
                textBox3.Text = TimeSpan.FromSeconds(0).ToString();
                cityPlan1.print_dist += (sender1, dist) =>
                {
                    if (dist == double.PositiveInfinity)
                    {
                        textBox2.Text = dist.ToString();
                        textBox3.Text = TimeSpan.FromSeconds(0).ToString();
                        return;
                    }
                    textBox2.Text = ((int)dist).ToString();
                    long speed = 1;
                    if (radioButton4.Checked) speed = 12;
                    if (radioButton5.Checked) speed = 4;
                    if (radioButton6.Checked) speed = 1;
                    TimeSpan t = TimeSpan.FromSeconds((int)(dist / speed));
                    textBox3.Text = t.ToString();
                };
                
            }

            uncheckRadioButtons();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cityPlan1.Characteristic = textBox1.Text;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            int x = 0;
            Int32.TryParse(textBox2.Text, out x);
            TimeSpan t = TimeSpan.FromSeconds((int)(x / 12));
            textBox3.Text = t.ToString();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            int x = 0;
            Int32.TryParse(textBox2.Text, out x);
            TimeSpan t = TimeSpan.FromSeconds((int)(x / 8));
            textBox3.Text = t.ToString();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            int x = 0;
            Int32.TryParse(textBox2.Text, out x);
            TimeSpan t = TimeSpan.FromSeconds((int)(x / 1));
            textBox3.Text = t.ToString();
        }
    }
}
