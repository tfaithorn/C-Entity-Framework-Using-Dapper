using Dapper;
using Microsoft.AspNetCore.Components;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Custom_C_Sharp_Entity_Framework.Classes;

public abstract class TableBase <T> where T : EntityBase
{
    public abstract string GetTableName();
    public abstract List<SqlField> GetFields();

    public T GetByGuid(Guid guid)
    {
        var db = SqlDataAccess.GetInstance();
        var sql = @"SELECT
                        {fields}
	                FROM {tablename}
	                WHERE 
		                guid = @guid";

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"fields", db.PrepareFields(this.GetFields())},
            {"tablename", this.GetTableName()}
        });

        var parameters = new Dictionary<string, object>()
        {
            { "@guid", guid }
        };

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.QuerySingleOrDefault<T>(sql, parameters);
        }
    }

    public List<T> GetByGuids(List<Guid> guids)
    {
        var db = SqlDataAccess.GetInstance();
        var sql = @"SELECT
                        {fields}
	                FROM {tablename}
	                WHERE 
		                guid IN @guid";

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"fields", db.PrepareFields(this.GetFields())},
            {"tablename", this.GetTableName()}
        });

        var parameters = new Dictionary<string, object>()
        {
            { "@guid", guids}
        };

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.Query<T>(sql, parameters).ToList();
        }
    }

    public void Update(List<SqlCondition> values, List<SqlCondition> conditions, Dictionary<string, object> parameters = null)
    {
        parameters = parameters ?? new Dictionary<string, object>();
        var db = SqlDataAccess.GetInstance();
        var sql = @"UPDATE {tableName} SET {values} WHERE {criteria}";

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"tableName", this.GetTableName()},
            {"values", db.PrepareUpdate(values, parameters)},
            {"criteria", db.PrepareWhere(conditions, parameters)}
        });

        using (SqlConnection conn = db.GetConnection())
        {
            conn.Execute(sql, parameters);
        }
    }

    public T GetById(int id)
    {
        var db = SqlDataAccess.GetInstance();
        var sql = @"SELECT
                        {fields}
	                FROM {tablename}
	                WHERE 
		                id = @id";

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"fields", db.PrepareFields(this.GetFields())},
            {"tablename", this.GetTableName()}
        });

        var parameters = new Dictionary<string, object>()
        {
            { "@id", id}
        };

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.QuerySingleOrDefault<T> (sql, parameters);
        }
    }

    public List<T> Select(List<SqlCondition> sqlConditions, Dictionary<string, object> parameters = null)
    {
        var db = SqlDataAccess.GetInstance();
        var sql = @"SELECT
                        {fields}
	                FROM {tablename}
	                WHERE 
		                {criteria}";
        int i = 0;
        parameters = parameters != null ? parameters : new Dictionary<string, object>();

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"fields", db.PrepareFields(this.GetFields())},
            {"tablename", this.GetTableName()},
            {"criteria", db.PrepareWhere(sqlConditions, parameters)}
        });

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.Query<T>(sql, parameters).ToList();
        }
    }

    public void Delete(List<SqlCondition> sqlConditions)
    {
        var db = SqlDataAccess.GetInstance();
        var sql = @"DELETE FROM {tableName} WHERE {criteria};";

        int i = 1;
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        List<SqlCondition> parameterizedConditions = new List<SqlCondition>();

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"tableName", this.GetTableName()},
            {"criteria", db.PrepareWhere(sqlConditions, parameters)}
        });

        using (SqlConnection conn = db.GetConnection())
        {
            Debug.WriteLine(sql);
            conn.Execute(sql, parameters);
        }
    }

    public int? Insert(List<SqlCondition> values, Boolean returnId = false, Dictionary<string, object> parameters = null)
    {
        var db = SqlDataAccess.GetInstance();
        parameters = parameters ?? new Dictionary<string, object>();
        var sql = @"INSERT INTO {tableName} {fieldsValues};";
        sql += returnId ? "SELECT SCOPE_IDENTITY();" : "";

        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"tableName", this.GetTableName()},
            {"fieldsValues", db.PrepareInsert(values, parameters)}
        });

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.ExecuteScalar<int?>(sql, parameters);
        }
    }
}
