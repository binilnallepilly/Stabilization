using DCPServiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DCPServiceRepository.Common.Enums;

namespace DCPServiceRepository
{
    public interface ICallLogRepository
    {
        //Task<CallLogStatus> CreateCallLog();
        void CreateCallLog(string hashKey, CallLogStatus status);
        void UpdateCallLog(ConfigCallLog callLogRow, string hashKey, CallLogStatus status);
        void DeleteCallLog(ConfigCallLog callLogRow, string hashKey);
    }

    //public enum CallLogStatus
    //{
    //    InProgress,
    //    InProgressByOtherThread,
    //    Completed
    //}
}
