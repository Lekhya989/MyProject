namespace ApptManager.DTOs
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string? UserId { get; set; }
    }

}
