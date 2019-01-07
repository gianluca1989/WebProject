using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Autentication;

namespace WebProject.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public decimal? Freight { get; set; }

        public DateTime? OrderDate { get; set; }

        public string ShipCity { get; set; }

        public string ShipName{ get; set; }

    }
}
