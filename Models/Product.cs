using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name="Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Details { get; set; }
        [Display(Name = "Main Image")]
        public string MainImgUrl { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }

        [NotMapped]
        [Display(Name = "Main Image")]
        public IFormFile IFormMainImageFile { get; set; }
        [NotMapped]
        [Display(Name = "Thumbnail Images")]
        public List<IFormFile> IFormMoreImageFile { get; set; }
    }
}
