namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Events
    {
        public int Id { get; set; }

       
        public string Description { get; set; }

       
        public string Title { get; set; }

        
        public string ImageUrl { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        public Events()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }
    }
}
