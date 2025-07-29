namespace ApptManager.DTOs
{
    public class SlotDto
    {
        public int Id { get; set; }
        public int TaxProfessionalId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

}
