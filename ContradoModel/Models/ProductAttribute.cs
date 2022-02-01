using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContradoModel.Models
{
    public class GetProductAttributeRequest
    {
        public int ProdCatId { get; set; }
    }
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public int ProdCatId { get; set; }
        public string AttributeName { get; set; }
    }
}
