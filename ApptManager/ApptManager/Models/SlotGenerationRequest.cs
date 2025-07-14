namespace ApptManager.Models
{
    public class SlotGenerationRequest
    {
        public int TaxProfessionalId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
