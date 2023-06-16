namespace backend.Models {
    public class RefreshToken {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public override string ToString()
        {
            return "ID: " + Id + ", " + "UserId: " + UserId + ", " + "Token: " + Token + ", " + "JwtId: " +  JwtId + ", " 
                   + "IsUsed: " +  IsUsed + ", " + "IsRevoked: " +  IsRevoked + ", " + "CreatedAt: " + CreatedAt.ToString() + 
                   ", " + "ExpiredAt: " + ExpiredAt.ToString();
        }
    }
}
