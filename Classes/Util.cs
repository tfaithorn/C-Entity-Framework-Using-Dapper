namespace Custom_C_Sharp_Entity_Framework.Classes;

public class Util
{
    public static string ReplaceTokens(string str, Dictionary<string, string> tokens)
    {
        foreach (var item in tokens)
        {
            str = str.Replace("{"+item.Key+"}", item.Value);
        }

        return str;
    }
}
