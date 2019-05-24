using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Germinmed.Models
{
    public class InnerBanner
    {
        public int Id { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100)]
        public string PageName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ImageUrl { get; set; }


        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }


        [NotMapped]
        //[ImageFile(Width = 2400, Height = 608, ErrorMessage = "Please upload image 100x100")]
        public HttpPostedFileBase ImageUpload { get; set; }


        public InnerBanner()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }

    }
}