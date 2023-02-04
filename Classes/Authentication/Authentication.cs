using Microsoft.IdentityEntity.Tokens;
using System.IdentityEntity.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Custom_C_Sharp_Entity_Framework.Classes.User;
using System.Configuration;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Custom_C_Sharp_Entity_Framework.Classes.Authentication;

public class Authentication
{
    public AuthToken GenerateToken(UserOutput user, string issuer, string audience, string signingKey)
    {
        try
        {
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.guid.ToString()),
            new Claim(ClaimTypes.Name, user.username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthToken(tokenString, token.ValidTo);
        }
        catch (Exception e) {
            Debug.WriteLine("Generating Token Exception?");
            Debug.WriteLine(e.Message);
        }
        return null;
    }

    public List<Error> ValidateLogin(LoginInput loginInput)
    {
        List<Error> errors = new List<Error>();
        ValidationHelper.Required("username", loginInput.username, ref errors, "Username");
        ValidationHelper.Required("password", loginInput.password, ref errors, "Password");
        return errors;
    }

    public Boolean VerifyToken(AuthToken authToken, string issuer, string audience, string signingKey)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
            };

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken.token, validationParameters, out validatedToken);
            return true;
        }
            return false;
        }
    }

    public dynamic GetGuidFromContext(HttpContext context)
    {
        var userGuid = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid guid;
        var isGuid = Guid.TryParse(userGuid, out guid);
        return isGuid ? guid : null;
    }
}
