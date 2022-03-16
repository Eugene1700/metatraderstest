using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IpGeoInformer.Controllers
{
    [ApiController]
    public class SearcherController: ControllerBase
    {
        
        [Route("ping")]
        [AllowAnonymous]
        public string Ping()
        {
            return "Hello";
        }
    }
}