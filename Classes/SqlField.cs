namespace Custom_C_Sharp_Entity_Framework.Classes;

public class SqlField
{
    public string fieldName;
    public string? tableAlias;
    public string? selectAlias;

    public SqlField(string fieldName, string tableAlias, string selectAlias)
    {
        this.fieldName = fieldName;
        this.tableAlias = tableAlias;
        this.selectAlias = selectAlias;
    }

    public SqlField(string fieldName, string tableAlias)
    {
        this.fieldName = fieldName;
        this.tableAlias = tableAlias;
    }

    public SqlField(string field)
    {
        this.fieldName = field;
    }
}
