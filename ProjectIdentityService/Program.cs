using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectIdentityService.Data;
using ProjectIdentityService.Model;
using SayyehBanTools.ConnectionDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
  AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(p => p.UseSqlServer(SqlServerConnection.ConnectionString(".", "aspIdentityDB", "TestConnection", "@123456")));
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer().
    AddDeveloperSigningCredential().
    AddInMemoryClients(new List<Client> {
      new Client
      {
          ClientName="Web FrontEnd Code",
          ClientId="webfrontendcode",
          ClientSecrets={new Secret ("123456".Sha256()) },
          AllowedGrantTypes=GrantTypes.Code,
          RedirectUris={
              LinkServer.FrontEndUser+"/signin-oidc"
          },
          PostLogoutRedirectUris={
              LinkServer.FrontEndUser+"/signout-callback-oidc"
          },
          AllowedScopes={ "openid", "profile" , "orderservice.getorders", "basket.fullaccess" , "apigatewayforweb.fullaccess" },
          AllowOfflineAccess=true,
          AccessTokenLifetime=60,
          RefreshTokenUsage=TokenUsage.ReUse,
          RefreshTokenExpiration = TokenExpiration.Sliding,
      },
       new Client
        {
            ClientName="AdminFront End",
            ClientId ="adminfrontend",
            ClientSecrets={new Secret("123456".Sha256())},
            AllowedGrantTypes=GrantTypes.Code,
          RedirectUris={
              LinkServer.FrontEndAdmin+"/signin-oidc"
          },
          PostLogoutRedirectUris={
              LinkServer.FrontEndAdmin+"/signout-callback-oidc"
          },
           AllowedScopes={"openid","profile",
               "orderservice.getorders",
               "orderservice.management" ,
              "apigatewayadmin.fullaccess",
              "productservice.admin"}
        }
    })
   .AddInMemoryIdentityResources(new List<IdentityResource> {

        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    })
   .AddInMemoryApiScopes(new List<ApiScope> {
       new ApiScope("orderservice.management"),
       new ApiScope("orderservice.getorders"),
       new ApiScope("basket.fullaccess"),
       new ApiScope("apigatewayforweb.fullaccess"),
       new ApiScope("apigatewayadmin.fullaccess"),
       new ApiScope("productservice.admin"),
    })
   .AddInMemoryApiResources(new List<ApiResource>
    {
        new ApiResource("orderservice","Order Service Api")
         {
              Scopes={ "orderservice.management" , "orderservice.getorders" }
         },
        new ApiResource("basketService","Baket Api Service")
          {
          Scopes={ "basket.fullaccess" }
          },
        new ApiResource("apigatewayforweb","Api gateway For FrontEnd Web")
        {
            Scopes={ "apigatewayforweb.fullaccess" }
        },
        new ApiResource("apigatewayadmin","Api gateway For Admin")
        {
            Scopes={ "apigatewayadmin.fullaccess" }
        },
        new ApiResource("productservice","Api gateway For FrontEnd Web")
        {
            Scopes={ "productservice.admin" }
        }
    }).
    AddAspNetIdentity<IdentityUser>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    SeedUserData.Seed(app);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
