using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MDIPaint
{
    public partial class FormDocument : Form
    {
        int old_x = 0, old_y = 0;
        Bitmap bitmap = new Bitmap(2000, 2000);
        private Bitmap bmpTemp;
        private Bitmap bmp;
        private Cursor penCursor;
        private Cursor circleCursor;
        private Cursor rectangleCursor;
        public bool IsModified = false;
        public string filepath = null;
        public int CanvasWidth => bmp.Width;
        public int CanvasHeight => bmp.Height;
        public bool isCreated { get; set; } = false;




        public FormDocument()
        {
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            bmpTemp = bmp;
            isCreated = true;

        }

        public FormDocument(Bitmap bmp)
        {
            InitializeComponent();
            this.bmp = bmp;
            bmpTemp = bmp;
            isCreated = true;
        }

        private void FormDocument_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                old_x = e.X; old_y = e.Y;
            }

        }

        private void FormDocument_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pen = new Pen(MainForm.CurrentColor, MainForm.CurrentWidth);
                switch (MainForm.CurrentTool)
                {
                    case Tools.Pen:
                        this.Cursor = new Cursor("cursor_pen.cur");
                        var g = Graphics.FromImage(bmp);
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        if (MainForm.CurrentWidth < 10)
                        {
                            g.DrawLine(new Pen(MainForm.CurrentColor, MainForm.CurrentWidth), old_x, old_y, e.X, e.Y);
                        }
                        else
                        {
                            int dx = Math.Abs(e.X - old_x);
                            int dy = Math.Abs(e.Y - old_y);
                            int steps = Math.Max(dx, dy);

                            for (int i = 0; i < steps; i++)
                            {
                                float t = (float)i / steps;
                                int x = (int)(old_x + (e.X - old_x) * t);
                                int y = (int)(old_y + (e.Y - old_y) * t);
                                g.FillEllipse(new SolidBrush(MainForm.CurrentColor), x - MainForm.CurrentWidth / 2, y - MainForm.CurrentWidth / 2, MainForm.CurrentWidth, MainForm.CurrentWidth);
                            }
                        }

                        old_x = e.X;
                        old_y = e.Y;
                        bmpTemp = bmp;
                        Invalidate();
                        MarkAsModified();
                        break;
                    case Tools.Circle:
                        bmpTemp = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(bmpTemp);
                        g.DrawEllipse(pen, new Rectangle(old_x, old_y, e.X - old_x, e.Y - old_y));
                        Invalidate();
                        break;
                    case Tools.Rectangle:
                        bmpTemp = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(bmpTemp);
                        int width = Math.Abs(e.X - old_x);
                        int height = Math.Abs(e.Y - old_y);
                        int rectX = Math.Min(old_x, e.X);
                        int rectY = Math.Min(old_y, e.Y);
                        g.DrawRectangle(pen, new Rectangle(rectX, rectY, width, height));
                        Invalidate();
                        break;
                    case Tools.Eraser:
                        var eraser = new Pen(Color.White, MainForm.CurrentWidth);
                        g = Graphics.FromImage(bmp);
                        g.DrawLine(eraser, old_x, old_y, e.X, e.Y);
                        old_x = e.X;
                        old_y = e.Y;
                        bmpTemp = bmp;
                        Invalidate();
                        MarkAsModified();
                        break;
                    case Tools.Line:
                        bmpTemp = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(bmpTemp);
                        g.DrawLine(pen, old_x, old_y, e.X, e.Y);
                        Invalidate();
                        break;

                }


            }
            var parent = MdiParent as MainForm;
            parent?.ShowPos(e.X, e.Y);
        }

        private void FormDocument_MouseUp(object sender, MouseEventArgs e)
        {
            switch (MainForm.CurrentTool)
            {
                case Tools.Circle:
                case Tools.Rectangle:
                case Tools.Line:
                    bmp = bmpTemp;
                    Invalidate();
                    MarkAsModified();
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmpTemp, 0, 0);
            }
            else
            {
                Console.WriteLine("Ошибка, изображение не обработано");
            }
        }

        private void FormDocument_MouseLeave(object sender, EventArgs e)
        {
            var parent = MdiParent as MainForm;
            parent?.ShowPos(-1, -1);
        }

        public void UpdateCursor()
        {
            try
            {
                switch (MainForm.CurrentTool)
                {
                    case Tools.Pen:
                        this.Cursor = new Cursor("cursor_pen.cur");
                        break;
                    case Tools.Circle:
                    case Tools.Rectangle:
                    case Tools.Line:
                        this.Cursor = Cursors.Cross;
                        break;
                    default:
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки курсора: {ex.Message}");
                this.Cursor = Cursors.Default;
            }
        }

        public void Save()
        {
            if (filepath == null)
            {
                SaveAs();
            }
            else
            {
                bmp.Save(filepath);
                IsModified = false;
                Text = filepath;
            }
        }

        public void SaveAs()
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "PNG |*.png|JPEG |*.jpg|Bitmap |*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filepath = dlg.FileName;
                    Save();
                }
            }
        }

        private void MarkAsModified()
        {
            IsModified = true;
            Text = "*" + (filepath ?? "Untitled");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (IsModified)
            {
                var result = MessageBox.Show("Сохранить изменения перед выходом?", "Выход",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            isCreated = false;
            var parent = MdiParent as MainForm;
            parent?.ClickableButtons();
            base.OnFormClosing(e);
        }

        public void ResizeCanvas(int newWidth, int newHeight)
        {
            if (newWidth <= 0 || newHeight <= 0) return;

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            if (bmp != null)
            {
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.Clear(Color.White);
                    g.DrawImage(bmp, 0, 0);
                }
            }

            bmp = newImage;
            bmpTemp = (Bitmap)bmp.Clone();
            Invalidate();
            Refresh();
            ClientSize = new Size(newWidth, newHeight);
        }

        public void OpenFile()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.png;*.jpg;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Bitmap tempBitmap = new Bitmap(dlg.FileName))
                        {
                            bmp = new Bitmap(tempBitmap);
                        }
                        ResizeCanvas(bmp.Width, bmp.Height);
                        bmpTemp = (Bitmap)bmp.Clone();
                        filepath = dlg.FileName;
                        IsModified = false;
                        Text = filepath;
                        Invalidate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }





    }
}
