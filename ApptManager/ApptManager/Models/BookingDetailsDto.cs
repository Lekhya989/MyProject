namespace ApptManager.Models
{
    public class BookingDetailsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }         
        public int SlotId { get; set; }
        public string UserName { get; set; }
        public DateTime BookedOn { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TaxProfessionalName { get; set; }
        public bool IsApproved {  get; set; }
    }
}
