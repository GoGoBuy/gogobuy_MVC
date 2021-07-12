using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class ProductViewModel
    {
        
        public int fProductID { get; set; }
        public string fProductName { get; set; }
        public string fImgPath { get; set; }
        public decimal? fPrice { get; set; }
        public string fProductLocation { get; set; }
        public int? fQuantity { get; set; }
        public string fArrivalTime { get; set; }
        public DateTime? fUpdateTime { get; set; }
        public bool isLike { get; set; }
    }
}