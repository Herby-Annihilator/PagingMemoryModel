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
    public partial class CreateNewProcess : Form
    {
        private OS os;
        public CreateNewProcess(ref OS OSref)
        {
            InitializeComponent();
            os = OSref;
            label3.ForeColor = Color.Red;
            label3.Hide();
        }
        /// <summary>
        /// Кнопка создания процесса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxName.Text))
            {
                label3.Text = "Задайте имя процесса";
                label3.Show();
                textBoxName.Focus();
            }
            else if (string.IsNullOrEmpty(textBoxNecessaryMemory.Text))
            {
                label3.Text = "Некорректное значение количества памяти";
                label3.Show();
                textBoxNecessaryMemory.Focus();
            }
            else if (string.IsNullOrEmpty(textBoxPriority.Text))
            {
                label3.Text = "Некорректное значение начального приоритета";
                label3.Show();
                textBoxPriority.Focus();
            }
            else
            {
                os.CreateNewProcess(Convert.ToInt32(textBoxNecessaryMemory.Text));
            }
        }
        /// <summary>
        /// Защита от букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPriority_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                e.Handled = false;
            }
            label3.Hide();
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            label3.Hide();
        }
    }
}
