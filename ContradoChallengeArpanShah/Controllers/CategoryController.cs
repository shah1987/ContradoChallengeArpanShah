using ContradoChallengeAPI.Repository;
using ContradoCommon;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpPost]
        [Route("getallcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            using (CategoryRepository repository = new CategoryRepository())
            {
                return CommonResponse.CommonAPIResponse(await repository.GetAllCategories());
            }
        }
    }
}
