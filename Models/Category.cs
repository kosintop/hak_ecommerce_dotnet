﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
