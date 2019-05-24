using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Germinmed.Models
{
    public class Branches
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }

        public string PoBox { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }


        public string ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }



        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        public Branches()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
        }


    }
}