namespace Custom_C_Sharp_Entity_Framework.Classes.Authentication;

public class AuthToken
{
    public string token { get; set; }
    public DateTime validTo { get; set; }

    public AuthToken(string token, DateTime validTo)
    { 
        this.token = token;
        this.validTo = validTo;
    }
}
