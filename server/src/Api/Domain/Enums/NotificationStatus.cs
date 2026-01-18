namespace Api.Domain.Enums
{
    public enum NotificationStatus
    {
        Unread,    // Notification hasn't been read by the user
        Read,      // Notification has been read
        Pending,   // Notification is waiting for action (e.g., an invitation that hasn't been accepted)
        Accepted,  // Notification has been acted on (e.g., invitation accepted)
        Expired,   // Notification is no longer valid (e.g., invitation expired)
        Declined   // Notification was acted on but declined (e.g., invitation declined)
    }
}