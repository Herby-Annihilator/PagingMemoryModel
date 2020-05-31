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
using System.Threading;


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
        /// <summary>
        /// Ссылка на родителя
        /// </summary>
        public Form1 ParentForm { get; set; }

        public MainForm(ref OS os, ref Hardware hardware, Form1 form)
        {
            ParentForm = form;
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
            createNewProcessForm.ShowDialog();
            //createNewProcessForm.Show();
        }
        /// <summary>
        /// Показывает, приостановлена или нет симуляция.
        /// </summary>
        private bool OSmode = true;
        /// <summary>
        /// Приостановить/возобновить симуляцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OSmode = !OSmode;
            if (OSmode)
            {
                buttonCreateNewProcess.Enabled = true;
                buttonKillProcess.Enabled = true;
                button1.Enabled = true;
                buttonStartProcess.Enabled = true;

                textBoxPIDkill.Enabled = true;              

                groupBoxProcess.Enabled = false;
                button1.Enabled = false;
                textBox1.Clear();
            }
            else
            {
                buttonCreateNewProcess.Enabled = false;
                buttonKillProcess.Enabled = false;
                button1.Enabled = false;
                buttonStartProcess.Enabled = false;

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
            string str1 = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                str1 += str[i];
            }

            dataGridView1.Rows[index].Cells[6].Value = str1 + "(" + adress.ToString() + ")";
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
                    textBoxPIDtoStart.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

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
                            }



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
                    button1.Enabled = true;
                    textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    Process process = os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text));
                    textBoxPageTableEntry.Text = os.GetBitArrayStringFormat(process.PageTable.PageTableEntries[e.RowIndex].Entry);
                    textBoxNumberInPageTable.Text = e.RowIndex.ToString();
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParentForm.Close();
        }
        /// <summary>
        /// Имена процессов, которые не могут быть убиты
        /// </summary>
        private string[] CanNotBeKilled = new string[] { "systemDaemonA", "systemDaemonB", "systemDaemonC" };
        /// <summary>
        /// Кнопка "Убить процесс"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKillProcess_Click(object sender, EventArgs e)
        {
            try
            {
                Process process = os.GetCheckedProcess(Convert.ToInt32(this.textBoxPIDkill.Text));
                if (process != null)
                {
                    bool isCorret = true;
                    for (int i = 0; i < CanNotBeKilled.Length; i++)
                    {
                        if (process.Name == CanNotBeKilled[i])
                        {
                            isCorret = false;
                            break;
                        }
                    }
                    if (isCorret)
                    {
                        os.KillProcess(process.PID);
                    }
                    else
                    {
                        MessageBox.Show("Данный процесс не может быть убит", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Такого процесса нет", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception e1)
            {
                MessageBox.Show("Поле напротив кнопки 'Убить процесс' имеет некорректное значение", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxPIDkill_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Кнопка "Обратиться к странице"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bool isPageFault = false;
            int pageNumberInPageTable = Convert.ToInt32(textBoxNumberInPageTable.Text);

            Process process = os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text));
            int startAvailableMemory = process.AvailableMemory;
            try
            {
                process.AccessPage(pageNumberInPageTable, Hardware);
            }
            catch(PageFault pageFault)
            {
                isPageFault = true;
                os.PageFaultExeptionHandler(os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text)), pageFault.PageFaultNumber);
                UpdateDataGridView2();
            }
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            textBoxListing.Clear();

            if (os.IsProcessKilled(Convert.ToInt32(textBoxPID.Text)))
            {
                textBoxListing.Text = "Процесс был убит";
                dataGridView2.Rows.Clear();
                int pid = Convert.ToInt32(textBoxPID.Text);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((int)dataGridView1.Rows[i].Cells[1].Value == pid)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        dataGridView1.Update();
                        break;
                    }
                }
                textBoxPID.Clear();
                textBoxTableAdress.Clear();
                textBoxPageTableEntry.Clear();
                textBoxNumberInPageTable.Clear();
                textBoxPIDtoStart.Clear();
            }
            else
            {
                int numberOfNewlySelectedPages = (process.AvailableMemory - startAvailableMemory) / os.PageSize;
                int numberOfReplacedPages;
                if (numberOfNewlySelectedPages == 0)
                {
                    numberOfReplacedPages = 1;
                }
                else
                {
                    numberOfReplacedPages = 0;
                }

                UpdateDataGridView2();

                textBoxListing.Text = "Обработка ошибки отсутствия страницы " + isPageFault.ToString() + "\r\n";
                if (isPageFault)
                {
                    textBoxListing.Text += "Выделено новых страниц " + numberOfNewlySelectedPages + "\r\n" +
                        "Замещено страниц " + numberOfReplacedPages + "\r\n";
                }

                textBoxListing.Text += "\r\nИзмененные данные процесса находятся тут: " + os.CurrentDirectoryName + "\\" + process.FileName;
            }

            
        }
        /// <summary>
        /// Обновляет данные таблицы dataGridView2
        /// </summary>
        public void UpdateDataGridView2()
        {
            dataGridView2.Rows.Clear();
            Process process = os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text));
            for (int i = 0; i < process.PageTable.Size; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = process.PageTable.PageTableEntries[i].Adress * os.PageSize;
                dataGridView2.Rows[i].Cells[1].Value = process.PageTable.PageTableEntries[i].Present;
                dataGridView2.Rows[i].Cells[2].Value = process.PageTable.PageTableEntries[i].RW;
                dataGridView2.Rows[i].Cells[3].Value = process.PageTable.PageTableEntries[i].UserSupervisor;
            }
        }

        /// <summary>
        /// Запускаем процесс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartProcess_Click(object sender, EventArgs e)
        {
            Process process;
            try
            {
                process = os.GetCheckedProcess(Convert.ToInt32(textBoxPIDtoStart.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Random random = new Random();
            int numberOfStarts = random.Next(100, 500);
            int numberOfMisses = 0;
            int maxNumberOfMisses = process.WSClockEntryMirrors.Count * 3;
            int numberOfBitChanges = 0;

            int numberOfNewlySelectedPages;  // количество вновь выделенных страниц
            int numberOfReplacedPages;  // количество замещенных страниц 
            int StartAvailableMemory = process.AvailableMemory;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            long time = 0;

            for (int i = 0; i < numberOfStarts; i++)
            {
                process.CurrentVirtualTime += 10;   // пусть это будет кол-во миллисекунд
                if (process.CurrentVirtualTime % 20 == 0)   // пусть раз в 20 миллисекунд биты изменения и обращения будут обновляться
                {
                    numberOfBitChanges++;
                    // обновляем биты обращения и модификации
                    for(int k = 0; k < process.WSClockEntryMirrors.Count; k++)
                    {
                        process.WSClockEntryMirrors[k].Modified = false;
                        process.WSClockEntryMirrors[k].Referenced = false;
                    }
                }
                process.Status = CommonModules.Status.IsPerformed;
                process.UsedMemory = process.WSClockEntryMirrors.Count * os.PageSize;
                process.AvailableMemory = process.UsedMemory;
                try
                {
                    int nextPage;
                    if (numberOfMisses < maxNumberOfMisses)
                    {
                        nextPage = random.Next(0, process.PageTable.Size);
                    }
                    else
                    {
                        bool isCorrect = false;
                        do
                        {
                            nextPage = random.Next(0, process.PageTable.Size);
                            for (int j = 0; j < process.WSClockEntryMirrors.Count; j++)
                            {
                                if (nextPage == process.WSClockEntryMirrors[j].PageTableEntryIndex)
                                {
                                    isCorrect = true;
                                    break;
                                }
                            }
                        } while (!isCorrect);
                    }
                    process.AccessPage(nextPage, Hardware);
                }
                catch(PageFault pageFault)
                {
                    numberOfMisses++;
                    stopwatch.Start();
                    os.PageFaultExeptionHandler(process, pageFault.PageFaultNumber);
                    stopwatch.Stop();
                    time += stopwatch.ElapsedMilliseconds;
                    stopwatch.Reset();

                    if (os.IsProcessKilled(Convert.ToInt32(textBoxPIDtoStart.Text)))
                    {
                        textBoxListing.Text = "Процесс был убит";
                        dataGridView2.Rows.Clear();
                        int pid = Convert.ToInt32(textBoxPID.Text);
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            if ((int)dataGridView1.Rows[j].Cells[1].Value == pid)
                            {
                                dataGridView1.Rows.RemoveAt(j);
                                dataGridView1.Update();
                                break;
                            }
                        }
                        textBoxPID.Clear();
                        textBoxTableAdress.Clear();
                        textBoxPageTableEntry.Clear();
                        textBoxNumberInPageTable.Clear();
                        textBoxPIDtoStart.Clear();

                        return;
                    }
                }   
            }
            //
            // Поместить процесс в очередь
            //
            numberOfNewlySelectedPages = (process.AvailableMemory - StartAvailableMemory) / os.PageSize; // насколько увеличилось использование памяти, на столько же увеличилось число выделенных вновь страниц
            numberOfReplacedPages = numberOfMisses - numberOfNewlySelectedPages; // число замещенных страниц равно разности между числом промахов и числом выделенных вновь станиц
            process.UsedMemory = 0;
            process.Status = CommonModules.Status.Queue;
            //
            // Отобразить статистику
            //
            textBoxListing.Text = "Число обращений к страницам = " + numberOfStarts + "\r\n" +
                "Число ошибок отсутсвия страницы = " + numberOfMisses + "\r\n" +
                "Биты обращения и изменения были сброшены " + numberOfBitChanges + " раз\r\n" +
                "Число вновь выделенных страниц = " + numberOfNewlySelectedPages + "\r\n" +
                "Число замещенных страниц = " + numberOfReplacedPages + "\r\n" +
                "Время, затраченное на обработку исключений = " + time + "мс" + "\r\n\r\n" +
                "Измененные данные процесса находятся тут: " + os.CurrentDirectoryName + "\\" + process.FileName;

            UpdateCurrentRowInDataGridView1(process.PID);
        }
        /// <summary>
        /// Обновляет строку в таблице для указанного процесса
        /// </summary>
        /// <param name="processPID">PID процесса</param>
        private void UpdateCurrentRowInDataGridView1(int processPID)
        {
            Process process = os.GetCheckedProcess(processPID);
            int updatingRow = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((int)dataGridView1.Rows[i].Cells[1].Value == processPID)
                {
                    updatingRow = i;
                    break;
                }
            }
            dataGridView1.Rows[updatingRow].Cells[2].Value = process.Status;
            dataGridView1.Rows[updatingRow].Cells[3].Value = process.AvailableMemory;
            dataGridView1.Rows[updatingRow].Cells[4].Value = process.UsedMemory;

            uint adress = 0;
            bool isCorrect;
            for (int i = 0; i < Hardware.RAMs.Length; i++)
            {
                adress = process.PageTable.GetRealPhysicalAdress(ref Hardware.RAMs[i], out isCorrect);
                if (isCorrect)
                {
                    break;
                }
            }
            BitArray bitArray = new BitArray(new int[] { (int)adress });

            string str = os.GetBitArrayStringFormat(bitArray);
            string str1 = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                str1 += str[i];
            }

            dataGridView1.Rows[updatingRow].Cells[6].Value = str1 + "(" + adress.ToString() + ")";
        }
        /// <summary>
        /// Сбросить бит отображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonThrowOffPresentBit_Click(object sender, EventArgs e)
        {
            Process process = os.GetCheckedProcess(Convert.ToInt32(textBoxPID.Text));
            process.PageTable.PageTableEntries[Convert.ToInt32(textBoxNumberInPageTable)].Present = false;
        }
    }
}
