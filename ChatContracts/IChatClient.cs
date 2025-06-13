namespace ChatContracts
{
    public interface IChatClient
    {
        Task ReceiveSystemMessage(string message);
        Task UpdateUserList(List<ConnectedUser> users);
        Task ReceiveMessage(string fromUserId, string fromConnectionId, string message);
    }
}
