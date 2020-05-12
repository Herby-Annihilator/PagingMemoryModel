using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModel.PagingModel
{
    /// <summary>
    /// Таблица страниц
    /// </summary>
    public class PageTable
    {
        /// <summary>
        /// Индекс начала таблицы
        /// </summary>
        public UInt32 StartIndex { get; set; }
        private BitArray[] bitArray;
        /// <summary>
        /// Индекс конца таблицы
        /// </summary>
        public UInt32 EndIndex { get; set; }
        private PageTableEntry[] pageTableEntries;
        public PageTable(UInt32 startIndex, ref BitArray[] ram)
        {
            StartIndex = startIndex;
            bitArray = ram;
            EndIndex = startIndex + 128;
            pageTableEntries = new PageTableEntry[1024];
            for (int i = 0; i < 1024; i++)
            {
                pageTableEntries[i] = new PageTableEntry(ref bitArray[i + startIndex]);
                pageTableEntries[i].Adress = i;
                pageTableEntries[i].Accessed = false;
                pageTableEntries[i].Dirty = false;
                pageTableEntries[i].GlobalPage = false;
                pageTableEntries[i].PAT = false;
                pageTableEntries[i].PCD = true;
                pageTableEntries[i].Present = false;  // по умолчанию будет false
                pageTableEntries[i].PWT = false;
                pageTableEntries[i].RW = true;
                pageTableEntries[i].UserSupervisor = true;
            }
        }


    }
}
