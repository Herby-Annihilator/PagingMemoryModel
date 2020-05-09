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
    class Hardware
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

        public Hardware(RAM[] rams, ROM[] roms, CPU cpu)
        {
            RAMs = rams;
            ROMs = roms;
            CPU = cpu;
        }

        public Hardware(int ramSize, int romSize = 3145728, int ramSlotsNumber = 1, int romNumber = 1, ROMType type = ROMType.HDD, int bitsDepth = 32)
        {
            RamSlotsNumber = ramSlotsNumber;
            RAMs = new RAM[RamSlotsNumber];
            for (int i = 0; i < RamSlotsNumber; i++)
            {
                RAMs[i] = new RAM(ramSize);
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
    }

    /// <summary>
    /// Оперативная память
    /// </summary>
    class RAM
    {
        /// <summary>
        /// Размер плашки в байтах
        /// </summary>
        int Size { get; set; }
        /// <summary>
        /// "Физическая плашка"
        /// </summary>
        public byte[] ByteCells;

        public RAM(int size)
        {
            Size = size;
            ByteCells = new byte[Size];
        }
    }

    /// <summary>
    /// Постоянная память
    /// </summary>
    class ROM
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
    enum ROMType
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

    class Registr
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
