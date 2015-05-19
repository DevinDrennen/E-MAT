using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMAT3.Windows.Core
{
    public static class Session
    {
        /// <summary>
        /// Enum with different session states.
        /// </summary>
        public enum Status
        {
            Uninitialized = 0,
            Initialized = 1,
            Busy = 2,
            Free = 3
        }

        /// <summary>
        /// Status of the EMAT Session.
        /// </summary>
        public static Status SessionStatus { get; private set; }

        public static void Initialize()
        {
            SessionStatus = Status.Initialized;
        }
        public static void Quit()
        {
            SessionStatus = Status.Uninitialized;
        }
    }
}
