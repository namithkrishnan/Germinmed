using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Germinmed.Models
{
    public class HomeViewModel
    {

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public bool ShowInHomePage { get; set; }

        public int ProductId { get; set; }
        public string ImageUrl { get; set; }



    }


    public class SearchViewModel
    {

        public int Id { get; set; }
       
        public List<Products> ProductsList { get; set; }
        
        public List<Events> EventList { get; set; }


    }
}