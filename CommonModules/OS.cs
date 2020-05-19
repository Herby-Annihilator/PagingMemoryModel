using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SimpleModel.PagingModel;
using System.IO;
using System.Windows.Forms;

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

        public string CurrentDirectoryName { get; set; }

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
            CurrentDirectoryName = Directory.GetCurrentDirectory() + "\\Processies";
        }


        /// <summary>
        /// Создает новый процесс
        /// </summary>
        /// <param name="memory">количество требуемой памяти в байтах</param>
        public void CreateNewProcess(string name, int memory, int prioryty)
        {
            int pid = 0;

            // вычислить размер таблицы - количество страниц для храненния таблицы
            int pageCountForTable = PageNumberForTable(memory * 2);
            try
            {
                BitArray[] bits = GetFreePages(GetFreePageIndexesInListOfPages(pageCountForTable), out uint offset);
                PageTable pageTable = new PageTable(offset, ref bits, pageCountForTable);
                pid = CreateRandomPid();
                //
                // инициализировать таблицу страниц
                //               
                int pageCountToInit = memory / pageSize;
                if (FreePages.Count >= pageCountToInit)
                {
                    for (int i = 0; i < pageCountToInit; i++)
                    {
                        pageTable.PageTableEntries[i].Adress = FreePages[0];
                        FreePages.RemoveAt(0);
                    }
                }
                else
                {
                    throw new OutOfMemoryException("Невозможно выделить минимум памяти процессу");
                }
                processes.Add(new Process(name, memory, pid, prioryty, ref pageTable));
                //
                // создать файл на диске, отвечающий за данный процесс
                //
                File.Create(CurrentDirectoryName + processes[processes.Count - 1].Name + ".prc");
            }
            catch (OutOfMemoryException e)
            {
                MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch(Exception)
            {
                MessageBox.Show("Произошло что-то непредвиденное", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Получает ссылку на массив BitArray[] (физическая память) и смещение в этой памяти, с которого начинается свободный блок
        /// памяти размером в указанное количество страниц. В случае неудачи выкидывает исключение OutOfMemoryException.
        /// </summary>
        /// <param name="indexesInFreePageList"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private ref BitArray[] GetFreePages(int[] indexesInFreePageList, out uint offset)
        {
            uint firstPageAdress = (uint)indexesInFreePageList[0] * (uint)pageSize;
            offset = firstPageAdress;
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint physicalEndAdress = hardware.RAMs[i].PhisicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * bitDepth / 8);
                if (firstPageAdress >= hardware.RAMs[i].PhisicalAdress && firstPageAdress < physicalEndAdress)
                {
                    uint lastPageAdress = (uint)indexesInFreePageList[indexesInFreePageList.Length - 1] * (uint)pageSize;
                    //
                    // это смежные блоки памяти, рсположенные на одной физической плашке
                    //
                    if (lastPageAdress >= hardware.RAMs[i].PhisicalAdress && lastPageAdress < physicalEndAdress)
                    {                      
                        for (int j = 0; j < indexesInFreePageList.Length; j++)
                        {
                            FreePages.Remove(indexesInFreePageList[j]);
                        }
                        return ref hardware.RAMs[i].ByteCells;
                    }
                }
            }
            throw new OutOfMemoryException("В физической памяти данные страницы находятся на разных физических модулях");
        }
        /// <summary>
        /// Возвращает массив номеров элементов списка FreePages. Эти элементы по своему значению идут единым блоком (по порядку).
        /// Вроде бы работает, но это не точно.
        /// </summary>
        /// <param name="pageCount">количество страниц, которые должны быть единым блоком</param>
        /// <returns></returns>
        private int[] GetFreePageIndexesInListOfPages(int pageCount)
        {
            // Условие - список страниц всегда упорядочен по возрастанию
            if (pageCount == 1)
            {
                return new int[] { 0 };
            }
            else
            {
                bool isCorrect = false;
                int numberOfRealyFreePages = 1;
                int currentFreePageIndex = FreePages[0];
                int[] freePageIndexes = new int[pageCount];
                freePageIndexes[0] = 0;
                for (int i = 1; i < FreePages.Count; i++)
                {
                    if (FreePages[i] - currentFreePageIndex == 1)
                    {
                        freePageIndexes[numberOfRealyFreePages] = i;
                        numberOfRealyFreePages++;
                        currentFreePageIndex = FreePages[i];
                        if (numberOfRealyFreePages == pageCount)
                        {
                            isCorrect = true;
                            break;
                        }
                    }
                    else
                    {
                        numberOfRealyFreePages = 1;
                        currentFreePageIndex = FreePages[i];
                        freePageIndexes[0] = i;
                    }
                }
                if (!isCorrect)
                {
                    return null;
                }
                else
                {
                    return freePageIndexes;
                }
            }
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

        // создать механизмы создания файлов в директории для каждого процесса
    }

    public class PageFault : System.Exception
    {
        public override string Message { get { return "Ошибка доступа к странице"; } }
    }
}