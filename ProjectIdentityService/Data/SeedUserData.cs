using Microsoft.AspNetCore.Identity;

namespace ProjectIdentityService.Data;
public static class SeedUserData
{
    public static void Seed(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {


            var context = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (context.Users.Count() == 0)
            {


                IdentityRole identityRoleAdmin = new IdentityRole
                {
                    Name = "Admin",
                };

                IdentityRole identityRoleCustomer = new IdentityRole
                {
                    Name = "Customer",
                };

                var rolecontext = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                rolecontext.CreateAsync(identityRoleAdmin).Wait();
                rolecontext.CreateAsync(identityRoleCustomer).Wait();


                foreach (var item in UserList())
                {
                    var result = context.CreateAsync(item, "123456A@aa").Result;
                    if (item.UserName == "Cyrus")
                    {
                        var addToAdminRole = context.AddToRoleAsync(item, "Admin").Result;
                    }
                    else
                    {
                        var addToAdminRole = context.AddToRoleAsync(item, "Customer").Result;
                    }
                }
            }
        }

    }


    private static List<IdentityUser> UserList()
    {
        return new List<IdentityUser>()
            {
                new IdentityUser()
                {
                     UserName="test" ,
                     Email="sayyehban@gmail.com",
                     EmailConfirmed=true,
                },
               new IdentityUser()
                {
                     UserName="Cyrus" ,
                     Email="sdvp1991david@gmail.com",
                     EmailConfirmed=true,
                },
            };
    }
}
