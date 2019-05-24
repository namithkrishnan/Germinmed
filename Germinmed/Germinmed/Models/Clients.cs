namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Clients
    {
        public int Id { get; set; }

        public int DisplayOrder { get; set; }

       
        
        public string Title { get; set; }

       
        public string ImageUrl { get; set; }

        public string Url { get; set; }


        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }


        public Clients()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }
    }
}
