using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioEngine.Web.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class ValuesController : BaseApiController
    {
        [HttpGet]
        [Route("/")]
        public async void TestApiCall()
        {
            Console.WriteLine("the endpoint TestApiCall has been hit!");
        }
    }
}
