using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigServiceAdapter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using ConfigServiceAdapter;
namespace DenomConfigService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<string> GetOrderCodes()
        {
            
            ConfigServiceAdapterClient client = new ConfigServiceAdapterClient();
            var response = await client.GetOrderCodesAsync("asdasd");
            return "dell";


        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<string> GetOrderCodeDetails()
        {

            ConfigServiceAdapterClient client = new ConfigServiceAdapterClient();
            var response = await client.GetOrderCodeDetailsAsync();
            return "dell";


        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<string> GetOrderCodeDetailsWithParams(string orderCode, string customerSet, string counrtry, string language)
        {

            ConfigServiceAdapterClient client = new ConfigServiceAdapterClient();
            var response = await client.GetOrderCodeDetailsWithParamsAsync(orderCode, customerSet, counrtry, language);

            return response;


        }
    }
}