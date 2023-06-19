using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models {

    public class ConfirmationToken {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Token { get; set; }
        public string? ResendToken { get; set; }
        public bool? isUsed { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiredAt { get; set; } = DateTime.Now.AddHours(24);

        public virtual User? User { get; set; }
        public string UserId { get; set; }


        public override string ToString() {
            return Id + ", " + Token + ", " + ResendToken + ", " + UserId;
        }
    }
}
