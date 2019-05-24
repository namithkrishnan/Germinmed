using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;

namespace Germinmed.Models
{
    public class Banner
    {

        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string ImageUrl { get; set; }

      
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        
        [NotMapped]
        //[ImageFile(Width = 2400, Height = 608, ErrorMessage = "Please upload image 100x100")]
        public HttpPostedFileBase ImageUpload { get; set; }


        public Banner()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }


    }
    public partial class ImageFileAttribute : ValidationAttribute
    {
        public int Width;
        public int Height;

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            var image = new Bitmap(file.InputStream);
            if (image == null)
                return true;

            if (image.Width != Width)
                return false;

            if (image.Height != Height)
                return false;

            return true;
        }
    }
}