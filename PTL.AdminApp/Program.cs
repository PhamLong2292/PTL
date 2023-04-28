using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PTL.ApiIClient;
using PTL.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
    });
builder.Services.AddControllers();
//builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<ILanguageApiClient, LanguageApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
//Dictionary
builder.Services.AddTransient<IRegionApiClient, RegionApiClient>();
builder.Services.AddTransient<ICountryApiClient, CountryApiClient>();
builder.Services.AddTransient<IProvinceApiClient, ProvinceApiClient>();
builder.Services.AddTransient<IDistrictApiClient, DistrictApiClient>();
builder.Services.AddTransient<IVillageApiClient, VillageApiClient>();
builder.Services.AddTransient<IAdministrativeUnitApiClient, AdministrativeUnitApiClient>();

builder.Services.AddTransient<IEthnicApiClient, EthnicApiClient>();
builder.Services.AddTransient<IExpertiseApiClient, ExpertiseApiClient>();
builder.Services.AddTransient<IGraduationApiClient, GraduationApiClient>();
builder.Services.AddTransient<ISchoolApiClient, SchoolApiClient>();
builder.Services.AddTransient<IWorkUnitApiClient, WorkUnitApiClient>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    if (app.Environment.IsEnvironment("Production"))
    {
        app.UseExceptionHandler("/Production");
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //app.UseHsts();
    }
}
else
{
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();