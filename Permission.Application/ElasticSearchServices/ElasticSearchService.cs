using Microsoft.Extensions.Configuration;
using Nest;
using Permission.Application.Abstractions;

namespace Permission.Application.ElasticSearchServices
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly IElasticClient _elasticClient;
        public ElasticSearchService(IConfiguration configuration) { 

            _configuration = configuration;
            _elasticClient = CreateInstance();
        
        }
        private ElasticClient CreateInstance()
        {
            var host = _configuration.GetSection("ElasticSearchServer:Host").Value;
            var port = _configuration.GetSection("ElasticSearchServer:Port").Value;
            var userName = _configuration.GetSection("ElasticSearchServer:Username").Value;
            var password = _configuration.GetSection("ElasticSearchServer:Password").Value;
            var settings = new ConnectionSettings(new Uri(host + ":" + port)).ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(userName, password);
            return new ElasticClient(settings);
        }
        public async Task<List<Domain.Models.Permission>> GetDocuments(string indexName)
        {
            var response = await _elasticClient.SearchAsync<Domain.Models.Permission>(s => s.Index(indexName).MatchAll());
            if (response.IsValid)
            {
                var permissionsFromElasticsearch = response.Documents.ToList();
                return permissionsFromElasticsearch;
            }
            else
            {                
                return new List<Domain.Models.Permission>();
            }
        }

        public async Task IndexPermissionInElasticsearch(string indexName, Domain.Models.Permission permision)
        {
            var response = await _elasticClient.CreateAsync(permision, i => i.Index(indexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                var updateResponse = await _elasticClient.UpdateAsync<Domain.Models.Permission>(permision.Id, u => u.Index(indexName).Doc(permision));
            }
        }

    }
}
