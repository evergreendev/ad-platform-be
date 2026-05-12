namespace API.Enums;

public enum EmailMessageStatus
{
    Draft = 0,
    Queued = 1,
    Sending = 2,
    Sent = 3,
    Failed = 4,
    Cancelled = 5
}