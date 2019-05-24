
    using System;
    using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
using System.Web;

namespace Germinmed.Models
{
    [Table("ProductImage")]
    public partial class ProductImage
    {
       
        public int Id { get; set; }

              
        public int ProductId { get; set; }

        [DisplayName("Image")]
      
        public string ImageUrl { get; set; }
        
        [DisplayName("Display Order")]
        public int? DisplayOrder { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        [NotMapped]
        public int txtProductId { get; set; }

        public ProductImage()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            DisplayOrder = 1;
        }
    }
}
