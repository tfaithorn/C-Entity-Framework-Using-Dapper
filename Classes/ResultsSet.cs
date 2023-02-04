namespace Custom_C_Sharp_Entity_Framework.Classes;

public class ResultsSet<T> where T : OutputBase
{
    public List<T> results;
    public int? count;
}
