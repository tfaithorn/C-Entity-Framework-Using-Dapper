using Microsoft.AspNetCore.Authorization;
using Custom_C_Sharp_Entity_Framework.Classes.User;
using Custom_C_Sharp_Entity_Framework.Classes.Policies.Requirements;
using Custom_C_Sharp_Entity_Framework.Classes.User;

namespace Custom_C_Sharp_Entity_Framework.Classes.Policies.Handlers;

public class UpdateUserHandler : AuthorizationHandler<UpdateUserRequirement>
{

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UpdateUserRequirement requirement)
    {
        try
        {
            //do logic to authorize updating accounts here
            var guid = context.User.Claims.First(i => i.Type == "guid").Value;
            var userRepository = new User.UserRepository();
            var userTable = new UserTable();
            UserEntity userEntity = userTable.GetByGuid(Guid.Parse(guid));

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
