using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OData_WCF_Services.Model
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Category Category { get; set; }
    }
}