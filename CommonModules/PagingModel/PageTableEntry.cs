﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules.PagingModel
{
    /// <summary>
    /// Элемент таблицы страниц
    /// </summary>
    public class PageTableEntry
    {
        /// <summary>
        /// Возвращает BitArray, который соответствует данной записи
        /// </summary>
        public BitArray Entry { get { return entry; } }
        /// <summary>
        /// Cсылка на существующий кусок памяти
        /// </summary>
        private BitArray entry;
        /// <summary>
        /// Если в параметрах передаешь массив битов, который не готов к использованию, то обязательно настрой все флаги
        /// </summary>
        /// <param name="bitArray"></param>
        public PageTableEntry(ref BitArray bitArray)
        {
            if (bitArray.Length > 32)
            {
                entry = new BitArray(32);
                // выдать сообщение о том, что присвоения не произошло
            }
            else if (bitArray.Length < 8)
            {
                entry = new BitArray(8);
                // выдать сообщение о том, что присвоения не произошло
            }
            else
            {
                // выделить память в физической памяти и сделать это не так
                entry = bitArray;
            }
        }

        /// <summary>
        /// Если 0, то страница не отображена на физическую память.
        /// Это значит, что она либо не определена, либо её содержимое было записано на диск операционной системой в процессе свопинга.
        /// Если происходит обращение к неприсутствующей странице (у которой бит P = 0), то процессор генерирует исключение страничного нарушения (#PF).
        /// Соответствует нулевому биту.
        /// </summary>
        public bool Present
        {
            get
            {
                return entry.Get(0);
            }
            set
            {
                entry.Set(0, value);
            }
        }
        /// <summary>
        /// Если 0, то для этой страницы разрешено только чтение, 1 - чтение и запись.
        /// </summary>
        public bool RW
        {
            get
            {
                return entry.Get(1);
            }
            set
            {
                entry.Set(1, value);
            }
        }
        /// <summary>
        /// Если 0, то доступ к странице разрешён только с нулевого уровня привилегий, если 1 - то со всех.
        /// </summary>
        public bool UserSupervisor
        {
            get
            {
                return entry.Get(2);
            }
            set
            {
                entry.Set(2, value);
            }
        }
        /// <summary>
        /// PWT (Write-Through - Сквозная запись). Когда этот флаг установлен,
        /// разрешено кэширование сквозной записи (write-through) для данной страницы, когда сброшен - кэширование обратной записи (write-back).
        /// </summary>
        public bool PWT
        {
            get
            {
                return entry.Get(3);
            }
            set
            {
                entry.Set(3, value);
            }
        }

        /// <summary>
        /// PCD (Cache Disabled - Кэширование запрещено).
        /// Когда установлен, кэширование данной страницы запрещено.
        /// Кэширование страниц запрещают для портов ввода/вывода, отображённых на память
        /// либо в случаях, когда кэширование не даёт выигрыша в производительности.
        /// </summary>
        public bool PCD
        {
            get
            {
                return entry.Get(4);
            }
            set
            {
                entry.Set(4, value);
            }
        }
        /// <summary>
        /// Устанавливается процессором каждый раз, когда он производит обращение к данной странице.
        /// Процессор не сбрасывает этот флаг - его может может сбросить программа, чтобы потом, через некоторое время определить,
        /// был ли доступ к этой странице, или нет.
        /// </summary>
        public bool Accessed
        {
            get
            {
                return entry.Get(5);
            }
            set
            {
                entry.Set(5, value);
            }
        }
        /// <summary>
        /// Устанавливается каждый раз, когда процессор производит запись в данную страницу.
        /// Этот флаг также не сбрасывается процессором и может использоваться программой, чтобы определить, была ли запись в страницу или нет.
        /// </summary>
        public bool Dirty
        {
            get
            {
                return entry.Get(6);
            }
            set
            {
                entry.Set(6, value);
            }
        }
        /// <summary>
        /// PAT (Page Table Attribute Index - Индекс атрибута таблицы страниц).
        /// Для процессоров, которые используют таблицу атрибутов страниц (PAT - page attribute table),
        /// этот флаг используется совместно с флагами PCD и PWT для выбора элемента в PAT,
        /// который выбирает тип памяти для страницы. Этот бит введён в процессоре Pentium III,
        /// а для процессоров, не использующих PAT, бит PAT должен быть равен 0
        /// </summary>
        public bool PAT
        {
            get
            {
                return entry.Get(7);
            }
            set
            {
                entry.Set(7, value);
            }
        }
        /// <summary>
        /// Когда установлен, определяет глобальную страницу.
        /// Такая страница остаётся достоверной в кэшах TLB при перезагрузке регистра CR3 или переключении задач.
        /// Этот бит был введён в Pentium Pro, для процессоров младше Pentium Pro, этот бит зарезервирован и должен быть равен 0.
        /// </summary>
        public bool GlobalPage
        {
            get
            {
                return entry.Get(8);
            }
            set
            {
                entry.Set(8, value);
            }
        }
        /// <summary>
        /// Возвращает или устанавливает значение, содержащееся в первых 20 битах данной записи в таблице страниц.
        /// Устанавливаемое значение должно быть меньше чем 2^20, иначе вы установите ложное значение,
        /// так как первые 12 битов в числе просто выкидываются. Это номер страницы в общем адресном пространстве максимум для 32 = 1 048 575.
        /// </summary>
        public int Adress
        {
            get
            {
                int[] arr = new int[1];
                BitArray bitArray = new BitArray(32);
                int index = 31;
                for (int i = 0; i < 20; i++)
                {
                    bitArray.Set(i, entry.Get(index));
                    index--;
                }

                bitArray.CopyTo(arr, 0);
                return arr[0];
            }
            set
            {
                int index = 31;
                int[] arr = new int[] { value };
                BitArray bitArray = new BitArray(arr);
                for (int i = 0; i < 20; i++)
                {
                    entry.Set(index, bitArray.Get(i));
                    index--;
                }
            }
        }
    }
}
