using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DCPServiceRepository.Common.Enums;

namespace DCPServiceRepository
{
    public interface IOrderCodeRepository
    {
        void CreateOrderCode(string response, string hashKey, string request);
        void UpdateOrderCode(string response, string hashKey, string request);
        void DeleteOrderCode(string hashKey);

    }

    //public enum CallLogStatus
    //{
    //    InProgress,
    //    InProgressByOtherThread,
    //    Completed
    //}
}
