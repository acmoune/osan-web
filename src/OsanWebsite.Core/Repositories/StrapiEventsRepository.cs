using GraphQL;
using GraphQL.Client.Abstractions;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Models.DataModels;

namespace OsanWebsite.Core.Repositories;

public class StrapiEventsRepository : IEventsRepository
{
    private IGraphQLClient _graphQLClient;

    public StrapiEventsRepository(IGraphQLClient graphQLClient) => _graphQLClient = graphQLClient;

    public async Task<Event> GetEventAsync(string slug)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetEvent($slug: String!) {
                events(filters: {Slug: {eq: $slug}}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date Archived Description Keywords IsVideo YoutubeVideoId Body
                            Image { data { id attributes { url } } }
                            service { data { id attributes { Name Description StartTime EndTime } } }
                        }
                    }
                }
            }",
            OperationName = "GetEvent",
            Variables = new { slug = slug }
        };

        var response = await _graphQLClient.SendQueryAsync<EventCollectionResponse>(query);
        return response.Data.events.data.Select(data => data.ToEvent()).Single();
    }

    public async Task<PaggingResult<Event>> GetEventsAsync(bool isArchived, int page, int size)
    {

        var query = new GraphQLRequest
        {
            Query = @"query GetEvents($pageNumber: Int!, $pageSize: Int!){
                events(filters: {Archived: {eq: false}}, sort: ""Date:asc"", pagination: {page: $pageNumber, pageSize: $pageSize}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date Archived Description Keywords IsVideo YoutubeVideoId Body
                            Image {data { id attributes {url}}}
                            service { data { id attributes { Name Description StartTime EndTime } } }
                        }
                    },
                    meta { pagination { pageCount pageSize page total } }
                }
            }",
            OperationName = "GetEvents",
            Variables = new { pageNumber = page, pageSize = size }
        };

        if (isArchived)
        {
            query = new GraphQLRequest
            {
                Query = @"query GetEvents($pageNumber: Int!, $pageSize: Int!){
                events(filters: {Archived: {eq: true}}, sort: ""Date:desc"", pagination: {page: $pageNumber, pageSize: $pageSize}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date Archived Description Keywords IsVideo YoutubeVideoId Body
                            Image {data { id attributes {url}}}
                            service { data { id attributes { Name Description StartTime EndTime } } }
                        }
                    },
                    meta { pagination { pageCount pageSize page total } }
                }
            }",
                OperationName = "GetEvents",
                Variables = new { pageNumber = page, pageSize = size }
            };
        }

        var response = await _graphQLClient.SendQueryAsync<EventCollectionResponse>(query);
        var items = response.Data.events.data.Select(data => data.ToEvent());
        var meta = response.Data.events.meta!.pagination;

        return new PaggingResult<Event>(items, meta.total, meta.pageSize, meta.pageCount, meta.page);
    }

    public async Task<Event?> GetNextEventAsync()
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetNextEvent {
                events(filters: {Archived: {eq: false}}, sort: ""Date:asc"", pagination: {limit: 1}) {
                    data {
                        id
                        attributes { 
                            Title Slug Date Archived Description Keywords IsVideo YoutubeVideoId Body
                            Image {data { id attributes {url}}}
                            service {data { id attributes {Name Description StartTime EndTime}}}
                        }
                    }
                }
            }",
            OperationName = "GetNextEvent"
        };

        var response = await _graphQLClient.SendQueryAsync<EventCollectionResponse>(query);
        var item = response.Data.events.data.Select(data => data.ToEvent()).SingleOrDefault();

        return item;
    }
}
