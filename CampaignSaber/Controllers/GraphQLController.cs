using EntityGraphQL;
using EntityGraphQL.Schema;
using CampaignSaber.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CampaignSaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private readonly CampaignSaberContext _campaignSaberContext;
        private readonly SchemaProvider<CampaignSaberContext> _schemaProvider;

        public GraphQLController(CampaignSaberContext campaignSaberContext, SchemaProvider<CampaignSaberContext> schemaProvider)
        {
            _schemaProvider = schemaProvider;
            _campaignSaberContext = campaignSaberContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_schemaProvider.GetGraphQLSchema());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QueryRequest query)
        {
            try
            {
                var results = await _schemaProvider.ExecuteQueryAsync(query, _campaignSaberContext, HttpContext.RequestServices, null);
                return Ok(results);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}