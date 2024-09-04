using Microsoft.EntityFrameworkCore;
using RecyclingHub.Data;
using RecyclingHub.Dependencies;
using RecyclingHub.Models.DbModels;
using RecyclingHub.Components;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Extensions;
using AspNetCoreHero.ToastNotification;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add the DbContext 
builder.Services.AddDbContext<HubContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("hubConnection"));
});
//add identityDbContext
builder.Services.AddDbContext<IdentityContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")) ;
});

//add IdentitySupport
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();
//add blazor support
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//add the product service
builder.Services.AddScoped<IHubServices<Product>, ProductsService>();

//add the salesService
builder.Services.AddScoped<IHubServices<Sale>, SalesService>();

//add the insectionsService
builder.Services.AddScoped<IHubServices<Insection>, InsectionsService>();

//add inventory service
builder.Services.AddScoped<IHubServices<RecyclingHub.Models.DbModels.Inventory>, InventoryService>();

//add acquisitionService
builder.Services.AddScoped<IHubServices<Aquisition> , AcquisitionService>();

//add DisposalService
builder.Services.AddScoped<IHubServices<Disposal>, DisposalService>();

//add Identity Helper functions as a service
builder.Services.AddScoped<IdentityService>();

//add notification service
builder.Services.AddNotyf(opts =>
{
    opts.DurationInSeconds = 3;
    opts.IsDismissable = true;
    opts.Position = NotyfPosition.TopCenter;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseNotyf();
app.UseAntiforgery();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//ensure data is seeded
await IdentitySeedData.EnsureIdentitySeeded(app);
await DbSeedData.EnsurePopulatedAsync(app);
app.Run();
