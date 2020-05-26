using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules.PagingModel
{
    public class PageTable
    {
        /// <summary>
        /// Индекс блока (32 бита), с которого начинаеся страница (относительно начала блока физической памяти - 0)
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
        /// Индекс блока (32 бита), на котором заканчивается страница (относительно начала блока физической памяти - 0)
        /// </summary>
        public UInt32 EndIndex { get; set; }
        public PageTableEntry[] PageTableEntries { get; set; }
        /// <summary>
        /// Создает таблицу страниц в памяти, но не инициализиурет адреса, тольк выставляет биты по умолчанию. Если таблица занимает не одну страницу,
        /// то эти страницы должны идти непрерывным блоком, иначе нужные данные просто будут затерты. За это отвечаете Вы.
        /// </summary>
        /// <param name="startIndex">индекс блока (32 бита), с которого начинаеся страница (относительно начала блока физической памяти - 0)</param>
        /// <param name="ram">массив BitArray - физическая память машины</param>
        /// <param name="pageCount">число страниц, которое будет занимать таблица - вычисляется операционной системой</param>
        public PageTable(UInt32 startIndex, ref BitArray[] ram, int pageCount)
        {
            this.pageCount = pageCount;
            StartIndex = startIndex;
            bitArray = ram;
            EndIndex = startIndex + (uint)(pageCount * 4096 / 4);     // нужно умножать на размер страницы, а где его получить? (х его з - надо нормально выстраивать архитектуру)
            entryCount = pageCount * 1024;    // количество записей в таблице (умножаем на 1024 так как в странице размером 4096 байт 32768 бит -> записей по 32 бита будет 1024)
            PageTableEntries = new PageTableEntry[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                PageTableEntries[i] = new PageTableEntry(ref bitArray[i + startIndex]);
                PageTableEntries[i].Adress = i;
                PageTableEntries[i].Accessed = false;
                PageTableEntries[i].Dirty = false;
                PageTableEntries[i].GlobalPage = false;
                PageTableEntries[i].PAT = false;
                PageTableEntries[i].PCD = true;
                PageTableEntries[i].Present = false;  // по умолчанию будет false
                PageTableEntries[i].PWT = false;
                PageTableEntries[i].RW = true;
                PageTableEntries[i].UserSupervisor = true;
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
                PageTableEntries[i].Adress = 0;
                PageTableEntries[i].Accessed = false;
                PageTableEntries[i].Dirty = false;
                PageTableEntries[i].GlobalPage = false;
                PageTableEntries[i].PAT = false;
                PageTableEntries[i].PCD = false;
                PageTableEntries[i].Present = false;  // по умолчанию будет false
                PageTableEntries[i].PWT = false;
                PageTableEntries[i].RW = false;
                PageTableEntries[i].UserSupervisor = false;
            }
        }
        /// <summary>
        /// Размер таблицы страниц, измеряемый в записях по 32 бита
        /// </summary>
        public int Size { get { return PageTableEntries.Length; } }


        /// <summary>
        /// Возвращает реальный физический адрес начала таблицы
        /// </summary>
        /// <param name="ram">ссылка на физический блок памяти</param>
        /// <param name="isCorrect">флажок - если false, то результат не верен</param>
        /// <returns></returns>
        public uint GetRealPhysicalAdress(ref RAM ram, out bool isCorrect)
        {
            isCorrect = false;
            uint realPhysicalAdress = 0;
            if (this.bitArray.Equals(ram.ByteCells))
            {
                isCorrect = true;
                uint offset = this.StartIndex * 4;
                realPhysicalAdress = ram.PhysicalAdress + offset;
            }
            return realPhysicalAdress;
        }
    }
}
