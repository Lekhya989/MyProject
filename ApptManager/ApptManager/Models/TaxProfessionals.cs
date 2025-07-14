namespace ApptManager.Models
{
    public class TaxProfessional
    {
        public int Id { get; set; }               
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;       
        public string PhoneNumber { get; set; } = string.Empty;
        public bool speaks_spanish { get; set; }
        public bool SMB_certified { get; set; }
        public DateTime CreatedOn { get; set; }                  
        public DateTime? UpdatedOn { get; set; }
        
    }
}