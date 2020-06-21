namespace AuxiliaryService.Domain.Notification
{
    public class NotificationMessageAttachment
    {
        public string EntityType { get; set; }
        public string ConfigurationCodeName { get; set; }
        public string ConfigurationCodeVersion { get; set; }
        public string DocumentNumber { get; set; }
        public string AttachmentType { get; set; }
    }
}