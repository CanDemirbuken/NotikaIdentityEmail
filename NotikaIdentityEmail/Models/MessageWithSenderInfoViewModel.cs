namespace NotikaIdentityEmail.Models;

public class MessageWithSenderInfoViewModel
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Detail { get; set; }
    public DateTime SendDate { get; set; }
    public string SenderName { get; set; }
    public string SenderSurname { get; set; }
    public string SenderEmail { get; set; }
    public string CategoryName { get; set; }
}
