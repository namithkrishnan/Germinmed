using Germinmed.DAL;
using System;
    using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Models
{
    [Table("Products")]
    public  class Products
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50)]
        public string ProductName { get; set; }

       // [Required(ErrorMessage = "This field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "This field is required.")]
        public int CategoryId { get; set; }

       

        [AllowHtml]
        public string Description { get; set; }

        public int? BrandId { get; set; }

        
        public string ItemCode { get; set; }

        [StringLength(200)]
        public string WebUrl { get; set; }

        [StringLength(200)]
        public string VideoUrl { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Price { get; set; }

        [DisplayName("Promotion")]
        public int? PromotionCode { get; set; }

        [DisplayName("Featured")]
        [Column(TypeName = "bit")]
        public bool IsFeatured { get; set; }

        [DisplayName("Is Offer")]
        [Column(TypeName = "bit")]
        public bool IsOffer { get; set; }


        public string OfferPercentage { get; set; }

        [DisplayName("Show In Home")]
        [Column(TypeName = "bit")]
        public bool ShowInHomePage { get; set; }

        public DateTime? CreationDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DisplayName("Inner Banner")]
        public string InnerBannerImage { get; set; }


        [DisplayName("FeaturedImage")]
        [RequiredIf("IsFeatured", true, ErrorMessage = "This field is required.")]
        public string ImagePath { get; set; }

        public string Catalogue { get; set; }
        [AllowHtml]
        public string TechnicalSpecifications { get; set; }



        [NotMapped]
        public string ImageUrl { get; set; }
        [NotMapped]
        public string BrandTitle { get; set; }

        [NotMapped]
        public string CategoryTitle { get; set; }
        [NotMapped]
        public HttpPostedFileBase CatalogueUplaoder { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload1 { get; set; }


        [NotMapped]
        public List<Category> CategoryList { get; set; }

        [NotMapped]
        public List<Brands> BrandList { get; set; }


        [NotMapped]
       public static string SqlQuery1 = "select prod1.Id,prod1.ProductName,prod1.Description,prod1.IsFeatured,prod1.ShowInHomePage," 
                                 + " prod1.CategoryId,prod1.BrandId,img.ImageUrl,brnd.Title as BrandTitle,prod1.ItemCode, "
                                 + " prod1.WebUrl ,prod1.VideoUrl,prod1.Price,prod1.PromotionCode,prod1.IsFeatured,prod1.IsOffer,prod1.ShowInHomePage, "
                                + " prod1.CreationDate,prod1.CreatedBy,prod1.ImagePath "
                                  + " FROM Products as prod1 "
                                 +" join ProductImage img on prod1.Id = img.ProductId "
                               + " left join Brands brnd on prod1.BrandId = brnd.Id "
                               + " order by img.DisplayOrder asc ";


        public Products()
        {
            Description = "";
            CategoryId = 0;
            BrandId = 0;
            ImagePath = "~/AppFiles/Images/Default.png";
            InnerBannerImage = "~/AppFiles/Images/Default.png";

            CreationDate = DateTime.Now;
        }
    }
}
