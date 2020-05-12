namespace SystemModel
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxOSParameters = new System.Windows.Forms.GroupBox();
            this.comboBoxPageSize = new System.Windows.Forms.ComboBox();
            this.labelPageSize = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelBitDepth = new System.Windows.Forms.Label();
            this.groupBoxHardwareParameters = new System.Windows.Forms.GroupBox();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.groupBoxRAM = new System.Windows.Forms.GroupBox();
            this.groupBoxROM = new System.Windows.Forms.GroupBox();
            this.groupBoxCPU = new System.Windows.Forms.GroupBox();
            this.checkBoxSlotNuber1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSlotNuber2 = new System.Windows.Forms.CheckBox();
            this.checkBoxSlotNuber3 = new System.Windows.Forms.CheckBox();
            this.checkBoxSlotNuber4 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxRAMSize1 = new System.Windows.Forms.ComboBox();
            this.comboBoxRAMSize2 = new System.Windows.Forms.ComboBox();
            this.comboBoxRAMSize3 = new System.Windows.Forms.ComboBox();
            this.comboBoxRAMSize4 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxOSParameters.SuspendLayout();
            this.groupBoxHardwareParameters.SuspendLayout();
            this.groupBoxRAM.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxOSParameters, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxHardwareParameters, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 101);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 403F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(738, 403);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBoxOSParameters
            // 
            this.groupBoxOSParameters.Controls.Add(this.comboBoxPageSize);
            this.groupBoxOSParameters.Controls.Add(this.labelPageSize);
            this.groupBoxOSParameters.Controls.Add(this.comboBox1);
            this.groupBoxOSParameters.Controls.Add(this.labelBitDepth);
            this.groupBoxOSParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOSParameters.Location = new System.Drawing.Point(3, 3);
            this.groupBoxOSParameters.Name = "groupBoxOSParameters";
            this.groupBoxOSParameters.Size = new System.Drawing.Size(363, 397);
            this.groupBoxOSParameters.TabIndex = 0;
            this.groupBoxOSParameters.TabStop = false;
            this.groupBoxOSParameters.Text = "Параметры ОС";
            // 
            // comboBoxPageSize
            // 
            this.comboBoxPageSize.FormattingEnabled = true;
            this.comboBoxPageSize.Items.AddRange(new object[] {
            "4096"});
            this.comboBoxPageSize.Location = new System.Drawing.Point(143, 86);
            this.comboBoxPageSize.Name = "comboBoxPageSize";
            this.comboBoxPageSize.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPageSize.TabIndex = 3;
            // 
            // labelPageSize
            // 
            this.labelPageSize.AutoSize = true;
            this.labelPageSize.Location = new System.Drawing.Point(7, 89);
            this.labelPageSize.Name = "labelPageSize";
            this.labelPageSize.Size = new System.Drawing.Size(130, 13);
            this.labelPageSize.TabIndex = 2;
            this.labelPageSize.Text = "Размер страницы (байт)";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "32",
            "64"});
            this.comboBox1.Location = new System.Drawing.Point(143, 37);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // labelBitDepth
            // 
            this.labelBitDepth.AutoSize = true;
            this.labelBitDepth.Location = new System.Drawing.Point(7, 40);
            this.labelBitDepth.Name = "labelBitDepth";
            this.labelBitDepth.Size = new System.Drawing.Size(91, 13);
            this.labelBitDepth.TabIndex = 0;
            this.labelBitDepth.Text = "Разрядность ОС";
            // 
            // groupBoxHardwareParameters
            // 
            this.groupBoxHardwareParameters.Controls.Add(this.groupBoxCPU);
            this.groupBoxHardwareParameters.Controls.Add(this.groupBoxROM);
            this.groupBoxHardwareParameters.Controls.Add(this.groupBoxRAM);
            this.groupBoxHardwareParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHardwareParameters.Location = new System.Drawing.Point(372, 3);
            this.groupBoxHardwareParameters.Name = "groupBoxHardwareParameters";
            this.groupBoxHardwareParameters.Size = new System.Drawing.Size(363, 397);
            this.groupBoxHardwareParameters.TabIndex = 1;
            this.groupBoxHardwareParameters.TabStop = false;
            this.groupBoxHardwareParameters.Text = "Параметры железа";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Enabled = false;
            this.buttonAccept.Location = new System.Drawing.Point(648, 533);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(98, 23);
            this.buttonAccept.TabIndex = 2;
            this.buttonAccept.Text = "Принять";
            this.buttonAccept.UseVisualStyleBackColor = true;
            // 
            // groupBoxRAM
            // 
            this.groupBoxRAM.Controls.Add(this.comboBoxRAMSize4);
            this.groupBoxRAM.Controls.Add(this.comboBoxRAMSize3);
            this.groupBoxRAM.Controls.Add(this.comboBoxRAMSize2);
            this.groupBoxRAM.Controls.Add(this.comboBoxRAMSize1);
            this.groupBoxRAM.Controls.Add(this.label2);
            this.groupBoxRAM.Controls.Add(this.label1);
            this.groupBoxRAM.Controls.Add(this.checkBoxSlotNuber4);
            this.groupBoxRAM.Controls.Add(this.checkBoxSlotNuber3);
            this.groupBoxRAM.Controls.Add(this.checkBoxSlotNuber2);
            this.groupBoxRAM.Controls.Add(this.checkBoxSlotNuber1);
            this.groupBoxRAM.Location = new System.Drawing.Point(7, 20);
            this.groupBoxRAM.Name = "groupBoxRAM";
            this.groupBoxRAM.Size = new System.Drawing.Size(350, 156);
            this.groupBoxRAM.TabIndex = 0;
            this.groupBoxRAM.TabStop = false;
            this.groupBoxRAM.Text = "Оперативная память";
            // 
            // groupBoxROM
            // 
            this.groupBoxROM.Location = new System.Drawing.Point(7, 182);
            this.groupBoxROM.Name = "groupBoxROM";
            this.groupBoxROM.Size = new System.Drawing.Size(350, 103);
            this.groupBoxROM.TabIndex = 1;
            this.groupBoxROM.TabStop = false;
            this.groupBoxROM.Text = "ПЗУ (необязательно)";
            // 
            // groupBoxCPU
            // 
            this.groupBoxCPU.Location = new System.Drawing.Point(7, 291);
            this.groupBoxCPU.Name = "groupBoxCPU";
            this.groupBoxCPU.Size = new System.Drawing.Size(350, 100);
            this.groupBoxCPU.TabIndex = 2;
            this.groupBoxCPU.TabStop = false;
            this.groupBoxCPU.Text = "Процессор (необязательно)";
            // 
            // checkBoxSlotNuber1
            // 
            this.checkBoxSlotNuber1.AutoSize = true;
            this.checkBoxSlotNuber1.Checked = true;
            this.checkBoxSlotNuber1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSlotNuber1.Location = new System.Drawing.Point(27, 41);
            this.checkBoxSlotNuber1.Name = "checkBoxSlotNuber1";
            this.checkBoxSlotNuber1.Size = new System.Drawing.Size(32, 17);
            this.checkBoxSlotNuber1.TabIndex = 0;
            this.checkBoxSlotNuber1.Text = "1";
            this.checkBoxSlotNuber1.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlotNuber2
            // 
            this.checkBoxSlotNuber2.AutoSize = true;
            this.checkBoxSlotNuber2.Location = new System.Drawing.Point(26, 70);
            this.checkBoxSlotNuber2.Name = "checkBoxSlotNuber2";
            this.checkBoxSlotNuber2.Size = new System.Drawing.Size(32, 17);
            this.checkBoxSlotNuber2.TabIndex = 1;
            this.checkBoxSlotNuber2.Text = "2";
            this.checkBoxSlotNuber2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlotNuber3
            // 
            this.checkBoxSlotNuber3.AutoSize = true;
            this.checkBoxSlotNuber3.Location = new System.Drawing.Point(27, 99);
            this.checkBoxSlotNuber3.Name = "checkBoxSlotNuber3";
            this.checkBoxSlotNuber3.Size = new System.Drawing.Size(32, 17);
            this.checkBoxSlotNuber3.TabIndex = 2;
            this.checkBoxSlotNuber3.Text = "3";
            this.checkBoxSlotNuber3.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlotNuber4
            // 
            this.checkBoxSlotNuber4.AutoSize = true;
            this.checkBoxSlotNuber4.Location = new System.Drawing.Point(27, 131);
            this.checkBoxSlotNuber4.Name = "checkBoxSlotNuber4";
            this.checkBoxSlotNuber4.Size = new System.Drawing.Size(32, 17);
            this.checkBoxSlotNuber4.TabIndex = 3;
            this.checkBoxSlotNuber4.Text = "4";
            this.checkBoxSlotNuber4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Номер слота";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Объем памяти (МБ)";
            // 
            // comboBoxRAMSize1
            // 
            this.comboBoxRAMSize1.FormattingEnabled = true;
            this.comboBoxRAMSize1.Items.AddRange(new object[] {
            "1",
            "5",
            "10"});
            this.comboBoxRAMSize1.Location = new System.Drawing.Point(146, 39);
            this.comboBoxRAMSize1.Name = "comboBoxRAMSize1";
            this.comboBoxRAMSize1.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRAMSize1.TabIndex = 6;
            // 
            // comboBoxRAMSize2
            // 
            this.comboBoxRAMSize2.Enabled = false;
            this.comboBoxRAMSize2.FormattingEnabled = true;
            this.comboBoxRAMSize2.Items.AddRange(new object[] {
            "1",
            "5",
            "10"});
            this.comboBoxRAMSize2.Location = new System.Drawing.Point(146, 66);
            this.comboBoxRAMSize2.Name = "comboBoxRAMSize2";
            this.comboBoxRAMSize2.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRAMSize2.TabIndex = 7;
            // 
            // comboBoxRAMSize3
            // 
            this.comboBoxRAMSize3.Enabled = false;
            this.comboBoxRAMSize3.FormattingEnabled = true;
            this.comboBoxRAMSize3.Items.AddRange(new object[] {
            "1",
            "5",
            "10"});
            this.comboBoxRAMSize3.Location = new System.Drawing.Point(146, 97);
            this.comboBoxRAMSize3.Name = "comboBoxRAMSize3";
            this.comboBoxRAMSize3.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRAMSize3.TabIndex = 8;
            // 
            // comboBoxRAMSize4
            // 
            this.comboBoxRAMSize4.Enabled = false;
            this.comboBoxRAMSize4.FormattingEnabled = true;
            this.comboBoxRAMSize4.Items.AddRange(new object[] {
            "1",
            "5",
            "10"});
            this.comboBoxRAMSize4.Location = new System.Drawing.Point(146, 129);
            this.comboBoxRAMSize4.Name = "comboBoxRAMSize4";
            this.comboBoxRAMSize4.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRAMSize4.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(734, 71);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Программа на данный момент расчитана на работу с 32-х разрядным адресом, страница" +
    "ми размером в 4096 байт. Настройки ПЗУ и процессора не влияют на работу программ" +
    "ы (пока что).";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 580);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxOSParameters.ResumeLayout(false);
            this.groupBoxOSParameters.PerformLayout();
            this.groupBoxHardwareParameters.ResumeLayout(false);
            this.groupBoxRAM.ResumeLayout(false);
            this.groupBoxRAM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxOSParameters;
        private System.Windows.Forms.ComboBox comboBoxPageSize;
        private System.Windows.Forms.Label labelPageSize;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelBitDepth;
        private System.Windows.Forms.GroupBox groupBoxHardwareParameters;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.GroupBox groupBoxCPU;
        private System.Windows.Forms.GroupBox groupBoxROM;
        private System.Windows.Forms.GroupBox groupBoxRAM;
        private System.Windows.Forms.ComboBox comboBoxRAMSize4;
        private System.Windows.Forms.ComboBox comboBoxRAMSize3;
        private System.Windows.Forms.ComboBox comboBoxRAMSize2;
        private System.Windows.Forms.ComboBox comboBoxRAMSize1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSlotNuber4;
        private System.Windows.Forms.CheckBox checkBoxSlotNuber3;
        private System.Windows.Forms.CheckBox checkBoxSlotNuber2;
        private System.Windows.Forms.CheckBox checkBoxSlotNuber1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

