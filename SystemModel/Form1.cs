using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonModules;

namespace SystemModel
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// MainForm
        /// </summary>
        private MainForm mainForm;
        /// <summary>
        /// Массив чекбоксов на данной форме
        /// </summary>
        private CheckBox[] checkBoxes;

        private ComboBox[] comboBoxes;
        public Form1()
        {
            InitializeComponent();
            checkBoxes = new CheckBox[] { checkBoxSlotNuber1, checkBoxSlotNuber2, checkBoxSlotNuber3, checkBoxSlotNuber4 };
            comboBoxes = new ComboBox[] { comboBoxRAMSize1, comboBoxRAMSize2, comboBoxRAMSize3, comboBoxRAMSize4 };
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (!CheckOSbitDepth())
            {
                MessageBox.Show("Warning", "Пока что работает только 32-х разрядная система", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }
            if (Convert.ToInt32(comboBoxPageSize.SelectedText) != 4096)
            {
                MessageBox.Show("Warning", "Выберите размер страницы", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxPageSize.Focus();
                return;
            }
            int memory = 0;
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (checkBoxes[i].Checked)
                {
                    if (comboBoxes[i].SelectedIndex > -1)
                    {
                        memory += Convert.ToInt32(comboBoxes[i].SelectedItem.ToString());
                    }
                }
            }
            if (memory == 0)
            {
                MessageBox.Show("Warning", "Установите количество памяти", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mainForm = new MainForm();
            mainForm.Hardware = new Hardware();
            mainForm.OS = new OS();
            this.Hide();

            mainForm.Show();
        }


        private bool CheckOSbitDepth()
        {
            try
            {
                if (Convert.ToInt32(this.comboBox1.SelectedValue.ToString()) != 32)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Warning", "Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.Focus();
            }
            return false;
        }
    }
}
