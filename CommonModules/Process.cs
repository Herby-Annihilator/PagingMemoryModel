﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules
{
    public class Process
    {
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

        public Process(int memory, int pid)
        {
            AvailableMemory = memory;
            PID = pid;
            Status = Status.Queue;
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

}
