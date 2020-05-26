using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules
{
    /// <summary>
    /// Простейшая модель железа в компьютере (самое нужное для лабы)
    /// </summary>
    public class Hardware
    {
        /// <summary>
        /// Число слотов для оперативки
        /// </summary>
        private int ramSlotsNumber;
        /// <summary>
        /// Число слотов для оперативки
        /// </summary>
        public int RamSlotsNumber
        {
            get { return ramSlotsNumber; }
            set
            {
                if (value > 4)
                {
                    ramSlotsNumber = 4;
                }
                else if (value < 1)
                {
                    ramSlotsNumber = 1;
                }
                else
                {
                    ramSlotsNumber = value;
                }
            }
        }
        /// <summary>
        /// Физические слоты для оперативки
        /// </summary>
        public RAM[] RAMs { get; private set; }


        /// <summary>
        /// Число возможноустанавливаемых дисков
        /// </summary>
        private int romNumber;
        /// <summary>
        /// Число устанавливаемых накопителей
        /// </summary>
        public int RomNumber
        {
            get { return romNumber; }
            set
            {
                if (value > 5)
                {
                    romNumber = 5;
                }
                else if (value < 0)
                {
                    romNumber = 0;
                }
                else
                {
                    romNumber = value;
                }
            }
        }
        /// <summary>
        /// Физические накопители
        /// </summary>
        public ROM[] ROMs { get; set; }

        public Hardware(ref RAM[] rams, ref ROM[] roms, CPU cpu)
        {
            RAMs = rams;
            RamSlotsNumber = rams.Length;
            ROMs = roms;
            RomNumber = ROMs.Length;
            CPU = cpu;
        }

        public Hardware(int ramSize, int romSize = 3145728, int ramSlotsNumber = 1, int romNumber = 1, ROMType type = ROMType.HDD, int bitsDepth = 32)
        {
            RamSlotsNumber = ramSlotsNumber;
            RAMs = new RAM[RamSlotsNumber];
            for (int i = 0; i < RamSlotsNumber; i++)
            {
                uint adress = (uint)(i * ramSize);
                RAMs[i] = new RAM(ramSize, adress, bitsDepth);
            }

            RomNumber = romNumber;
            ROMs = new ROM[RomNumber];
            for (int i = 0; i < RomNumber; i++)
            {
                ROMs[i] = new ROM(romSize, type);
            }

            CPU = new CPU(bitsDepth);
        }

        public CPU CPU { get; set; }


        /// <summary>
        /// Вернет конкретную "физическую плашку" и смещение в ней (блок по 32 бита). В случае отсутствия памяти выдаст исключение.
        /// Метод гарантирует, что в памяти есть свободный блок указанного размера (если не выдает исключения)
        /// </summary>
        /// <param name="countOfSlots">Число блоков по 32 бита</param>
        /// <param name="offset">смещение внутри физической плашки</param>
        /// <returns></returns>
        public ref BitArray[] GetFreeMemoryBlock(int countOfSlots, out UInt32 offset)
        {
            BitArray bitArray = new BitArray(32);
            int currentFreeBlocks = 0;
            
            for (int i = 0; i < RamSlotsNumber; i++)
            {
                offset = 0;
                for (uint j = 0; j < RAMs[i].ByteCells.Length; j++)
                {
                    if (RAMs[i].ByteCells[j] == bitArray)
                    {
                        currentFreeBlocks++;
                    }
                    else
                    {
                        currentFreeBlocks = 0;
                    }
                    if (currentFreeBlocks == countOfSlots)
                    {
                        offset = j;
                        return ref RAMs[i].ByteCells;
                    }
                }
            }

            throw new OutOfMemoryException();
        }
    }

    /// <summary>
    /// Оперативная память
    /// </summary>
    public class RAM
    {
        /// <summary>
        /// Физический адрес начала данного блока оперативки
        /// </summary>
        public UInt32 PhysicalAdress { get; set; }
        /// <summary>
        /// Разрядность
        /// </summary>
        int BitDepth { get; set; }
        /// <summary>
        /// Размер плашки в байтах
        /// </summary>
        int Size { get; set; }
        /// <summary>
        /// "Физическая плашка" - разделена на отрезки размером с разрядность
        /// </summary>
        public BitArray[] ByteCells;
        /// <summary>
        /// Создает физическую память указанного размера - кратно 32
        /// </summary>
        /// <param name="size">размер в байтах</param>
        /// <param name="adress">адрес начала физического блока</param>
        /// <param name="bitDepth">разрядность машины</param>
        public RAM(int size, UInt32 adress, int bitDepth)
        {
            BitDepth = bitDepth;
            PhysicalAdress = adress;
            Size = size;
            int count = Size / BitDepth;
            ByteCells = new BitArray[count];
            for (int i = 0; i < count; i++)
            {
                ByteCells[i] = new BitArray(BitDepth);
            }
        }
    }

    /// <summary>
    /// Постоянная память
    /// </summary>
    public class ROM
    {
        /// <summary>
        /// Тип накопителя
        /// </summary>
        public ROMType Type { get; private set; }
        /// <summary>
        /// "Физический накопитель"
        /// </summary>
        public byte[] ByteCells;

        public ROM(int size, ROMType type = ROMType.HDD)
        {
            ByteCells = new byte[size];
        }
    }

    /// <summary>
    /// Тип накопителя
    /// </summary>
    public enum ROMType
    {
        SSD,
        HDD,
    }


    public class CPU
    {
        /// <summary>
        /// Регистр отвечает за адрес таблицы страниц в корневом каталоге
        /// </summary>
        Registr CR3;
        /// <summary>
        /// Пока инициализируется только один регистр cr3
        /// </summary>
        /// <param name="bitDepth">разрядность процессора</param>
        public CPU(int bitDepth)
        {
            CR3 = new Registr(bitDepth);
        }
    }

    public class Registr
    {
        private BitArray bits;
        /// <summary>
        /// Получает значение, хранимое регистром (содержимое регистра должно быть 32 бита, можно меньше,
        /// но лучше 32. Для больших размеров регистра этот метод нужно переписать)
        /// </summary>
        /// <returns></returns>
        public int GetValue()
        {
            int[] toReturn = new int[1];
            bits.CopyTo(toReturn, 0);
            return toReturn[0];
        }

        internal Registr(int bitsCount = 32)
        {
            bits = new BitArray(bitsCount);
        }
    }


   

}
