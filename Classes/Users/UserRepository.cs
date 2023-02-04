using AutoMapper;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.IdentityEntity.Tokens;
using Microsoft.OpenApi.Entitys;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using Custom_C_Sharp_Entity_Framework.Classes;
using Custom_C_Sharp_Entity_Framework.Controllers;
using Custom_C_Sharp_Entity_Framework.Classes.User;

namespace Custom_C_Sharp_Entity_Framework.Classes.User;
public class UserRepository : RepositoryBase, IHasOutput<UserOutput, UserOutputParameters>, IHasResultsSet<UserOutput, UserOutputParameters>
{
    private const int FIRST_NAME_MAX_CHARACTERS = 60;
    private const int MIDDLE_NAME_MAX_CHARACTERS = 60;
    private const int LAST_NAME_MAX_CHARACTERS = 60;
    private const int PASSWORD_MAX_CHARACTERS = 60;
    private const int USERNAME_MAX_CHARACTERS = 60;
    private const int DEFAULT_NUMBER_OF_ROWS = 50;

    public List<UserOutput> GetOutput(
        List<SqlCondition> conditions = null,
        UserOutputParameters outputParameters = null)
    {
        conditions = conditions ?? new List<SqlCondition>();
        outputParameters = outputParameters ?? new UserOutputParameters();

        var userTable = new UserTable();
        List<SqlField> fields = userTable.GetOutputFields();
        var parameters = new Dictionary<string, object>();
        var tokens = GenerateSharedTokens(conditions, parameters, outputParameters);
        tokens["fields"] = "DISTINCT " + db.PrepareFields(fields);
        tokens["orderBy"] = @"ORDER BY 
                                users.firstName ASC,
                                users.middleName ASC,
                                users.lastName ASC";
        var sql = GenerateOutputQuery(tokens);
        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            {"fields", "DISTINCT " + db.PrepareFields(fields)},
        });

        List<UserOutput> userOutputs = new List<UserOutput>();
        using (SqlConnection conn = db.GetConnection())
        {
            userOutputs = conn.Query<UserOutput>(sql, parameters).ToList();
        }

        return  userOutputs;
    }

    public ResultsSet<UserOutput> GetResultsSet(
        List<SqlCondition> conditions = null,
        UserOutputParameters outputParameters = null)
    {
        int numberOfRows = outputParameters.numberOfRows.HasValue ? outputParameters.numberOfRows.Value : DEFAULT_NUMBER_OF_ROWS;
        var resultsSet = new ResultsSet<UserOutput>();
        var parameters = new Dictionary<string, object>();
        var tokens = GenerateSharedTokens(conditions, parameters, outputParameters);
        tokens["fields"] = "COUNT(DISTINCT users.id)";
        tokens["orderBy"] = "";
        var sql = GenerateOutputQuery(tokens);
        using (var conn = db.GetConnection())
        {
            Debug.WriteLine(sql);
            return new ResultsSet<UserOutput>() { results = GetOutput(conditions, outputParameters), count = conn.ExecuteScalar<int>(sql, parameters) };
        }
    }

    private string GenerateOutputQuery(Dictionary<string, string> tokens)
    {
        var sql = @"SELECT
                        {fields}
                    FROM users
                    {criteria}
                    {orderBy}";
        return Util.ReplaceTokens(sql, tokens);
    }

    private Dictionary<string, string> GenerateSharedTokens(List<SqlCondition> conditions, Dictionary<string, object> parameters, UserOutputParameters outputParameters)
    {
        return new Dictionary<string, string>
        {
            {"criteria", conditions != null && conditions.Count > 0 ? "WHERE " + db.PrepareWhere(conditions, parameters) : ""}
        };
    }

    public UserOutput Create(UserInput userInput)
    {

        var values = new List<SqlCondition>()
        {
            new SqlCondition("firstName", userInput.firstName),
            new SqlCondition("middleName", userInput.middleName),
            new SqlCondition("lastName", userInput.lastName),
            new SqlCondition("username", userInput.username),
            new SqlCondition("email", userInput.email),
            new SqlCondition("createdAt", "GETDATE()"){ raw = true },
            new SqlCondition("password", "Hashbytes('SHA2_256', @password)"){raw = true},
        };

        var parameters = new Dictionary<string, object>();
        parameters["@password"] = userInput.password;

        var userTable = new UserTable();
        int userId = userTable.Insert(values, true, parameters).Value;

        var userOutputs = GetOutput(new List<SqlCondition>()
        {
            new SqlCondition("users.id", userId)
        });
        return userOutputs.Count > 0 ? userOutputs[0] : null;
    }

    public List<Error> UserValidation(UserInput userInput)
    {
        var errors = new List<Error>();
        ValidationHelper.Required("firstName", userInput.firstName, ref errors, "First Name");
        ValidationHelper.Required("lastName", userInput.lastName, ref errors, "Last Name");
        ValidationHelper.Required("username", userInput.username, ref errors, "Username");
        ValidationHelper.Required("password", userInput.password, ref errors, "password");
        ValidationHelper.MaxCharacters("username", userInput.username, USERNAME_MAX_CHARACTERS, ref errors, "Username");
        ValidationHelper.MaxCharacters("password", userInput.password, PASSWORD_MAX_CHARACTERS, ref errors, "Password");
        ValidationHelper.MaxCharacters("firstName", userInput.firstName, FIRST_NAME_MAX_CHARACTERS, ref errors, "First Name");
        ValidationHelper.MaxCharacters("lastName", userInput.lastName, LAST_NAME_MAX_CHARACTERS, ref errors, "Last Name");
        ValidationHelper.MaxCharacters("middleName", userInput.middleName, MIDDLE_NAME_MAX_CHARACTERS, ref errors, "Middle Name");
        ValidationHelper.Email("email", userInput.email, ref errors, "Email");
        return errors;
    }

    /// <summary>
    /// Gets the user by their login & password.
    /// Returns a dao of the user so the value in their token can be set
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="outputParameters"></param>
    /// <returns></returns>
    public UserOutput GetByLogin(string username, string password, UserOutputParameters outputParameters)
    {
        var sql = @"SELECT   
                         {fields}
	                FROM users
	                WHERE 
		                users.username = @username 
		                AND users.password = Hashbytes('SHA2_256', @password)";
        var userTable = new UserTable();
        var parameters = new Dictionary<string, object>()
        {
            {"@username", username},
            {"@password", password}
        };
        sql = Util.ReplaceTokens(sql, new Dictionary<string, string>()
        {
            { "fields", db.PrepareFields(userTable.GetOutputFields())},
        });

        using (SqlConnection conn = db.GetConnection())
        {
            return conn.QueryFirstOrDefault<UserOutput>(sql, parameters);
        }
    }

    public UserOutput Update(UserEntity userEntity, UserInput userInput)
    {
        var updateCriteria = new List<SqlCondition>();
        var parameters = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(userInput.firstName))
        {
            updateCriteria.Add(new SqlCondition("firstName", userInput.firstName));
        }

        if (!string.IsNullOrEmpty(userInput.lastName))
        {
            updateCriteria.Add(new SqlCondition("lastName", userInput.lastName));
        }

        if (userInput.status != null)
        {
            updateCriteria.Add(new SqlCondition("status", userInput.status));
        }

        if (userInput.middleName != null)
        {
            updateCriteria.Add(new SqlCondition("middleName", userInput.middleName));
        }

        if (!string.IsNullOrEmpty(userInput.username))
        {
            updateCriteria.Add(new SqlCondition("username", userInput.username));
        }

        if (!string.IsNullOrEmpty(userInput.email))
        {
            updateCriteria.Add(new SqlCondition("email", userInput.email));
        }

        if (!string.IsNullOrEmpty(userInput.password))
        {
            parameters.Add("@password", userInput.password);
            updateCriteria.Add(new SqlCondition("password", "Hashbytes('SHA2_256', @password)") { raw = true });
        }

        var userTable = new UserTable();

        var whereCriteria = new List<SqlCondition>()
        {
            new SqlCondition("id", userEntity.id)
        };
        userTable.Update(updateCriteria, whereCriteria, parameters);

        var userOutputs = GetOutput(whereCriteria);
        return userOutputs.Count > 0 ? userOutputs[0] : null;
    }
}
