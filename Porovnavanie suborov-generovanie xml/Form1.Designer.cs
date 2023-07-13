namespace Porovnavanie_suborov_generovanie_xml
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.tb_pathToFile1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonButton3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.tb_pathToFile2 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btn_generateXML = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.tb_outputFolder = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(74, 34);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Palette = this.kryptonPalette1;
            this.kryptonLabel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonLabel1.Size = new System.Drawing.Size(53, 20);
            this.kryptonLabel1.TabIndex = 18;
            this.kryptonLabel1.Values.Text = "Súbor 1";
            // 
            // tb_pathToFile1
            // 
            this.tb_pathToFile1.Location = new System.Drawing.Point(133, 32);
            this.tb_pathToFile1.Name = "tb_pathToFile1";
            this.tb_pathToFile1.Palette = this.kryptonPalette1;
            this.tb_pathToFile1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.tb_pathToFile1.Size = new System.Drawing.Size(340, 23);
            this.tb_pathToFile1.TabIndex = 17;
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.Location = new System.Drawing.Point(479, 32);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.Palette = this.kryptonPalette1;
            this.kryptonButton3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonButton3.Size = new System.Drawing.Size(101, 24);
            this.kryptonButton3.TabIndex = 16;
            this.kryptonButton3.Values.Text = "Vybrať súbor";
            this.kryptonButton3.Click += new System.EventHandler(this.kryptonButton3_Click);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(74, 64);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Palette = this.kryptonPalette1;
            this.kryptonLabel2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonLabel2.Size = new System.Drawing.Size(53, 20);
            this.kryptonLabel2.TabIndex = 21;
            this.kryptonLabel2.Values.Text = "Súbor 2";
            // 
            // tb_pathToFile2
            // 
            this.tb_pathToFile2.Location = new System.Drawing.Point(133, 61);
            this.tb_pathToFile2.Name = "tb_pathToFile2";
            this.tb_pathToFile2.Palette = this.kryptonPalette1;
            this.tb_pathToFile2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.tb_pathToFile2.Size = new System.Drawing.Size(340, 23);
            this.tb_pathToFile2.TabIndex = 20;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(479, 60);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Palette = this.kryptonPalette1;
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonButton1.Size = new System.Drawing.Size(101, 24);
            this.kryptonButton1.TabIndex = 22;
            this.kryptonButton1.Values.Text = "Vybrať súbor";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // btn_generateXML
            // 
            this.btn_generateXML.Location = new System.Drawing.Point(239, 160);
            this.btn_generateXML.Name = "btn_generateXML";
            this.btn_generateXML.Palette = this.kryptonPalette1;
            this.btn_generateXML.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.btn_generateXML.Size = new System.Drawing.Size(127, 24);
            this.btn_generateXML.TabIndex = 23;
            this.btn_generateXML.Values.Text = "Vygenerovať XML";
            this.btn_generateXML.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // kryptonButton4
            // 
            this.kryptonButton4.Location = new System.Drawing.Point(479, 89);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Palette = this.kryptonPalette1;
            this.kryptonButton4.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonButton4.Size = new System.Drawing.Size(101, 24);
            this.kryptonButton4.TabIndex = 26;
            this.kryptonButton4.Values.Text = "Vybrať priečinok";
            this.kryptonButton4.Click += new System.EventHandler(this.kryptonButton4_Click);
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(13, 90);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Palette = this.kryptonPalette1;
            this.kryptonLabel3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.kryptonLabel3.Size = new System.Drawing.Size(114, 20);
            this.kryptonLabel3.TabIndex = 25;
            this.kryptonLabel3.Values.Text = "Výstupný priečinok";
            // 
            // tb_outputFolder
            // 
            this.tb_outputFolder.Location = new System.Drawing.Point(133, 90);
            this.tb_outputFolder.Name = "tb_outputFolder";
            this.tb_outputFolder.Palette = this.kryptonPalette1;
            this.tb_outputFolder.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.tb_outputFolder.Size = new System.Drawing.Size(340, 23);
            this.tb_outputFolder.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 213);
            this.Controls.Add(this.kryptonButton4);
            this.Controls.Add(this.kryptonLabel3);
            this.Controls.Add(this.tb_outputFolder);
            this.Controls.Add(this.btn_generateXML);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.tb_pathToFile2);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.tb_pathToFile1);
            this.Controls.Add(this.kryptonButton3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "BIN to XML - Generovanie XML súborov";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tb_pathToFile1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tb_pathToFile2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btn_generateXML;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tb_outputFolder;
    }
}

