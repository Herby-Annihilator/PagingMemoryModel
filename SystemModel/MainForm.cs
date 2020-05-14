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
    public partial class MainForm : Form
    {
        /// <summary>
        /// OS
        /// </summary>
        public  OS OS { get; set; }
        /// <summary>
        /// Фома для создания нового процесса
        /// </summary>
        private CreateNewProcess createNewProcessForm;
        /// <summary>
        /// Железо в данном компьютере
        /// </summary>
        public Hardware Hardware { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void labelTableAdress_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Показывает форму для создания нового процесса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateNewProcess_Click(object sender, EventArgs e)
        {
            createNewProcessForm = new CreateNewProcess();
            createNewProcessForm.Show();
        }
    }
}
