using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace backend.Models {
    public class ConfirmationToken {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfirmationTokenId { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string ResendEmailConfirmationToken { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddHours(24);

        public virtual User? User { get; set; }
        public string UserId { get; set; }


        public override string ToString() {
            return ConfirmationTokenId + ", " + EmailConfirmationToken + ", " + UserId;
        }
    }
}
