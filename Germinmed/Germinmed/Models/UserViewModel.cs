using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Germinmed.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }
              
        public string FirstName { get; set; }
                
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }


        [Required(ErrorMessage = "This field is required.")]
        [MinLength(6, ErrorMessage = "Password Minimum Length Should Be 6 Char")]
        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(6, ErrorMessage = "Password Minimum Length Should Be 6 Char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password")]
        [NotMapped]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password Minimum Length Should Be 6 Char")]
        public string ConfirmPassword { get; set; }

    }
}