using System.ComponentModel.DataAnnotations;

public record MailRequestDto
{
    [Required, EmailAddress]
    public string ToEmail { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;
}
