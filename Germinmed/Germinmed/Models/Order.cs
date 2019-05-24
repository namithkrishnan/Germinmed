namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int? Qty { get; set; }

        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public string ProducName { get; set; }
        [NotMapped]
        public string UserName { get; set; }


        public Order()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
