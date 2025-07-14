namespace ApptManager.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int SlotId { get; set; }

        public DateTime BookedOn { get; set; }

        public bool IsApproved { get; set; } = false;

    }
}
