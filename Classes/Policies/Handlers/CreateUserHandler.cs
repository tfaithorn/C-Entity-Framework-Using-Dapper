using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Custom_C_Sharp_Entity_Framework.Classes.User;
using Custom_C_Sharp_Entity_Framework.Classes.Policies.Requirements;
using Custom_C_Sharp_Entity_Framework.Classes.User;

namespace Custom_C_Sharp_Entity_Framework.Classes.Policies.Handlers;

public class CreateUserHandler : AuthorizationHandler<CreateUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateUserRequirement requirement)
    {
        try
        {
            var guid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (guid is null)
            {
                return Task.CompletedTask;
            }

            var userRepository = new User.UserRepository();
            var userTable = new UserTable();
            UserEntity userEntity = userTable.GetByGuid(Guid.Parse(guid)) as UserEntity;

            if (userEntity != null)
            {
                context.Succeed(requirement);
            }
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}
