using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using ProjectIdentityService.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer().
    AddDeveloperSigningCredential().
    AddTestUsers(new List<TestUser>
    {
        new TestUser
        {
            IsActive = true,
            Password="test",
            Username="test",
            SubjectId="1"
        }
    }).AddInMemoryClients(new List<Client> {

      new Client
      {
          ClientName="frontend Web",
          ClientId="webfrontend",
          ClientSecrets={new Secret ("123456".Sha256()) },
          AllowedGrantTypes=GrantTypes.ClientCredentials,
          AllowedScopes={ "orderservice.fullaccsess" }
      },
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
          AllowedScopes={ "openid", "profile" }
      }
    })
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    })
    .AddInMemoryApiScopes(new List<ApiScope> {
       new ApiScope("orderservice.fullaccsess")
    })
    .AddInMemoryApiResources(new List<ApiResource>
    {
         new ApiResource("orderservice","Order Service Api")
         {
              Scopes={ "orderservice.fullaccsess" }
         }
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
