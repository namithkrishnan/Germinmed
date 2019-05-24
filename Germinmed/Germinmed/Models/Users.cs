namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Users")]
    public partial class Users
    {
        public int Id { get; set; }
        [DisplayName("Role")]
        [Required(ErrorMessage = "This field is required.")]
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MinLength(6, ErrorMessage = "Password Minimum Length Should Be 6 Char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }

        public string ActivationCode { get; set; }

        public string ResetPasswordCode { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string ClinicName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsAdmin { get; set; }

        public DateTime? CreatedDate { get; set; }




        [DisplayName("Confirm Password")]
        [Compare("Password")]
        [NotMapped]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password Minimum Length Should Be 6 Char")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string LoginErrorMessage { get; set; }

        public Users()
        {
           // IsEmailVerified = false;
            UserTypeId = 1;
            CreatedDate = DateTime.Now;
        }

    }
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}



