namespace ChatContracts
{
    public class ChatMessage
    {
        public string? FromUserId { get; set; }
        public string? ToUserId { get; set; }
        public string? Message { get; set; }
        public bool Unread { get; set; } = true;
    }
}
