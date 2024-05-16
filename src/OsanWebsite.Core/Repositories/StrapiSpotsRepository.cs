using GraphQL;
using GraphQL.Client.Abstractions;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Models.DataModels;

namespace OsanWebsite.Core.Repositories;

public class StrapiSpotsRepository : ISpotsRepository
{
    private IGraphQLClient _graphQLClient;

    public StrapiSpotsRepository(IGraphQLClient graphQLClient) => _graphQLClient = graphQLClient;

    public async Task<Spot> GetSpotAsync(string slug)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetSpot($slug: String!) {
                spots(filters: {Slug: {eq: $slug}}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date IsVideo Description Keywords YoutubeVideoId Body
                            Image { data { id attributes { url } } }
                        }
                    }
                }
            }",
            OperationName = "GetSpot",
            Variables = new { slug = slug }
        };

        var response = await _graphQLClient.SendQueryAsync<SpotCollectionResponse>(query);
        return response.Data.spots.data.Select(data => data.ToSpot()).Single();
    }

    public async Task<PaggingResult<Spot>> GetSpotsAsync(int page, int size)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetSpots($pageNumber: Int!, $pageSize: Int!){
                spots(sort: ""Date:desc"", pagination: {page: $pageNumber, pageSize: $pageSize}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date IsVideo Description Keywords YoutubeVideoId Body
                            Image { data { id attributes { url } } }
                        }
                    },
                    meta { pagination { pageCount pageSize page total } }
                }
            }",
            OperationName = "GetSpots",
            Variables = new { pageNumber = page, pageSize = size }
        };

        var response = await _graphQLClient.SendQueryAsync<SpotCollectionResponse>(query);
        var items = response.Data.spots.data.Select(data => data.ToSpot());
        var meta = response.Data.spots.meta!.pagination;

        return new PaggingResult<Spot>(items, meta.total, meta.pageSize, meta.pageCount, meta.page);
    }
}
