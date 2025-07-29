namespace ApptManager.DTOs
{
    public class CreateTaxProfessionalDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool SpeaksSpanish { get; set; }
        public bool SMBCertified { get; set; }
    }
}
