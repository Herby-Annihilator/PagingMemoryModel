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
        /// Таймер, по каждому прерыванию запускает старт следующего в списке процесса.
        /// </summary>
        System.Timers.Timer timer;
        private OS os;
        /// <summary>
        /// OS
        /// </summary>
        public  OS OS { get { return os; } set { os = value; } }
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
            os.CreateNewProcess("systemDaemonA", 4096, 1);
            os.CreateNewProcess("systemDaemonB", 4096, 1);
            os.CreateNewProcess("systemDaemonC", 4096, 1);

            timer = new System.Timers.Timer(5000);
            timer.Elapsed += os.StartNextProcess;
            timer.Elapsed += DataGridVeiwUpdate;
            timer.AutoReset = true;
            timer.Enabled = true;
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
            createNewProcessForm = new CreateNewProcess(ref os);
            createNewProcessForm.Show();
        }
        /// <summary>
        /// Показывает, приостановлена или нет симуляция.
        /// </summary>
        private bool pause = false;
        /// <summary>
        /// Приостановить/возобновить симуляцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            pause = !pause;
            if (pause)
            {
                button2.Text = "Возобновить симуляцию";
                timer.Stop();
                buttonCreateNewProcess.Enabled = true;
                buttonDriveOutTheProcess.Enabled = true;
                buttonKillProcess.Enabled = true;
                button1.Enabled = true;

                textBoxDriveOutTheProcess.Enabled = true;
                textBoxPIDkill.Enabled = true;
                textBox1.Enabled = true;
            }
            else
            {
                button2.Text = "Приостановить симуляцию";
                timer.Start();

                buttonCreateNewProcess.Enabled = false;
                buttonDriveOutTheProcess.Enabled = false;
                buttonKillProcess.Enabled = false;
                button1.Enabled = false;

                textBoxDriveOutTheProcess.Enabled = false;
                textBoxPIDkill.Enabled = false;
                textBox1.Enabled = false;
            }
        }


        private void DataGridVeiwUpdate(object sender, EventArgs e)
        {
            int index = os.currentProcessNumber;
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.Name;
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.PID;
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.Status;
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.AvailableMemory;
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.UsedMemory;
            dataGridView1.Rows[index].Cells[0].Value = 2147483648;   // 2^32 / 2
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.PageTable.StartIndex;
            
        }
    }
}
