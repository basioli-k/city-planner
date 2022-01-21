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
        private List<Label> labels = new List<Label>();
        private List<Button> buttons = new List<Button>();

        public CityPlanner()
        {
            InitializeComponent();
            radioButton1.Checked = true;

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
                cityPlan1.deleteObject -= handler;
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
            else if (radioButton3.Checked)
            {
                radioButton3.Checked = false;
            }
            else if (radioButton4.Checked)
            {
                radioButton4.Checked = false;
            }
            else if (radioButton5.Checked)
            {
                radioButton5.Checked = false;
            }
            else if (radioButton6.Checked)
            {
                radioButton6.Checked = false;
            }
            else if (radioButton7.Checked)
            {
                radioButton7.Checked = false;
            }
            else if (radioButton8.Checked)
            {
                radioButton8.Checked = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cityPlan1.IndexTab = tabControl1.SelectedIndex;
            uncheckRadioButtons();
            if (tabControl1.SelectedIndex == 0) 
            {
                radioButton1.Checked = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                radioButton4.Checked = true;
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
            else if (tabControl1.SelectedIndex == 2)
            {
                radioButton7.Checked = true;
            }
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

        private Label GetLabelFromText(string text, bool bold = false)
        {
            Label label = new Label();
            label.Text = text;
            label.Left = panel1.Location.X + 5;
            label.Width = panel1.Width / 2;
            if (bold) label.Font = new Font(label.Font, FontStyle.Bold); ;
            return label;
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler<PlannerObject> handler = (snd, obj) =>
            {
                labels.Clear();
                Pen roadPen = new Pen(Color.Red, (float)cityPlan1.RoadWidth);
                switch (obj)
                {
                    case Node n:
                        if (n.Characteristics.Count == 0) labels.Add(GetLabelFromText("No characteristics.", true));
                        else labels.Add(GetLabelFromText("Characteristics:", true));
                        foreach (var c in n.Characteristics)
                        {
                            Label label = GetLabelFromText(c);
                            labels.Add(label);
                        }

                        var from = cityPlan1.ListRoads.Count<Road>(road => road.Src == n.Id);
                        var to = cityPlan1.ListRoads.Count<Road>(road => road.Dest == n.Id);

                        labels.Add(GetLabelFromText(from.ToString() + (from == 1 ? " road ": " roads ") + "from"));
                        labels.Add(GetLabelFromText(to.ToString() + (to == 1 ? " road " : " roads ") + "to"));
                        drawPanel();
                        
                        cityPlan1.DrawAllPointsAndRoads(new List<Node> { n }, new List<Road>(), Brushes.Red, roadPen);
                        break;
                    case Road r:
                        if (r.Characteristics.Count == 0) labels.Add(GetLabelFromText("No characteristics.", true));
                        else labels.Add(GetLabelFromText("Characteristics:", true));
                        foreach (var c in r.Characteristics)
                        {
                            Label label = GetLabelFromText(c);
                            labels.Add(label);
                        }

                        var trafficIntensity = cityPlan1.GetTrafficIntensity(r);
                        labels.Add(GetLabelFromText("Traffic intensity is: " + trafficIntensity.ToString()));

                        drawPanel();
                        cityPlan1.DrawAllPointsAndRoads(new List<Node>(), new List<Road> { r }, Brushes.Red, roadPen);
                        break;
                    default:
                        break;
                }
            };


            if (radioButton7.Checked)
            {
                cityPlan1.getObjectDetails += handler;
                button1.Visible = false;
                button2.Visible = false;
                textBox4.Visible = false;
                panel1.Controls.Clear();
                labels.Clear();
                buttons.Clear();
            }
            else
            {
                cityPlan1.getObjectDetails -= handler;
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                button1.Visible = true;
                button2.Visible = true;
                textBox4.Visible = true;
                panel1.Controls.Clear();
                labels.Clear();
                buttons.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != null)
            {
                Label label = new Label();
                label.Text = textBox4.Text;
                label.Left = panel1.Location.X + 5;
                label.Width = panel1.Width / 2;

                Button btn = new Button();
                btn.Text = "delete";
                btn.Left = panel1.Location.X + panel1.Width - btn.Width - 30;
                btn.Height += 10;
                labels.Add(label);
                buttons.Add(btn);

                btn.Click += (send, ee) =>
                {
                    var ind = buttons.IndexOf((Button)send);
                    buttons.Remove(buttons[ind]);
                    labels.Remove(labels[ind]);
                    drawPanel();
                };
                textBox4.Text = null;
                drawPanel();
            }
        }

        void drawPanel()
        {
            SuspendLayout();
            panel1.Controls.Clear();
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].Top = 8 + 25 * i;

                if (radioButton8.Checked)
                    buttons[i].Top = 5 + 25 * i;

                panel1.Controls.Add(labels[i]);

                if (radioButton8.Checked)
                    panel1.Controls.Add(buttons[i]);

            }
            ResumeLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> lbls = (from lbl in labels select lbl.Text).ToList<string>();
            var nodes = cityPlan1.ListNodes.FindAll(node => node.hasAllCharacteristics(lbls));
            var roads = cityPlan1.ListRoads.FindAll(road=> road.hasAllCharacteristics(lbls));
            cityPlan1.DrawAllPointsAndRoads(nodes, roads, Brushes.Red, Pens.Red);
        }
    }
}
