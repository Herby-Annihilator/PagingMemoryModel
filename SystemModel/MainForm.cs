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
using System.Collections;


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

        public MainForm(ref OS os, ref Hardware hardware)
        {
            InitializeComponent();
            OS = os;
            Hardware = hardware;

            OS.CreateNewProcess("systemDaemonA", 4096, 1);
            OS.CreateNewProcess("systemDaemonB", 4096, 1);
            OS.CreateNewProcess("systemDaemonC", 4096, 1);

            //timer = new System.Timers.Timer(5000);
            //timer.Elapsed += os.StartNextProcess;
            //timer.Elapsed += DataGridVeiwUpdate;
            //timer.AutoReset = true;
            //timer.Enabled = true;          
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
                //timer.Stop();
                buttonCreateNewProcess.Enabled = true;
                buttonDriveOutTheProcess.Enabled = true;
                buttonKillProcess.Enabled = true;
                button1.Enabled = true;

                textBoxDriveOutTheProcess.Enabled = true;
                textBoxPIDkill.Enabled = true;              

                groupBoxProcess.Enabled = true;
            }
            else
            {
                button2.Text = "Приостановить симуляцию";
                //timer.Start();

                buttonCreateNewProcess.Enabled = false;
                buttonDriveOutTheProcess.Enabled = false;
                buttonKillProcess.Enabled = false;
                button1.Enabled = false;

                textBoxDriveOutTheProcess.Enabled = false;
                textBoxPIDkill.Enabled = false;

                groupBoxProcess.Enabled = true;
            }
        }

        /// <summary>
        /// Обновляет данные таблицы (главной внизу) в соответсвии с тем, какой процесс работает
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridVeiwUpdate(/*object sender, EventArgs e*/)
        {
            int index = os.currentProcessNumber;
            if (index >= dataGridView1.Rows.Count)
            {
                dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows.Add(); }); ;
            }
            dataGridView1.Rows[index].Cells[0].Value = os.CurrentProcess.Name;
            dataGridView1.Rows[index].Cells[1].Value = os.CurrentProcess.PID;
            dataGridView1.Rows[index].Cells[2].Value = os.CurrentProcess.Status;
            dataGridView1.Rows[index].Cells[3].Value = os.CurrentProcess.AvailableMemory;
            dataGridView1.Rows[index].Cells[4].Value = os.CurrentProcess.UsedMemory;
            dataGridView1.Rows[index].Cells[5].Value = 2147483648;   // 2^32 / 2

            uint adress = 0;
            bool isCorrect;
            for (int i = 0; i < Hardware.RAMs.Length; i++)
            {
                adress = os.CurrentProcess.PageTable.GetRealPhysicalAdress(ref Hardware.RAMs[i], out isCorrect);
                if (isCorrect)
                {
                    break;
                }
            }
            BitArray bitArray = new BitArray(new int[] { (int)adress });

            string str = os.GetBitArrayStringFormat(bitArray);
            char[] rstr = str.ToCharArray();
            Array.Reverse(rstr);

            dataGridView1.Rows[index].Cells[6].Value = Convert.ToString(rstr) + "(" + adress.ToString() + ")";
        }
        /// <summary>
        /// Тыкаем на любую ячейку на главной таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.RowIndex < dataGridView1.Rows.Count)
                {
                    textBoxPID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBoxTableAdress.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    dataGridView2.Rows.Clear();

                    Process process = os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text));

                    try
                    {
                        //bool isCorrect = false;
                        uint adress = 0;
                        int pageTableSize = process.PageTable.Size;
                        for (int i = 0; i < pageTableSize; i++)
                        {
                            dataGridView2.Rows.Add();

                            for (int j = 0; j < Hardware.RAMs.Length; j++)
                            {
                                adress = (uint)process.PageTable.PageTableEntries[i].Adress * 4096;     // костыль
                                //adress = process.PageTable.GetRealPhysicalAdress(ref Hardware.RAMs[j], out isCorrect);
                                //if (isCorrect)
                                //{
                                //    break;
                                //}
                            }
                            //if (!isCorrect)
                            //{
                            //    throw new Exception("Ошибка при вычислении реального адреса таблицы");
                            //}

                            dataGridView2.Rows[i].Cells[0].Value = adress;
                            dataGridView2.Rows[i].Cells[1].Value = process.PageTable.PageTableEntries[i].Present;
                            dataGridView2.Rows[i].Cells[2].Value = process.PageTable.PageTableEntries[i].RW;
                            dataGridView2.Rows[i].Cells[3].Value = process.PageTable.PageTableEntries[i].UserSupervisor;
                        }
                    }
                    catch(Exception e1)
                    {
                        MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        /// <summary>
        /// Тыкаем любую ячейку на таблице, которая отображает более подробную инфу о процессе.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.RowIndex < dataGridView2.Rows.Count)
                {
                    textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
            }
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            os.StartNextProcess();
            DataGridVeiwUpdate();
            os.CurrentProcess.PageTable.PageTableEntries[0].Present = false;
            os.ChangeCurrentProcessNumber();
        }
    }
}
