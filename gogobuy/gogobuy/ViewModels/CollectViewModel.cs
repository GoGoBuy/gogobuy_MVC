using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class CollectViewModel
    {
        public int fCollectID { get; set; }
        public string fProductName { get; set; }
        public string fImgPath { get; set; }
        public decimal? fPrice { get; set; }
    }
}