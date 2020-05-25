using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleModel.PagingModel;

namespace CommonModules
{
    public class Process
    {
        /// <summary>
        /// Текущее виртуальное время
        /// </summary>
        public int CurrentVirtualTime { get; set; }
        /// <summary>
        /// Имя процесса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Имя файла, отвечающего за процесс
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Статус
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Доступная память (байт)
        /// </summary>
        public int AvailableMemory { get; private set; }
        /// <summary>
        /// Реально используемая память (байт)
        /// </summary>
        public int UsedMemory { get; private set; }

        public Process(string name, int memory, int pid, int priority, ref PageTable pageTable)
        {
            CurrentVirtualTime = 0;
            Name = name;
            FileName = Name + ".prc";
            Priority = priority;
            AvailableMemory = memory;
            PID = pid;
            Status = Status.Queue;
            PageTable = pageTable;
            for (int i = 0; i < PageTable.Size; i++)
            {
                WSClockEntryMirrors.Add(new WSClockEntryMirror(ref PageTable.PageTableEntries[i], CurrentVirtualTime, i));
            }
        }
        /// <summary>
        /// Ссылка на таблицу страниц данного процесса
        /// </summary>
        public PageTable PageTable { get; set; }

        public void Run(ref Hardware hardware)
        {
            Random random = new Random();
            AccessPage(random.Next(0, PageTable.Size), hardware);
        }

        //
        // Так делать плохо, но вы меня простите - так надо, не могу придумать, куда прикрутить это дело
        //
        /// <summary>
        /// Список записей о страницах рабочего набора. Их зеркала, содержащие время последнего использования.
        /// </summary>
        public List<WSClockEntryMirror> WSClockEntryMirrors { get; set; }
        /// <summary>
        /// Метод обращается к памяти. Делает отрицание начала страницы (отрицает первые 32 бита в странице).
        /// Выкидывает исключение PageFault, если страница в памяти не найдена.
        /// </summary>
        /// <param name="pageNumberInTable"></param>
        /// <param name="hardware"></param>
        public void AccessPage(int pageNumberInTable, Hardware hardware)
        {
            if (PageTable.PageTableEntries[pageNumberInTable].Present == false)
            {
                throw new PageFault();
            }
            else
            {
                for (int i = 0; i < hardware.RAMs.Length; i++)
                {
                    uint endAdress = (uint)(hardware.RAMs[i].PhysicalAdress + hardware.RAMs[i].ByteCells.Length * 4); // потомучто криво и под 32 бита
                    if (PageTable.PageTableEntries[pageNumberInTable].Adress >= hardware.RAMs[i].PhysicalAdress && PageTable.PageTableEntries[pageNumberInTable].Adress < endAdress)
                    {
                        int blockNumber = PageTable.PageTableEntries[pageNumberInTable].Adress / 4;
                        hardware.RAMs[i].ByteCells[blockNumber].Not();
                        break;
                    }
                }
            }
        }

    }
    /// <summary>
    /// Состояние процесса
    /// </summary>
    public enum Status
    { 
        /// <summary>
        /// В очереди
        /// </summary>
        Queue,
        /// <summary>
        /// В ожидании
        /// </summary>
        Awaiting,
        /// <summary>
        /// Завершен
        /// </summary>
        Completed,
    }
    /// <summary>
    /// Зеркало страницы рабочего набора
    /// </summary>
    public class WSClockEntryMirror
    {
        /// <summary>
        /// Индекс записи в таблице страниц
        /// </summary>
        public int PageTableEntryIndex { get; set; }
        /// <summary>
        /// Находится ли данная страница в файле
        /// </summary>
        public bool WrittenIntoFile { get; set; }
        public PageTableEntry PageTableEntry { get; set; }
        /// <summary>
        /// Время последнего использования
        /// </summary>
        public int LastUseTime { get; set; }
        /// <summary>
        /// Бит обращения
        /// </summary>
        public bool Referenced
        {
            get { return PageTableEntry.Accessed; }
            set
            {
                PageTableEntry.Accessed = value;
            }
        }
        /// <summary>
        /// Бит изменения
        /// </summary>
        public bool Modified
        {
            get { return PageTableEntry.Dirty; }
            set
            {
                PageTableEntry.Dirty = value;
            }
        }
        /// <summary>
        /// Создает новый экземпляр "зеркала страницы рабочего набора"
        /// </summary>
        public WSClockEntryMirror(ref PageTableEntry pageTableEntry, int currentVirtualTime, int index)
        {
            PageTableEntryIndex = index;
            PageTableEntry = pageTableEntry;
            LastUseTime = currentVirtualTime;
            Referenced = false;
            Modified = false;
            WrittenIntoFile = false;
        }
    }

}
