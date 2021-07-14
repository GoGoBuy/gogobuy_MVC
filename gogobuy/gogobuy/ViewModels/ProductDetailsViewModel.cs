using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int fProductID { get; set; }

        public string fProductName { get; set; }

        public decimal? fPrice { get; set; }

        public string fDescription { get; set; }

        public string fCategory { get; set; }

        public string fProductLocation { get; set; }

        public string fImgPath { get; set; }

        public string fArrivalTime { get; set; }

        public int? fQuantity { get; set; }

        public int? fMemberID { get; set; }

        public int fIsWish { get; set; }

        public DateTime? fUpdateTime { get; set; }

        public string fFirstName { get; set; }

        public string fLastName { get; set; }

        public string fPname { get; set; }
        public bool isLike { get; set; }
        public List<string> listImgPath = new List<string>();

        public List<string> listStyle = new List<string>();
    }
}