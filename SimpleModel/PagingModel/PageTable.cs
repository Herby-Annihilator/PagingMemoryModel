using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModel.PagingModel
{
    /// <summary>
    /// Таблица страниц, инициализировать должна строго сама ОС вне этого класса
    /// </summary>
    public class PageTable
    {
        /// <summary>
        /// Индекс начала таблицы
        /// </summary>
        public UInt32 StartIndex { get; set; }
        /// <summary>
        /// Ссылка на массив, в котором будет располагаться таблица
        /// </summary>
        private BitArray[] bitArray;
        /// <summary>
        /// Число страниц, занимаемые таблицей
        /// </summary>
        private int pageCount;
        /// <summary>
        /// Число записей в таблице
        /// </summary>
        private int entryCount;
        /// <summary>
        /// Индекс конца таблицы
        /// </summary>
        public UInt32 EndIndex { get; set; }
        private PageTableEntry[] pageTableEntries;
        /// <summary>
        /// Создает таблицу страниц в памяти, но не инициализиурет адреса, тольк выставляет биты по умолчанию. Если таблица занимает не одну страницу,
        /// то эти страницы должны идти непрерывным блоком, иначе нужные данные просто будут затерты. За это отвечаете Вы.
        /// </summary>
        /// <param name="startIndex">адрес начала таблицы</param>
        /// <param name="ram">массив BitArray - физическая память машины</param>
        /// <param name="pageCount">число страниц, которое будет занимать таблица - вычисляется операционной системой</param>
        public PageTable(UInt32 startIndex, ref BitArray[] ram, int pageCount)
        {
            this.pageCount = pageCount;
            StartIndex = startIndex;
            bitArray = ram;
            EndIndex = startIndex + (uint)(pageCount * 4096);     // нужно умножать на размер страницы, а где его получить? (х его з - надо нормально выстраивать архитектуру)
            entryCount = pageCount * 1024;    // количество записей в таблице (умножаем на 1024 так как в странице размером 4096 байт 32768 бит -> записей по 32 бита будет 1024)
            pageTableEntries = new PageTableEntry[entryCount];
            for (int i = 0; i < entryCount; i++)
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
        /// <summary>
        /// Очищает занимаемые таблицей BitArray по 32 бита
        /// </summary>
        public void Clear()
        {
            int entryCount = pageCount * 1024;
            for (int i = 0; i < entryCount; i++)
            {
                pageTableEntries[i].Adress = 0;
                pageTableEntries[i].Accessed = false;
                pageTableEntries[i].Dirty = false;
                pageTableEntries[i].GlobalPage = false;
                pageTableEntries[i].PAT = false;
                pageTableEntries[i].PCD = false;
                pageTableEntries[i].Present = false;  // по умолчанию будет false
                pageTableEntries[i].PWT = false;
                pageTableEntries[i].RW = false;
                pageTableEntries[i].UserSupervisor = false;
            }
        }


    }
}
