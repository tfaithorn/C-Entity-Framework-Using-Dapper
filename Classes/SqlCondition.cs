namespace Custom_C_Sharp_Entity_Framework.Classes;

public class SqlCondition
{
    public const string OP_EQUALS = "=";
    public const string OP_ISNULL = "IS NULL";
    public const string OP_LIKE = "LIKE";
    public const string OP_IS_NOT = "IS NOT";
    public const string OP_LESS = "<";
    public const string OP_GREATER = ">";
    public const string OP_GREATER_EQ = ">=";
    public const string OP_IN = "IN";
    public const string OP_IS = "IS";
    public const string OP_OR = "OR";

    public string field { get; set; }
    public object value { get; set; }
    public string operation { get; set; }
    public string groupOperation { get; set; }
    public Boolean raw = false;

    public SqlCondition(string field, object value, string operation)
    {
        this.field = field;
        this.value = value;
        this.operation = operation;
    }

    public SqlCondition(string field, object value)
    {
        this.field = field;
        this.value = value;
        this.operation = OP_EQUALS;
    }

    public SqlCondition(List<SqlCondition> conditions, string groupOperation)
    {
        this.value = conditions;
        this.groupOperation = groupOperation;
    }
}
