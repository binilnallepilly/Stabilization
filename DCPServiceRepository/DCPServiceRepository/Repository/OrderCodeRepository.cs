using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCPServiceRepository.Common;
using DCPServiceRepository.Models;

namespace DCPServiceRepository.Repository
{
    public class OrderCodeRepository : IOrderCodeRepository
    {
        private readonly DenormRepoDbContext _context;

        public OrderCodeRepository(DenormRepoDbContext denormRepoDbContext)
        {
            this._context = denormRepoDbContext;
        }

        public void CreateOrderCode(string response, string hashKey, string request)
        {
            OrderCode oc = new OrderCode
            {
                Key = hashKey,
                FinishTime = DateTime.UtcNow,
                Response = response,
                Request = request
            };

            _context.OrderCode.Add(oc);
            _context.SaveChanges();
        }

        public void UpdateOrderCode(string response, string hashKey, string request)
        {

            var orderCodeRow = _context.OrderCode
                .Where(b => b.Key == hashKey)
                .FirstOrDefault();
            if (null != orderCodeRow)
            {

                orderCodeRow.Response = response;
                orderCodeRow.Request = request;

                _context.OrderCode.Update(orderCodeRow);
                _context.SaveChanges();
            }
        }

        public void DeleteOrderCode(string hashKey)
        {
            var orderCodeRow = _context.OrderCode
                .Where(b => b.Key == hashKey)
                .FirstOrDefault();
            if (null != orderCodeRow)
            {
                _context.OrderCode.Remove(orderCodeRow);
                _context.SaveChanges();
            }
        }
    }
}
