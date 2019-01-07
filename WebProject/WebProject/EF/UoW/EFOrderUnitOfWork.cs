using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebProject.EFInterface;
using WebProject.EFInterface.UoW;
using WebProject.Entity;

namespace WebProject.EF.UoW
{
    public class EFOrderUnitOfWork : IOrderUnitOfWork
    {
        public IOrderRepository OrderRepo => repo;

        private MyContext ctx;
        private IOrderRepository repo;
        public EFOrderUnitOfWork(IOrderRepository repo, MyContext ctx)
        {
            this.repo = repo;
            this.ctx = ctx;
        }

        public void Begin()
        {
            try
            {
                ctx.Database.BeginTransaction();
            }
            catch(Exception e)
            {
                throw new Exception("Error in Begin Transaction" + e.Message);
            }
            
        }

        public void Cancel()
        {
            try
            {
                ctx.Database.RollbackTransaction();
            }
            catch (Exception e)
            {
                throw new Exception("Error in Rollback Transaction" + e.Message);
            }
            
        }

        public void End()
        {
            try
            {
                ctx.Database.CommitTransaction();
            }
            catch(Exception e)
            {
                throw new Exception("Error in Commit Transaction" + e.Message);
            }
            
        }

        public void Save()
        {
            try
            {
                ctx.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DataException("Update Error, element not existing", e);
            }
        }
    }
}
