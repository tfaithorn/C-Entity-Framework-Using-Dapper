using Microsoft.IdentityEntity.Tokens;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Custom_C_Sharp_Entity_Framework.Classes;

public class Error
{
    public string field;
    public List<string> messages;

    public Error(string field, List<string> messages)
    { 
        this.field = field;
        this.messages= messages;
    }
}
