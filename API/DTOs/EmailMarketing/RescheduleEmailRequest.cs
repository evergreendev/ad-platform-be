using System.ComponentModel.DataAnnotations;

namespace API.DTOs.EmailMarketing;

public class RescheduleEmailRequest
{
    [Required]
    public DateTimeOffset ScheduledFor { get; set; }
}
