using Bus_Ticket_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddXmlSerializerFormatters();
builder.Services.AddScoped<IBusDBRepository, ClassBusDBRepository>();

// Database
var connectionString = builder.Configuration.GetConnectionString("BusDBConnection");
builder.Services.AddDbContextPool<AppDbContext>(x => x.UseSqlServer(connectionString));


// Identity Service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{

    option.User.AllowedUserNameCharacters = null;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedPhoneNumber = false;
    option.SignIn.RequireConfirmedEmail = false;    
    option.SignIn.RequireConfirmedAccount = false;

    option.Password.RequiredLength = 4;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequiredUniqueChars = 3;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;

    

}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});



var app = builder.Build();

// +++--------------- Configure ---------------++++ //

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


// for attribute routing


// for conventional routing
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/
app.Run();

