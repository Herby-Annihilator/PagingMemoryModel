﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using CommonModules.PagingModel;

namespace CommonModules
{
    public class OS
    {    
        /// <summary>
        /// Получает ссылку на список процессов
        /// </summary>
        public List<Process> Processes { get { return processes; } }
        private int PageMaxAge { get; set; } = 10;
        /// <summary>
        /// Список свободных страниц. Элемент содержит номер свободной страницы и он не обязательно равен индексу
        /// </summary>
        private List<int> FreePages { get; set; }
        /// <summary>
        /// Число байт в странице
        /// </summary>
        public int PageSize { get; set; }
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
        /// Путь в директорию Processies
        /// </summary>
        public string CurrentDirectoryName { get; set; }

        /// <summary>
        /// Конструктор заточен под 32 бита и 4096 размер страниц
        /// </summary>
        /// <param name="bitDepth">разрядность</param>
        /// <param name="hardware">железо в машине</param>
        /// <param name="pageSize">размер страницы</param>
        public OS(int bitDepth, Hardware hardware, int pageSize)
        {
            PageMaxAge = 70;
            this.PageSize = pageSize;
            BitDepth = bitDepth;
            this.hardware = hardware;
            int freePageCount = 0;
            for (int i = 0; i < hardware.RamSlotsNumber; i++)
            {
                freePageCount += (hardware.RAMs[i].ByteCells.Length * 4) / pageSize; // число блоков по 32 бита * 4 = число байт. Поделим на размер страницы в байтах и получим число страниц
            }
            this.FreePages = new List<int>();
            for (int i = 0; i < freePageCount; i++)    
            {                                                           
                FreePages.Add(i);    
            }
            processes = new List<Process>();
            //
            // Создаю директорию в которой будут храниться текстовики для каждого процесса (один процесс = один файл)
            //

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Processies");
            CurrentDirectoryName = Directory.GetCurrentDirectory() + "\\Processies";
        }
        /// <summary>
        /// Возвращает текущий работающий процесс.
        /// </summary>
        public Process CurrentProcess { get { return processes[currentProcessNumber]; } }
        /// <summary>
        /// Индекс текущего исполняемого процесса
        /// </summary>
        public int currentProcessNumber = 0;
        /// <summary>
        /// Запускает следующий в списке процесс
        /// </summary>
        public void StartNextProcess(/*object sender, System.Timers.ElapsedEventArgs e*/)
        {
            try
            {
                processes[currentProcessNumber].Run(ref hardware);
            }
            catch(PageFault pageFault)
            {
                PageFaultExeptionHandler(processes[currentProcessNumber], pageFault.PageFaultNumber);
            }
            //currentProcessNumber++;
            //if (currentProcessNumber >= processes.Count)
            //{
            //    currentProcessNumber = 0;
            //}
        }
        public void ChangeCurrentProcessNumber()
        {
            currentProcessNumber++;
            if (currentProcessNumber >= processes.Count)
            {
                currentProcessNumber = 0;
            }
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
                int pageCountToInit = memory / PageSize;
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
                //File.Create(CurrentDirectoryName + processes[processes.Count - 1].Name + ".prc");
                StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + processes[processes.Count - 1].FileName);
                int recordCount = processes[processes.Count - 1].WSClockEntryMirrors.Count;
                for (int i = 0; i < recordCount; i++)
                {
                    WritePageToDrive(writer, processes[processes.Count - 1].PageTable.PageTableEntries[i], i);
                    processes[processes.Count - 1].WSClockEntryMirrors[i].WrittenIntoFile = true;
                }
                writer.Close();
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
        /// <param name="indexesInFreePageList">результат метода private int[] GetFreePageIndexesInListOfPages(int pageCount)</param>
        /// <param name="offset">начало блока размером в 32(64) бита</param>
        /// <returns></returns>
        private ref BitArray[] GetFreePages(int[] indexesInFreePageList, out uint offset)
        {
            uint firstPageAdress = (uint)FreePages[indexesInFreePageList[0]] * (uint)PageSize;
            offset = firstPageAdress / (uint)(bitDepth / 8);   // начало блока размером в 32(64) бита
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint physicalEndAdress = hardware.RAMs[i].PhysicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * bitDepth / 8);
                if (firstPageAdress >= hardware.RAMs[i].PhysicalAdress && firstPageAdress < physicalEndAdress)
                {
                    uint lastPageAdress = (uint)FreePages[indexesInFreePageList[indexesInFreePageList.Length - 1]] * (uint)PageSize;
                    //
                    // это смежные блоки памяти, рсположенные на одной физической плашке
                    //
                    if (lastPageAdress >= hardware.RAMs[i].PhysicalAdress && lastPageAdress < physicalEndAdress)
                    {                      
                        for (int j = 0; j < indexesInFreePageList.Length; j++)
                        {
                            FreePages.RemoveAt(indexesInFreePageList[j]);
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
            int pageCount = memory / PageSize; // число страниц которое нужно отобразить
            int recordCount = PageSize / (BitDepth / 8);  // число записей по 32(64) бита, которое помещается в страницу
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
            if (processes.Count == 0)
            {
                pid = random.Next(1000, 9999);
                isCorrect = true;
            }
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
        public void PageFaultExeptionHandler(Process process, int pageNumber)
        {
            bool memoryAllocated = false;
            if (process.PageTable.PageTableEntries[pageNumber].Adress == 0)
            {
                if (FreePages.Count > 0)
                {
                    process.PageTable.PageTableEntries[pageNumber].Present = true;
                    process.PageTable.PageTableEntries[pageNumber].Adress = FreePages[0];
                    FreePages.RemoveAt(0);
                    process.WSClockEntryMirrors.Add(new WSClockEntryMirror(ref process.PageTable.PageTableEntries[pageNumber], process.CurrentVirtualTime, pageNumber));
                    memoryAllocated = true;
                    StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + process.FileName, true);
                    WritePageToDrive(writer, process.PageTable.PageTableEntries[pageNumber], pageNumber);
                    writer.Close();
                }
            }

            if(!memoryAllocated)
            {
                bool ableToRemove = false;
                List<int> removalCandidates = new List<int>();
                int removalPage = 0;
                int removalPageIndex = 0;
                int workingSetPagesNumber = process.WSClockEntryMirrors.Count;
                for (int i = 0; i < workingSetPagesNumber; i++)
                {
                    if (!ableToRemove)
                    {
                        if (process.WSClockEntryMirrors[i].PageTableEntryIndex == pageNumber)   // то такую страницу в расчет не берем, т.к. к ней и обратились
                        { 
                            process.WSClockEntryMirrors[i].Referenced = false;
                            process.WSClockEntryMirrors[i].LastUseTime = process.CurrentVirtualTime;
                            //continue;
                        }
                        else if (process.WSClockEntryMirrors[i].Referenced == true)
                        {
                            process.WSClockEntryMirrors[i].Referenced = false;
                            process.WSClockEntryMirrors[i].LastUseTime = process.CurrentVirtualTime;
                        }
                        else if ((process.CurrentVirtualTime - process.WSClockEntryMirrors[i].LastUseTime) <= PageMaxAge) // это еще рабочий набор
                        {
                            removalCandidates.Add(i);
                        }
                        else  // можно удалить
                        {
                            ableToRemove = true;
                            removalPage = process.WSClockEntryMirrors[i].PageTableEntryIndex;
                            removalPageIndex = i;
                            break;
                        }
                    }
                    else if (process.WSClockEntryMirrors[i].Referenced == true)
                    {
                        process.WSClockEntryMirrors[i].Referenced = false;
                        process.WSClockEntryMirrors[i].LastUseTime = process.CurrentVirtualTime;
                    }
                }
                if (!ableToRemove)
                {
                    if (removalCandidates.Count == 0)
                    {
                        Random random = new Random();
                        do
                        {
                            removalPageIndex = random.Next(0, process.WSClockEntryMirrors.Count - 1);
                            removalPage = process.WSClockEntryMirrors[removalPageIndex].PageTableEntryIndex;
                        } while (removalPage == pageNumber);
                    }
                    else
                    {
                        for (int i = 0; i < removalCandidates.Count; i++)
                        {
                            if (!process.WSClockEntryMirrors[removalCandidates[i]].Modified)
                            {
                                ableToRemove = true;
                                removalPage = process.WSClockEntryMirrors[removalCandidates[i]].PageTableEntryIndex;
                                removalPageIndex = removalCandidates[i];
                                break;
                            }
                        }
                        if (!ableToRemove)
                        {
                            removalPage = process.WSClockEntryMirrors[removalCandidates[0]].PageTableEntryIndex;
                            removalPageIndex = removalCandidates[0];
                        }
                    }
                }
                // если страница модифицирована, то ее данные нужно перезаписать на диск
                if (process.WSClockEntryMirrors[removalPageIndex].Modified == true)
                {
                    // если страница записана на диск - обновляем данные
                    if (process.WSClockEntryMirrors[removalPageIndex].WrittenIntoFile)
                    {
                        if (!WritePageToDrive(process.FileName, process.WSClockEntryMirrors[removalPageIndex].PageTableEntry, process.WSClockEntryMirrors[removalPageIndex].PageTableEntryIndex, true))
                        {
                            throw new Exception("Дисковая операция с процессом крашнулась");
                        }
                    }
                    else  // иначе просто пишем в файл
                    {
                        StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + process.FileName, true);
                        WritePageToDrive(writer, process.WSClockEntryMirrors[removalPageIndex].PageTableEntry, process.WSClockEntryMirrors[removalPageIndex].PageTableEntryIndex);
                        process.WSClockEntryMirrors[removalPageIndex].WrittenIntoFile = true;
                        writer.Close();
                    }
                }
                // в записи "удаляемой" страницы выставляем бит отображения в 0. Больше с этой записью ничего делать не надо
                process.PageTable.PageTableEntries[process.WSClockEntryMirrors[removalPageIndex].PageTableEntryIndex].Present = false;

                // В ту запись (номер в таблице страниц), обращение к которой вызвало ошибку нужно записать адрес "удаленной страницы"
                // Удаляемую страницу нужно записать на диск, если нужно, и в таблице страниц выставить бит отображения в 0
                int oldAdress = process.PageTable.PageTableEntries[pageNumber].Adress;

                // вычисляем физическое местоположение страницы, в которую будем писать новые данные
                uint adress = (uint)process.PageTable.PageTableEntries[removalPageIndex].Adress * (uint)PageSize;
                int ramBlockNumber = 0;
                for (int j = 0; j < hardware.RAMs.Length; j++)
                {
                    uint endPhysicalAdress = hardware.RAMs[j].PhysicalAdress + (uint)(hardware.RAMs[j].ByteCells.Length * (bitDepth / 8));
                    if (adress >= hardware.RAMs[j].PhysicalAdress && adress < endPhysicalAdress)
                    {
                        ramBlockNumber = j;
                        break;
                    }
                }
                // начало страницы
                int startBitArrayBlock = (int)(adress - hardware.RAMs[ramBlockNumber].PhysicalAdress) / (bitDepth / 8);
                // если не удалось восстановить (нет на диске)
                if (!RestoreProcessPage(ref process, pageNumber, ref hardware.RAMs[ramBlockNumber].ByteCells, startBitArrayBlock, adress))
                {
                    MessageBox.Show("Данные поцесса на диске не найдены. Процесс будет убит.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    KillProcess(process.PID);
                }
                else
                {
                    process.PageTable.PageTableEntries[pageNumber].Present = true;
                    // костыль, но так нужно, чтобы адрес "удаляемой" страницы не был забыт
                    process.PageTable.PageTableEntries[process.WSClockEntryMirrors[removalPageIndex].PageTableEntryIndex].Adress = oldAdress;
                }
            }
        }
        /// <summary>
        /// Возвращает соответсвующий указанному адрему блок оперативной памяти
        /// </summary>
        /// <param name="adress">настоящий физический адрес байта</param>
        /// <returns></returns>
        private ref RAM AppropriatePhysicalRamBlock(uint adress)
        {
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint physicalEndAdress = hardware.RAMs[i].PhysicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * (bitDepth / 8));
                if (adress >= hardware.RAMs[i].PhysicalAdress && adress < physicalEndAdress)
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
            return (uint)((uint)pageAdressInPageTableEntry * PageSize);
        }
        /// <summary>
        /// "Убивает" процесс с указанным индексом в списке процессов
        /// </summary>
        /// <param name="processIndex"></param>
        public void KillProcess(int pid)
        {
            int processIndex = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].PID == pid)
                {
                    processIndex = i;
                    break;
                }
            }
            for (int i = 0; i < processes[processIndex].PageTable.Size; i++)
            {
                //
                // очистил страницы
                //
                uint adress = ConvertToRealPhysicalAdress(processes[processIndex].PageTable.PageTableEntries[i].Adress);
                RAM ram = AppropriatePhysicalRamBlock(adress);
                ClearPage(ref ram.ByteCells, ram.PhysicalAdress, adress);
                //
                // добавить в список свободных страниц
                //
                if (processes[processIndex].PageTable.PageTableEntries[i].Present)
                {
                    FreePages.Add(processes[processIndex].PageTable.PageTableEntries[i].Adress);
                }
            }
            // страницы, занимаемые таблицей тоже надо очистить
            uint realAdress = 0;
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                bool isCorrect;
                realAdress = processes[processIndex].PageTable.GetRealPhysicalAdress(ref hardware.RAMs[i], out isCorrect);
                if (isCorrect)
                {
                    break;
                }
            }
            //
            // Тут по хорошему надо бы проверку на значение isCorrect, но не буду делать (посмотрим, что получится)
            //
            int pageCount = processes[processIndex].PageTable.Size * (bitDepth / 8) / PageSize;
            for (int i = 0; i < pageCount; i++)
            {
                int pageNumber = ConvertRealAdressToPageNumber(realAdress);
                FreePages.Add(pageNumber);
            }
            processes[processIndex].PageTable.Clear();

            FreePages.Sort();

            File.Delete(CurrentDirectoryName + "\\" + processes[processIndex].FileName);

            processes.RemoveAt(processIndex);
        }
        private int ConvertRealAdressToPageNumber(uint adress)
        {
            return (int)(adress / PageSize);
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
            int numberOfBitsBlocks = PageSize / bitDepth;
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
                StreamWriter writer = new StreamWriter(CurrentDirectoryName + "\\" + "tmp.prc", true);
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
                            reader.ReadLine(); // чтобы скипнуть содержиммое этой страницы
                        }
                        else
                        {
                            writer.WriteLine("[ " + number + " ]");
                            writer.WriteLine(reader.ReadLine());
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
            uint adress = (uint)(pageTableEntry.Adress * PageSize);
            for (int i = 0; i < hardware.RAMs.Length; i++)
            {
                uint endPhisycalAdress = hardware.RAMs[i].PhysicalAdress + (uint)(hardware.RAMs[i].ByteCells.Length * (bitDepth / 8));
                if (adress >= hardware.RAMs[i].PhysicalAdress && adress < endPhisycalAdress)
                {
                    uint pageStartIndex = (adress - hardware.RAMs[i].PhysicalAdress) / (uint)(bitDepth / 8);  // стартовый индекс BitArray, с которого начинается страница
                    uint pageEndIndex = pageStartIndex + (uint)PageSize / 4;   // блоки по 32 бита
                    writer.WriteLine("[ " + pageIndexInPageTable + " ]");
                    for (uint j = pageStartIndex; j < pageEndIndex; j++)
                    {
                        writer.Write(GetBitArrayStringFormat(hardware.RAMs[i].ByteCells[j]) + " ");
                    }
                    writer.WriteLine();
                }
            }
        }
        /// <summary>
        /// Возвращает строковое предтавление BitArray
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public string GetBitArrayStringFormat(BitArray bitArray)
        {
            string toReturn = "";
            for (int i = 0; i < bitArray.Count; i++)
            {
                if(bitArray.Get(i))
                {
                    toReturn += '1';
                }
                else
                {
                    toReturn += '0';
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Восстанавливает данные страницы с диска. Если не удалось восстановить (не нашлось такой страницы), то вернет false.
        /// Если формат неверен, то выкинет исключение.
        /// </summary>
        /// <param name="process">процесс, для которого производится восстановление</param>
        /// <param name="pageIndexInPageTable">номер записи, в таблице страниц, котрая вызвала ошибку</param>
        /// <param name="ram">физическая память, нужен соответсвующий массив BitArray[]</param>
        /// <param name="startBitArrayBlock">Начало страницы, в которую будем восстанавливать данные, номер блока из 32 битов, номер BitArray(я)</param>
        /// <param name="newPageAdress">новый физический адрес страницы</param>
        /// <returns></returns>
        private bool RestoreProcessPage(ref Process process, int pageIndexInPageTable, ref BitArray[] ram, int startBitArrayBlock, uint newPageAdress)
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
                    process.PageTable.PageTableEntries[pageIndexInPageTable].Adress = (int)(newPageAdress / PageSize);   // потому что номер страницы в общем адресном пространстве
                    process.PageTable.PageTableEntries[pageIndexInPageTable].Present = true;
                    int endBitArrayBlock = startBitArrayBlock + PageSize / (bitDepth / 8);     // 4096 байт / 4 байта = 128 блоков
                    int k = 0;
                    for (int i = startBitArrayBlock; i < endBitArrayBlock; i++)
                    {
                        for (int j = 0; j < ram[i].Length; j++)
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
                    process.WSClockEntryMirrors[pageIndexInPageTable].WrittenIntoFile = true;
                    process.WSClockEntryMirrors[pageIndexInPageTable].Modified = false;
                    process.WSClockEntryMirrors[pageIndexInPageTable].Referenced = true;
                    isRead = true;
                }
                else
                {
                    reader.ReadLine();  // чтобы скипнуть ненужную строку
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

        /// <summary>
        /// Возвращает процесс по его PID
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public Process GetCheckedProcess(int PID)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].PID == PID)
                {
                    return processes[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Сообщает, был ли убит процесс с указанным PID
        /// </summary>
        /// <param name="pid">PID процесса</param>
        /// <returns></returns>
        public bool IsProcessKilled(int pid)
        {
            bool isKilled = true;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].PID == pid)
                {
                    isKilled = false;
                    break;
                }
            }
            return isKilled;
        }
    }

    public class PageFault : System.Exception
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public override string Message { get { return "Ошибка доступа к странице"; } }
        /// <summary>
        /// Номер страницы в таблице страниц, вызвавшей ошибку
        /// </summary>
        public int PageFaultNumber { get; set; }

        public PageFault(string message, int pageFaultNumber) : base(message)
        {
            PageFaultNumber = pageFaultNumber;
        }
    }


}