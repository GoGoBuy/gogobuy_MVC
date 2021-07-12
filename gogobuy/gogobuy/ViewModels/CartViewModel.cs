using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class CartViewModel
    {
        public int fCartID { get; set; }
        public int? fMemberID { get; set; }
        public int? fProductID { get; set; }
        public int? fQuantity { get; set; }
        public string fProductName { get; set; }
        public string fImgPath { get; set; }
        public decimal? fPrice { get; set; }
        public string fDescription { get; set; }

    }
}