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
    public partial class Characteristics : Form
    {
        private int i = 0;
        private bool biDirectional = false;
        List<Button> listOfButtons = new List<Button>();
        List<Label> listOfLabels = new List<Label>();
        public string text { get; set; }

        private List<string> finalCharacteristics = new List<string>();
        public List<string> FinalCharacterictics
        {
            get { return finalCharacteristics; }
            set { finalCharacteristics = value; }
        }
        public bool BiDirectional {
            get { return biDirectional; }
            set { biDirectional = value; }
        }

        public Characteristics(bool isRoad)
        {
            InitializeComponent();
            checkBox1.Visible = isRoad;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            text = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                Label label = new Label();
                label.Text = textBox1.Text;
                label.Left = 5;
                label.Width = panel1.Width / 2;
                label.Tag = i;

                Button btn = new Button();
                btn.Text = "delete";
                btn.Left = panel1.Width - btn.Width;
                btn.Tag = i;

                i++;

                listOfLabels.Add(label);
                listOfButtons.Add(btn);

                btn.Click += (send, ee) =>
                {
                    for (int i = 0; i < listOfButtons.Count; i++)
                    {
                        int ii = i;
                        if (listOfButtons[ii].Tag == btn.Tag)
                        {
                            listOfButtons.Remove(listOfButtons[ii]);
                            listOfLabels.Remove(listOfLabels[ii]);
                        }
                    }
                    drawPanel();
                };
                textBox1.Text = null;
                drawPanel();
            }
        }

        void drawPanel()
        {
            SuspendLayout();
            panel1.Controls.Clear();
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                var ii = i;

                listOfLabels[ii].Top = 5 + 20 * ii;

                listOfButtons[ii].Top= 20 * ii;

                panel1.Controls.Add(listOfLabels[ii]);
                panel1.Controls.Add(listOfButtons[ii]);

            }
            ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listOfLabels.Count; i++)
            {
                finalCharacteristics.Add(listOfLabels[i].Text);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            biDirectional = checkBox1.Checked;
        }
    }
}
