using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Models;

namespace WebProject.EFInterface
{
    public interface IOrderRepository
    {
        IEnumerable<Order> FindAll();

        void Add(Order ord);
        void Update(Order ord);
        void Delete(Order ord);
    }
}
