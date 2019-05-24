using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Models
{
    public class NewsLetter
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Subject { get; set; }
        [AllowHtml]
        public string Message { get; set; }

        public string Recipients { get; set; }

   

        public string ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }



        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [NotMapped]
        public IEnumerable<Users> UserList { get; set; }

        public NewsLetter()
        {
          
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }
    }
}