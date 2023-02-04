using SystemConfig = System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Custom_C_Sharp_Entity_Framework.Classes;

public sealed class SqlDataAccess
{
    static SqlDataAccess instance;

    public static SqlDataAccess GetInstance()
    {
        if (instance == null)
        {
            instance = new SqlDataAccess();
        }

        return instance;
    }

    public SqlConnection GetConnection()
    {
        var builder = WebApplication.CreateBuilder();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        return new System.Data.SqlClient.SqlConnection(connectionString);
    }

    public List<SqlField> ApplyAlias(List<SqlField> sqlFields, string alias)
    {
        foreach (var field in sqlFields)
        {
            field.tableAlias = alias;
        }
        return sqlFields;
    }

    public string PrepareUpdate(List<SqlCondition> values, Dictionary<string, object> parameters)
    {
        string sql = "";
        int count = values.Count;
        int i = 0;
        int criteriaIncrement = GetNextCriteriaIncrement(parameters);

        foreach (var item in values)
        {
            if (item.raw)
            {
                sql += " " + item.field + " " + item.operation + " " + item.value;
            }
            else
            {
                var criteriaString = "@criteria" + criteriaIncrement;
                sql += " " + item.field + " = " + criteriaString;
                parameters.Add(criteriaString, item.value);
                criteriaIncrement++;
            }

            i++;
            if (i < count)
            {
                sql += ",";
            }
        }

        return sql;
    }

    public string PrepareFields(List<string> fields)
    {
        string sql = "";
        int count = fields.Count;
        int i = 0;

        foreach (var field in fields)
        {
            sql += field;
            i++;
            if (i < count)
            {
                sql += ",";
            }
        }
        return sql;
    }

    public string PrepareFields(List<SqlField> fields)
    {
        string sql = "";
        int count = fields.Count;
        int i = 0;

        foreach (var field in fields)
        {
            if (field.tableAlias != null) 
            {
                sql += field.tableAlias + ".";
            }

            sql += field.fieldName;

            if (field.selectAlias != null)
            {
                sql += " AS " + field.selectAlias;
            }

            i++;
            if (i < count)
            {
                sql += ",";
            }
        }

        return sql;
    }

    public string PrepareWhere(List<SqlCondition> conditions, Dictionary<string, object> parameters)
    {
        var sql = "";
        int i = 0;
        int criteriaIncrement = GetNextCriteriaIncrement(parameters);
        
        foreach (SqlCondition condition in conditions)
        {
            if (i > 0)
            {
                sql += " AND ";
            }

            //nested conditions
            if (condition.value is List<SqlCondition> && ((List<SqlCondition>)condition.value).Count >  0)
            {
                var nestedList = (List<SqlCondition>)condition.value;
                int nestedIndex = 0;
                sql += " (";
                foreach (SqlCondition nestedCondition in nestedList)
                {
                    sql+= PrepareWhere(new List<SqlCondition>() { nestedCondition }, parameters);
                    if (nestedIndex < (nestedList.Count - 1))
                    {
                        sql += " " + condition.groupOperation + " ";
                    }
                    nestedIndex++;
                }
                sql += ")";
            }
            else
            {
                if (condition.raw)
                {
                    sql += " " + condition.field + " " + condition.operation + " " + condition.value;
                }
                else
                {
                    string criteriaString = "@criteria" + criteriaIncrement;
                    parameters.Add(criteriaString, condition.value);
                    sql += condition.field + " " + condition.operation + " " + criteriaString;
                    criteriaIncrement++;
                }
                i++;
            }
        }

        return sql;
    }

    public string PrepareInsert(List<SqlCondition> values, Dictionary<string, object> parameters)
    {
        string fieldsText = "";
        string valuesText = "";
        int i = 0;
        int count = values.Count();
        int criteriaIncrement = GetNextCriteriaIncrement(parameters);

        foreach (var condition in values)
        {
            fieldsText += condition.field;

            if (condition.raw)
            {
                valuesText += condition.value;
            }
            else
            {
                string criteriaParam = "@criteria" + criteriaIncrement;
                valuesText += criteriaParam;
                parameters.Add(criteriaParam, condition.value);
            }
            
            i++;
            criteriaIncrement++;

            if (i < count)
            {
                fieldsText += ", ";
                valuesText += ", ";
            }
        }

        return "(" + fieldsText + ") VALUES(" + valuesText + ")";
    }

    private int GetNextCriteriaIncrement(Dictionary<string, object> parameters)
    {
        int criteriaIncrement = 0;
        while (parameters.ContainsKey("@criteria" + criteriaIncrement))
        {
            criteriaIncrement++;
        }
        return criteriaIncrement;
    }

    public string PrepareLimitOffset(int page, int numberOfRows, Dictionary<string, object> parameters)
    {
        int rowsToSkip = (page - 1) * numberOfRows;
        parameters.Add("@rowsToSkip", rowsToSkip);
        parameters.Add("@resultsPerPage", numberOfRows);
        return @" OFFSET @rowsToSkip ROWS 
                      FETCH NEXT @resultsPerPage ROWS ONLY";
    }
}
