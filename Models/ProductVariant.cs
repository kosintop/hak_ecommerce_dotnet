using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models
{
    public class ProductVariant
    {
        public int ProductVariantId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Additional Price")]
        public double AdditionalPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
