using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Germinmed.Models
{
    public class ProductViewModel
    {
        public int? BrandId { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Category> SubCategory { get; set; }
        public IEnumerable<Brands> Brands { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public Products ProductsDetails { get; set; }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsOffer { get; set; }
        public string OfferPercentage { get; set; }
        public bool ShowInHomePage { get; set; }
        public string WebUrl { get; set; }
        public string VideoUrl { get; set; }

        public string Catalogue { get; set; }
      
        public string TechnicalSpecifications { get; set; }


        public string InnerBannerImage { get; set; }


        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public string BrandTitle { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
    }
}