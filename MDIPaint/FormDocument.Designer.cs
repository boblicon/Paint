﻿namespace MDIPaint
{
    partial class FormDocument
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.Name = "FormDocument";
            this.Text = "FormDocument";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDocument_MouseDown);
            this.MouseLeave += new System.EventHandler(this.FormDocument_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDocument_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDocument_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}