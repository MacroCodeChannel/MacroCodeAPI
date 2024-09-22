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
    public class InvoicesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _navisionAddress;
        public InvoicesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _navisionAddress = configuration.GetSection("DirectorySettings:NavisionAddress").Value;
        }

        [HttpGet(Name = "GetInvoices")]
        public async Task<IEnumerable<Sales_Invoice>> GetInvoices()
        {
            try
            {
                var navisionadresslink = $"{_configuration.GetSection("DirectorySettings:NavisionAddress").Value}";
                var context = new NAV(new Uri(_navisionAddress));
                context.Credentials = CredentialCache.DefaultCredentials;

                var basicQuery = context.Sales_Invoice.AddQueryOption("$top", "20"); // Fetch only the top record
                var response = await basicQuery.ExecuteAsync();
                return response;
            }
            catch (DataServiceClientException dsqEx)
            {
                Console.WriteLine($"OData client error: {dsqEx.Message}");
                Console.WriteLine($"Response Status Code: {dsqEx.StatusCode}");
                Console.WriteLine($"OData query error: {dsqEx.Message}");
                return Enumerable.Empty<Sales_Invoice>(); // Return an empty collection instead of null
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine($"Invalid operation error: {ioe.Message}");
                return Enumerable.Empty<Sales_Invoice>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return Enumerable.Empty<Sales_Invoice>();
            }
        }
    }
}
