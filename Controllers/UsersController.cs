using Microsoft.AspNetCore.Mvc;
using Custom_C_Sharp_Entity_Framework.Classes;
using Custom_C_Sharp_Entity_Framework.Classes.User;

namespace Custom_C_Sharp_Entity_Framework.Controllers
{
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            var conditions = new List<SqlCondition>()
            {
                new SqlCondition("status", "active")
            };
            var userRepository = new UserRepository();
            return Ok(userRepository.GetOutput(conditions));
        }
    }
}
