using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SimpleModel.PagingModel;
using System.IO;

namespace CommonModules
{
    public class OS
    {
        /// <summary>
        /// Список свободных страниц. Элемент содержит номер свободной страницы и он не обязательно равен индексу
        /// </summary>
        private List<int> FreePages { get; set; }
        /// <summary>
        /// Число байт в странице
        /// </summary>
        private int pageSize;
        /// <summary>
        /// Железо
        /// </summary>
        private Hardware hardware;
        /// <summary>
        /// Список процессов 
        /// </summary>
        private List<Process> processes;
        /// <summary>
        /// Разрядность
        /// </summary>
        private int bitDepth;
        /// <summary>
        /// Разрядность ОС
        /// </summary>
        public int BitDepth
        {
            get
            {
                return bitDepth;
            }
            set
            {
                if (value == 64)
                {
                    bitDepth = value;
                }
                else
                {
                    bitDepth = 32;
                }
            }
        }

        /// <summary>
        /// Конструктор заточен под 32 бита и 4096 размер страниц
        /// </summary>
        /// <param name="bitDepth">разрядность</param>
        /// <param name="hardware">железо в машине</param>
        /// <param name="pageSize">размер страницы</param>
        public OS(int bitDepth, Hardware hardware, int pageSize)
        {
            this.pageSize = pageSize;
            BitDepth = bitDepth;
            this.hardware = hardware;
            int freePageCount = 0;
            for (int i = 0; i < hardware.RamSlotsNumber; i++)
            {
                freePageCount += (hardware.RAMs[i].ByteCells.Length * 4) / pageSize; // число блоков по 32 бита * 4 = число байт. Поделим на размер страницы в байтах и получим число страниц
            }
            this.FreePages = new List<int>(freePageCount);
            for (int i = 0; i < freePageCount; i++)
            {
                FreePages[i] = i;
            }

            //
            // Создаю директорию в которой будут храниться текстовики для каждого процесса (один процесс = один файл)
            //

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Processies");
        }


        /// <summary>
        /// Создает новый процесс
        /// </summary>
        /// <param name="memory">количество требуемой памяти в байтах</param>
        public void CreateNewProcess(int memory)
        {
            int pid = 0;

            // вычислить размер таблицы - количество страниц для храненния таблицы
            int pageCountForTable = PageNumberForTable(memory);
            try
            {
                BitArray[] ram = hardware.GetFreeMemoryBlock((pageCountForTable * pageSize) / (bitDepth / 8), out uint offset);
                //**********************
                PageTable pageTable = new PageTable(offset, ref ram);
                //***********************

                pid = CreateRandomPid();

            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("\nПамять для процесса не была выделена");
                Console.ReadKey(true);
            }

            processes.Add(new Process(memory, pid));
        }
        /// <summary>
        /// Вернет количество страниц для хранения таблицы страниц для данного процесса
        /// </summary>
        /// <param name="memory">количество памяти (в байтах), которой должен владеть процесс</param>
        /// <returns></returns>
        private int PageNumberForTable(int memory)
        {
            int pageCount = memory / pageSize; // число страниц которое нужно отобразить
            int recordCount = pageSize / (BitDepth / 8);  // число записей по 32(64) бита, которое помещается в страницу
            if (pageCount % recordCount > 0)
            {
                return pageCount / recordCount + 1;
            }
            else
            {
                return pageCount / recordCount;
            }
        }
        /// <summary>
        /// Создает PID для процесса, отличный от других
        /// </summary>
        /// <returns></returns>
        private int CreateRandomPid()
        {
            Random random = new Random();
            bool isCorrect = false;
            int pid = 0;
            while (isCorrect == false)
            {
                pid = random.Next(1000, 9999);
                for (int i = 0; i < processes.Count; i++)
                {
                    isCorrect = true;
                    if (processes[i].PID == pid)
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }
            return pid;
        }

        private ref BitArray[] GetNextFreePage(ref BitArray[] startArray, uint startIndex, out uint endIndex)
        {
            hardware.
        }


        // создать механизмы создания файлов в директории для каждого процесса
    }
}