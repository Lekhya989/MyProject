using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApptManager.DTOs
{
    public record SlotGenerationRequestDto
    {
        [Required]
        public int TaxProfessionalId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        //public DateTime CreatedOn {  get; set; }

        [DefaultValue(false)]
        public bool IsBooked { get; set; } = false;

    }
}
