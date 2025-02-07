using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class MainForm : Form
    {
        public static Color CurrentColor { get; set; }
        public static int CurrentWidth { get; set; }
        public static Tools CurrentTool { get; set; }

        public MainForm()
        {
            InitializeComponent();
            CurrentColor = Color.Black;
            CurrentWidth = 1;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ClickableButtons();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormAbout();
            frm.ShowDialog();
        }

        private void новыйДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var document = new FormDocument();
            document.MdiParent = this;
            document.Show();
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Red;
        }

        private void голубойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Green;
        }

        private void выбратьСвойЦветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK) { CurrentColor = dlg.Color; }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CurrentWidth = 10;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CurrentWidth = 50;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CurrentWidth = 100;
        }

        private void окружностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTool = Tools.Circle;
            UpdateCursorsInDocuments();

        }

        private void кистьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTool = Tools.Pen;
            UpdateCursorsInDocuments();

        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTool = Tools.Rectangle;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is FormDocument doc)
            {
                doc.SaveAs();
            }
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void ластикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTool = Tools.Eraser;
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTool = Tools.Line;
            UpdateCursorsInDocuments();
        }

        private void UpdateCursorsInDocuments()
        {
            foreach (Form form in MdiChildren)
            {
                if (form is FormDocument doc)
                {
                    doc.UpdateCursor();
                }
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        public void ShowPos(int x, int y)
        {
            if (x != -1)
                status2.Text = $"X: {x} Y: {y}";
            else
                status2.Text = string.Empty;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is FormDocument doc)
            {
                doc.Save();
            }
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeDoc = ActiveMdiChild as FormDocument;
            if (activeDoc == null) return;

            var sizeForm = new FormSize(activeDoc.CanvasWidth, activeDoc.CanvasHeight);
            sizeForm.Owner = activeDoc;
            sizeForm.ShowDialog();
        }

        private void defalutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentWidth = 1;
        }

        public void ClickableButtons ()
        {
            bool isEnabled = ActiveMdiChild is FormDocument doc && doc.isCreated;

            сохранитьToolStripMenuItem.Enabled = isEnabled;
            размерХолстаToolStripMenuItem.Enabled = isEnabled;
            сохранитьКакToolStripMenuItem.Enabled = isEnabled;
            каскадомToolStripMenuItem.Enabled = isEnabled;
            слеваНаправоToolStripMenuItem.Enabled = isEnabled;
            упорядочитьЗначкиToolStripMenuItem.Enabled = isEnabled;
            сверхуВнизToolStripMenuItem.Enabled = isEnabled;
        }

        public void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            ClickableButtons();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDocument doc = new FormDocument();
            doc.MdiParent = this;
            doc.Show();
            doc.OpenFile();
        }
    }
}
