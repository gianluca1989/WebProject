using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebProject.Models;
using WebProject.EFInterface.UoW;
using Microsoft.AspNetCore.Hosting;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace WebProject.Controllers
{
    [Authorize]
    public class GridController : Controller
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOrderUnitOfWork work;
        private IHostingEnvironment _hostingEnvironment;

        public GridController(IOrderUnitOfWork work, IHostingEnvironment hostingEnvironment)
        {
            this.work = work;
            _hostingEnvironment = hostingEnvironment;
        }

        //reads everything from the Order table and returns it to the view
        public IActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<Order> lista = new List<Order>();
            try
            {
                lista = work.OrderRepo.FindAll().ToList();
                if(lista.Count>0) log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nItems found");
                else log.Warn($"Warning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nThere are no elements in the database");
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
            }
            
            return Ok(lista.ToDataSourceResult(request));
        }


        //adds an input element to the database
        [AcceptVerbs("Post")]
        public IActionResult Orders_Create([DataSourceRequest] DataSourceRequest request, Order product)
        {
            try
            {
                if (product != null && ModelState.IsValid)
                {
                    work.Begin();
                    work.OrderRepo.Add(product);
                    work.Save();
                    work.End();
                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nAdded element");
                }
                else throw new Exception("The entered data are wrong");
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nThe item was not added" + e.Message);
            }
            
            return Ok(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        //update an element in input to the database
        [AcceptVerbs("Post")]
        public IActionResult Orders_Update([DataSourceRequest] DataSourceRequest request, Order product)
        {
            try
            {
                if (product != null && ModelState.IsValid)
                {
                    work.Begin();
                    work.OrderRepo.Update(product);
                    work.Save();
                    work.End();
                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nUpdated element");
                }
                else throw new Exception("The entered data are wrong");
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nThe item was not updated" + e.Message);
            }
            

            return Ok(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        //delete an element in input to the database
        [AcceptVerbs("Post")]
        public IActionResult Orders_Destroy([DataSourceRequest] DataSourceRequest request, Order product)
        {
            try
            {
                if (product != null)
                {
                    work.Begin();
                    work.OrderRepo.Delete(product);
                    work.Save();
                    work.End();
                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nDeleted item");
                }
                else throw new Exception("The entered data are wrong");
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nThe item was not deleted" + e.Message);
            }
            

            return Ok(new[] { product }.ToDataSourceResult(request, ModelState));
        }
    }
}