using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gogobuy.ViewModels
{
    public class OrderlistBuyViewModel
    {
        public string fOrderUUID { get; set; }
        public DateTime? fOrderDate { get; set; }
        public string fOrderStatus { get; set; }
        public decimal? fPrice { get; set; }
        public string fOrderPayWay { get; set; }
        public List<tOrderDetails> lsttOrderDetailslist = new List<tOrderDetails>();
        public List<tProduct> lsttProductlist = new List<tProduct>();
    }

    
}