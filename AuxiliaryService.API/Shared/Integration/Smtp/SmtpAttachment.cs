namespace AuxiliaryService.API.Shared.Integration.Smtp
{
    public class SmtpAttachment
    {
        public string Name { get; set; }

        public byte[] Data { get; set; }

        public string MediaType { get; set; } 
    }
}
