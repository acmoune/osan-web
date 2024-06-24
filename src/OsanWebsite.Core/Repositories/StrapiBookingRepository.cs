using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Models.DataModels;

namespace OsanWebsite.Core.Repositories;

public class StrapiBookingRepository : IBookingRepository
{
    private IGraphQLClient _graphQLClient;
    private ILogger<StrapiBookingRepository> _logger;


    public StrapiBookingRepository(IGraphQLClient graphQLClient, ILogger<StrapiBookingRepository> logger)
    {
        _graphQLClient = graphQLClient;
        _logger = logger;
    }

    // Campaign

    // Mutations
    public async Task<BookingCampaign> SaveCampaign(BookingCampaign campaign) // Create or Replace
    {
        if (campaign.Id != null)
        {
            // Replace
            var mutation = new GraphQLRequest
            {
                Query = @"mutation CreateCampaign($campaign_id: ID!, $bookingDate: Date!, $service_id: ID!, $status: ENUM_BOOKINGCAMPAIGN_STATUS!) {
                    updateBookingCampaign(
                        id: $campaign_id,
                        data: {
                            BookingDate: $bookingDate,
                            service: $service_id,
                            Status: $status
                        }
                    ){
                        data {
                            id
                            attributes { 
                                BookingDate Status
                                service { data { id attributes {Name Description StartTime EndTime }} }
                            }
                        }
                    }
                }",
                OperationName = "CreateCampaign",
                Variables = new
                {
                    campaign_id = campaign.Id,
                    bookingDate = campaign.BookingDate,
                    service_id = campaign.Service.Id,
                    status = campaign.Status,
                }
            };

            var response = await _graphQLClient.SendMutationAsync<UpdateCampaignResponse>(mutation);
            return response.Data.updateBookingCampaign.data.ToBookingCampaign();
        }
        else
        {
            // Create
            var mutation = new GraphQLRequest
            {
                Query = @"mutation CreateCampaign($bookingDate: Date!, $service_id: ID!, $status: ENUM_BOOKINGCAMPAIGN_STATUS!) {
                    createBookingCampaign(
                        data: {
                            BookingDate: $bookingDate,
                            service: $service_id,
                            Status: $status
                        }
                    ){
                        data {
                            id
                            attributes { 
                                BookingDate Status
                                service { data { id attributes {Name Description StartTime EndTime }} }
                            }
                        }
                    }
                }",
                OperationName = "CreateCampaign",
                Variables = new 
                {
                    bookingDate = campaign.BookingDate,
                    service_id = campaign.Service.Id,
                    status = campaign.Status,
                }
            };

            var response = await _graphQLClient.SendMutationAsync<CreateCampaignResponse>(mutation);
            return response.Data.createBookingCampaign.data.ToBookingCampaign();
        }
    }

    public async Task CompleteCampaign(BookingCampaign campaign)
    {
        var bookings = await GetUnconsumedBookings(campaign.Id!);

        foreach (var booking in bookings)
        {
            booking.SetStatus(BookingStatus.Unconsumed);
            await Save(booking);
        }
    }

    // Queries
    public async Task<BookingCampaign?> GetCampaign(DateOnly bookingDate, string serviceName)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetCampaign($bookingDate: Date!, $serviceName: String!) {
                bookingCampaigns(filters: { BookingDate: {eq: $bookingDate}, service: {Name: {eq: $serviceName}}}) {
                    data { 
                        id
                        attributes { 
                            BookingDate Status
                            service { data { id attributes {Name Description StartTime EndTime }} }
                        }
                    }
                }
            }",
            OperationName = "GetCampaign",
            Variables = new { bookingDate = bookingDate, serviceName = serviceName },
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCampaignCollectionResponse>(query);
        var result = response.Data.bookingCampaigns.data.SingleOrDefault();

        return result != null ? result.ToBookingCampaign() : null;
    }

    public async Task<BookingCampaign?> GetCampaign(string campaign_id)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetCampaignById($campaign_id: ID!) {
                bookingCampaigns(filters: { id: {eq: $campaign_id} }) {
                    data { 
                        id
                        attributes { 
                            BookingDate Status
                            service { data { id attributes {Name Description StartTime EndTime }} }
                        }
                    }
                }
            }",
            OperationName = "GetCampaignById",
            Variables = new { campaign_id = campaign_id },
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCampaignCollectionResponse>(query);
        var result = response.Data.bookingCampaigns.data.SingleOrDefault();

        return result != null ? result.ToBookingCampaign() : null;
    }

    public async Task<IEnumerable<BookingCampaign>> GetCampaigns(int year, int month)
    {
        var start = new DateOnly(year, month, 1);
        var end = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

        var query = new GraphQLRequest
        {
            Query = @"query GetCampaigns($startDate: Date!, $endDate: Date!){
                bookingCampaigns(filters: {BookingDate: {between: [$startDate, $endDate]}}, sort: ""BookingDate:desc""){
                    data {
                        id
                        attributes {
                            BookingDate
                            Status
                            service { 
                                data { 
                                    id 
                                    attributes {
                                        Name
                                        Description 
                                        StartTime 
                                        EndTime
                                    }
                                }
                            }
                        }
                    }
                }
            }",
            OperationName = "GetCampaigns",
            Variables = new { startDate = start, endDate = end }
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCampaignCollectionResponse>(query);
        return response.Data.bookingCampaigns.data.Select(d => d.ToBookingCampaign());
    }


    // Booking

    // Mutations
    public async Task<Booking> Create(Booking booking)
    {
        // Create
        var mutation = new GraphQLRequest
        {
            Query = @"mutation CreateBooking($bookingId: String!, $name: String!, $phone: String!, $email: String!, $status: ENUM_BOOKING_STATUS!, $campaign_id: ID!, $tableType_id: ID!) {
                createBooking(
                    data: {
                        BookingId: $bookingId,
                        CustomerName: $name,
                        CustomerPhone: $phone,
                        CustomerEmail: $email,
                        Status: $status,
                        booking_campaign: $campaign_id,
                        table_type: $tableType_id
                    }
                ){
                    data { 
                        id
                        attributes { 
                            BookingId CustomerName CustomerPhone CustomerEmail Status QrCodeUrl CancellationReason
                            booking_campaign { 
                                data {
                                    id
                                    attributes {
                                        BookingDate Status 
                                        service { data { id attributes { Name Description StartTime EndTime } } }
                                    }
                                } 
                            }
                            table_type { data { id attributes { Name NumberOfPlaces } } }
                        }
                    }
                }
            }",
            OperationName = "CreateBooking",
            Variables = new
            {
                bookingId = booking.BookingId,
                name = booking.CustomerName,
                phone = booking.CustomerPhone,
                email = booking.CustomerEmail,
                status = booking.Status,
                campaign_id = booking.Campaign.Id,
                tableType_id = booking.TableType.Id,
            }
        };

        var response = await _graphQLClient.SendMutationAsync<CreateBookingResponse>(mutation);
        return response.Data.createBooking.data.ToBooking();
    }

    public async Task<Booking> Save(Booking booking)
    {
        // Update
        var mutation = new GraphQLRequest
        {
            Query = @"mutation UpdateBooking($booking_id: ID!, $bookingId: String!, $name: String!, $phone: String!, $email: String!, $status: ENUM_BOOKING_STATUS!, $campaign_id: ID!, $tableType_id: ID!, $qrCodeUrl: String, $cancellationReason: String) {
                    updateBooking(
                        id: $booking_id,
                        data: {
                            BookingId: $bookingId,
                            CustomerName: $name,
                            CustomerPhone: $phone,
                            CustomerEmail: $email,
                            Status: $status,
                            booking_campaign: $campaign_id,
                            table_type: $tableType_id,
                            QrCodeUrl: $qrCodeUrl,
                            CancellationReason: $cancellationReason
                        }
                    ){
                        data { 
                            id
                            attributes { 
                                BookingId CustomerName CustomerPhone CustomerEmail Status QrCodeUrl CancellationReason
                                booking_campaign { 
                                    data {
                                        id
                                        attributes {
                                            BookingDate Status 
                                            service { data { id attributes { Name Description StartTime EndTime } } }
                                        }
                                    } 
                                }
                                table_type { data { id attributes { Name NumberOfPlaces } } }
                            }
                        }
                    }
                }",
            OperationName = "UpdateBooking",
            Variables = new
            {
                booking_id = booking.Id,
                bookingId = booking.BookingId,
                name = booking.CustomerName,
                phone = booking.CustomerPhone,
                email = booking.CustomerEmail,
                status = booking.Status,
                campaign_id = booking.Campaign.Id,
                tableType_id = booking.TableType.Id,
                qrCodeUrl = booking.QrCodeUrl,
                cancellationReason = booking.CancellationReason,
            }
        };

        var response = await _graphQLClient.SendMutationAsync<UpdateBookingResponse>(mutation);
        return response.Data.updateBooking.data.ToBooking();
    }

    // Queries
    public async Task<Booking> GetById(Guid bookingId)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetBookingById($bookingId: String!) {
                bookings(filters: { BookingId: {eq: $bookingId} }) {
                    data { 
                        id
                        attributes { 
                            BookingId CustomerName CustomerPhone CustomerEmail Status QrCodeUrl CancellationReason
                            booking_campaign { 
                                data {
                                    id
                                    attributes {
                                        BookingDate Status 
                                        service { data { id attributes { Name Description StartTime EndTime } } }
                                    }
                                } 
                            }
                            table_type { data { id attributes { Name NumberOfPlaces } } }
                        }
                    }
                }
            }",
            OperationName = "GetBookingById",
            Variables = new { bookingId = bookingId.ToString() },
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCollectionResponse>(query);
        return response.Data.bookings.data.Select(data => data.ToBooking()).Single();
    }

    public async Task<PaggingResult<BookingItem>> GetBookings(string campaign_id, int page, int size)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetBookings($campaign_id: ID!, $pageNumber: Int!, $pageSize: Int!){
                bookings(filters: { booking_campaign: {id: {eq: $campaign_id}} }, sort: ""CustomerName:asc"", pagination: {page: $pageNumber, pageSize: $pageSize}) {
                    data { 
                        id
                        attributes { 
                            BookingId CustomerName CustomerPhone CustomerEmail Status QrCodeUrl CancellationReason
                            booking_campaign { 
                                data {
                                    id
                                    attributes {
                                        BookingDate Status 
                                        service { data { id attributes { Name Description StartTime EndTime } } }
                                    }
                                } 
                            }
                            table_type { data { attributes { Name NumberOfPlaces } } }
                        }
                    },
                    meta { pagination { pageCount pageSize page total } }
                }
            }",
            OperationName = "GetBookings",
            Variables = new { campaign_id = campaign_id, pageNumber = page, pageSize = size }
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCollectionResponse>(query);
        var items = response.Data.bookings.data.Select(data => new BookingItem(data.ToBooking()));
        var meta = response.Data.bookings.meta!.pagination;

        return new PaggingResult<BookingItem>(items, meta.total, meta.pageSize, meta.pageCount, meta.page);
    }

    private async Task<IEnumerable<Booking>> GetUnconsumedBookings(string campaign_id)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetBookings($campaign_id: ID!){
                bookings(filters: {booking_campaign: {id: {eq: $campaign_id}}, Status: {eq: ""CONFIRMED""}}) {
                    data { 
                        id
                        attributes { 
                            BookingId CustomerName CustomerPhone CustomerEmail Status QrCodeUrl CancellationReason
                            booking_campaign { 
                                data {
                                    id
                                    attributes {
                                        BookingDate Status 
                                        service { data { id attributes { Name Description StartTime EndTime } } }
                                    }
                                } 
                            }
                            table_type { data { id attributes { Name NumberOfPlaces } } }
                        }
                    }
                }
            }",
            OperationName = "GetBookings",
            Variables = new { campaign_id = campaign_id }
        };

        var response = await _graphQLClient.SendQueryAsync<BookingCollectionResponse>(query);
        return response.Data.bookings.data.Select(data => data.ToBooking());
    }


    // Service

    public async Task<IEnumerable<Service>> GetServicesAsync()
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetServices {
                services(sort: ""StartTime:asc"") {
                    data {
                        id
                        attributes { Name Description StartTime EndTime }
                    }
                }
            }",
            OperationName = "GetServices"
        };

        var response = await _graphQLClient.SendQueryAsync<ServiceCollectionResponse>(query);
        return response.Data.services.data.Select(data => data.ToService());
    }

    public async Task<Service> GetServiceByBame(string name)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetService($name: String!) {
                services(filters: { Name: { eq: $name } }) {
                    data {
                        id
                        attributes { Name Description StartTime EndTime }
                    }
                }
            }",
            OperationName = "GetService",
            Variables = new { name = name }
        };

        var response = await _graphQLClient.SendQueryAsync<ServiceCollectionResponse>(query);
        return response.Data.services.data.Select(data => data.ToService()).Single();
    }

    // TableType

    public async Task<IEnumerable<TableType>> GetTableTypesAsync()
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetTableTypes {
                tableTypes(sort: ""NumberOfPlaces:asc"") {
                    data { 
                        id 
                        attributes { Name NumberOfPlaces }
                    }
                }
            }",
            OperationName = "GetTableTypes"
        };

        var response = await _graphQLClient.SendQueryAsync<TableTypeCollectionResponse>(query);
        return response.Data.tableTypes.data.Select(data => data.ToTableType());
    }

    public async Task<TableType> GetTableTypeByBame(string name)
    {
        var query = new GraphQLRequest
        {
            Query = @"query GetTableType($name: String!) {
                tableTypes(filters: { Name: { eq: $name } }) {
                    data {
                        id
                        attributes { Name NumberOfPlaces }
                    }
                }
            }",
            OperationName = "GetTableType",
            Variables = new { name = name }
        };

        var response = await _graphQLClient.SendQueryAsync<TableTypeCollectionResponse>(query);
        return response.Data.tableTypes.data.Select(data => data.ToTableType()).Single();
    }
}
