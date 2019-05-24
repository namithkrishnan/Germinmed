namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Brands")]
    public partial class Brands
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }

        
        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public bool ShowInBrandPage { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }


        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [NotMapped]
        public List<Brands> BrandList { get; set; }

        public Brands()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }
    }
}
