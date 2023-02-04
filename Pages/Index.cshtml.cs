using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Custom_C_Sharp_Entity_Framework.Pages
{
    public class IndexEntity : PageEntity
    {
        private readonly ILogger<IndexEntity> _logger;

        public IndexEntity(ILogger<IndexEntity> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}