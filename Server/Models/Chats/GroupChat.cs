namespace Server.Models.Chats;

public class GroupChat : BaseChat
{
    public string Name { get; set; } = string.Empty;

    public ApplicationUser Owner { get; set; } = null!;
}
