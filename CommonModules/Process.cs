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
        /// Имя процесса
        /// </summary>
        public string Name { get; set; }
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
            Name = name;
            Priority = priority;
            AvailableMemory = memory;
            PID = pid;
            Status = Status.Queue;
            PageTable = pageTable;
            for (int i = 0; i < PageTable.Size; i++)
            {
                WSClockEntryMirrors.Add(new WSClockEntryMirror(ref PageTable.PageTableEntries[i]));
            }
        }
        /// <summary>
        /// Ссылка на таблицу страниц данного процесса
        /// </summary>
        public PageTable PageTable { get; set; }



        //
        // Так делать плохо, но вы меня простите - так надо, не могу придумать, куда прикрутить это дело
        //
        /// <summary>
        /// Список записей о страницах рабочего набора. Их зеркала, содержащие время последнего использования.
        /// </summary>
        public List<WSClockEntryMirror> WSClockEntryMirrors { get; set; }

        public void AccessPage(int pageNumberInTable, Hardware hardware)
        {
            if (PageTable.PageTableEntries[pageNumberInTable].Present == false)
            {
                throw new PageFault();
            }
            else
            {
                //hardware.RAMs
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
        PageTableEntry PageTableEntry { get; set; }
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
        public WSClockEntryMirror(ref PageTableEntry pageTableEntry)
        {
            PageTableEntry = pageTableEntry;
            LastUseTime = 0;
            Referenced = false;
            Modified = false;
        }
    }

}
