using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Slot
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("TaxProfessional")]
    public int TaxProfessionalId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [DefaultValue(false)]
    public bool IsBooked { get; set; } = false;

    public DateTime CreatedOn { get; set; }= DateTime.Now;

    public DateTime UpdatedOn {  get; set; } = DateTime.Now;
   
}
