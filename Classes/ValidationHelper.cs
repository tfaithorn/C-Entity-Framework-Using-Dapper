using System;
using System.Reflection;
using static Custom_C_Sharp_Entity_Framework.Classes.Error;

namespace Custom_C_Sharp_Entity_Framework.Classes;

public static class ValidationHelper
{
    private static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public static void Required(string field, object obj, ref List<Error> errors, string? displayName = null)
    {
        if (obj == null || ((obj is string) && ((string)obj == "")))
        {
            string message = $"The {displayName ?? field} field is required.";
            var index = errors.FindIndex(x => x.field == field);
            if (index == -1)
            {
                errors.Add(new Error(field, new List<string>() { message }));
            }
            else
            {
                errors[index].messages.Add(message);
            }
        }
    }

    public static void MaxCharacters(string field, string value, int maxCharacters, ref List<Error> errors, string? displayName = null)
    {
        if (value == null)
        {
            return;
        }

        if (value.Length > maxCharacters)
        {
            string message = $"The maximum characters allowed for the {displayName ?? field} is ${maxCharacters}.";
            var index = errors.FindIndex(x => x.field == field);
            if (index == -1)
            {
                errors.Add(new Error(field, new List<string>() { message }));
            }
            else
            {
                errors[index].messages.Add(message);
            }
        }
    }

    public static void Email(string field, string email, ref List<Error> errors, string? displayName = null)
    {
        if (email == null)
        {
            return;
        }

        if (!ValidationHelper.IsValidEmail(email))
        {
            string message = $"The {displayName ?? field} is invalid.";
            var index = errors.FindIndex(x => x.field == field);
            if (index == -1)
            {
                errors.Add(new Error(field, new List<string>() { message }));
            }
            else
            {
                errors[index].messages.Add(message);
            }
        }
    }

    public static void AddInvalid(string field, ref List<Error> errors, string? displayName = null)
    {
        string message = "1 or more invalid year guids";
        var index = errors.FindIndex(x => x.field == field);
        if (index == -1)
        {
            errors.Add(new Error(field, new List<string>() { message }));
        }
        else
        {
            errors[index].messages.Add(message);
        }
    }
}
