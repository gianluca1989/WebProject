using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebProject.EFInterface;
using WebProject.Entity;
using WebProject.Models;

namespace WebProject.EF
{
    public class EFOrderRepository : IOrderRepository
    {
        private MyContext context;
        public EFOrderRepository(MyContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Order> FindAll()
        {
            try
            {
                return context.Order;
            }
            catch(Exception e)
            {
                throw new Exception($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n"+e.Message);
            }
            
        }

        public void Add(Order ord)
        {
            try
            {
                context.Order.Add(ord);
            }
            catch(Exception e)
            {
                throw new Exception($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n"+e.Message);
            }
            
        }

        public void Update(Order ord)
        {
            try
            {
                context.Order.Update(ord);
            }
            catch(Exception e)
            {
                throw new Exception($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n"+e.Message);
            }
            
        }

        public void Delete(Order ord)
        {
            try
            {
                context.Order.Remove(ord);
            }
            catch(Exception e)
            {
                throw new Exception($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n"+e.Message);
            }
            
        }
    }
}
