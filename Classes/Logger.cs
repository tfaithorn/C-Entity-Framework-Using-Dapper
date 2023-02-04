namespace Custom_C_Sharp_Entity_Framework.Classes;

public static class Logger
{
    public static void Dump(this object obj)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
    }
}
