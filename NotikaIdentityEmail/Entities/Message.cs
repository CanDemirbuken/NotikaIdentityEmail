namespace NotikaIdentityEmail.Entities;

public class Message
{
    public int Id { get; set; }
    public string SenderEmail { get; set; }
    public string ReceiverEmail { get; set; }
    public string Subject { get; set; }
    public DateTime SendDate { get; set; }
    public string Detail { get; set; }
    public bool IsRead { get; set; }

    //Relations
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
