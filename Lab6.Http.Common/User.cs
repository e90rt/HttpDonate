namespace Lab6.Http.Common;

public class TaskItem
{
    public TaskItem()
    {
    }

    public TaskItem(int id, string nickname, int donate,string desc)
    {
        Id = id;
        Nickname = nickname;
        Donate = donate;
        Description = desc;
    }

    public int Id { get; set; }

    public string Nickname { get; set; } = string.Empty;

    public int Donate { get; set; }

    public string Description { get; set; } = string.Empty;
}
