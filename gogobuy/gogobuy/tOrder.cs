//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace gogobuy
{
    using System;
    using System.Collections.Generic;
    
    public partial class tOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tOrder()
        {
            this.tOrderDetails = new HashSet<tOrderDetails>();
        }
    
        public int fOrderID { get; set; }
        public Nullable<decimal> fPrice { get; set; }
        public Nullable<int> fResCount { get; set; }
        public Nullable<System.DateTime> fOrderDate { get; set; }
        public Nullable<int> fSellerID { get; set; }
        public Nullable<int> fBuyerID { get; set; }
        public string fOrderAddress { get; set; }
        public string fOrderPhone { get; set; }
        public string fBuyerName { get; set; }
        public string fOrderStatus { get; set; }
        public string fOrderUUID { get; set; }
        public string fOrderPayWay { get; set; }
        public string fOrderNote { get; set; }
    
        public virtual tMembership tMembership { get; set; }
        public virtual tMembership tMembership1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrderDetails> tOrderDetails { get; set; }
    }
}