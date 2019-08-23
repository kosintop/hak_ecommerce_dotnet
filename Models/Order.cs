using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Display(Name ="Date")]
        public DateTime DateCreated { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "Username")]
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Total Amount")]
        public double TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
