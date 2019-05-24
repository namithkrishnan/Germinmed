namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MailSettings
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(320)]
        public string Username { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(320)]
        [Required(ErrorMessage = "This field is required.")]
        public string Server { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int Port { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public bool IsSSLEnabled { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(320)]
        public string Contact { get; set; }

        [StringLength(320)]
        public string Cc { get; set; }
    }
}
