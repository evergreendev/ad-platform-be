using System.ComponentModel.DataAnnotations;

namespace API.DTOs.EmailMarketing;

public class ScheduleEmailRequest : SendEmailRequest
{
    [Required]
    public DateTimeOffset ScheduledFor { get; set; }
}
