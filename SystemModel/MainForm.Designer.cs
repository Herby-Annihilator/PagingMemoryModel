namespace SystemModel
{
    partial class MainForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableMemory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsingMemory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VirtualMemory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PageTableAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxOS = new System.Windows.Forms.GroupBox();
            this.groupBoxProcess = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.PagePhisicalAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Present = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReadWrite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserSupervisor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTableAdress = new System.Windows.Forms.Label();
            this.textBoxTableAdress = new System.Windows.Forms.TextBox();
            this.labelByteCount = new System.Windows.Forms.Label();
            this.textBoxRequestMemory = new System.Windows.Forms.TextBox();
            this.buttonReqiestMemory = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelPID = new System.Windows.Forms.Label();
            this.textBoxPID = new System.Windows.Forms.TextBox();
            this.buttonCreateNewProcess = new System.Windows.Forms.Button();
            this.buttonKillProcess = new System.Windows.Forms.Button();
            this.textBoxPIDkill = new System.Windows.Forms.TextBox();
            this.labelPIDkill = new System.Windows.Forms.Label();
            this.labelDriveOutTheProcess = new System.Windows.Forms.Label();
            this.textBoxDriveOutTheProcess = new System.Windows.Forms.TextBox();
            this.buttonDriveOutTheProcess = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxOS.SuspendLayout();
            this.groupBoxProcess.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Process,
            this.PID,
            this.Status,
            this.AvailableMemory,
            this.UsingMemory,
            this.VirtualMemory,
            this.PageTableAdress});
            this.dataGridView1.Location = new System.Drawing.Point(3, 453);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1232, 187);
            this.dataGridView1.TabIndex = 0;
            // 
            // Process
            // 
            this.Process.HeaderText = "Процесс";
            this.Process.Name = "Process";
            this.Process.ReadOnly = true;
            this.Process.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PID
            // 
            this.PID.HeaderText = "PID";
            this.PID.Name = "PID";
            this.PID.ReadOnly = true;
            this.PID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Status
            // 
            this.Status.HeaderText = "Статус";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AvailableMemory
            // 
            this.AvailableMemory.HeaderText = "Действительно доступная память (байт)";
            this.AvailableMemory.Name = "AvailableMemory";
            this.AvailableMemory.ReadOnly = true;
            this.AvailableMemory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UsingMemory
            // 
            this.UsingMemory.HeaderText = "Используемая память (байт)";
            this.UsingMemory.Name = "UsingMemory";
            this.UsingMemory.ReadOnly = true;
            this.UsingMemory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // VirtualMemory
            // 
            this.VirtualMemory.HeaderText = "Доступная виртуальная память (байт)";
            this.VirtualMemory.Name = "VirtualMemory";
            this.VirtualMemory.ReadOnly = true;
            this.VirtualMemory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PageTableAdress
            // 
            this.PageTableAdress.HeaderText = "Адрес таблицы адресов";
            this.PageTableAdress.Name = "PageTableAdress";
            this.PageTableAdress.ReadOnly = true;
            this.PageTableAdress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(810, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1238, 643);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBoxOS, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBoxProcess, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1232, 444);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBoxOS
            // 
            this.groupBoxOS.Controls.Add(this.groupBox1);
            this.groupBoxOS.Controls.Add(this.statusStrip1);
            this.groupBoxOS.Controls.Add(this.labelDriveOutTheProcess);
            this.groupBoxOS.Controls.Add(this.textBoxDriveOutTheProcess);
            this.groupBoxOS.Controls.Add(this.buttonDriveOutTheProcess);
            this.groupBoxOS.Controls.Add(this.labelPIDkill);
            this.groupBoxOS.Controls.Add(this.textBoxPIDkill);
            this.groupBoxOS.Controls.Add(this.buttonKillProcess);
            this.groupBoxOS.Controls.Add(this.buttonCreateNewProcess);
            this.groupBoxOS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOS.Location = new System.Drawing.Point(3, 3);
            this.groupBoxOS.Name = "groupBoxOS";
            this.groupBoxOS.Size = new System.Drawing.Size(610, 438);
            this.groupBoxOS.TabIndex = 0;
            this.groupBoxOS.TabStop = false;
            this.groupBoxOS.Text = "Отдать команду ОС";
            // 
            // groupBoxProcess
            // 
            this.groupBoxProcess.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcess.Location = new System.Drawing.Point(619, 3);
            this.groupBoxProcess.Name = "groupBoxProcess";
            this.groupBoxProcess.Size = new System.Drawing.Size(610, 438);
            this.groupBoxProcess.TabIndex = 1;
            this.groupBoxProcess.TabStop = false;
            this.groupBoxProcess.Text = "Управление процессом";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dataGridView2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(604, 419);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PagePhisicalAdress,
            this.Present,
            this.ReadWrite,
            this.UserSupervisor});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 254);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(598, 162);
            this.dataGridView2.TabIndex = 0;
            // 
            // PagePhisicalAdress
            // 
            this.PagePhisicalAdress.HeaderText = "Физический адрес страницы (20 бит)";
            this.PagePhisicalAdress.Name = "PagePhisicalAdress";
            this.PagePhisicalAdress.ReadOnly = true;
            // 
            // Present
            // 
            this.Present.HeaderText = "Отображение в память";
            this.Present.Name = "Present";
            this.Present.ReadOnly = true;
            // 
            // ReadWrite
            // 
            this.ReadWrite.HeaderText = "Доступ на чтение/запись";
            this.ReadWrite.Name = "ReadWrite";
            this.ReadWrite.ReadOnly = true;
            // 
            // UserSupervisor
            // 
            this.UserSupervisor.HeaderText = "Права доступа к странице";
            this.UserSupervisor.Name = "UserSupervisor";
            this.UserSupervisor.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTableAdress);
            this.panel1.Controls.Add(this.textBoxTableAdress);
            this.panel1.Controls.Add(this.labelByteCount);
            this.panel1.Controls.Add(this.textBoxRequestMemory);
            this.panel1.Controls.Add(this.buttonReqiestMemory);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.labelPID);
            this.panel1.Controls.Add(this.textBoxPID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(598, 245);
            this.panel1.TabIndex = 1;
            // 
            // labelTableAdress
            // 
            this.labelTableAdress.AutoSize = true;
            this.labelTableAdress.Location = new System.Drawing.Point(331, 16);
            this.labelTableAdress.Name = "labelTableAdress";
            this.labelTableAdress.Size = new System.Drawing.Size(84, 13);
            this.labelTableAdress.TabIndex = 8;
            this.labelTableAdress.Text = "Адрес таблицы";
            // 
            // textBoxTableAdress
            // 
            this.textBoxTableAdress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxTableAdress.Location = new System.Drawing.Point(435, 13);
            this.textBoxTableAdress.Name = "textBoxTableAdress";
            this.textBoxTableAdress.ReadOnly = true;
            this.textBoxTableAdress.Size = new System.Drawing.Size(150, 20);
            this.textBoxTableAdress.TabIndex = 7;
            this.textBoxTableAdress.Click += new System.EventHandler(this.labelTableAdress_Click);
            // 
            // labelByteCount
            // 
            this.labelByteCount.AutoSize = true;
            this.labelByteCount.Location = new System.Drawing.Point(236, 148);
            this.labelByteCount.Name = "labelByteCount";
            this.labelByteCount.Size = new System.Drawing.Size(30, 13);
            this.labelByteCount.TabIndex = 6;
            this.labelByteCount.Text = "байт";
            // 
            // textBoxRequestMemory
            // 
            this.textBoxRequestMemory.Location = new System.Drawing.Point(118, 145);
            this.textBoxRequestMemory.Name = "textBoxRequestMemory";
            this.textBoxRequestMemory.Size = new System.Drawing.Size(112, 20);
            this.textBoxRequestMemory.TabIndex = 5;
            // 
            // buttonReqiestMemory
            // 
            this.buttonReqiestMemory.Location = new System.Drawing.Point(16, 128);
            this.buttonReqiestMemory.Name = "buttonReqiestMemory";
            this.buttonReqiestMemory.Size = new System.Drawing.Size(86, 47);
            this.buttonReqiestMemory.TabIndex = 4;
            this.buttonReqiestMemory.Text = "Запросить память";
            this.buttonReqiestMemory.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(118, 78);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 20);
            this.textBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обратиться к странице";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // labelPID
            // 
            this.labelPID.AutoSize = true;
            this.labelPID.Location = new System.Drawing.Point(13, 13);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(25, 13);
            this.labelPID.TabIndex = 1;
            this.labelPID.Text = "PID";
            // 
            // textBoxPID
            // 
            this.textBoxPID.Location = new System.Drawing.Point(44, 10);
            this.textBoxPID.Name = "textBoxPID";
            this.textBoxPID.Size = new System.Drawing.Size(100, 20);
            this.textBoxPID.TabIndex = 0;
            // 
            // buttonCreateNewProcess
            // 
            this.buttonCreateNewProcess.Location = new System.Drawing.Point(6, 32);
            this.buttonCreateNewProcess.Name = "buttonCreateNewProcess";
            this.buttonCreateNewProcess.Size = new System.Drawing.Size(122, 36);
            this.buttonCreateNewProcess.TabIndex = 0;
            this.buttonCreateNewProcess.Text = "Создать новый процесс";
            this.buttonCreateNewProcess.UseVisualStyleBackColor = true;
            // 
            // buttonKillProcess
            // 
            this.buttonKillProcess.Location = new System.Drawing.Point(6, 97);
            this.buttonKillProcess.Name = "buttonKillProcess";
            this.buttonKillProcess.Size = new System.Drawing.Size(122, 30);
            this.buttonKillProcess.TabIndex = 1;
            this.buttonKillProcess.Text = "Убить процесс";
            this.buttonKillProcess.UseVisualStyleBackColor = true;
            // 
            // textBoxPIDkill
            // 
            this.textBoxPIDkill.Location = new System.Drawing.Point(146, 103);
            this.textBoxPIDkill.Name = "textBoxPIDkill";
            this.textBoxPIDkill.Size = new System.Drawing.Size(114, 20);
            this.textBoxPIDkill.TabIndex = 2;
            // 
            // labelPIDkill
            // 
            this.labelPIDkill.AutoSize = true;
            this.labelPIDkill.Location = new System.Drawing.Point(266, 106);
            this.labelPIDkill.Name = "labelPIDkill";
            this.labelPIDkill.Size = new System.Drawing.Size(25, 13);
            this.labelPIDkill.TabIndex = 3;
            this.labelPIDkill.Text = "PID";
            // 
            // labelDriveOutTheProcess
            // 
            this.labelDriveOutTheProcess.AutoSize = true;
            this.labelDriveOutTheProcess.Location = new System.Drawing.Point(266, 156);
            this.labelDriveOutTheProcess.Name = "labelDriveOutTheProcess";
            this.labelDriveOutTheProcess.Size = new System.Drawing.Size(25, 13);
            this.labelDriveOutTheProcess.TabIndex = 6;
            this.labelDriveOutTheProcess.Text = "PID";
            // 
            // textBoxDriveOutTheProcess
            // 
            this.textBoxDriveOutTheProcess.Location = new System.Drawing.Point(146, 153);
            this.textBoxDriveOutTheProcess.Name = "textBoxDriveOutTheProcess";
            this.textBoxDriveOutTheProcess.Size = new System.Drawing.Size(114, 20);
            this.textBoxDriveOutTheProcess.TabIndex = 5;
            // 
            // buttonDriveOutTheProcess
            // 
            this.buttonDriveOutTheProcess.Location = new System.Drawing.Point(6, 147);
            this.buttonDriveOutTheProcess.Name = "buttonDriveOutTheProcess";
            this.buttonDriveOutTheProcess.Size = new System.Drawing.Size(122, 30);
            this.buttonDriveOutTheProcess.TabIndex = 4;
            this.buttonDriveOutTheProcess.Text = "Вытеснить процесс";
            this.buttonDriveOutTheProcess.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(3, 413);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(604, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(173, 17);
            this.toolStripStatusLabel1.Text = "Выполняю в данный момент: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(6, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(601, 215);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Детальное взаимодействие ОС и процесса";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 643);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxOS.ResumeLayout(false);
            this.groupBoxOS.PerformLayout();
            this.groupBoxProcess.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Process;
        private System.Windows.Forms.DataGridViewTextBoxColumn PID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsingMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn VirtualMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn PageTableAdress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBoxOS;
        private System.Windows.Forms.GroupBox groupBoxProcess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PagePhisicalAdress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Present;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReadWrite;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserSupervisor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTableAdress;
        private System.Windows.Forms.TextBox textBoxTableAdress;
        private System.Windows.Forms.Label labelByteCount;
        private System.Windows.Forms.TextBox textBoxRequestMemory;
        private System.Windows.Forms.Button buttonReqiestMemory;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelPID;
        private System.Windows.Forms.TextBox textBoxPID;
        private System.Windows.Forms.Label labelDriveOutTheProcess;
        private System.Windows.Forms.TextBox textBoxDriveOutTheProcess;
        private System.Windows.Forms.Button buttonDriveOutTheProcess;
        private System.Windows.Forms.Label labelPIDkill;
        private System.Windows.Forms.TextBox textBoxPIDkill;
        private System.Windows.Forms.Button buttonKillProcess;
        private System.Windows.Forms.Button buttonCreateNewProcess;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}