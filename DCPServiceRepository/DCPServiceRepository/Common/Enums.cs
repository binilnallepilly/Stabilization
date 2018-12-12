using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCPServiceRepository.Common
{
    public class Enums
    {

        public enum CallLogStatus
        {
            InProgress,
            InProgressByOtherThread,
            Completed
        }

        public enum Error
        {
            FromConfigService,
            FromDCPServiceRepository
          
        }
    }
}
