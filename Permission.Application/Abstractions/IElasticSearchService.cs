namespace Permission.Application.Abstractions
{
    public interface IElasticSearchService
    {
        /// <summary>
        /// Indexes a permission in Elasticsearch.
        /// </summary>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="permission">The permission to index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task IndexPermissionInElasticsearch(string indexName, Domain.Models.Permission permission);
        /// <summary>
        /// Retrieves a list of documents from Elasticsearch.
        /// </summary>
        /// <param name="indexName">The name of the index.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<Domain.Models.Permission>> GetDocuments(string indexName);
    }
}
