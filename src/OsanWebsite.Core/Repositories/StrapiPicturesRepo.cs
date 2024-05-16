using GraphQL;
using GraphQL.Client.Abstractions;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Models.DataModels;

namespace OsanWebsite.Core.Repositories;

public class StrapiPicturesRepo : IPicturesRepo
{
    private IGraphQLClient _graphQLClient;

    public StrapiPicturesRepo(IGraphQLClient graphQLClient) => _graphQLClient = graphQLClient;

    public async Task<PictureSet> GetSlides()
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetSlides {
                pictureSets(filters: { Name: {eq: ""slideshow""} }) {
                    data { 
                        id
                        attributes { 
                            pictures(sort: ""Position:asc"") {
                                data {
                                    id
                                    attributes {
                                        Title
                                        Image {data { id attributes { url}}}
                                    }
                                }
                            }
                        }
                    }
                }
            }",
            OperationName = "GetSlides"
        };

        var response = await _graphQLClient.SendQueryAsync<PictureSetResponse>(query);
        return response.Data.pictureSets.data.Single().ToPictureSet("slideshow");
    }
}
