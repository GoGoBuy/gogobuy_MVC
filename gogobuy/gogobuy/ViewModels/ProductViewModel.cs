using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class ProductViewModel
    {
        
        public int fProductID { get; set; }
        public string fProductName { get; set; }
        public string fImgPath { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
        public decimal? fPrice { get; set; }
        public string fProductLocation { get; set; }
        public int? fQuantity { get; set; }
        public string fArrivalTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? fUpdateTime { get; set; }
        public string fDescription { get; set; }
        public bool isLike { get; set; }
        public bool? fIsWish { get; set; }
    }
}