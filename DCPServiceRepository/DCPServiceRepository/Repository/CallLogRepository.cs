using DCPServiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DCPServiceRepository.Common.Enums;

namespace DCPServiceRepository.Repository
{
    public class CallLogRepository : ICallLogRepository
    {
        private readonly DenormRepoDbContext _context;

        public CallLogRepository(DenormRepoDbContext denormRepoDbContext)
        {
            this._context = denormRepoDbContext;
        }

        public void CreateCallLog(string hashKey, CallLogStatus status)
        {
            ConfigCallLog cl = new ConfigCallLog
            {
                Key = hashKey,
                Status = status.ToString(),
                RequestStartTime = DateTime.UtcNow

            };

            _context.CallLog.Add(cl);
            _context.SaveChanges();
        }

        public void UpdateCallLog(ConfigCallLog callLogRow, string hashKey, CallLogStatus status)
        {
            if (callLogRow == null)
            {
                callLogRow = _context.CallLog
               .Where(b => b.Key == hashKey)
               .FirstOrDefault();
            }

            callLogRow.Status = status.ToString();
            callLogRow.FinishTime = DateTime.UtcNow;


            _context.CallLog.Update(callLogRow);
            _context.SaveChanges();
        }

        public void DeleteCallLog(ConfigCallLog callLogRow, string hashKey)
        {
            if (callLogRow == null)
            {
                callLogRow = _context.CallLog
                    .Where(b => b.Key == hashKey)
                    .FirstOrDefault();
            }

            _context.CallLog.Remove(callLogRow);
            _context.SaveChanges();
        }

        //public Task<CallLogStatus> CreateCallLog()
        //{
        //    //TODO : check if the log is already available for the request key (with expirytime check),
        //    // 2. if not insert new
        //    // 3. if the old call didnt complete in threashold time create a new call
        //    // 4. if the log is available and not complete return as "INPROGRESSBYOTHERTHREAD so that the current can sleep

        //    //denormRepoDbContext.CallLog.Any(a=>a.)
        //    return null;
        //}
    }
}
