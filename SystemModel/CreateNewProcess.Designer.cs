namespace SystemModel
{
    partial class CreateNewProcess
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPriority = new System.Windows.Forms.TextBox();
            this.labelNecessaryMemory = new System.Windows.Forms.Label();
            this.textBoxNecessaryMemory = new System.Windows.Forms.TextBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttoncCncellation = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(350, 35);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(199, 20);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Имя процесса";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Начальный приоритет";
            // 
            // textBoxPriority
            // 
            this.textBoxPriority.Location = new System.Drawing.Point(350, 74);
            this.textBoxPriority.MaxLength = 2;
            this.textBoxPriority.Name = "textBoxPriority";
            this.textBoxPriority.Size = new System.Drawing.Size(199, 20);
            this.textBoxPriority.TabIndex = 2;
            this.textBoxPriority.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPriority_KeyPress);
            // 
            // labelNecessaryMemory
            // 
            this.labelNecessaryMemory.AutoSize = true;
            this.labelNecessaryMemory.Location = new System.Drawing.Point(12, 118);
            this.labelNecessaryMemory.Name = "labelNecessaryMemory";
            this.labelNecessaryMemory.Size = new System.Drawing.Size(281, 13);
            this.labelNecessaryMemory.TabIndex = 5;
            this.labelNecessaryMemory.Text = "Минимальная необходимая для работы память (байт)";
            // 
            // textBoxNecessaryMemory
            // 
            this.textBoxNecessaryMemory.Location = new System.Drawing.Point(350, 115);
            this.textBoxNecessaryMemory.Name = "textBoxNecessaryMemory";
            this.textBoxNecessaryMemory.Size = new System.Drawing.Size(199, 20);
            this.textBoxNecessaryMemory.TabIndex = 4;
            this.textBoxNecessaryMemory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPriority_KeyPress);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(350, 176);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(100, 23);
            this.buttonCreate.TabIndex = 6;
            this.buttonCreate.Text = "Создать";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttoncCncellation
            // 
            this.buttoncCncellation.Location = new System.Drawing.Point(456, 176);
            this.buttoncCncellation.Name = "buttoncCncellation";
            this.buttoncCncellation.Size = new System.Drawing.Size(100, 23);
            this.buttoncCncellation.TabIndex = 7;
            this.buttoncCncellation.Text = "Отмена";
            this.buttoncCncellation.UseVisualStyleBackColor = true;
            this.buttoncCncellation.Click += new System.EventHandler(this.buttoncCncellation_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(14, 176);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 8;
            // 
            // CreateNewProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 211);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttoncCncellation);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.labelNecessaryMemory);
            this.Controls.Add(this.textBoxNecessaryMemory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPriority);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateNewProcess";
            this.Text = "CreateNewProcess";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPriority;
        private System.Windows.Forms.TextBox textBoxNecessaryMemory;
        private System.Windows.Forms.Label labelNecessaryMemory;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttoncCncellation;
        private System.Windows.Forms.Label label3;
    }
}