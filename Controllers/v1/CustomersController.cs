using Asp.Versioning;
using MacroCodeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC;
using BC.NAV;
using System.ServiceModel;
using System.Net;
using System.ComponentModel;
using Microsoft.OData.Client;
using Microsoft.AspNetCore.Authorization;

namespace MacroCodeAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _navisionAddress;
        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _navisionAddress = configuration.GetSection("DirectorySettings:NavisionAddress").Value;
        }
        [HttpPost]
        [Route("/GetCustomers")]
        public async Task<IEnumerable<Customers>> GetCustomers()
        {
            try
            {
                var context = new NAV(new Uri(_navisionAddress));
                context.Credentials = CredentialCache.DefaultCredentials;

                var basicQuery = context.Customers.AddQueryOption("$top", "20"); 
                var response = await basicQuery.ExecuteAsync();
                return response;
            }
            catch (DataServiceClientException dsqEx)
            {
                Console.WriteLine($"OData client error: {dsqEx.Message}");
                Console.WriteLine($"Response Status Code: {dsqEx.StatusCode}");
                Console.WriteLine($"OData query error: {dsqEx.Message}");
                return Enumerable.Empty<Customers>(); 
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine($"Invalid operation error: {ioe.Message}");
                return Enumerable.Empty<Customers>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return Enumerable.Empty<Customers>();
            }

        }



        [HttpPost]
        [Route("/AddCustomer")]
        public async Task AddCustomer(Customers vm)
        {
            try
            {
                var context = new NAV(new Uri(_navisionAddress));
                context.Credentials = CredentialCache.DefaultCredentials;

               context.AddToCustomers(vm);
            }
            catch (DataServiceClientException dsqEx)
            {
                Console.WriteLine($"OData client error: {dsqEx.Message}");
                Console.WriteLine($"Response Status Code: {dsqEx.StatusCode}");
                Console.WriteLine($"OData query error: {dsqEx.Message}");
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine($"Invalid operation error: {ioe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }

        }


    }
}
