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
        private int PageMaxAge { get; set; } = 10;
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
                        pageTable.PageTableEntries[i].Present = true;
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

        /// <summary>
        /// Обрабатывает исключение PageFault алгоритмом WSClock. доработать, не использовать
        /// </summary>
        /// <param name="process">процесс, вызвавший ошибку</param>
        /// <param name="pageNumber">номер страницы в таблице страниц, обращение к которой вызвало ошибку</param>
        /// <param name="processIndex">Индекс процесса в списке процессов</param>
        public void PageFaultExeptionHandler(Process process, int pageNumber, int processIndex)
        {
            if (FreePages.Count > 0)   // вот тут подумать
            {
                process.PageTable.PageTableEntries[pageNumber].Present = true;
                process.PageTable.PageTableEntries[pageNumber].Adress = FreePages[0];
                FreePages.RemoveAt(0);
            }
            else
            {
                bool ableToRemove = false;
                List<WSClockEntryMirror> removalCandidates = new List<WSClockEntryMirror>();
                int workingSetPagesNumber = process.WSClockEntryMirrors.Count;
                for (int i = 0; i < workingSetPagesNumber; i++)
                {
                    if (!ableToRemove)
                    {
                        if (process.WSClockEntryMirrors[i].Referenced == true)
                        {
                            process.WSClockEntryMirrors[i].Referenced = false;
                            process.WSClockEntryMirrors[i].LastUseTime = process.CurrentVirtualTime;
                            continue;
                        }
                        else if (process.CurrentVirtualTime - process.WSClockEntryMirrors[i].LastUseTime <= PageMaxAge) // это еще рабочий набор
                        {
                            removalCandidates.Add(process.WSClockEntryMirrors[i]);
                        }
                        else  // можно удалить
                        {
                            ableToRemove = true;
                            // если страница модифицирована, то ее данные нужно перезаписать на диск
                            if (process.WSClockEntryMirrors[i].Modified == true)
                            {
                                // если страница записана на диск - обновляем данные
                                if (process.WSClockEntryMirrors[i].WrittenIntoFile)
                                {
                                    if (!WritePageToDrive(process.FileName, process.WSClockEntryMirrors[i].PageTableEntry, process.WSClockEntryMirrors[i].PageTableEntryIndex, true))
                                    {
                                        throw new Exception("Дисковая операция с процессом крашнулась");
                                    }
                                }
                                else  // иначе просто пишем в файл
                                {
                                    StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + process.FileName);
                                    WritePageToDrive(writer, process.WSClockEntryMirrors[i].PageTableEntry, process.WSClockEntryMirrors[i].PageTableEntryIndex);
                                    process.WSClockEntryMirrors[i].WrittenIntoFile = true;
                                    writer.Close();
                                }
                            }
                            //
                            // удаляем страницу из памяти
                            //
                            process.WSClockEntryMirrors.RemoveAt(i);
                            //
                            // на ее место нужно поставить нужную страницу
                            //

                            // вычисляем физическое местоположение страницы, вызвавшей ошибку
                            int adress = process.PageTable.PageTableEntries[pageNumber].Adress;
                            int ramBlockNumber = 0;
                            for (int j = 0; j < hardware.RAMs.Length; j++)
                            {
                                uint endPhysicalAdress = hardware.RAMs[j].PhisicalAdress + (uint)(hardware.RAMs[j].ByteCells.Length * (bitDepth / 8));
                                if (adress >= hardware.RAMs[j].PhisicalAdress && adress < endPhysicalAdress)
                                {
                                    ramBlockNumber = j;
                                    break;
                                }
                            }
                            // начало страницы
                            int startBitArrayBlock = (int)(adress - hardware.RAMs[ramBlockNumber].PhisicalAdress) / (bitDepth / 8);
                            // если не удалось восстановить (нет на диске)
                            if (!RestoreProcessPage(ref process, pageNumber, ref hardware.RAMs[ramBlockNumber].ByteCells, startBitArrayBlock, adress))
                            {
                                MessageBox.Show("Данные поцесса на диске не найдены. Процесс будет убит.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                    }
                }
            }
        }

        private ref RAM AppropriatePhysicalRamBlock(uint adress)
        {
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint physicalEndAdress = hardware.RAMs[i].PhisicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * (bitDepth / 8));
                if (adress >= hardware.RAMs[i].PhisicalAdress && adress < physicalEndAdress)
                {
                    return ref hardware.RAMs[i];
                }
            }
            throw new Exception("Не удалось найти соответсвующую физическую память");
        }
        /// <summary>
        /// Превращает номер страницы в общем адресном пространстве в реальный физический адрес.
        /// </summary>
        /// <param name="pageAdressInPageTableEntry">грубо говоря - номер страницы в общем адресном пространстве (поле Adress)</param>
        /// <returns></returns>
        private uint ConvertToRealPhysicalAdress(int pageAdressInPageTableEntry)
        {
            return (uint)((uint)pageAdressInPageTableEntry * pageSize);
        }

        public void KillProcess(int processIndex)
        {
            
            for (int i = 0; i < processes[processIndex].PageTable.Size; i++)
            {
                //
                // очистил страницы
                //
                uint adress = ConvertToRealPhysicalAdress(processes[processIndex].PageTable.PageTableEntries[i].Adress);
                RAM ram = AppropriatePhysicalRamBlock(adress);
                ClearPage(ref ram.ByteCells, ram.PhisicalAdress, adress);
                //
                // добавить в список свободных страниц
                //
                if (processes[processIndex].PageTable.PageTableEntries[i].Adress != 0 && processes[processIndex].PageTable.PageTableEntries[i].Present)
                {
                    FreePages.Add(processes[processIndex].PageTable.PageTableEntries[i].Adress);
                }
            }
            FreePages.Sort();
            

        }
        /// <summary>
        /// Очищает физическую страницу
        /// </summary>
        /// <param name="ram">физическая память</param>
        /// <param name="physicalStartAdress">настоящий адрес начала данного блока физической памяти</param>
        /// <param name="pageStartAdress">физический адрес страницы</param>
        private void ClearPage(ref BitArray[] ram, uint physicalStartAdress, uint pageStartAdress)
        {
            int startBitArrayBlock = ((int)(pageStartAdress - physicalStartAdress) / (bitDepth / 8));
            int numberOfBitsBlocks = pageSize / bitDepth;
            for (int i = startBitArrayBlock; i < numberOfBitsBlocks; i++)
            {
                ram[i].SetAll(false);
            }
        }
        //
        // создать механизмы создания файлов в директории для каждого процесса
        //
        /// <summary>
        /// Записывает страницу процесса на диск. В случае неудачи перезаписи вернет false.
        /// </summary>
        /// <param name="processFileName">имя файла, отвечающего за данный процесс</param>
        /// <param name="pageTableEntry">сведения о странице в таблице страниц</param>
        /// <param name="pageNumber">номер страницы в таблицы страниц</param>
        /// <param name="rewrite">Если true, то данные указанной страницы перезаписываются, если false, то данные просто добавляются в конец файла</param>
        /// <returns></returns>
        private bool WritePageToDrive(string processFileName, PageTableEntry pageTableEntry, int pageNumber, bool rewrite)
        {
            if (rewrite)
            {
                StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + "tmp.prc");
                StreamReader reader = new StreamReader(CurrentDirectoryName + "\\" + processFileName);

                string pageIndexInPageTable;
                string[] buffer;
                int number = 0;
                bool isRewrite = false;
                while (!reader.EndOfStream)
                {
                    if (!isRewrite)
                    {
                        pageIndexInPageTable = reader.ReadLine();
                        buffer = pageIndexInPageTable.Split(new char[] { ' ', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                        bool isNumber = false;
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if (Int32.TryParse(buffer[i], out number))
                            {
                                isNumber = true;
                                break;
                            }
                        }
                        if (!isNumber)
                        {
                            return false;
                        }

                        if (number == pageNumber)
                        {
                            WritePageToDrive(writer, pageTableEntry, pageNumber);
                            isRewrite = true;
                        }
                        else
                        {
                            writer.Write(reader.ReadLine());
                        }
                    }
                    else
                    {
                        writer.Write(reader.ReadToEnd());
                    }
                }
                reader.Close();
                writer.Close();
                File.Delete(CurrentDirectoryName + "\\" + processFileName);
                File.Move(CurrentDirectoryName + "\\" + "tmp.prc", CurrentDirectoryName + "\\" + processFileName);
            }
            else
            {
                StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + processFileName, true);
                WritePageToDrive(writer, pageTableEntry, pageNumber);
                writer.Close();
            }
            return true;
        }
        private void WritePageToDrive(TextWriter writer, PageTableEntry pageTableEntry, int pageIndexInPageTable)
        {
            uint adress = (uint)(pageTableEntry.Adress * pageSize);
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint endPhisycalAdress = hardware.RAMs[i].PhisicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * (bitDepth / 8));
                if (adress >= hardware.RAMs[i].PhisicalAdress && adress < endPhisycalAdress)
                {
                    uint pageStartIndex = (adress - hardware.RAMs[i].PhisicalAdress) / (uint)(bitDepth / 8);  // стартовый индекс BitArray, с которого начинается страница
                    uint pageEndIndex = pageStartIndex + (uint)pageSize / 4;   // блоки по 32 бита
                    writer.WriteLine("[ " + pageIndexInPageTable + " ]");
                    for (uint j = pageStartIndex; j < pageEndIndex; j++)
                    {
                        writer.Write(hardware.RAMs[i].ByteCells[j].ToString() + " ");
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Восстанавливает данные страницы с диска. Если не удалось восстановить (не нашлось такой страницы), то вернет false.
        /// Если формат неверен, то выкинет исключение.
        /// </summary>
        /// <param name="process">процесс, для которого производится восстановление</param>
        /// <param name="pageIndexInPageTable">номер записи, которая отвечает за восстанавливаюмую страницу</param>
        /// <param name="ram">физическая память, нужен соответсвующий массив BitArray[]</param>
        /// <param name="startBitArrayBlock">Начало страницы, номер блока из 32 битов, номер BitArray(я)</param>
        /// <param name="newPageAdress">новый физический адрес страницы</param>
        /// <returns></returns>
        private bool RestoreProcessPage(ref Process process, int pageIndexInPageTable, ref BitArray[] ram, int startBitArrayBlock, int newPageAdress)
        {
            StreamReader reader = new StreamReader(CurrentDirectoryName + "\\" + process.FileName);
            //StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + "tmp.prc");
            bool isRead = false;
            string pageNumber;
            string[] buffer;
            int number = 0;
            while (!reader.EndOfStream && !isRead)
            {

                pageNumber = reader.ReadLine();
                buffer = pageNumber.Split(new char[] { ' ', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                bool isNumber = false;
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (Int32.TryParse(buffer[i], out number))
                    {
                        isNumber = true;
                        break;
                    }
                }
                if (!isNumber)
                {
                    throw new Exception("Неверный формат в файле");
                }

                if (number == pageIndexInPageTable)
                {
                    string[] bitBlocks = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    process.PageTable.PageTableEntries[pageIndexInPageTable].Adress = newPageAdress;
                    int endBitArrayBlock = startBitArrayBlock + pageSize / (bitDepth / 8);     // 4096 байт / 4 байта = 128 блоков
                    int k = 0;
                    for (int i = startBitArrayBlock; i < endBitArrayBlock; i++)
                    {
                        for (int j = 0; i < ram[i].Length; j++)
                        {
                            if (bitBlocks[k][j] == '0')
                            {
                                ram[i].Set(j, false);
                            }
                            else
                            {
                                ram[i].Set(j, true);
                            }
                        }
                        k++;
                    }
                    process.WSClockEntryMirrors.Add(new WSClockEntryMirror(ref process.PageTable.PageTableEntries[pageIndexInPageTable], process.CurrentVirtualTime, pageIndexInPageTable));
                    process.WSClockEntryMirrors[process.WSClockEntryMirrors.Count - 1].WrittenIntoFile = true;
                    isRead = true;
                }
            }
            reader.Close();
            if (isRead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class PageFault : System.Exception
    {
        public override string Message { get { return "Ошибка доступа к странице"; } }
    }

}