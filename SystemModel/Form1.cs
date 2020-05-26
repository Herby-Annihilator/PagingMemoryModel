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
        private RAM[] rams;
        private ROM[] roms;
        private CPU cpu;
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
        /// <summary>
        /// Согласиться с конфигурацией машины. Метод надо переписать, потому что - говнокод
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAccept_Click(object sender, EventArgs e)  // говнокод
        {
            if (!CheckOSbitDepth())
            {
                MessageBox.Show("Пока что работает только 32-х разрядная система", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }
            if (comboBoxPageSize.Text != "4096")
            {
                MessageBox.Show("Выберите размер страницы", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxPageSize.Focus();
                return;
            }
            int memory = 0;
            bool isCorrect = false;
            uint startAdress = 0;      // стартовый адрес первого байта в физической плашке
            List<RAM> ram = new List<RAM>();
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (checkBoxes[i].Checked)
                {
                    if (comboBoxes[i].SelectedIndex > -1)
                    {
                        isCorrect = true;
                        memory = Convert.ToInt32(comboBoxes[i].SelectedItem.ToString());  // говнокод
                        ram.Add(new RAM(memory * 1024 * 1024, startAdress, Convert.ToInt32(comboBox1.Text)));  // говнокод
                        startAdress += (uint)(memory * 1024 * 1024);  // говнокод
                    }
                }
            }
            if (!isCorrect)
            {
                MessageBox.Show("Установите количество памяти", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                rams = ram.ToArray();
                roms = new ROM[] { new ROM(1) };   // костыль
                cpu = new CPU(32);                   // костыль
            }
            Hardware hardware = new Hardware(ref rams, ref roms, cpu);
            OS os = new OS(Convert.ToInt32(comboBox1.Text), hardware, Convert.ToInt32(comboBoxPageSize.Text));
            mainForm = new MainForm(ref os, ref hardware);


            this.Hide();

            mainForm.Show();
        }

        /// <summary>
        /// Проверяет разрядность ОС (пока что только на 32 бита)
        /// </summary>
        /// <returns></returns>
        private bool CheckOSbitDepth()
        {
            try
            {
                if (Convert.ToInt32(this.comboBox1.Text) != 32)
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
                MessageBox.Show("Type Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.Focus();
            }
            return false;
        }

        private void checkBoxSlotNuber2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSlotNuber2.Checked)
            {
                comboBoxRAMSize2.Enabled = true;
            }
            else
            {
                comboBoxRAMSize2.Enabled = false;
                comboBoxRAMSize2.SelectedIndex = -1;
            }
        }

        private void checkBoxSlotNuber1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSlotNuber1.Checked)
            {
                comboBoxRAMSize1.Enabled = true;
            }
            else
            {
                comboBoxRAMSize1.Enabled = false;
                comboBoxRAMSize1.SelectedIndex = -1;
            }
        }

        private void checkBoxSlotNuber3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSlotNuber3.Checked)
            {
                comboBoxRAMSize3.Enabled = true;
            }
            else
            {
                comboBoxRAMSize3.Enabled = false;
                comboBoxRAMSize3.SelectedIndex = -1;
            }
        }

        private void checkBoxSlotNuber4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSlotNuber4.Checked)
            {
                comboBoxRAMSize4.Enabled = true;
            }
            else
            {
                comboBoxRAMSize4.Enabled = false;
                comboBoxRAMSize4.SelectedIndex = -1;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
