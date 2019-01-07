using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.EFInterface.UoW
{
    public interface IOrderUnitOfWork : UnitOfWork
    {
        IOrderRepository OrderRepo { get; }
    }
}
