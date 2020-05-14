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
                        ram.Add(new RAM(memory * 1024 * 1024, startAdress));  // говнокод
                        startAdress += (uint)(memory * 1024 * 1024);  // говнокод
                    }
                }
            }
            if (!isCorrect)
            {
                MessageBox.Show("Warning", "Установите количество памяти", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                rams = ram.ToArray();
                roms = new ROM[] { new ROM(1) };   // костыль
                cpu = new CPU(32);                   // костыль
            }
            mainForm = new MainForm();
            mainForm.Hardware = new Hardware(ref rams, ref roms, cpu);
            mainForm.OS = new OS(Convert.ToInt32(comboBox1.SelectedItem.ToString()), mainForm.Hardware, Convert.ToInt32(comboBoxPageSize.SelectedItem.ToString()));
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
