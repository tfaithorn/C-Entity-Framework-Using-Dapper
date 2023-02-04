using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Custom_C_Sharp_Entity_Framework.Classes;

public class UserEntity : EntityBase
{
    public int id { get; set; }
    public Guid guid { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string status { get; set; }
    public DateTime createdAt { get; set;}
}
