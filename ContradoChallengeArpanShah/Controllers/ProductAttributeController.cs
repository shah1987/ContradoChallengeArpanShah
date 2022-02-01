using ContradoChallengeAPI.Repository;
using ContradoCommon;
using ContradoModel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        [HttpPost]
        [Route("getallproductattribute")]
        public async Task<IActionResult> GetAllProductAttribute(GetProductAttributeRequest getProductAttributeRequest)
        {
            using (ProductAttributeRepository repository = new ProductAttributeRepository())
            {
                return CommonResponse.CommonAPIResponse(await repository.GetAllProductAttribute(getProductAttributeRequest));
            }
        }
    }
}
