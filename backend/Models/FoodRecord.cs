using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend.Models {
    public class FoodRecord {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Precision(18, 2)]
        [Required(ErrorMessage = "Value is required")]
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public string FoodCategory { get; set; }

        public virtual User? User { get; set; }
        public string UserId { get; set; }

    }

    public enum FoodCategory {
        Poultry , Beverages, Sweets, Vegetables, Fruits, Chips, Nuts
    }
}
