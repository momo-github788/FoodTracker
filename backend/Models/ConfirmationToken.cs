using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models {
    public class ConfirmationToken {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfirmationTokenId { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? ResendEmailConfirmationToken { get; set; }

        public virtual User? User { get; set; }
        public string UserId { get; set; }

    }
}
