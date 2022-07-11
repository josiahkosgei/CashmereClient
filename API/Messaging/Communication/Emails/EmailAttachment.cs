namespace Cashmere.API.Messaging.Communication.Emails
{
  public class EmailAttachment
  {
    public string Name { get; set; }

    public EmailAttachmentMIMEType MimeType { get; set; }

    public byte[] Data { get; set; }
  }
}
