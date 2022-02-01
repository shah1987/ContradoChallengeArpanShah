using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContradoModel.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        [Required(ErrorMessage = "select Category first")]
        [Display(Name = "Product Category")]
        public int ProdCatId { get; set; }

        [Required(ErrorMessage = "select Attribute first")]

        [Display(Name = "Product Attribute")]
        public int AttributeID { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        public IEnumerable<SelectListItem> ProductCatList { get; set; }
        public IEnumerable<SelectListItem> ProductAttributeList { get; set; }
    }

    public class GetAllProductsRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetProductByID
    {
        public long ProductID { get; set; }
    }

    public class ProductAPIModel
    {
        public long ProductId { get; set; }
        public int ProdCatId { get; set; }
        public int AttributeId { get; set; }
        public string ProdName { get; set; }
        public string ProdDescription { get; set; }
        public string AttributeValue { get; set; }
    }

    public class GetAllProductsResponse : ProductAPIModel
    {
        public string CategoryName { get; set; }
        public string AttributeName { get; set; }
        public int TotalRecords { get; set; }
    }
}
