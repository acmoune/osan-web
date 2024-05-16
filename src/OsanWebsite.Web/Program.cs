using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Authentication;
using OsanWebsite.Core.Infrastructure;
using OsanWebsite.Core.Models;
using OsanWebsite.Core.Repositories;
using OsanWebsite.Core.Services;
using OsanWebsite.Web.Components;
using OsanWebsite.Web.Components.Admin;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IGraphQLClient>(sp =>
{
    var serializer = new NewtonsoftJsonSerializer();
    string graphQLUri = Environment.GetEnvironmentVariable("GRAPHQL_URI")!;
    string apiKey = Environment.GetEnvironmentVariable("GRAPHQL_API_TOKEN")!;

    var client = new GraphQLHttpClient(graphQLUri, serializer);
    client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {apiKey}");

    return client;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";
        options.ClientId = "Web";
        options.ClientSecret = "8cfc91c9-350d-42dc-886e-a918d4c57749";
        options.ResponseType = "code";
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("roles");
        options.GetClaimsFromUserInfoEndpoint = true;

        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.SaveTokens = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Administrator");
    });
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Account");
});


builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
});

// Blazorise setup:
builder.Services.AddBlazorise(opts =>
{
    opts.Immediate = true;
})
.AddBootstrapProviders()
.AddFontAwesomeIcons();

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssemblies([
        typeof(Booking).Assembly,
        typeof(Program).Assembly,
    ]);
});

builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IEventsRepository, StrapiEventsRepository>();
builder.Services.AddSingleton<ISpotsRepository, StrapiSpotsRepository>();
builder.Services.AddSingleton<IPicturesRepo, StrapiPicturesRepo>();
builder.Services.AddSingleton<IBookingRepository, StrapiBookingRepository>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IQrCodeGenerator, QrCodeGenerator>();

builder.Services.AddSingleton<DataService>();
builder.Services.AddScoped<AlertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression(); // SignalR Guidelines

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();

app.Run();
