using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ASync.eTermPlugIn {
    /// <summary>
    /// 日志处理队列
    /// </summary>
    public class ASyncLogQueue {
        private readonly static ASyncLogQueue __instance = new ASyncLogQueue();
        private Queue<Async_Log> __logQueue = new Queue<Async_Log>();
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncLogQueue"/> class.
        /// </summary>
        private ASyncLogQueue() { }


    }
}
