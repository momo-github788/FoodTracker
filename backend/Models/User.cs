using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend.Models {
    public class User : IdentityUser{
        public virtual ICollection<FoodRecord> FoodRecords { get; set; }

        [StringLength(250, ErrorMessage = "About is limited to 250 characters in length.")]
        public string? About { get; set; }

        [StringLength(250, ErrorMessage = "Name is limited to 250 characters in length.", MinimumLength = 3)]
        public string? Name { get; set; }
    }
}
