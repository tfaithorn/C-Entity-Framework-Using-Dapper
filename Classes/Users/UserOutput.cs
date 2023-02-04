namespace Custom_C_Sharp_Entity_Framework.Classes.User;

public class UserOutput : EntityBase
{
    public Guid guid { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string username { get; set; }
    public string status { get; set; }
    public string? email { get; set; }
    public string? middleName { get; set; }
    public DateTime? createdAt { get; set; }
}
