using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class FormSize : Form
    {
        public int CanvasWidth { get; private set; }
        public int CanvasHeight { get; private set; }

        public FormSize(int currentWidth, int currentHeight)
        {
            InitializeComponent();
            textBoxWidth.Text = currentWidth.ToString();
            textBoxHeight.Text = currentHeight.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width, height;
            if (Owner is FormDocument doc)
            {
                if (int.TryParse(textBoxWidth.Text, out width) && int.TryParse(textBoxHeight.Text, out height))
                {
                    CanvasWidth = width;
                    CanvasHeight = height;
                    doc.ResizeCanvas(CanvasWidth, CanvasHeight);
                }
                else
                {
                    MessageBox.Show("Введите корректные размеры", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
