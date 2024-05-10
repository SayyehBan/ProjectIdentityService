using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

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
    }).
    AddInMemoryClients(new List<Client> { }).
    AddInMemoryIdentityResources(new List<IdentityResource> { });
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
