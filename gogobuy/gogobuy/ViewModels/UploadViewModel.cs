using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class UploadViewModel
    {
        public int fProductID { get; set; }
        [Required(ErrorMessage = "必填")]
        public string fProductName { get; set; }
        [Required(ErrorMessage = "必填")]
        public string fCategory { get; set; }
        [Required(ErrorMessage = "必填")]
        public int? fQuantity { get; set; }
        [Required(ErrorMessage = "必填")]
        public decimal? fPrice { get; set; }
        public string fDescription { get; set; }
        [Required(ErrorMessage = "必填")]
        public string fProductLocation { get; set; }
        public string fArrivalTime { get; set; }
        public List<HttpPostedFileBase> photo { get; set; }
        public List<string> imgName { get; set; }
    }
}