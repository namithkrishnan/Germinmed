namespace Germinmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;
    using System.Web.Mvc;

    [Table("Category")]
    public partial class Category
    {
        public int Id { get; set; }

        [DisplayName("Parent Category")]
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string InnerBannerImageUrl { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public List<Category> CategoryList { get; set; }

        [NotMapped]
        public List<Brands> BrandList { get; set; }

        [NotMapped]
        public string Sort { get; set; }


        public string Title1 { get; set; }

        public int Level { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [NotMapped]
        public HttpPostedFileBase InnerBannerImageUpload { get; set; }

        [NotMapped]
        public static string sqlQuery = "WITh Tree ( Id, Title1,ImageUrl,InnerBannerImageUrl,Description,CreatedDate, ParentId, level, Title) AS"
       + "( SELECT Id, Title,ImageUrl,InnerBannerImageUrl,Description,CreatedDate, ParentId, 0 AS level,"
       + "  CONVERT(varchar(255), Title) AS Title"
       + " FROM Category"
       + " WHERE ParentId = 0"
       + " UNION ALL"
       + " SELECT RT.Id, RT.Title,RT.ImageUrl,RT.InnerBannerImageUrl,RT.Description,RT.CreatedDate, RT.ParentId, Parent.level + 1 AS level,"
       + " CONVERT(varchar(255), concat(Parent.Title, ' / ', RT.Title)) as Title"
       + " FROM Category RT"
       + " INNER JOIN Tree as Parent ON Parent.Id = RT.ParentId )"
       + " SELECT * FROM Tree"
       + " ORDER BY Id";

        [NotMapped]
        public static string sqlQuery1 = "WITh Tree ( Id, Title,ImageUrl,InnerBannerImageUrl,Description,CreatedDate, ParentId, level, Title1) AS"
       + "( SELECT Id, Title,ImageUrl,InnerBannerImageUrl,Description,CreatedDate, ParentId, 0 AS level,"
       + "  CONVERT(varchar(255), Title) AS Title1"
       + " FROM Category"
       + " WHERE ParentId = 0"
       + " UNION ALL"
       + " SELECT RT.Id, RT.Title,RT.ImageUrl,RT.InnerBannerImageUrl,RT.Description,RT.CreatedDate, RT.ParentId, Parent.level + 1 AS level,"
       + " CONVERT(varchar(255), concat(Parent.Title1, ' / ', RT.Title)) as Title1"
       + " FROM Category RT"
       + " INNER JOIN Tree as Parent ON Parent.Id = RT.ParentId )"
       + " SELECT * FROM Tree"
       + " ORDER BY Id";

        public Category()
        {
            ImageUrl = "~/AppFiles/Images/Default.png";
            InnerBannerImageUrl = "~/AppFiles/Images/Default.png";
            CreatedDate = DateTime.Now;
            ParentId = 0;
        }
    }
}
