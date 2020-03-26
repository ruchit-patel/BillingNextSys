using System;
using System.Configuration;
using BillingNextSys.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BillingNextSys.Areas.Identity.IdentityHostingStartup))]
namespace BillingNextSys.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BillingNextSysIdentityDbContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("BillingNextSysIdentityDbContextConnection")
                       ));

                services.AddIdentity<BillingNextUser,IdentityRole> (config =>
                {
                  //  config.SignIn.RequireConfirmedEmail = true;
                    //config.SignIn.RequireConfirmedPhoneNumber = true;
                })
                    .AddEntityFrameworkStores<BillingNextSysIdentityDbContext>()
                .AddDefaultTokenProviders()
                 .AddDefaultUI();
            });
        }
    }
}