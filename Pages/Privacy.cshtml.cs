using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Custom_C_Sharp_Entity_Framework.Pages
{
    public class PrivacyEntity : PageEntity
    {
        private readonly ILogger<PrivacyEntity> _logger;

        public PrivacyEntity(ILogger<PrivacyEntity> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}