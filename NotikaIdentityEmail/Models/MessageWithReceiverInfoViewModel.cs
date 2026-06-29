namespace NotikaIdentityEmail.Models;

public class MessageWithReceiverInfoViewModel
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Detail { get; set; }
    public DateTime SendDate { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverSurname { get; set; }
    public string ReceiverEmail { get; set; }
    public string CategoryName { get; set; }
}