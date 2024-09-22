using Asp.Versioning;
using BC.NAV;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.Client;
using System.Net;

namespace MacroCodeAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public VendorsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "GetVendors")]
        public async Task<IEnumerable<Vendors>> GetVendors()
        {
            try
            {
                var navisionadresslink = $"{_configuration.GetSection("DirectorySettings:NavisionAddress").Value}";
                var context = new NAV(new Uri(navisionadresslink));
                context.Credentials = CredentialCache.DefaultCredentials;

                var basicQuery = context.Vendors.AddQueryOption("$top", "20"); // Fetch only the top record
                var response = await basicQuery.ExecuteAsync();
                return response;
            }
            catch (DataServiceClientException dsqEx)
            {
                Console.WriteLine($"OData client error: {dsqEx.Message}");
                Console.WriteLine($"Response Status Code: {dsqEx.StatusCode}");
                Console.WriteLine($"OData query error: {dsqEx.Message}");
                return Enumerable.Empty<Vendors>(); // Return an empty collection instead of null
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine($"Invalid operation error: {ioe.Message}");
                return Enumerable.Empty<Vendors>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return Enumerable.Empty<Vendors>();
            }

        }

    }
}
